using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class specialEvent : MonoBehaviour
{
    private initManager im_;

    private void Awake()
    {
        im_ = GameObject.Find("initManager").GetComponent<initManager>();
    }

    /*
     *
     * 
     */
    [Header("放置时将该块地形改变为“box”")] 
    public bool isWall;
    public bool isWallFunction()
    {
        bool allFound = true;
        im_.setMp(transform.position,4);
        foreach (var i in im_.ECList)
        {
            foreach (var j in i)
            {
                if (!j.gameObject.activeSelf) continue;
                allFound = allFound & j.changeRoute();
            }
        }

        foreach (var i in im_.blueDoorPathList)
        {
            allFound = allFound & i.FindPath();
        }

        return allFound;
    }
    public void reWallFunction()
    {
        im_.resetMp(transform.position);
        foreach (var i in im_.ECList)
        {
            foreach (var j in i)
            {
                if (!j.gameObject.activeSelf) continue;
                j.changeRoute();
            }
        }
    }
    
    
}
