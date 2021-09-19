using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class giveStagnation : MonoBehaviour
{
    public enemyControl ec_;
    private Animator anim;
    private initManager im_;
    private bool imPreExist;
    private SpriteRenderer sr_;
    public float during;
    private Vector3 preObPos;
    private Vector3 preAnimLocalPos;

    public void Init(enemyControl EC, float During)
    {
        ec_ = EC;
        during = During;
        anim = ec_.anim;
        im_ = ec_.im_;
        imPreExist = im_.sceneCanAtkEnemyList.Contains(ec_);
        if(imPreExist)
            im_.sceneCanAtkEnemyList.Remove(ec_);
        sr_ = anim.GetComponent<SpriteRenderer>();
        preObPos = ec_.transform.position;
        preAnimLocalPos = anim.transform.localPosition;
        
        Vector3 nPos = preObPos;
        Vector3 nLPos = preAnimLocalPos;
        nPos.z += 100;
        nLPos.z -= 100;
        ec_.transform.position = nPos;
        anim.transform.localPosition = nLPos;
        ec_.isStagnation = true;

        Color c = new Color(1, 0.4f, 0, 0.5f);
        sr_.color = c;
    }
    
    void Update()
    {
        if (during > 0)
        {
            during -= Time.deltaTime;
            if (during <= 0) Invalid();
        }
    }

    void Invalid()
    {
        ec_.transform.position = preObPos;
        anim.transform.localPosition = preAnimLocalPos;
        ec_.isStagnation = false;
        sr_.color = Color.white;
        if (imPreExist)
            im_.sceneCanAtkEnemyList.Add(ec_);
        gameObject.SetActive(false);
    }
    
}
