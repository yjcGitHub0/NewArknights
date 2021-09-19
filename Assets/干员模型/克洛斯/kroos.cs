using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class kroos : MonoBehaviour
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
        GameObject arrow = poolManager.kroos_arrow();
        
        Vector3 initPos = transform.position;
        initPos.x += Math.Abs(transform.rotation.y) < TOLERANCE ? 1 : -1;
        initPos.y += 0.2f;
        arrow.transform.position = initPos;
        kroosArror ka_ = arrow.GetComponent<kroosArror>();
        ka_.oc_ = oc_;
        ka_.of_ = of_;
        
        bool isCrit = Random.Range(0f, 1f) <= critRate;
        if (isSkill_2)
            ka_.Init(isCrit, slowDuring[skillLevel], slowRate[skillLevel]);
        else
            ka_.Init(isCrit, 0, 1);
        
        
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
    public float[] increaseAtk = new float[7];
    public void skill_0()//技能函数，在此实现其功能
    {
        float during = od_.duration0[skillLevel];
        sp_.useSkill(during);
        buffManager.GiveAtkModification(atk_, 10, increaseAtk[skillLevel], during);
    }

    [Header("技能1")]
    //技能变量
    public float[] increaseRate = new float[7];
    public void skill_1()
    {
        critRate *= increaseRate[skillLevel];
        of_.ignoreStealth = true;
    }
    void skill_1_End()
    {
        of_.ignoreStealth = false;
    }
    
    [Header("技能2")]
    //技能变量
    public float[] increaseAtkSpeed = new float[7];
    public float[] slowDuring = new float[7];
    public float[] slowRate = new float[7];
    public void skill_2()
    {
        float during = od_.duration2[skillLevel];
        buffManager.GiveAtkSpeedModification(atkSpeed_, 0, increaseAtkSpeed[skillLevel], during);
        sp_.useSkill(during);
        anim.SetInteger("sta",1);
        isSkill_2 = true;
        Invoke(nameof(skill_2_End), during);
    }
    void skill_2_End()
    {
        anim.SetInteger("sta", 0);
        isSkill_2 = false;
    }
    
    
    [Header("天赋变量")]
    public float[] CritRate = new float[3];
    private float critRate;
    private bool isSkill_2;
}
