using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spController : MonoBehaviour
{
    private Transform ob;
    private bool isEnemy;
    private bool isOper;
    private operControl oc_;
    private enemyControl ec_;
    private int skillNum = 0;
    
    public float sp;
    public float maxSP;
    public float maxSkillDuring = 0;//最大技能持续时间
    public float skillDuring = 0;//技能持续时间，>0时不可回复技力
    
    private float skillTime = 0;//辅助变量，用于控制自动回复
    
    public recoverType recover;


    [Header("自动恢复，每秒ato点")]
    public float atoSP;

    
    [Header("攻击恢复，每次攻击atk点")]
    public float atkSP;
    
    [Header("受击恢复，每次受击beat点")]
    public float beatSP;


    private void Start()
    {
        atoSP = atkSP = beatSP = 1;
        
        ob = transform.parent;
        if (ob == null) 
            isEnemy = isOper = false;
        else
        {
            isEnemy = ob.CompareTag("enemy");
            isOper = ob.CompareTag("operator");
        }
        if (isOper)
        {
            oc_ = ob.GetComponent<operControl>();
            skillNum = oc_.cd_.skillNum;
            
            switch (skillNum)
            {
                case 0:
                    recover = oc_.od_.skill0_recoverType;
                    break;
                case 1:
                    recover = oc_.od_.skill1_recoverType;
                    break;
                default:
                    recover = oc_.od_.skill2_recoverType;
                    break;
            }
        }
        if(isEnemy)
        {
            ec_ = ob.GetComponent<enemyControl>();
            recover = ec_.ei_.skill_recoverType;
            sp = ec_.ei_.startSP;
            maxSP = ec_.ei_.maxSP;
        }
        
    }

    void Update()
    {
        if (maxSkillDuring > 0)
        {
            if (skillDuring <= 0)
            {
                skillDuring = 0;
                maxSkillDuring = 0;
                skillTime = 0;
            }
            else
            {
                skillDuring -= Time.deltaTime;
                return;
            }
        }
        skillTime += Time.deltaTime;
        if (recover == recoverType.auto && skillTime >= 1 && sp + atoSP <= maxSP)
        {
            sp += atoSP;
            skillTime = 0;
        }
    }

    public void useSkill(float duringTime)
    {
        sp = 0;
        maxSkillDuring = duringTime;
        skillDuring = duringTime;
    }

    public void GetAtk()
    {
        if (recover != recoverType.atk) return;
        if (sp + atkSP <= maxSP) sp += atkSP;
    }

    public void GetBeat()
    {
        if (recover != recoverType.beAtk) return;
        if (sp + beatSP <= maxSP) sp += beatSP;
    }
    
}
