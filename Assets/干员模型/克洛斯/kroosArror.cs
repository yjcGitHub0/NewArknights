using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class kroosArror : MonoBehaviour
{
    public float speed;
    public operFight of_;
    public operControl oc_;
    private Quaternion rol;
    private bool tarIsNull;
    private Vector3 tarPos = new Vector3();
    private enemyControl tarEC;
    private float speedDownDur;
    private float speedDownRate;

    public Transform target_trans;
    // 最小接近距离, 以停止运动
    public float min_distance = 0.2f;
    private float distanceToTarget;
    private bool move_flag = true;
    private bool initFinish = false;
    private Transform m_trans;
    private bool isCrit = false;
    private List<enemyControl> InRangeList = new List<enemyControl>();

    public void Init(bool IsCrit, float SpeedDownDur, float SpeedDownRate)
    {
        InRangeList.Clear();
        move_flag = true;
        isCrit = IsCrit;
        speedDownDur = SpeedDownDur;
        speedDownRate = SpeedDownRate;
        if (of_.tarEnemyIsNull)
        {
            gameObject.SetActive(false);
        }
        else
        {
            tarEC = of_.tarEnemy.ec_;
            target_trans = tarEC.transform;
            m_trans = transform;
            distanceToTarget = Vector3.Distance(m_trans.position, target_trans.position);
            tarIsNull = false;
            tarEC.enemyDie += TarNull;
        }
        initFinish = true;
    }

    private void Update()
    {
        if (!initFinish) return;
        if (move_flag)
        {
            if (!tarIsNull)
                tarPos = target_trans.position;
            // 朝向目标, 以计算运动
            m_trans.LookAt(tarPos);
            // 根据距离衰减 角度
            float angle = Mathf.Min(1, Vector3.Distance(m_trans.position, tarPos) / distanceToTarget) * 30;
            // 旋转对应的角度（线性插值一定角度，然后每帧绕X轴旋转）
            m_trans.rotation = m_trans.rotation * Quaternion.Euler(Mathf.Clamp(-angle, -42, 42), 0, 0);
            // 当前距离目标点
            float currentDist = Vector2.Distance(m_trans.position, tarPos);
            // 很接近目标了, 准备结束循环
            if (currentDist < min_distance)
            {
                move_flag = false; 
            }
            // 平移 (朝向Z轴移动)
            m_trans.Translate(Vector3.forward * Mathf.Min(speed * Time.deltaTime, currentDist));
        }
        if (move_flag == false)
        {
            if (tarIsNull)
            {
                gameObject.SetActive(false);
            }
            if (!isCrit)
            {//箭未暴击
                GameObject boom = poolManager.kroos_norBoom();
                follow f_ = boom.GetComponent<follow>();
                f_.Init(target_trans);
                oc_.atk_.causePhyDamage(oc_.atk_.atk, tarEC.life_, true);
                if (speedDownDur != 0)
                    buffManager.GiveSlow(tarEC, speedDownRate, speedDownDur);
            }
            else
            {
                GameObject boom = poolManager.kroos_Crit_yellow();
                follow f_ = boom.GetComponent<follow>();
                f_.Init(tarEC.anim.transform);

                oc_.atk_.causePhyDamage(2 * oc_.atk_.atk, tarEC.life_, true);
                foreach (var i in InRangeList)
                {
                    if (speedDownDur != 0)
                        buffManager.GiveSlow(i, speedDownRate, speedDownDur);
                    if (i == tarEC) continue;
                    oc_.atk_.causePhyDamage(oc_.atk_.atk, i.life_, false);
                }
            }

            m_trans.position = target_trans.position;
            if (!tarIsNull)
                tarEC.enemyDie -= TarNull;
            initFinish = false;
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isCrit || !other.CompareTag("enemy")) return;
        enemyControl ec = other.GetComponent<enemyControl>();
        InRangeList.Add(ec);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("enemy")) return;
        enemyControl ec = other.GetComponent<enemyControl>();
        InRangeList.Remove(ec);
    }

    private void TarNull(enemyControl ec)
    {
        
        tarIsNull = true;
        if (InRangeList.Contains(ec))
            InRangeList.Remove(ec);
    }
}
