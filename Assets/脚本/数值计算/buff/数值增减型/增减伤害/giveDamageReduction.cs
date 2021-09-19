using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class giveDamageReduction : MonoBehaviour
{
    private lifeController life_;
    private NumericalModification dr = new NumericalModification();


    [Header("priority=0:固定增伤，最后结算，增伤幅度为v(一个固定的数)")]
    [Header("priority=10:百分比增伤，增伤幅度为v(处于(0,无穷]之间的数)")]
    
    private int priority;
    private float v;
    private float duration;
    
    
    void Start()
    {
        
    }

    public void Init(lifeController Life_, int Priority, float V, float Duration)
    {
        life_ = Life_;
        priority = Priority;
        v = V;
        duration = Duration;
        
        dr.ancestor = gameObject;
        dr.priority = priority;
        dr.v = v;
        life_.AddNewDamageReduction(dr);
    }

    private void Update()
    {
        duration -= Time.deltaTime;
        if (duration <= 0)
            Invalid();
    }

    public void Invalid()
    {
        life_.RemoveDamageReduction(dr);
       gameObject.SetActive(false);
    }
    
}
