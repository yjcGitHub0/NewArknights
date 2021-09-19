using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class enemyAnimEvent : MonoBehaviour
{
    private initManager im_;
    
    private Animator anim;
    private GameObject fa;
    private enemyControl ec_;
    private enemyFight ef_;
    private enemyInfo ei_;
    private Renderer rend;
    public bool dying = false;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rend = GetComponent<Renderer>();
        fa = transform.parent.gameObject;
        ec_ = fa.GetComponent<enemyControl>();
        ef_ = fa.GetComponent<enemyFight>();
        ei_ = ec_.ei_;

        im_ = ec_.im_;
    }
    
    private void Update()
    {
        if (dying)
        {
            Color newColor = rend.material.color;
            newColor.b = newColor.b - 0.02f >= 0 ? newColor.b - 0.02f  : 0;
            newColor.g = newColor.b;
            newColor.r = newColor.b;
            rend.material.color = newColor;
        }
    }
    
    public void fightBegin()
    {
        if (!ef_.blocking && ef_.atkRangeListOC.Count != 0)
        {
            ef_.ChangeFightTarByAtkRangeList();
        }
        ec_.fighting = true;
        anim.SetBool("fight", false);
    }

    public void fighting()
    {
        ef_.alreadyFight = true;
    }
    
    public void idleBegin()
    {
        ec_.fighting = false;
    }

    public void dieBegin()
    {
        dying = true;
        anim.SetBool("die", false);
        ef_.dieExit();
        Vector3 nPos = ec_.transform.position;
        Vector3 nLPos = transform.localPosition;
        nPos.z += 100;
        nLPos.z -= 100;
        ec_.transform.position = nPos;
        transform.localPosition = nLPos;
    }

    public void becomeBlack()
    {
        dying = true;
    }
    
    public void dieEnd()
    {
        im_.defeatEnemy++;
        Destroy(fa);
    }

    public void DownBegin()
    {
        anim.SetBool("down",false);
    }
    
}
