using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;
using Random = UnityEngine.Random;
using Slider = UnityEngine.UI.Slider;

public class operControl : MonoBehaviour
{
    public bool alreadyThere;
    public bool dirBack;
    public GameObject Data;
    public Drag dr_;
    public currentData cd_;

    private float rolSpeed = 50;
    private Vector3 tiltRolNormal = new Vector3(-25f, 0f, 0f);
    private Vector3 tiltRolBack = new Vector3(25f, 180f, 0f);
    private Quaternion tarRol;
    private Quaternion rn;
    private Quaternion rb;
    public int priority;

    public GameObject Calculations;
    public CalUICon cuc_;
    public lifeController life_;
    public atkController atk_;
    public atkSpeedController atkSpeed_;
    public defController def_;
    public spController sp_;

    private GameObject operAnim;
    public Animator anim;
    public GameObject atkRange;
    public operData od_;
    public operFight of_;
    public animEvent ae_;
    public SpriteRenderer animRender;

    private GameObject initManager;
    public initManager im_;
    public UIconnect uc_;
    
    public GameObject slot;
    public GameObject sUI;
    public Image skillImage;
    private specialEvent se_;
    public bool is3D;

    private Vector3 basePos = new Vector3();
    private Vector3 baseRot = new Vector3();
    private Quaternion br;
    private Color greenColor = new Color(0.5f, 1, 0.5f, 1);
    private Color tarColor;
    private Color preColor;
    private float maxColorTime;
    private float colorTime;
    private bool colorRecover;

    //UI相关
    public Text spText;
    public Slider skillGreenSlider;
    public Slider skillYellowSlider;
    
    //状态表情相关
    public Emoji emoji;
    
    //观察者委托
    public delegate void WhatWillDoWhenOperDie(operControl oc);
    public event WhatWillDoWhenOperDie operDie;

    void Awake()
    {
        of_ = GetComponent<operFight>();
        operAnim = transform.Find("anim").gameObject;

        Calculations = transform.Find("Calculations").gameObject;
        cuc_ = Calculations.GetComponent<CalUICon>();
        life_ = Calculations.GetComponent<lifeController>();
        atk_ = Calculations.GetComponent<atkController>();
        atkSpeed_ = Calculations.GetComponent<atkSpeedController>();
        def_ = Calculations.GetComponent<defController>();
        sp_ = Calculations.GetComponent<spController>();

        life_.Healing += ChangeColorHealing;
        
        sUI = transform.Find("skillUI").Find("SUICanvas").Find("SUI").gameObject;
        skillImage = sUI.transform.Find("skillButton").GetComponent<Image>();
        spText = skillImage.transform.Find("spPanel").Find("spText").GetComponent<Text>();
        skillGreenSlider = skillImage.transform.Find("greenSkillSlider").GetComponent<Slider>();
        skillYellowSlider = skillImage.transform.Find("yellowSkillSlider").GetComponent<Slider>();
        anim = operAnim.GetComponent<Animator>();
        ae_ = operAnim.GetComponent<animEvent>();
        animRender = anim.GetComponent<SpriteRenderer>();
        
        GameObject sceneInit = GameObject.Find("sceneInit");
        initManager = sceneInit.transform.Find("initManager").gameObject;
        im_ = initManager.GetComponent<initManager>();
        se_ = GetComponent<specialEvent>();

        if (is3D)
        {
            rb = rn = Quaternion.Euler(Vector3.zero);
        }
        else
        {
            emoji = operAnim.transform.Find("emoji").GetComponent<Emoji>();
            rn = Quaternion.Euler(tiltRolNormal);
            rb = Quaternion.Euler(tiltRolBack);
        }
        
        if (alreadyThere)//如果该干员是提前放置的
        {
            cd_ = Data.GetComponent<currentData>();
            od_ = cd_.od_;
            alreadyThereInit();
        }
    }

    private void Start()
    {
        //执行入场specialEvent
        uc_ = im_.uc_;
        priority = im_.priority++;
        specialIn();
        if (od_.Deploy.Count == 0) return;
        int index = Random.Range(0, od_.Deploy.Count);
        AudioManager.OperatorTalk(od_.Deploy[index]);
    }

