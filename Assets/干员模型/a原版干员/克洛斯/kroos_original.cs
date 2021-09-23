using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using System;

public class kroos_original : MonoBehaviour
{
    private float TOLERANCE = 0.2f;
    private GameObject fa;
    private operControl oc_;
    private operFight of_;
    private operData od_;
    private UIconnect uc_;
    private Animator anim;
    private initManager im_;
    private currentData cd_;
    private GameObject Calculation;
    private spController sp_;
    private atkController atk_;
    private atkSpeedController atkSpeed_;
    private lifeController life_;
    private bool isAutoRelease;

    private int skillNum;
    private int skillLevel;

    void Start()
    {
        fa = transform.parent.gameObject;
        anim = GetComponent<Animator>();
        oc_ = fa.GetComponent<operControl>();
        of_ = fa.GetComponent<operFight>();
        od_ = oc_.od_;
        uc_ = oc_.uc_;
        cd_ = oc_.cd_;
        im_ = oc_.im_;
        Calculation = oc_.Calculations;
        atk_ = oc_.atk_;
        atkSpeed_ = oc_.atkSpeed_;
        sp_ = oc_.sp_;
        life_ = oc_.life_;
        skillNum = oc_.cd_.skillNum;
        skillLevel = cd_.skillLevel[skillNum];
        
        //*****************此处为干员独特部分********************
        critRate = CritRate[cd_.elitismLevel];
        //****************************************************

        isAutoRelease = false;
        switch (cd_.skillNum)
        {
            case 0:
                if (od_.skill0_releaseType == releaseType.passive)
                    skill_0();
                if (od_.skill0_releaseType == releaseType.auto)
                    isAutoRelease = true;
                break;
            case 1:
                if (od_.skill1_releaseType == releaseType.passive)
                    skill_1();
                if (od_.skill1_releaseType == releaseType.auto)
                    isAutoRelease = true;
                break;
            case 2:
                if (od_.skill2_releaseType == releaseType.passive)
                    skill_2();
                if (od_.skill2_releaseType == releaseType.auto)
                    isAutoRelease = true;
                break;
        }
        //*****************此处为干员独特部分********************
        
        //****************************************************
    }

    public void Update()
    {
        if (isAutoRelease)
        {
            if (sp_.sp == sp_.maxSP)
            {
                switch (skillNum)
                {
                    case 0: 
                        skill_0();
                        break;
                    case 1: 
                        skill_1();
                        break;
                    case 2: 
                        skill_2();
                        break;
                }
            }
        }
    }

    public void attack()
    {
        GameObject arrow = Instantiate(kroosArror);
        
        Vector3 initPos = transform.position;
        initPos.x += Math.Abs(transform.rotation.y) < TOLERANCE ? 1 : -1;
        initPos.y += 0.2f;
        arrow.transform.position = initPos;
        kroosArror_original ka_ = arrow.GetComponent<kroosArror_original>();
        ka_.oc_ = oc_;
        ka_.of_ = of_;
        
        bool isCrit = Random.Range(0f, 1f) <= critRate;
        ka_.Init(isCrit, isSkill_0 ? atkRate[skillLevel] : 1);
        if (isSkill_0)
        {
            GameObject aarrow = Instantiate(kroosArror);
        
            Vector3 iinitPos = transform.position;
            iinitPos.x += Math.Abs(transform.rotation.y) < TOLERANCE ? 1 : -1;
            iinitPos.y += 0.2f;
            aarrow.transform.position = iinitPos;
            kroosArror_original kka_ = aarrow.GetComponent<kroosArror_original>();
            kka_.oc_ = oc_;
            kka_.of_ = of_;
        
            bool iisCrit = Random.Range(0f, 1f) <= critRate;
            kka_.Init(iisCrit, atkRate[skillLevel]);
            isSkill_0 = false;
        }
        
        of_.beforeAtk = false;
    }
    
    public void pressSkill()
    {
        if (sp_.sp < sp_.maxSP) return;
        switch (skillNum)
        {
            case 0: 
                skill_0();
                break;
            case 1: 
                skill_1();
                break;
            case 2: 
                skill_2();
                break;
        }
    }
    
    [Header("技能0")]
    //技能变量
    public float[] atkRate = new float[7];
    public void skill_0()//技能函数，在此实现其功能
    {
        sp_.useSkill(0);
        isSkill_0 = true;
    }
    public void skill_1()
    {
        
    }
    //技能变量
    public void skill_2()
    {
        
    }


    [Header("天赋变量")]
    public float[] CritRate = new float[3];
    public GameObject kroosArror;
    private float critRate;
    private bool isSkill_0;
}
