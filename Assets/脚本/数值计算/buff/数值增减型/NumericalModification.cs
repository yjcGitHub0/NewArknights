using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumericalModification 
{
    //注意，数值改变默认均为增加，若减少则需改为负数等
    
    /*
     最终伤害增加类：
     priority:
     0:固定增伤，最后结算，增伤幅度为v(一个固定的数)
     10:百分比增伤，增伤幅度为v(处于(0,1]之间的数)
    */
    
    public GameObject ancestor;//承载该buff的物体
    public int priority;
    public float v;
}
