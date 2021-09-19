using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using Random = UnityEngine.Random;

public class SceneTarTrack : MonoBehaviour
{
    private Transform tarTransform;
    private Transform m_trans;
    private Quaternion tarRol = new Quaternion();
    private Quaternion nowRol = new Quaternion();
    private bool tarIsNull;
    private initManager im_;
    private float speed = 12;
    private float rolSpeed = 1f;
    private float min_distance = 0.2f;
    private enemyControl tarEC;
    private bool defFirst;

    public delegate void WhatWillDoWhenTrackGetIt(lifeController life_, Vector3 pos);
    public event WhatWillDoWhenTrackGetIt getIt;


    public void Init(Vector3 pos, bool DefFirst)
    {
        tarIsNull = true;
        defFirst = DefFirst;
        im_ = gameManager.im_;
        m_trans = transform;
        m_trans.position = pos;
        m_trans.localEulerAngles += new Vector3(0, 0, Random.value * 360f);
        getIt -= getIt;
    }

    void Update()
    {
        if (tarIsNull)
        {
            if (im_.sceneCanAtkEnemyList.Count == 0)
            {
                Invalid();
            }
            else
            {
                GetTar();
            }
        }

        if (tarIsNull) return;
        nowRol = m_trans.rotation;
        m_trans.LookAt(tarTransform);
        tarRol = m_trans.rotation;
        m_trans.rotation = Quaternion.RotateTowards(nowRol, tarRol, rolSpeed);


        float currentDist = Vector2.Distance(m_trans.position, tarTransform.position);
        // 很接近目标了, 准备结束循环
        if (currentDist < min_distance)
        {
            if (getIt != null) getIt(tarEC.life_, tarTransform.position);
            Invalid();
        }
        m_trans.Translate(Vector3.forward * Mathf.Min(speed * Time.deltaTime, currentDist));
        Vector3 pos = m_trans.position;
        pos.z = -1f;
        m_trans.position = pos;
    }


    public void DoWhenTarNull(enemyControl ec_)
    {
        tarIsNull = true;
    }

    void GetTar()
    {
        if (im_.sceneCanAtkEnemyList.Count == 0)
        {
            Invalid();
            return;
        }
        int id = 0;
        if (defFirst)
        {
            float maxDef = im_.sceneCanAtkEnemyList[0].def_.def;
            for (int i = 1; i < im_.sceneCanAtkEnemyList.Count; i++)
            {
                if (im_.sceneCanAtkEnemyList[i].def_.def > maxDef)
                {
                    maxDef = im_.sceneCanAtkEnemyList[i].def_.def;
                    id = i;
                }
            }
        }
        else
        {
            id = Random.Range(0, im_.sceneCanAtkEnemyList.Count);
        }
        tarEC = im_.sceneCanAtkEnemyList[id];
        tarTransform = tarEC.transform;
        tarIsNull = false;
        tarEC.enemyDie += DoWhenTarNull;
    }
    
    void Invalid()
    {
        tarIsNull = true;
        gameObject.SetActive(false);
    }
}
