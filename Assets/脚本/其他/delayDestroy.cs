using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class delayDestroy : MonoBehaviour
{
    public bool willDestory;
    public float delay;
    private float t;
    
    private void OnEnable()
    {
        t = delay;
    }

    private void Update()
    {
        t -= Time.deltaTime;
        if (t <= 0)
        {
            if (willDestory)
                Destroy(gameObject);
            else
                gameObject.SetActive(false);
            t = 1e9f;
        }
    }
}
