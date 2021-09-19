using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBullet : MonoBehaviour
{
    public float speed;
    public operFight of_;
    public operControl oc_;
    private bool tarIsNull;
    private Vector3 tarPos = new Vector3();
    private enemyControl tarEC;
    private bool spAtk;
    
    // 最小接近距离, 以停止运动
    public float min_distance = 0.2f;
    private bool move_flag = true;
    private Transform m_trans;

    public void Init(enemyControl TarEC, bool SpAtk)
    {
        move_flag = true;
        tarEC = TarEC;
        spAtk = SpAtk;
        if (TarEC == null)
        {
            tarIsNull = true;
            tarPos = of_.tarPos;
        }
        else
        {
            m_trans = transform;
            tarIsNull = false;
            tarEC.enemyDie += TarNull;
        }
    }
    void Update()
    {
        if (!tarIsNull)
            tarPos = tarEC.transform.position;
        m_trans.LookAt(tarPos);
        float currentDist = Vector2.Distance(m_trans.position, tarEC.transform.position);
        m_trans.Translate(Vector3.forward * Mathf.Min(speed * Time.deltaTime, currentDist));
        if (currentDist < min_distance)
        {
            oc_.atk_.causeMagicDamage(oc_.atk_.atk, tarEC.life_, spAtk);
            //buffManager.GiveDizzy(ec_,1.2f);
            m_trans.position = tarEC.transform.position;
            tarEC.enemyDie -= TarNull;
            gameObject.SetActive(false);
        }
    }
    
    private void TarNull(enemyControl ec)
    {
        tarIsNull = true;
    }
}
