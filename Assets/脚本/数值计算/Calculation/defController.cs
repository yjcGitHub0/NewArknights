using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class defController : MonoBehaviour
{
    private float baseDef;
    private float baseMagicDef;
    public float def;//防御
    public float magicDef;//法抗

    private List<NumericalModification> defList = new List<NumericalModification>();
    private List<NumericalModification> magicDefList = new List<NumericalModification>();

    

    public void ChangeBaseDef(float BaseDef, float BaseMagicDef)
    {
        baseDef = BaseDef;
        baseMagicDef = BaseMagicDef;
        buffDef();
        buffMagicDef();
    }

    void buffDef()
    {
        def = baseDef;
        foreach (var i in defList)
        {
            if (i.priority == 10) def = def * i.v;
            else if (i.priority == 0) def = def + i.v;
        }
    }
    void buffMagicDef()
    {
        magicDef = baseMagicDef;
        foreach (var i in magicDefList)
        {
            if (i.priority == 10) magicDef = magicDef * i.v;
            else if (i.priority == 0) magicDef = magicDef + i.v;
        }
    }
    
    public void AddNewDefModification(NumericalModification dd)
    {
        defList.Add(dd);
        //降序排序
        defList.Sort((x, y) => -x.v.CompareTo(y.v));
        buffDef();
    }
    public void RemoveDefModification(NumericalModification dd)
    {
        defList.Remove(dd);
        buffDef();
    }
    
}
