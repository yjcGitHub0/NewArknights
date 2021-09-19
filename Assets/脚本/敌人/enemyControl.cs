using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental;
using UnityEngine;
using UnityEngine.UI;

public class enemyControl : MonoBehaviour
{
    [Header("敌人的行动轨迹")] 
    public float appearTime;//该敌人出现的时间
    public List<Vector3> pointList = new List<Vector3>();//该敌人的路径锚点(x,y,time,sta)
    public float defaultZ;
    
    public enemyInfo ei_;
    public float speed;
    public GameObject self;
    private const float eps = 1e-3f;
    private bool isDrone;
    private float rolSpeed = 50;
    private Vector3 tiltRolNormal = new Vector3(-25f,0f,0f);
    private Vector3 tiltRolBack = new Vector3(25f,180f,0f);
    private Quaternion tarRol;
    private Quaternion rn;
    private Quaternion rb;
    public initManager im_;
    private enemyAnimEvent eae_;
    public enemyFight ef_;
    public SpriteRenderer animRender;
    
    //观察者委托
    public delegate void WhatWillDoWhenEnemyDie(enemyControl ec);
    public event WhatWillDoWhenEnemyDie enemyDie;
    
    public bool moving;
    private GameObject enemyAnim;
    private Axing ax_;
    public Animator anim;
    
    public GameObject Calculations;
    public lifeController life_;
    public atkController atk_;
    public defController def_;
    public spController sp_;
    
    public bool canMove;
    public bool otherAnim = false;
    public int pauseAnim = 0;
    public bool fighting = false;
    public float speedDeta = 1f;
    public bool isStagnation;

    public Queue<Vector3> pointQueue = new Queue<Vector3>();//路径锚点队列
    public Queue<Vector2> realPointQueue = new Queue<Vector2>();//生成的实际路径锚点
    public Stack<Vector2> lastPointStack = new Stack<Vector2>();//经过的方块
    public bool lastPointStackValid;
    public bool nxtPointListValid;
    public Vector3 endPoint = new Vector3();
    public Vector3 finalPoint = new Vector3();
    public Vector3 nxtPoint = new Vector3();
    private Vector3 lastPoint = new Vector3();
    public int blueDoorNum;

    //状态表情相关
    public Emoji emoji;

    // Start is called before the first frame update
    void Awake()
    {
        im_ = GameObject.Find("initManager").GetComponent<initManager>();
        Registe();
        self = gameObject;
        
        rn = Quaternion.Euler(tiltRolNormal);
        rb = Quaternion.Euler(tiltRolBack);

        Transform transform1 = transform;
        
        ax_ = GetComponent<Axing>();
        enemyAnim = transform1.Find("anim").gameObject;
        anim = enemyAnim.GetComponent<Animator>();
        emoji = enemyAnim.transform.Find("emoji").GetComponent<Emoji>();
        ef_ = GetComponent<enemyFight>();
        eae_ = enemyAnim.GetComponent<enemyAnimEvent>();
        foreach (var point in pointList)
        {
            pointQueue.Enqueue(point);
        }
        finalPoint = pointList[pointList.Count - 1];
        lastPointStackValid = true;
        nxtPointListValid = true;
        isDrone = ei_.isDrone;
        endPoint = pointQueue.Dequeue();
        nxtPoint = endPoint;
        nxtPoint.z = defaultZ;
        lastPoint = nxtPoint;
        speed = ei_.speed;
        animRender = anim.GetComponent<SpriteRenderer>();
        
        tarRol = enemyAnim.transform.rotation;
        transform1.position = nxtPoint;
        
        canMove = true;
        moving = true;
        isStagnation = false;

        Calculations = transform.Find("Calculations").gameObject;
        life_ = Calculations.GetComponent<lifeController>();
        atk_ = Calculations.GetComponent<atkController>();
        def_ = Calculations.GetComponent<defController>();
        sp_ = Calculations.GetComponent<spController>();

        gameObject.SetActive(false);
    }

