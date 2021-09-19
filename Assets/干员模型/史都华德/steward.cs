using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class steward : MonoBehaviour
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
    private bool skill_1_release = false;

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
        sp_ = oc_.sp_;
        life_ = oc_.life_;
        skillNum = cd_.skillNum;
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
        of_.defFirst = true;
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

    public void attack()
    {
        Vector3 initPos = transform.position;
        initPos.x += Math.Abs(transform.rotation.y) < TOLERANCE ? 0.5f : -0.5f;
        initPos.y += 0.2f;
        initPos.z = -1f;
        bool spAtk = true;
        if (skillNum == 1 && sp_.sp >= sp_.maxSP)
        {
            sp_.useSkill(0);
            GameObject bigBullet = poolManager.Big_Track_Magic_Fire();
            SceneTarTrack stt_ = bigBullet.GetComponent<SceneTarTrack>();
            stt_.Init(initPos, true);
            stt_.getIt += skill_1_cause_dam;
            skill_1_release = false;
            spAtk = false;
        }
        
        GameObject bullet = poolManager.Magic_Bullet();
        bullet.transform.position = initPos;
        MagicBullet mb_ = bullet.GetComponent<MagicBullet>();
        mb_.oc_ = oc_;
        mb_.of_ = of_;
        mb_.Init(of_.tarEnemy.ec_, spAtk);
        
        of_.beforeAtk = false;
    }
    //[Header("技能0")]
    //技能变量

    public void skill_0()//技能函数，在此实现其功能
    {
        
    }

    [Header("技能1")]
    //技能变量
    public float[] skill_1_damage_rate = new float[7];
    void skill_1_cause_dam(lifeController life_, Vector3 pos)
    {
        atk_.causeMagicDamage(atk_.atk * skill_1_damage_rate[skillLevel], life_, false);
        GameObject stewardBoom = poolManager.Steward_Boom();
        steward_skill_1 ss1 = stewardBoom.GetComponent<steward_skill_1>();
        pos.z = -1.5f;
        ss1.Init(pos, atk_, skill_1_damage_rate[skillLevel]);
    }
    public void skill_1()
    {

    }
    
    //[Header("技能2")]
    //技能变量
    private GameObject surPar;
    public float[] skill_2_damage_rate = new float[7];
    public void skill_2()
    {
        float during = od_.duration2[skillLevel];
        sp_.useSkill(during);
        anim.SetInteger("sta",1);
        Invoke(nameof(skill_2_end), during);
        
        surPar = poolManager.Surrounding_Particles();
        Vector3 initPos = transform.position;
        initPos.x += Math.Abs(transform.rotation.y) < TOLERANCE ? 0.7f : -0.7f;
        initPos.y += 0.2f;
        surPar.transform.position = initPos;
        oc_.operDie += DoWhenOperDie_Skill_2;
    }
    void skill_2_end()
    {
        surPar.SetActive(false);
        anim.SetInteger("sta", 0);
        oc_.operDie -= DoWhenOperDie_Skill_2;
    }
    void DoWhenOperDie_Skill_2(operControl oc)
    {
        surPar.SetActive(false);
    }
    public void skill_2_fire_attack()
    {
        GameObject magicFire = poolManager.Track_Magic_Fire();
        SceneTarTrack stt_ = magicFire.GetComponent<SceneTarTrack>();
        Vector3 pos = surPar.transform.position;
        pos.z = -1f;
        stt_.Init(pos, false);
        stt_.getIt += skill_2_fire_cause_dam;
    }

    void skill_2_fire_cause_dam(lifeController life_, Vector3 pos)
    {
        atk_.causeMagicDamage(atk_.atk * skill_2_damage_rate[skillLevel], life_, false);
    }
    
}
