using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class noirc_skill_1 : MonoBehaviour
{
    public float delay;
    private float t;

    public float during;
    
    private void Awake()
    {
        t = delay;
    }

    // Update is called once per frame
    void Update()
    {
        if (t > 0) t -= Time.deltaTime;
        else
        {
            t = delay;
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("enemy")) return;
        enemyControl ec_ = other.GetComponent<enemyControl>();
        buffManager.GiveFear(ec_, transform.position, during);
    }
}
