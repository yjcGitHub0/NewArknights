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
        for (int i = 0; i < im_.ECList.Count; i++)
        {
            if (im_.ECList[i] == null)
            {
                im_.ECList.RemoveAt(i);
                i--;
            }
        }
        foreach (var i in im_.ECList)
        {
            if (!i.gameObject.activeSelf) continue;
            allFound = allFound & i.changeRoute();
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
        for (int i = 0; i < im_.ECList.Count; i++)
        {
            if (im_.ECList[i] == null)
            {
                im_.ECList.RemoveAt(i);
                i--;
            }
            else
            {
                if (!im_.ECList[i].gameObject.activeSelf) continue;
                im_.ECList[i].changeRoute();
            }
        }
    }
    
    
}
