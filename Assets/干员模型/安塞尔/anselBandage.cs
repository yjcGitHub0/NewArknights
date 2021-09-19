using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class anselBandage : MonoBehaviour
{
    public float speed;
    public operFight of_;
    public operControl oc_;
    private bool tarIsNull;
    private Vector3 tarPos = new Vector3();

    public Transform target_trans;
    // 最小接近距离, 以停止运动
    public float min_distance = 0.2f;
    private float distanceToTarget;
    private bool move_flag = true;
    private Transform m_trans;
    private operControl tarOC;
    private float addPer;

    public void Init(operControl TarOC, float AddPer)
    {
        move_flag = true;
        tarOC = TarOC;
        addPer = AddPer;
        if (TarOC == null)
        {
            gameObject.SetActive(false);
            return;
        }
        else
        {
            target_trans = TarOC.transform;
            m_trans = transform;
            distanceToTarget = Vector3.Distance(m_trans.position, target_trans.position);
            tarIsNull = false;
            StartCoroutine(Parabola());
        }
    }

    IEnumerator Parabola()
    {
        while (move_flag)
        {
            if (!tarIsNull && target_trans == null)
                tarIsNull = true;
            if (!tarIsNull)
                tarPos = target_trans.position;
            // 朝向目标, 以计算运动
            m_trans.LookAt(tarPos);
            // 根据距离衰减 角度
            float angle = Mathf.Min(1, Vector3.Distance(m_trans.position, tarPos) / distanceToTarget) * 30;
            // 旋转对应的角度（线性插值一定角度，然后每帧绕X轴旋转）
            m_trans.rotation = m_trans.rotation * Quaternion.Euler(Mathf.Clamp(-angle, -42, 42), 0, 0);
            // 当前距离目标点
            float currentDist = Vector3.Distance(m_trans.position, tarPos);
            // 很接近目标了, 准备结束循环
            if (currentDist < min_distance)
            {
                move_flag = false; 
            }
            // 平移 (朝向Z轴移动)
            m_trans.Translate(Vector3.forward * Mathf.Min(speed * Time.deltaTime, currentDist));
            yield return null;
        }
        if (move_flag == false)
        {
            if (tarIsNull)
            {
                StopCoroutine(Parabola());
                gameObject.SetActive(false);
            }

            float heal = oc_.atk_.atk + tarOC.life_.life * addPer;
            oc_.atk_.causeHeal(heal, tarOC.life_, true);

            m_trans.position = target_trans.position;
            StopCoroutine(Parabola());
            gameObject.SetActive(false);
        }
    }
}
