using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class giveDizzy : MonoBehaviour
{
    private Animator anim;
    private Transform obj;
    public float durTime = 0;
    
    private enemyControl ec_;
    private enemyFight ef_;
    
    private operControl oc_;
    private operFight of_;

    private lifeController life_;
    private int downEnd = 0;
    private bool isOper;


    private void Start()
    {
        obj = transform.parent.parent;
        anim = obj.Find("anim").GetComponent<Animator>();
        isOper = obj.CompareTag("operator");
        if (isOper)
        {
            oc_ = obj.GetComponent<operControl>();
            of_ = oc_.of_;
            life_ = oc_.life_;
        }
        else
        {
            ec_ = obj.GetComponent<enemyControl>();
            ef_ = ec_.ef_;
            life_ = ec_.life_;
        }
    }

    private void Update()
    {
        if (durTime > 0)
        {
            durTime -= Time.deltaTime;
            if (durTime <= 0) DizzyEnd();
            else DizzyDuring();
        }
    }

    void DizzyDuring()
    {
        if (downEnd == 0 && life_.life != 1e9)
        {
            anim.SetBool("down", true);
            downEnd++;
            anim.SetInteger("downEnd", downEnd);
            if (!isOper)
            {
                ec_.pauseAnim++;
                ef_.isDizzy = true;
                ef_.FightTar_Empty();
            }
        }
    }

    void DizzyEnd()
    {
        if (downEnd > 0)
        {
            downEnd--;
            anim.SetInteger("downEnd", downEnd);
            if (!isOper)
            {
                ef_.isDizzy = false;
                ec_.pauseAnim--;
                ec_.emoji.dizzyEmoji.SetActive(false);
            }
            else
            {
                oc_.emoji.dizzyEmoji.SetActive(false);
            }
        }
    }
    
}