    // Update is called once per frame
    void Update()
    {
        //旋转
        operAnim.transform.rotation = Quaternion.Slerp(operAnim.transform.rotation, tarRol, rolSpeed * Time.deltaTime); 
        ChangeColor();
    }

    float fixCoordinate(float x)
    {
        if (x > 0) return (int) x + 0.5f;
        else return (int) x - 0.5f;
    }

    public void AppearIn(Vector3 pos, Vector3 rot, bool DirBack, Vector3 rangeRot)
    {
        //Debug.Log(new Vector2(fixCoordinate(pos.x),fixCoordinate(pos.y)));
        pos.x = fixCoordinate(pos.x);
        pos.y = fixCoordinate(pos.y);
        basePos = pos;
        baseRot = rot;
        if (is3D) baseRot = Vector3.zero;
        br = Quaternion.Euler(baseRot);
        transform.position = basePos;
        tarRol = br;

        operAnim.transform.rotation = Quaternion.Euler(baseRot);
        atkRange.transform.rotation = Quaternion.Euler(rangeRot);
        dirBack = DirBack;
        anim.SetBool("dirBack", DirBack);
    }

    void alreadyThereInit()
    {
        basePos = transform.position;
        baseRot = anim.transform.rotation.eulerAngles;
        br = anim.transform.rotation;
        tarRol = br;
        cd_ = Data.GetComponent<currentData>();
        atkRange = Instantiate(cd_.od_.atkRange[cd_.elitismLevel], transform, true);
        atkRange.name = "atkRange";
        atkRange.transform.localPosition = Vector3.zero;
        
        anim.SetBool("dirBack", dirBack);
    }

    void specialIn()
    {
        if (se_.isWall)
        {//是箱子
            if (!se_.isWallFunction())
            {
                im_.ShowMessage("不能在此摆放");
                retreat(true,false);
            }
        }
    }

    void specialOut()
    {
        if (se_.isWall) se_.reWallFunction();
    }
    
    public void retreat(bool des,bool beKilled)
    {
        uc_.CloseLeftUI(this);
        
        specialOut();
        
        OperDie();

        if (!beKilled) 
        {//正常撤退返还原cost的一半,否则不返还
            im_.cost_.GetCost(od_.cost / 2);
        }
        //撤退后cost增加原cost的50%，上限为100%
        if (!is3D)
        {
            if (cd_.cost < od_.cost * 2) cd_.cost += od_.cost / 2;
        }

        for (int i = 0; i < of_.blockList.Count; i++)
        {
            enemyFight ef_ = of_.blockList[i];
            ef_.pretendExit(gameObject, true, false);
        }
        
        //撤退后返还占用的位置数
        im_.remainPlace += od_.consumPlace;

        /*
        if(des)//摧毁自己
            Invoke("DESTORY",0.1f);
        */
        if (!alreadyThere)
        {
            dr_.Reverse(1, !is3D);
            dr_.changeText();
        }
        if (des) Destroy(gameObject);
    }

    public void OpenSUI()
    {
        dontCloseSUI();
        sUI.SetActive(true);
        uc_.OpenLeftUI(this);
    }

    public void dontCloseSUI()
    {
        uc_.dontCloseUI = true;
    }
    
    public void RolNor()
    {
        tarRol = rn;
    }

    public void RolBack()
    {
        tarRol = rb;
    }

    public void reRol()
    {
        tarRol = br;
    }

    void ChangeColor()
    {
        if (colorTime <= 0)
        {
            if (colorRecover)
            {
                tarColor=Color.white;
                preColor = animRender.color;
                colorTime = maxColorTime;
            }
            else
                return;
        }
        float lerp = (maxColorTime - colorTime) / maxColorTime;
        animRender.color = Color.Lerp(preColor, tarColor, lerp);
        colorTime -= Time.deltaTime;
    }

    public void ColorAndRecover(Color col, float t, bool recover)
    {
        tarColor = col;
        colorTime = maxColorTime = t;
        colorRecover = recover;
        preColor = animRender.color;
    }
    private void ChangeColorHealing()
    {
        ColorAndRecover(greenColor, 0.3f, true);
    }
    
    public void OperDie()
    {
        if (operDie != null) operDie(this);
    }
}
