using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class giveStealth : MonoBehaviour
{
    private enemyControl ec_;
    private enemyFight ef_;
    private Animator anim;
    private initManager im_;
    private SpriteRenderer animRender;
    private bool willGrow;
    private float ASpeed = 0.005f;
    private bool initFinish;
    private bool lastBlocking;

    public void Init(enemyControl EC)
    {
        ec_ = EC;
        ef_ = ec_.ef_;
        ef_.isStealth = true;
        anim = ec_.anim;
        im_ = ec_.im_;
        im_.sceneCanAtkEnemyList.Remove(ec_);
        animRender = anim.transform.GetComponent<SpriteRenderer>();
        
        Color color = animRender.color;
        color.a = 0.6f;
        animRender.color = color;
        willGrow = false;

        ec_.enemyDie += Invalid;
        initFinish = true;
    }
    
    
    void Update()
    {
        if (!initFinish) return;
        Color color = animRender.color;

        if (!lastBlocking && ef_.blocking)
        {
            color.a = 1;
            animRender.color = color;
            ef_.isStealth = false;
            lastBlocking = true;
            im_.sceneCanAtkEnemyList.Add(ec_);
            return;
        }

        if (lastBlocking && !ef_.blocking)
        {
            color.a = 0.6f;
            animRender.color = color;
            willGrow = false;
            ef_.isStealth = true;
            lastBlocking = false;
            im_.sceneCanAtkEnemyList.Remove(ec_);
            return;
        }
        
        if (lastBlocking) return;
        
        if (willGrow) color.a = color.a + ASpeed > 0.6f ? 0.6f : color.a + ASpeed;
        else color.a = color.a - ASpeed < 0 ? 0 : color.a - ASpeed;

        if (color.a == 0.6f) willGrow = false;
        if (color.a == 0) willGrow = true;

        animRender.color = color;
        lastBlocking = ef_.blocking;
    }

    public void Invalid(enemyControl ec)
    {
        initFinish = false;
        gameObject.SetActive(false);
    }
    
}
