using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buffManager : MonoBehaviour
{
    private static buffManager instance;

    [SerializeField] private pool giveDamageReduction_pool;
    [SerializeField] private pool giveDefModification_pool;
    [SerializeField] private pool giveAtkModification_pool;
    [SerializeField] private pool giveAtkSpeedModification_pool;
    [SerializeField] private pool giveBlockModification_pool;
    [SerializeField] private pool lockLife_pool;
    [SerializeField] private pool giveStagnation_pool;
    [SerializeField] private pool giveStealth_pool;
    

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this);
    }

    public static void GiveDamageReduction(lifeController life_, int priority, float v, float duration)
    {
        GameObject buff = instance.giveDamageReduction_pool.PrepareObject();
        giveDamageReduction gd_ = buff.GetComponent<giveDamageReduction>();
        gd_.Init(life_, priority, v, duration);
    }
    
    public static void GiveDefModification(defController def_, int priority,float v,float duration)
    {
        GameObject buff = instance.giveDefModification_pool.PrepareObject();
        giveDefModification gd_ = buff.GetComponent<giveDefModification>();
        gd_.Init(def_, priority, v, duration);
    }

    public static void GiveAtkModification(atkController atk_,int priority,float v,float duration)
    {
        GameObject buff = instance.giveAtkModification_pool.PrepareObject();
        giveAtkModification ga_ = buff.GetComponent<giveAtkModification>();
        ga_.Init(atk_, priority, v, duration);
    }
    
    public static void GiveAtkSpeedModification(atkSpeedController atkSpeed_,int priority,float v,float duration)
    {
        GameObject buff = instance.giveAtkSpeedModification_pool.PrepareObject();
        giveAtkSpeedModification gas_ = buff.GetComponent<giveAtkSpeedModification>();
        gas_.Init(atkSpeed_, priority, v, duration);
    }
    
    public static void GiveDizzy(operControl tar,float durTime)
    {
        giveDizzy gd_ = tar.transform.Find("debuffs").Find("giveDizzy").GetComponent<giveDizzy>();
        if (gd_ == null) return;
        gd_.durTime = Mathf.Max(durTime, gd_.durTime);
        operFight of_ = tar.of_;
        GiveBlockModification(of_, -1000000, durTime);
        tar.emoji.dizzyEmoji.SetActive(true);
    }
    
    public static void GiveDizzy(enemyControl tar,float durTime)
    {
        giveDizzy gd_ = tar.transform.Find("debuffs").Find("giveDizzy").GetComponent<giveDizzy>();
        if (gd_ == null) return;
        gd_.durTime = Mathf.Max(durTime, gd_.durTime);
        tar.emoji.dizzyEmoji.SetActive(true);
    }

    public static void GiveSlow(enemyControl tar, float speedDeta, float durTime)
    {
        giveSlow gs_ = tar.transform.Find("debuffs").Find("giveSlow").GetComponent<giveSlow>();
        if (gs_ == null) return;
        gs_.slowList.Add(new Vector2(speedDeta, durTime));
    }

    public static void GiveBlockModification(operFight of_,int deBlock, float durTime)
    {
        GameObject buff = instance.giveBlockModification_pool.PrepareObject();
        giveBlockModification gb = buff.GetComponent<giveBlockModification>();
        gb.Init(of_, deBlock, durTime);
    }

    public static void GiveLockLife(lifeController Life_, float lockLine, float duration)
    {
        GameObject buff = instance.lockLife_pool.PrepareObject();
        lockLife ll = buff.GetComponent<lockLife>();
        ll.Init(Life_,lockLine,duration);
    }

    public static void GiveFear(enemyControl ec_, Vector3 prePos, float during)
    {
        giveFear gf_ = ec_.transform.Find("debuffs").Find("giveFear").GetComponent<giveFear>();
        if (gf_ == null) return;
        gf_.Init(during, prePos);
        GiveSlow(ec_, 0.5f, during);
        ec_.emoji.fearEmoji.SetActive(true);
    }

    public static void GiveStagnation(enemyControl ec_, float during)
    {
        GameObject buff = instance.giveStagnation_pool.PrepareObject();
        giveStagnation gs_ = buff.GetComponent<giveStagnation>();
        gs_.Init(ec_,during);
    }
    
    public static void GiveStealth(enemyControl ec_)
    {
        GameObject buff = instance.giveStealth_pool.PrepareObject();
        giveStealth gs_ = buff.GetComponent<giveStealth>();
        gs_.Init(ec_);
    }
    
}
