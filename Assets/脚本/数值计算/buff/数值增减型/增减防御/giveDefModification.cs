using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class giveDefModification : MonoBehaviour
{
    private defController def_;
    private NumericalModification dr = new NumericalModification();


    [Header("priority=0:固定增防，最后结算，增防幅度为v(一个固定的数)")]
    [Header("priority=10:百分比增防，增防幅度为v(处于(0,无穷]之间的数)")]
    
    private int priority;
    private float v;
    private float duration;
    
    
    public void Init(defController Def_, int Priority, float V, float Duration)
    {
        def_ = Def_;
        priority = Priority;
        v = V;
        duration = Duration;
        
        dr.ancestor = gameObject;
        dr.priority = priority;
        dr.v = v;
        def_.AddNewDefModification(dr);
    }

    private void Update()
    {
        duration -= Time.deltaTime;
        if (duration <= 0)
            Invalid();
    }

    public void Invalid()
    {
        def_.RemoveDefModification(dr);
        gameObject.SetActive(false);
    }
}
