using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental;
using UnityEngine;

public class giveFear : MonoBehaviour
{
    private Transform ob;
    private enemyControl ec_;
    private enemyFight ef_;
    private Vector3 nxtPoint = new Vector3();
    private initManager im_;
    private float[] fx = new[] {0f, -1f, 0f, 1f};
    private float[] fy = new[] {-1f, 0f, 1f, 0f};

    public float durTime = 0;
    public Vector3 prePos = new Vector3();
    
    void Start()
    {
        ob = transform.parent.parent;
        if (ob.CompareTag("operator")) return;
        ec_ = ob.GetComponent<enemyControl>();
        ef_ = ec_.ef_;
        im_ = ec_.im_;
    }

    private int cmp(Vector3 x, Vector3 y)
    {
        if (x.z < y.z) return -1;
        else if (x.z > y.z) return 1;
        else return 0;
    }

    void GetNxtPoint()
    {
        List<Vector3> temp = new List<Vector3>();
        Vector3 standardPos = ob.transform.position;
        standardPos.x = initManager.FixCoordinate(standardPos.x);
        standardPos.y = initManager.FixCoordinate(standardPos.y);
        standardPos.z = Vector2.Distance(standardPos, prePos);;
        temp.Add(standardPos);
        
        for (int i = 0; i < 4; i++)
        {
            Vector3 newPos = standardPos;
            newPos.x += fx[i];
            newPos.y += fy[i];
            newPos.z = Vector2.Distance(newPos, prePos);
            temp.Add(newPos);
        }
        
        temp.Sort(cmp);
        
        nxtPoint = ob.transform.position;
        for (int i = temp.Count - 1; i >= 0; i--)
        {
            if (!tagManager.canPassFear(im_.getMp(temp[i]))) continue;
            nxtPoint = temp[i];
            nxtPoint.z = ob.transform.position.z;
            break;
        }
        ec_.nxtPoint = nxtPoint;
    }

    void GetNxtPointDrone()
    {
        Vector3 deta = transform.position - prePos;
        nxtPoint.x = deta.x * 100;
        nxtPoint.y = deta.y * 100;
        nxtPoint.z = transform.position.z;
        ec_.nxtPoint = nxtPoint;
    }
    
    public void Init(float dtime, Vector3 pPos)
    {
        durTime = dtime;
        prePos = pPos;
        
        if(ec_.ei_.isDrone)
            GetNxtPointDrone();
        else
            GetNxtPoint();
        
        ec_.nxtPointListValid = false;
        ef_.unBlock++;
        ef_.FightTar_Empty();
    }

    void Update()
    {
        if (durTime > 0)
        {
            durTime -= Time.deltaTime;
            if (durTime <= 0) FearEnd();
            else FearDuring();
        }
    }

    void FearDuring()
    {
        if (ob.transform.position != nxtPoint)
            return;
//******************************GetNxtPoint*******************************
        if(ec_.ei_.isDrone)
            GetNxtPointDrone();
        else
            GetNxtPoint();
//************************************************************************************
    }

    void FearEnd()
    {
        ec_.nxtPointListValid = true;
        ef_.unBlock--;
        ec_.changeRoute();
        ec_.emoji.fearEmoji.SetActive(false);
    }
    
}
