using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class lifeController : MonoBehaviour
{
    private float baseMaxLife;
    public float maxLife;//最大生命
    public float life;//当前生命
    
    public defController def_;
    private spController sp_;
    
    //buff
    private List<NumericalModification> finalDamageList = new List<NumericalModification>();
    private List<NumericalModification> finalHealList = new List<NumericalModification>();
    private float lockLine = 0;
    
    //被治疗时触发
    public Action Healing;
    
    private void Start()
    {
        sp_ = GetComponent<spController>();
    }
    
    
    public void ChangeBaseLife(float BaseMaxLife)
    {
        baseMaxLife = BaseMaxLife;
        float proportion = maxLife == 0 ? 1 : life / maxLife;
        maxLife = baseMaxLife;
        life = maxLife * proportion;
    }

    public void getPhyAtk(float x)
    {//受到x点物理伤害
        sp_.GetBeat();
        x -= def_.def;
        x = ChangeDamageByEveryBuff(x);
        life = life - x < lockLine ? lockLine : life - x;
    }
    
    public void getMagicAtk(float x)
    {//受到x点法术伤害
        sp_.GetBeat();
        x = x * ((100f - def_.magicDef) / 100f);
        x = ChangeDamageByEveryBuff(x);
        life = life - x < lockLine ? lockLine : life - x;
    }
    
    public void getRealAtk(float x)
    {//受到x点真实伤害
        sp_.GetBeat();
        x = ChangeDamageByEveryBuff(x);
        life = life - x < lockLine ? lockLine : life - x;
    }

    public void getHeal(float x)
    {//受到x点治疗
        x = ChangeHeal(x);
        life = life + x > maxLife ? maxLife : life + x;
        if (Healing != null) Healing();
    }
    
    float ChangeDamageByEveryBuff(float x)
    {//计算最终减伤
        foreach (var i in finalDamageList)
        {
            if (i.priority == 10)
            {
                x = x * i.v;
            }
            else if (i.priority == 0)
            {
                x = x + i.v;
            }
        }
        if (x < 0) x = 0;
        return x;
    }
    
    float ChangeHeal(float x)
    {//计算最终治疗量
        foreach (var i in finalHealList)
        {
            if (i.priority == 10)
            {
                x = x * i.v;
            }
            else if (i.priority == 0)
            {
                x = x + i.v;
            }
        }
        if (x < 0) x = 0;
        return x;
    }
    
    public void AddNewDamageReduction(NumericalModification dd)
    {
        finalDamageList.Add(dd);
        //降序排序
        finalDamageList.Sort((x, y) => -x.v.CompareTo(y.v));
    }
    public void RemoveDamageReduction(NumericalModification dd)
    {
        finalDamageList.Remove(dd);
    }

    public void AddNewHealReduction(NumericalModification hd)
    {
        finalHealList.Add(hd);
        //降序排序
        finalHealList.Sort((x, y) => -x.v.CompareTo(y.v));
    }
    public void RemoveHealReduction(NumericalModification hd)
    {
        finalHealList.Remove(hd);
    }
    
    public void SetLockLine(float x)
    {
        lockLine = x;
    }

}