    private void Start()
    {
        for (int i = 0; i < im_.blueDoorPathList.Count; i++)
        {
            if ((Vector2) finalPoint == im_.blueDoorPathList[i].pos)
            {
                blueDoorNum = i;
                break;
            }
        }
    }

    void Registe()
    {
        
        im_.ECList.Add(this);
        if (!im_.EnemyInfoList.Contains(ei_)) im_.EnemyInfoList.Add(ei_);
    }

    // Update is called once per frame
    void Update()
    {
        //死亡
        if (eae_.dying) return;

        //旋转
        enemyAnim.transform.rotation = Quaternion.Slerp(enemyAnim.transform.rotation, tarRol, rolSpeed * Time.deltaTime);
        
        if (vc2Equal(transform.position,endPoint))
        {
            if (endPoint.z > 0)
            {
                endPoint.z -= Time.deltaTime;
                canMove = false;
            }
            else if (pointQueue.Count == 0)
            {
                arrive();
                canMove = false;
            }
            else
            {
                gotoNextPoint();
                canMove = true;
            }
        }
        else if (otherAnim || pauseAnim > 0 || fighting) canMove = false;
        else canMove = true;
        
        Move();
    }

    bool vc2Equal(Vector2 x, Vector2 y)
    {
        return (Mathf.Abs(x.x - y.x) < eps) && (Mathf.Abs(x.y - y.y) < eps);
    }
    
    void gotoNextPoint()
    {
        if (pointQueue.Count == 0)
        {
            canMove = false;
            return;
        }
        endPoint = pointQueue.Dequeue();
        if (nxtPointListValid)
        {
            changeRoute();
        }
    }

    public bool changeRoute()
    {
        if (ei_.isDrone)
        {
            nxtPoint = endPoint;
            nxtPoint.z = defaultZ;
            return true;
        }

        bool found = ax_.Astar(realPointQueue, transform.position, endPoint);
        
        if (!found) return false;
        nxtPoint = realPointQueue.Dequeue();
        nxtPoint.z = defaultZ;
        return true;
    }

    void arrive()
    {
        if (enemyDie != null) enemyDie(this);
        
        eae_.becomeBlack();
        Invoke(nameof(DESTORY),0.5f);
        im_.defeatEnemy++;
        im_.life--;
        
        Vector3 nPos = transform.position;
        Vector3 nLPos = anim.transform.localPosition;
        nPos.z += 100;
        nLPos.z -= 100;
        transform.position = nPos;
        anim.transform.localPosition = nLPos;
    }

    void Move()
    {
        if (isStagnation) 
        {
            anim.speed = 0;
            return;
        }
        if (!canMove)
        {
            anim.SetBool("move",false);
            anim.speed = 1f;
            return;
        }

        anim.speed = speedDeta;

        if (isDrone)
        {
            if (Vector2.Distance(lastPoint, transform.position) >= 1f)
            {
                lastPoint = transform.position;
                if (lastPointStackValid) lastPointStack.Push(lastPoint);
            }
        }
        else if(transform.position == nxtPoint)
        {
            if (lastPointStackValid) lastPointStack.Push(nxtPoint);
        }

        
        
        if (transform.position == nxtPoint)
        {
            if (nxtPointListValid)
            {
                if (realPointQueue.Count != 0)
                {
                    nxtPoint = realPointQueue.Dequeue();
                    nxtPoint.z = defaultZ;
                }
            }
        }

        if (transform.position != nxtPoint)
            anim.SetBool("move", true);
        
        Vector2 tmp = nxtPoint - transform.position;
        if (tmp.x < 0)
            RolBack();
        else if (tmp.x > 0)
            RolNor();
        
        transform.position = Vector3.MoveTowards(transform.position, nxtPoint, speed * speedDeta * Time.deltaTime);
    }

    public void RolNor()
    {
        tarRol = rn;
    }

    public void RolBack()
    {
        tarRol = rb;
    }

    void DESTORY()
    {
        Destroy(gameObject);
    }

    public void EnemyDie()
    {
        if (enemyDie != null) enemyDie(this);
    }
}
