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
    public List<redDoorUI> redDoorWaveList = new List<redDoorUI>();
    public List<List<enemyControl>> ECList = new List<List<enemyControl>>();
    public List<List<guidePoint>> guideList = new List<List<guidePoint>>();
    private Queue<enemyControl> ECQueue = new Queue<enemyControl>();
    private Queue<guidePoint> guideQueue = new Queue<guidePoint>();
    private List<List<string>> redDoorTextList = new List<List<string>>();
    public List<Image> redDoorSliderImage = new List<Image>();
    private int wave = -1;
    private float nxtWaveTime;
    public List<enemyInfo> EnemyInfoList = new List<enemyInfo>();
    public List<enemyControl> sceneCanAtkEnemyList = new List<enemyControl>();
    
    
    public GameObject slot;
    public GameObject mainCanvas;
    public GameObject operPanel;
    public GameObject camera;
    public Camera orthoCamera;
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
            AddNewOperSlot(i);
        }

        uc_ = GetComponent<UIconnect>();
    }
    public void AddNewOperSlot(operData od)
    {
        operList.Add(od);
        GameObject newSlot;
        newSlot=Instantiate(slot, operPanel.transform, true);
        newSlot.GetComponent<Drag>().thisOper = od;
        slotList.Add(newSlot);
    }

    void Start()
    {
        foreach (var i in redDoorWaveList)
        {
            i.startButton.onClick.AddListener(NextWaveStart);
        }
        foreach (var i in ECList)
        {
            List<Dictionary<string, int>> map = new List<Dictionary<string, int>>();
            redDoorTextList.Add(new List<string>());
            int lastNum = redDoorTextList.Count - 1;
            for (int o = 0; o < redDoorWaveList.Count; o++)
            {
                redDoorTextList[lastNum].Add("");
                map.Add(new Dictionary<string, int>());
            }

            foreach (var j in i)
            {
                if (!startPointList.Contains(j.pointList[0]))
                {
                    startPointList.Add(j.pointList[0]);
                }
                int id = -1;
                for (int k = 0; k < redDoorWaveList.Count; k++)
                {
                    if ((Vector2)redDoorWaveList[k].transform.position == (Vector2)j.transform.position)
                    {
                        id = k;
                        break;
                    }
                }
                if (id != -1)
                {
                    if (!map[id].ContainsKey(j.ei_.name)) map[id][j.ei_.name] = 0;
                    map[id][j.ei_.name]++;
                }
            }
            for (int k = 0; k < redDoorWaveList.Count; k++)
            {
                foreach (var o in map[k])
                {
                    redDoorTextList[lastNum][k] += o.Key + "x" + o.Value.ToString() + "\n";
                }
            }
            i.Sort((x, y) => x.appearTime.CompareTo(y.appearTime));
        }
        foreach (var i in guideList)
        {
            i.Sort((x, y) => x.appearTime.CompareTo(y.appearTime));
        }
        NextWaveUI();
        foreach (var i in redDoorSliderImage)
        {
            i.fillAmount = 1;
        }

        cost_ = GetComponent<costController>();
        exp_ = GetComponent<expController>();
        life = maxLife;
        timeLine = -enemyDelay;

        IllustratedBookManager.ChangeBookPrefab(gameManager.formation[gameManager.formationNum], EnemyInfoList);
        
    }

    private void Update()
    {
        //相机旋转
        camera.transform.rotation= Quaternion.Slerp(camera.transform.rotation,tarRol , rolSpeed);
        camera.transform.position = Vector3.MoveTowards(camera.transform.position, tarPos, rolSpeed * 5);
        
        if (wave == -1) return;
        timeLine += Time.deltaTime;
        //怪物生成时间轴
        while (ECQueue.Count > 0 && timeLine >= ECQueue.Peek().appearTime)
        {
            enemyControl ec = ECQueue.Dequeue();
            ec.gameObject.SetActive(true);
            sceneCanAtkEnemyList.Add(ec);
            ec.enemyDie += DoWhenEnemyDie;
            if (ECQueue.Count == 0)
                NextWaveUI();
        }
        while (guideQueue.Count > 0 && timeLine >= guideQueue.Peek().appearTime)
        {
            guidePoint gp = guideQueue.Dequeue();
            gp.gameObject.SetActive(true);
        }
        
        //下一波开始倒计时
        if (ECQueue.Count == 0 && wave < ECList.Count)
        {
            nxtWaveTime += Time.deltaTime;
            foreach (var i in redDoorSliderImage)
            {
                i.fillAmount = nxtWaveTime / 60;
            }
            if (nxtWaveTime >= 10) NextWaveStart();
        }
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
        else if(!gameManager.forcePause)
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

    private void NextWaveUI()
    {
        int nwave = wave + 1;
        if (nwave >= ECList.Count) return;

        nxtWaveTime = 0;
        for (int i = 0; i < redDoorTextList[nwave].Count; i++)
        {
            var s = redDoorTextList[nwave][i];
            if (s == "") continue;
            redDoorWaveList[i].gc_.Show();
            redDoorWaveList[i].detailText.text = s;
        }
        
    }
    public void NextWaveStart()
    {
        wave++;
        if (wave >= ECList.Count) return;
        timeLine = 0;
        ECQueue.Clear();
        guideQueue.Clear();
        
        foreach (var i in redDoorWaveList)
        {
            i.gc_.Hide();
        }
        
        
        
        foreach (var i in ECList[wave])
        {
            ECQueue.Enqueue(i);
        }
        if (wave >= guideList.Count) return;
        foreach (var i in guideList[wave])
        {
            guideQueue.Enqueue(i);
        }

        
    }

    void DoWhenEnemyDie(enemyControl ec)
    {
        sceneCanAtkEnemyList.Remove(ec);
        ECList[ec.wave].Remove(ec);
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