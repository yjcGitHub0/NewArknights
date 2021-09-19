using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hole : MonoBehaviour
{
    private atkController atk_;
    void Start()
    {
        atk_ = GetComponent<atkController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("enemy")) return;
        enemyControl ec_ = other.GetComponent<enemyControl>();
        if (ec_.ei_.isDrone) return;
        atk_.causeRealDamage(atk_.atk, ec_.life_, false);
    }
}
