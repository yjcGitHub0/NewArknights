using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class giveAtkModification : MonoBehaviour
{
    private atkController atk_;
    private NumericalModification ar = new NumericalModification();


    [Header("priority=0:固定增防，最后结算，增防幅度为v(一个固定的数)")]
    [Header("priority=10:百分比增防，增防幅度为v(处于(0,无穷]之间的数)")]
    
    private int priority;
    private float v;
    private float duration;


    public void Init(atkController Atk_, int Priority, float V, float Duration)
    {
        atk_ = Atk_;
        priority = Priority;
        v = V;
        duration = Duration;
        
        ar.ancestor = gameObject;
        ar.priority = priority;
        ar.v = v;
        atk_.AddNewAtkModification(ar);
    }

    private void Update()
    {
        duration -= Time.deltaTime;
        if (duration <= 0)
            Invalid(true);
    }

    public void Invalid(bool willActiveFalse)
    {
        atk_.RemoveAtkModification(ar);
        if (willActiveFalse)
            gameObject.SetActive(false);
    }
}
