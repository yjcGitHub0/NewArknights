using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class steward_skill_1 : MonoBehaviour
{
    private atkController atk_;
    private float rate;
    private List<enemyControl> ECList = new List<enemyControl>();
    private bool afterAtk;
    private int temp;
    private float t;

    private void OnEnable()
    {
        t = 1.2f;
    }

    public void Init(Vector3 pos, atkController Atk_, float Rate)
    {
        transform.position = pos;
        atk_ = Atk_;
        rate = Rate;
        temp = 5;
        t = 1.2f;
    }

    private void Update()
    {
        temp--;
        if (temp <= 0)
        {
            attack();
        }
        t -= Time.deltaTime;
        if (t <= 0)
        {
            Invalid();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (afterAtk || !other.CompareTag("enemy")) return;
        enemyControl ec_ = other.GetComponent<enemyControl>();
        ECList.Add(ec_);
        ec_.enemyDie += DoWhenEnemyDie;
    }

    private void attack()
    {
        foreach (var i in ECList)
        {
            atk_.causeMagicDamage(atk_.atk * rate, i.life_, false);
            i.enemyDie -= DoWhenEnemyDie;
        }
        afterAtk = true;
    }

    private void Invalid()
    {
        ECList.Clear();
        afterAtk = false;
        gameObject.SetActive(false);
    }

    void DoWhenEnemyDie(enemyControl ec)
    {
        ECList.Remove(ec);
    }
    
}
