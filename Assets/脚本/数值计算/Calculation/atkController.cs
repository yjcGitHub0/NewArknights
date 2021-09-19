using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class atkController : MonoBehaviour
{
    private float baseAtk;
    public float atk;//攻击
    
    private List<NumericalModification> atkList = new List<NumericalModification>();
    
    private spController sp_;
    private void Start()
    {
        sp_ = GetComponent<spController>();
    }

    public void ChangeBaseAtk(float BaseAtk)
    {
        baseAtk = BaseAtk;
        atk = baseAtk;
        buffAtk();
    }
    void buffAtk()
    {
        atk = baseAtk;
        foreach (var i in atkList)
        {
            if (i.priority == 10) atk = atk * i.v;
            else if (i.priority == 0) atk = atk + i.v;
        }
    }

    public void causePhyDamage(float x, lifeController tarLife, bool atkSp)
    {
        if (atkSp) sp_.GetAtk();
        tarLife.getPhyAtk(x);
    }
    public void causeMagicDamage(float x, lifeController tarLife, bool atkSp)
    {
        if (atkSp) sp_.GetAtk();
        tarLife.getMagicAtk(x);
    }
    public void causeRealDamage(float x, lifeController tarLife, bool atkSp)
    {
        if (atkSp) sp_.GetAtk();
        tarLife.getRealAtk(x);
    }
    public void causeHeal(float x, lifeController tarLife, bool atkSp)
    {
        if (atkSp) sp_.GetAtk();
        tarLife.getHeal(x);
    }

    public void AddNewAtkModification(NumericalModification dd)
    {
        atkList.Add(dd);
        //降序排序
        atkList.Sort((x, y) => -x.v.CompareTo(y.v));
        buffAtk();
    }
    public void RemoveAtkModification(NumericalModification dd)
    {
        atkList.Remove(dd);
        buffAtk();
    }
    
}
