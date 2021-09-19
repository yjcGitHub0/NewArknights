using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyFight : MonoBehaviour
{
    private float eps = 1e-5f;
    
    public enemyInfo ei_;
    private initManager im_;
    public GameObject thisGameObject;
    public enemyControl ec_;
    private List<GameObject> blockList = new List<GameObject>();
    private List<operControl> blockListOC = new List<operControl>();
    public List<GameObject> atkRangeList = new List<GameObject>();
    public List<operControl> atkRangeListOC = new List<operControl>();
    public GameObject fightTar;
    public operControl tarOC_;
    public bool tarIsNull;
    private operFight tarOF_;
    public GameObject Empty;
    public EnemyFarAtk efa_;
    private Vector2 endPos = new Vector2();
    private GameObject enemyAnim;
    private Animator anim;
    public enemyAnimEvent eae_;
    public bool isDizzy = false;
    public bool isStealth;
    public int unBlock = 0;
    public float atkTime;
    public bool alreadyFight;
    public bool blocking;
    private Vector2 enterPos = new Vector2();
    private Vector2 lastPoint = Vector2.zero;
    
    [Header("被打的优先级")]
    public float priority;//该敌人被干员攻击的优先级
    private float disToEnd;//该敌人到终点的几何距离
    
    [Header("属性")]
    public GameObject Calculations;
    public lifeController life_;
    public atkController atk_;
    public defController def_;
    public spController sp_;
    public int consumeBlock;//消耗阻挡数

    void Start()
    {
        ec_ = GetComponent<enemyControl>();
        im_ = ec_.im_;
        enemyAnim = transform.Find("anim").gameObject;
        anim = enemyAnim.GetComponent<Animator>();
        eae_ = enemyAnim.GetComponent<enemyAnimEvent>();
        fightTar = Empty;

        ei_ = ec_.ei_;
        thisGameObject = gameObject;
        
        consumeBlock = ei_.consumeBlock;
        endPos = ec_.pointList[ec_.pointList.Count - 1];
        
        Calculations = ec_.Calculations;
        life_ = ec_.life_;
        atk_ = ec_.atk_;
        def_ = ec_.def_;
        sp_ = ec_.sp_;
        
        life_.maxLife = life_.life = ei_.life;
        life_.def_ = def_;
        atk_.atk = ei_.atk;
        def_.def = ei_.def;
        def_.magicDef = ei_.magicDef;
        
        FightTar_Empty();
    }
    
    void Update()
    {
        //死亡
        if (eae_.dying) return;
        
        getPriority();
        checkDie();
        fight();

        if (fightTar != Empty && ec_.fighting == false)
        {
            if (atkTime <= 0) StartFight();
            else if (!alreadyFight) StartFight();
            else EndFight(blocking);
        }

        if (atkTime > 0) atkTime -= Time.deltaTime;
    }
    
    void checkDie()
    {
        if (life_.life <= 0)
        {
            ec_.EnemyDie();
            life_.life = 1e9f;
            anim.SetBool("die",true);
            im_.cost_.GetCost(ei_.dropCost);
            im_.exp_.GetExp(ei_.dropExp);
        }
    }

    void getPriority()
    {
        var position = transform.position;

        if (ei_.isDrone)
        {
            disToEnd = Vector2.Distance(position, ec_.finalPoint);
            priority = disToEnd;
        }
        else
        {
            Vector2 nowPoint = initManager.FixCoordinate(transform.position);
            
            if (nowPoint != lastPoint)
            {
                lastPoint = nowPoint;
                enterPos = transform.position;
            }

            if (im_.blueDoorPathList[ec_.blueDoorNum].disMap.ContainsKey(nowPoint))
            {
                priority = im_.blueDoorPathList[ec_.blueDoorNum].disMap[nowPoint];
                priority -= Vector2.Distance(transform.position, enterPos);
            }
            else priority = 0;
        }
    }
    
    void fight()
    {
        if (unBlock > 0) return;
        if (fightTar == Empty && !isDizzy)
        {
            for (int i = 0; i < blockListOC.Count; i++)
            {
                var o = blockListOC[i];
                operFight of_ = o.of_;
                if (of_.block + consumeBlock > of_.maxBlock) continue;
                of_.block += consumeBlock;
                fightTar = o.gameObject;
                tarOC_ = o;
                tarOF_ = of_;
                tarIsNull = false;
                blocking = true;
                break;
            }
            if (fightTar == Empty)
            {
                ChangeFightTarByAtkRangeList();
            }
        }

        if (fightTar == Empty)
        {
            blocking = false;
            EndFight(false);
            return;
        }
        
        else if (ec_.fighting) 
        {
            if(transform.position.x-fightTar.transform.position.x>0)
                ec_.RolBack();
            else 
                ec_.RolNor();
            if (tarOF_.block > tarOF_.maxBlock)
            {
                fightTar = Empty;
                tarOF_.block -= consumeBlock;
            }
        }
    }

    public void ChangeFightTarByAtkRangeList()
    {
        for (int i = 0; i < atkRangeListOC.Count; i++)
        {
            var o = atkRangeListOC[i];
            fightTar = o.gameObject;
            tarOC_ = o;
            tarOF_ = o.of_;
            tarIsNull = false;
            break;
        }
    }

    public void FightTar_Empty()
    {
        if (fightTar != Empty)
        {
            if(blocking)
                tarOF_.block -= consumeBlock;
            fightTar = Empty;
            blocking = false;
            ec_.otherAnim = false;
            tarIsNull = true;
        }
    }

    void StartFight()
    {
        ec_.otherAnim = true;
        atkTime = ei_.minAtkInterval;
        alreadyFight = false;
        ec_.anim.SetBool("fight",true);
        ec_.anim.SetBool("fightEnd",false);
    }

    void EndFight(bool stop)
    {
        ec_.otherAnim = stop;
        ec_.anim.SetBool("fightEnd",true);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        pretendEnter(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        pretendExit(other.gameObject, true, true);
    }

    public void pretendEnter(GameObject oper)
    {
        if (oper.CompareTag("operator") || oper.CompareTag("box"))
        {
            if (blockList.Contains(oper)) return;
            operControl oc_ = oper.GetComponent<operControl>();
            operFight of_ = oc_.of_;
            blockList.Add(oper);
            blockListOC.Add(oc_);
            of_.blockList.Add(this);
            if (fightTar != Empty)
            {
                fightTar = Empty;
                tarIsNull = true;
                blocking = false;
                ec_.otherAnim = false;
            }
            oc_.operDie += DoWhenOperDie_Block;
        }
    }

    public void pretendExit(GameObject oper, bool willRemoveEf,bool willRemoveOf)
    {
        if (!oper.transform.CompareTag("operator") && !oper.transform.CompareTag("box")) return;
        if (blockList.Contains(oper.gameObject))
        {
            int i = blockList.IndexOf(oper);
            
            FightTar_Empty();
            operFight of_ = blockListOC[i].of_;
            of_.blockList.Remove(this);
            if (willRemoveEf)
            {
                blockList.RemoveAt(i);
                blockListOC.RemoveAt(i);
            }
            of_.oc_.operDie -= DoWhenOperDie_Block;
        }
    }

    public void dieExit()
    {
        foreach (var i in blockList)
        {
            pretendExit(i, false,true);
        }
    }
    
    public void DoWhenOperDie_Block(operControl oc)
    {
        if (tarOC_ == oc)
        {
            FightTar_Empty();
        }
        blockList.Remove(oc.gameObject);
        blockListOC.Remove(oc);
    }
    
}
