using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class currentData : MonoBehaviour
{
    public int num;//干员剩余可放置数量
    public int cost;//干员当前放置消耗的cost
    public float baseMaxLife;//干员当前最大生命值
    public float baseAtk;//干员当前基础攻击力
    public float baseDef;//干员当前基础防御力
    public float baseMagicDef;//干员当前基础法抗
    public int baseBlock;//干员当前最大阻挡数
    public int[] maxLevel = new int[3];//各精英阶段的最大等级
    
    public int skillNum;//干员当前选择的技能编号
    
    public int level;
    public int elitismLevel;
    public int[] skillLevel = new int[3];

    public GameObject slot;
    public Drag dr_;
    public operData od_;


    private void Start()
    {
        if (!transform.parent.CompareTag("operator") && !transform.parent.CompareTag("box"))
        {
            slot = transform.parent.gameObject;
            dr_ = slot.GetComponent<Drag>();
            od_ = dr_.thisOper;
        }
        cost = od_.cost;
        level = elitismLevel = 0;
        baseAtk = od_.atk;
        baseDef = od_.def;
        baseMagicDef = od_.magicDef;
        baseBlock = od_.maxBlock;
        baseMaxLife = od_.life;
        maxLevel[0] = od_.maxLevel[0];maxLevel[1] = od_.maxLevel[1]; maxLevel[2] = od_.maxLevel[2];
        if (!transform.parent.CompareTag("operator") && !transform.parent.CompareTag("box"))
        {
            dr_.changeText();
        }
    }

    public void LevelUP(int x)
    {
        level += x;
        baseAtk += x * od_.growingAtk[elitismLevel];
        baseDef += x * od_.growingDef[elitismLevel];
        baseMaxLife += x * od_.growingLife[elitismLevel];
        
    }
    
}
