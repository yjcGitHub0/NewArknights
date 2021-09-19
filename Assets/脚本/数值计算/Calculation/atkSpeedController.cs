using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class atkSpeedController : MonoBehaviour
{
    private float atkSpeed = 1;
    private List<NumericalModification> atkSpeedList = new List<NumericalModification>();
    private Animator anim;

    private void Start()
    {
        anim = transform.parent.Find("anim").GetComponent<Animator>();
    }

    void buffAtkSpeed()
    {
        atkSpeed = 1;
        foreach (var i in atkSpeedList)
        {
            if (i.priority == 10) atkSpeed = atkSpeed * i.v;
            else if (i.priority == 0) atkSpeed = atkSpeed + i.v;
        }
        anim.speed = atkSpeed;
    }
    
    public void AddNewAtkSpeedModification(NumericalModification dd)
    {
        atkSpeedList.Add(dd);
        //降序排序
        atkSpeedList.Sort((x, y) => -x.v.CompareTo(y.v));
        buffAtkSpeed();
    }
    public void RemoveAtkSpeedModification(NumericalModification dd)
    {
        atkSpeedList.Remove(dd);
        buffAtkSpeed();
    }
}
