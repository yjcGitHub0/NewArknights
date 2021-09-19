using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class expController : MonoBehaviour
{
    private initManager im_;
    public int exp;
    
    void Start()
    {
        im_ = GetComponent<initManager>();
        exp = im_.exp;
    }

    public void GetExp(int x)
    {
        exp += x;
    }
    public void UseExp(int x)
    {
        exp -= x;
    }
    
    void LateUpdate()
    {
        im_.exp = exp;
    }
}
