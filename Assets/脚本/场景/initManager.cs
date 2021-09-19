using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class initManager : MonoBehaviour
{
    [Header("关卡初始数据")]
    public int cost;
    public int exp;
    public int maxLife;
    public int remainPlace;
    public float enemyDelay;
    
    [Header("")]
    
    
    public List<operData> operList = new List<operData>();
    public List<enemyControl> ECList = new List<enemyControl>();
    public List<enemyInfo> EnemyInfoList = new List<enemyInfo>();
    public List<enemyControl> appearOrder = new List<enemyControl>();
    public List<enemyControl> sceneCanAtkEnemyList = new List<enemyControl>();
    
    
    public GameObject slot;
    public GameObject mainCanvas;
    public GameObject operPanel;
    public GameObject camera;
    public costController cost_;
    public expController exp_;

    public List<GameObject> slotList = new List<GameObject>();
    public UIconnect uc_;
    private float rolSpeed = 0.1f;
    private Quaternion slowRol;
    private Quaternion baseRol;
    private Quaternion tarRol;
    private Vector3 tarPos = new Vector3(0,0,-10);
    private Vector3 basePos = new Vector3(0,0,-10);
    private Vector3 rightPos = new Vector3(-2, 0, -10);
    public bool draging;
    public int life;
    public int totEnemy = 0;
    public int defeatEnemy = 0;
    public int priority = 0;
    private float timeLine;
    
    
    private Dictionary<Vector2, int> baseMp = new Dictionary<Vector2, int>();
    private Dictionary<Vector2, int> mp = new Dictionary<Vector2, int>();
    public List<BlueDoorPath> blueDoorPathList = new List<BlueDoorPath>();
    public List<Vector2> startPointList = new List<Vector2>();

    public Sprite pauseSprite;
    public Sprite pauseRecoverSprite;
    public Sprite twoFastSprite;
    public Sprite oneFastSprite;
    public Button pauseButton;
    public Button twoFastButton;

    private void Awake()
    {
        slowRol = Quaternion.Euler(new Vector3(-25, -5, 0));
        baseRol = Quaternion.Euler(new Vector3(-25, 0, 0));
        tarRol = baseRol;
        gameManager.im_ = this;

        foreach (var i in gameManager.formation[gameManager.formationNum])
        {
            operList.Add(i);
            GameObject newSlot;
            newSlot=Instantiate(slot, operPanel.transform, true);
            newSlot.GetComponent<Drag>().thisOper = i;
            //newSlot.transform.Find("Data").GetComponent<currentData>().od_ = i;
            slotList.Add(newSlot);
        }

        uc_ = GetComponent<UIconnect>();
    }


    void Start()
    {
        foreach (var i in ECList)
        {
            appearOrder.Add(i);
            if (!startPointList.Contains(i.pointList[0]))
            {
                startPointList.Add(i.pointList[0]);
            }
        }
            
        appearOrder.Sort((x, y) => x.appearTime.CompareTo(y.appearTime));

        cost_ = GetComponent<costController>();
        exp_ = GetComponent<expController>();
        totEnemy = ECList.Count;
        life = maxLife;
        timeLine = -enemyDelay;

        IllustratedBookManager.ChangeBookPrefab(gameManager.formation[gameManager.formationNum], EnemyInfoList);
    }

    private void Update()
    {
        timeLine += Time.deltaTime;
        //怪物生成时间轴
        while (appearOrder.Count > 0 && timeLine >= appearOrder[0].appearTime)
        {
            appearOrder[0].gameObject.SetActive(true);
            sceneCanAtkEnemyList.Add(appearOrder[0]);
            appearOrder[0].enemyDie += DoWhenEnemyDie;
            appearOrder.RemoveAt(0);
        }
        
        //相机旋转
        camera.transform.rotation= Quaternion.Slerp(camera.transform.rotation,tarRol , rolSpeed);
        camera.transform.position = Vector3.MoveTowards(camera.transform.position, tarPos, rolSpeed * 5);
    }
    
    public static float FixCoordinate(float x)
    {
        if (x > 0) return (int) x + 0.5f;
        else return (int) x - 0.5f;
    }
    
    public static Vector2 FixCoordinate(Vector2 x)
    {
        x.x = FixCoordinate(x.x);
        x.y = FixCoordinate(x.y);
        return x;
    }

    public int getMp(Vector2 p)
    {
        p.x = FixCoordinate(p.x);
        p.y = FixCoordinate(p.y);
        if (!mp.ContainsKey(p)) return -1;
        return mp[p];
    }

    public void setMp(Vector2 p, int k)
    {
        p.x = FixCoordinate(p.x);
        p.y = FixCoordinate(p.y);
        mp[p] = k;
    }

    public void resetMp(Vector2 p)
    {
        p.x = FixCoordinate(p.x);
        p.y = FixCoordinate(p.y);
        mp[p] = baseMp[p];
    }

    public void regist(Vector2 p, int k)
    {
        p.x = FixCoordinate(p.x);
        p.y = FixCoordinate(p.y);
        mp[p] = baseMp[p] = k;
        
    }
    
    public void timeSlowDrag()
    {
        if(!gameManager.pause)
            Time.timeScale = 0.1f;
        tarRol = slowRol;
        tarPos = rightPos;
    }
    public void timeSlowpick(GameObject oper)
    {
        if(!gameManager.pause)
            Time.timeScale = 0.1f;
        tarRol = slowRol;
        tarPos = basePos;
        tarPos.x = oper.transform.position.x;
        tarPos.y = oper.transform.position.y - 5f;
    }
    public void timeRecover()
    {
        if (gameManager.pause)
            Time.timeScale = 0;
        else if (gameManager.twoFast)
            Time.timeScale = 2;
        else 
            Time.timeScale = 1;
        tarRol = baseRol;
        tarPos = basePos;
    }
    public void Pause()
    {
        if (!gameManager.pause)
        {
            pauseButton.image.sprite = pauseRecoverSprite;
            gameManager.Pause();
        }
        else
        {
            pauseButton.image.sprite = pauseSprite;
            gameManager.EndPause();
        }
    }
    public void TwoFast()
    {
        if (!gameManager.twoFast)
        {
            twoFastButton.image.sprite = twoFastSprite;
            gameManager.TwoFast();   
        }
        else
        {
            twoFastButton.image.sprite = oneFastSprite;
            gameManager.EndTwoFast();
        }
        
    }

    public void ShowMessage(string s)
    {
        uc_.ShowMessage(s);
    }

    void DoWhenEnemyDie(enemyControl ec)
    {
        sceneCanAtkEnemyList.Remove(ec);
    }
}

//地图信息
/*
 0: ground,低处，无法放置干员
 1: lowground,低处，可以放置干员
 2: wall，高处，无法放置干员
 3: highground，高处，可以放置干员
 4: box,箱子，低处，无法放置干员，敌人无法通过
 5: hole,落穴，敌人平时视作墙壁，恐惧时视作平地
 */
public enum platformType : byte
{
    [EnumLabel("低处无法放置")]
    danGround,
    [EnumLabel("低处可以放置")]
    lowGround,
    [EnumLabel("高处无法放置")]
    wall,
    [EnumLabel("高处可以放置")]
    highGround,
    [EnumLabel("箱子类地形")]
    box,
    [EnumLabel("落穴")]
    hole
}