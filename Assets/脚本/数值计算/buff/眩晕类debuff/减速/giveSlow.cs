using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class giveSlow : MonoBehaviour
{
    private Transform ob;
    private float speedDeta;
    private enemyControl ec_;
    private bool isEnemy;
    public List<Vector2> slowList = new List<Vector2>();
    
    private int cmp(Vector2 x, Vector2 y)
    {
        if (x.x < y.x) return -1;
        else if (x.x > y.x) return 1;
        else return 0;
    }
    
    void Start()
    {
        ob = transform.parent.parent;
        isEnemy = ob.CompareTag("enemy");
        if (!isEnemy) return;
        ec_ = ob.GetComponent<enemyControl>();
        speedDeta = 1;
    }
    
    void Update()
    {
        speedDeta = 1;
        
        for (int i = 0; i < slowList.Count; i++)
        {
            Vector2 sl = slowList[i];
            if (sl.y <= 0)
            {
                slowList.RemoveAt(i);
                i--;
            }
            else
            {
                speedDeta = Math.Min(speedDeta, sl.x);
                sl.y -= Time.deltaTime;
                slowList[i] = sl;
            }
        }

        if (isEnemy) ec_.speedDeta = speedDeta;
    }
}
