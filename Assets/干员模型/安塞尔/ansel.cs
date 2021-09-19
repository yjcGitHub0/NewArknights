using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Assertions.Must;

public class ansel : MonoBehaviour
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
    private lifeController life_;
    private bool isAutoRelease;
    
    
    private int skillNum;
    private int elitismLevel;
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
        sp_ = oc_.sp_;
        life_ = oc_.life_;
        skillNum = oc_.cd_.skillNum;
        elitismLevel = cd_.elitismLevel;
        skillLevel = cd_.skillLevel[skillNum];

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
        _giveAtkModification.Init(atk_, 10, 1, 0);
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
        GameObject bandage = poolManager.Ansel_Bandage();
        Vector3 initPos = transform.position;
        initPos.x += Math.Abs(transform.rotation.y) < TOLERANCE ? 1 : -1;
        initPos.y += 0.2f;
        bandage.transform.position = initPos;
        anselBandage ab_ = bandage.GetComponent<anselBandage>();
        ab_.oc_ = oc_;
        ab_.of_ = of_;
        ab_.Init(of_.tarOC, isSkill_2 ? additionalPercentage[skillLevel] * buffNum : 0);

        if (lastTarOC != of_.tarOC)
        {
            _giveAtkModification.Invalid(false);
            buffNum = 0;
        }
        else
        {
            _giveAtkModification.gameObject.SetActive(true);
            buffNum = buffNum + 1 <= talent[elitismLevel].y ? buffNum + 1 : buffNum;
            _giveAtkModification.Invalid(false);
            _giveAtkModification.Init(atk_, 10, 1 + (buffNum * talent[elitismLevel].x) * talentMul, 5);
        }

        lastTarOC = of_.tarOC;
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
    public float[] talentMulLevel = new float[7];
    public void skill_0() //技能函数，在此实现其功能
    {
        talentMul = talentMulLevel[skillLevel];
    }

    [Header("技能1")]
    //技能变量
    public float[] baseHealRate = new float[7];
    public float[] talentHealRate = new float[7];
    public void skill_1()
    {
        sp_.useSkill(0);
        float rate = baseHealRate[skillLevel] + buffNum * talentHealRate[skillLevel];
        foreach (var i in of_.tarOperList)
        {
            i.life_.getHeal(atk_.atk * rate);
            GameObject healing = poolManager.Healing();
            Vector3 pos = i.transform.position;
            pos.y -= 0.5f;
            healing.transform.position = pos;
        }
    }
    
    [Header("技能1")]
    //技能变量
    public float[] additionalPercentage = new float[7];
    public void skill_2()
    {
        float during = od_.duration2[skillLevel];
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
    
    public giveAtkModification _giveAtkModification;
    private operControl lastTarOC;
    private int buffNum = 0;
    private float talentMul = 1f;
    public bool isSkill_2;
    public Vector2[] talent = new Vector2[3];
    
}
