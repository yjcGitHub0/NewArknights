using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class operFight : MonoBehaviour
{
    private float eps = 1e-5f;
    
    public operData od_;
    public currentData cd_;
    public operControl oc_;
    public animEvent ae_;
    private Animator anim;
    
    public List<enemyFight> tarEnemyList = new List<enemyFight>();
    public List<operControl> tarOperList = new List<operControl>();
    public List<enemyFight> blockList = new List<enemyFight>();
    public enemyFight tarEnemy;
    public bool tarEnemyIsNull;
    public operControl tarOC;
    public bool tarOperIsNull;
    public Vector3 tarPos;
    public bool beforeAtk;//atk后无法立刻切换动画
    public bool appearEnd = false;//入场结束
    public bool defFirst = false;
    public bool ignoreStealth;
    public bool fighting;
    private float fightTalkProbability = 0.001f;

    [Header("属性")]
    public GameObject Calculations;
    public lifeController life_;
    public atkController atk_;
    public defController def_;
    public spController sp_;
    public int maxBlock;//最大阻挡数
    public int block;//当前阻挡数
    public int skillNum;//当前选择的技能编号
    
    
    
    

    private void Awake()
    {
        oc_ = GetComponent<operControl>();
        anim = transform.Find("anim").GetComponent<Animator>();
        ae_ = anim.GetComponent<animEvent>();
    }

    private void Start()
    {
        cd_ = oc_.Data.GetComponent<currentData>();
        od_ = cd_.od_;
        //初始化属性
        Calculations = oc_.Calculations;
        life_ = oc_.life_;
        atk_ = oc_.atk_;
        def_ = oc_.def_;
        sp_ = oc_.sp_;
        maxBlock = cd_.baseBlock;
        block = 0;
        tarEnemyIsNull = true;
        skillNum = cd_.skillNum;
        
        //初始化各Calculation内数据
        life_.def_ = def_;
        life_.ChangeBaseLife(cd_.baseMaxLife);
        atk_.ChangeBaseAtk(cd_.baseAtk);
        def_.ChangeBaseDef(cd_.baseDef,cd_.baseMagicDef);
        switch (skillNum)
        {
            case 0:
                sp_.sp = od_.initSP0[cd_.skillLevel[0]];
                sp_.maxSP = od_.maxSP0[cd_.skillLevel[0]];
                break;
            case 1:
                sp_.sp = od_.initSP1[cd_.skillLevel[1]];
                sp_.maxSP = od_.maxSP1[cd_.skillLevel[1]];
                break;
            case 2:
                sp_.sp = od_.initSP2[cd_.skillLevel[2]];
                sp_.maxSP = od_.maxSP2[cd_.skillLevel[2]];
                break;
        }
        
        
        
    }

    // Update is called once per frame
    void Update()
    {
        checkDie();
        fight();
        getTar();
        fightTalk();
    }
    
    void checkDie()
    {
        if (life_.life <= 0)
        {
            oc_.OperDie();
            life_.life = 1e9f;
            anim.SetBool("die",true);
            if (oc_.is3D)
            {
                ae_.dieBegin();
                Invoke(nameof(DESTORY), 0.3f);
            }
        }
    }

    void DESTORY()
    {
        Destroy(gameObject);
    }

    int CmpByPriority(enemyFight x, enemyFight y)
    {
        if (x.isStealth && !y.isStealth) return 1;
        if (!x.isStealth && y.isStealth) return -1;
        if (x.priority > y.priority) return 1;
        if (x.priority < y.priority) return -1;
        return 0;
    }
    int CmpByDef(enemyFight x, enemyFight y)
    {
        if (x.isStealth && !y.isStealth) return 1;
        if (!x.isStealth && y.isStealth) return -1;
        if (x.def_.def > y.def_.def) return 1;
        if (x.def_.def < y.def_.def) return -1;
        return 0;
    }

    private void LateUpdate()
    {
        if (defFirst)
            tarEnemyList.Sort(CmpByDef);
        else
            tarEnemyList.Sort(CmpByPriority);

        tarOperList.Sort((x, y) => -(x.life_.maxLife - x.life_.life).CompareTo(y.life_.maxLife - y.life_.life));
    }

    public void getTar()
    {
        if (!od_.isMedical)
        {
            if (tarEnemyList.Count > 0 && (!tarEnemyList[0].isStealth || ignoreStealth))
            {
                tarEnemy = tarEnemyList[0];
                tarPos = tarEnemy.transform.position;
                tarEnemyIsNull = false;
            }
            else
            {
                tarEnemy = null;
                tarEnemyIsNull = true;
            }
        }
        else
        {
            if (tarOperList.Count > 0)
            {
                tarOC = tarOperList[0];
                tarPos = tarOC.transform.position;
                tarOperIsNull = false;
            }
            else
            {
                tarOC = null;
                tarOperIsNull = true;
            }
        }
    }

    void fight()
    {
        if (!od_.isMedical)
        {
            if (tarEnemyIsNull)
            {
                if (beforeAtk) anim.SetBool("fighting", false);
                fighting = false;
                oc_.reRol();
            }
            else
            {
                anim.SetBool("fighting", true);
                fighting = true;
                zhuan();
            }
        }
        else
        {
            if (tarOperList.Count == 0 || tarOperList[0].life_.life == tarOperList[0].life_.maxLife)
            {
                if (beforeAtk) anim.SetBool("fighting", false);
                fighting = false;
                oc_.reRol();
            }
            else
            {
                anim.SetBool("fighting", true);
                fighting = true;
                zhuan();
            }
        }
    }

    void zhuan()
    {
        if (!appearEnd) return;
        Vector2 detaPos = transform.position - tarPos;
        if (detaPos.x < 0) oc_.RolNor();
        else oc_.RolBack();
        if (detaPos.y < 0) anim.SetBool("fightBack", true);
        else anim.SetBool("fightBack", false);
    }

    void fightTalk()
    {
        if (fighting)
        {
            float tmp = Random.Range(0f, 1f);
            if (tmp < fightTalkProbability && !AudioManager.Operator.isPlaying && AudioManager.coolDown)
            {
                int index = Random.Range(0, od_.Fighting.Count);
                AudioManager.OperatorTalkAndClod(od_.Fighting[index]);
            }
        }
    }

    public void DoWhenEnemyDie(enemyControl ec)
    {
        if (tarEnemy == ec.ef_)
        {
            tarEnemy = null;
            tarEnemyIsNull = true;
        }
        tarEnemyList.Remove(ec.ef_);
        blockList.Remove(ec.ef_);
    }
    public void DoWhenTarOperDie(operControl oc)
    {
        if (tarOC == oc)
        {
            tarOC = null;
            tarOperIsNull = true;
        }
        tarOperList.Remove(oc);
    }

}
