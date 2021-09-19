using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class costController : MonoBehaviour
{
    private initManager im_;
    public float maxCostTime;
    public float costTime = 0;
    public int cost;
    
    public List<float> speedDetaList;
    private float costDeta;

    void Start()
    {
        im_ = GetComponent<initManager>();
        cost = im_.cost;
        costDeta = 1;
        maxCostTime = 1;
    }
    
    public int AddSpeedDetaList(float x)
    {
        costDeta *= x;
        speedDetaList.Add(x);
        maxCostTime = 1f / costDeta;
        return speedDetaList.Count - 1;
    }

    public void RemoveSpeedDetaList(int i)
    {
        if (i >= speedDetaList.Count) return;
        speedDetaList.RemoveAt(i);
        costDeta = 1;
        foreach (var j in speedDetaList)
        {
            costDeta *= j;
        }
        maxCostTime = 1f / costDeta;
    }

    public void GetCost(int x)
    {
        cost += x;
    }

    public void UseCost(int x)
    {
        cost -= x;
    }
    

    // Update is called once per frame
    void Update()
    {
        //cost恢复
        costTime += Time.deltaTime;
        if (costTime > maxCostTime)
        {
            costTime = 0;
            cost++;
        }
    }

    private void LateUpdate()
    {
        im_.cost = cost;
    }
}
