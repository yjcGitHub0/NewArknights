using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class noirc : MonoBehaviour
{
    //**************本部分为每个干员脚本相同的内容*****************
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
    private defController def_;
    private bool isAutoRelease;

    private int skillNum;

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
        def_ = oc_.def_;
        skillNum = oc_.cd_.skillNum;

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
        s1 = skill_1_collider.GetComponent<noirc_skill_1>();
        giveCostSpeedID = im_.cost_.AddSpeedDetaList(costSpeed[cd_.elitismLevel]);
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

        if (life_.life == 1e9f && alreadyRemove == false) 
        {
            alreadyRemove = true;
            DoBeforeRetreat();
        }
    }

    public void attack()
    {
        if (of_.tarEnemyIsNull) return;
        atk_.causePhyDamage(atk_.atk, of_.tarEnemy.life_, true);
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
    
    //***************以下部分改变内容但不可改变函数名***************

    [Header("技能0")]
    //技能变量
    public float[] increaseDef0 = new float[7];

    public void skill_0()//技能函数，在此实现其功能
    {//防御力强化
        float duringTime = of_.od_.duration0[cd_.skillLevel[0]];
        sp_.useSkill(duringTime);
        buffManager.GiveDefModification(def_, 10, increaseDef0[cd_.skillLevel[0]], duringTime);
    }

    [Header("技能1")]
    //技能变量
    public GameObject skill_1_collider;
    public float[] fearDuring = new float[7];
    private noirc_skill_1 s1;
    public void skill_1()
    {
        //
        s1.during = fearDuring[cd_.skillLevel[1]];
        skill_1_collider.SetActive(true);
        sp_.useSkill(0);
        
    }
    
    [Header("技能2")]
    //技能变量
    private float storeLife;
    private GameObject shiningDefense;
    public void skill_2()
    {
        storeLife = life_.life - 1;
        float durTime = of_.od_.duration2[cd_.skillLevel[2]];
        buffManager.GiveLockLife(life_, 1, durTime);
        buffManager.GiveAtkModification(atk_, 10, 0, durTime);
        shiningDefense = poolManager.Shining_Defense();
        shiningDefense.transform.position = transform.position;
        life_.getRealAtk(1e8f);
        oc_.operDie += DoWhenOperDie_Skill_2;
        Invoke(nameof(skill_2_end), durTime);
        sp_.useSkill(durTime);
    }

    void skill_2_end()
    {
        life_.getHeal(storeLife);
        shiningDefense.SetActive(false);
        oc_.operDie -= DoWhenOperDie_Skill_2;
    }
    void DoWhenOperDie_Skill_2(operControl oc)
    {
        shiningDefense.SetActive(false);
    }

    private int giveCostSpeedID;
    private bool alreadyRemove = false;
    public float[] costSpeed = new float[3];
    public void DoBeforeRetreat()
    {
        im_.cost_.RemoveSpeedDetaList(giveCostSpeedID);
    }
    
}
