using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class animEvent : MonoBehaviour
{
    private GameObject fa;
    private operControl oc_;
    private operFight of_;
    private Animator anim;
    private Renderer rend;
    public bool dying = false;
    private static readonly int Down = Animator.StringToHash("down");
    private bool dontRetreatTwice = false;
    
    
    private void Start()
    {
        anim = GetComponent<Animator>();
        rend = GetComponent<Renderer>();
        fa = transform.parent.gameObject;
        oc_ = fa.GetComponent<operControl>();
        of_ = fa.GetComponent<operFight>();
    }

    private void Update()
    {
        if (dying)
        {/*
            oc_.ColorAndRecover(Color.black, 1, false);
            
            Color newColor = rend.material.color;
            newColor.b = newColor.b - 0.02f >= 0 ? newColor.b - 0.02f  : 0;
            newColor.g = newColor.b;
            newColor.r = newColor.b;
            rend.material.color = newColor;
            */
        }
    }

    public void appearEnd()
    {
        anim.SetBool("appearEnd",true);
        of_.appearEnd = true;
    }

    public void dieBegin()
    {
        if (dontRetreatTwice) return;
        dontRetreatTwice = true;
        dying = true;
        oc_.retreat(false,true);
        anim.SetBool("die",false);
        
        Vector3 nPos = oc_.transform.position;
        Vector3 nLPos = transform.localPosition;
        nPos.z += 100;
        nLPos.z -= 100;
        oc_.transform.position = nPos;
        transform.localPosition = nLPos;
        oc_.ColorAndRecover(Color.black, 0.3f, false);
    }

    public void idleBegin()
    {
        //ec_.fighting = false;
    }
    
    public void dieEnd()
    {
        anim.SetBool("appearEnd",false);
        Destroy(fa);
    }

    public void fightBegin()
    {
        of_.beforeAtk = true;
    }

    public void fighting()
    {
        of_.beforeAtk = false;
    }

    public void DownBegin()
    {
        anim.SetBool("down",false);
    }

}
