using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axing : MonoBehaviour
{

    private float[] dx = new float[] {1, -1, 0, 0};
    private float[] dy = new float[] {0, 0, 1, -1};
    
    private enemyControl ec_;

    private GameObject initManager;
    private initManager im_;

    //prt键为某个点，值为其最优父节点+最短距离
    private Dictionary<Vector2, Vector3> prt = new Dictionary<Vector2, Vector3>();
    
    private heap Heap = new heap();
    
    
    void Awake()
    {
        ec_ = GetComponent<enemyControl>();
        
        initManager = GameObject.Find("initManager");
        im_ = initManager.GetComponent<initManager>();
        
    }
    
    float fixCoordinate(float x)
    {
        if (x > 0) return (int) x + 0.5f;
        else return (int) x - 0.5f;
    }

    float distance(Vector2 x,Vector2 y)
    {
        return (x.x - y.x) * (x.x - y.x) + (x.y - y.y) * (x.y - y.y);
    }

    void getPrt(Queue<Vector2> realPointQueue, Vector2 x)
    {
        if (x == new Vector2(0, 0)) return;
        getPrt(realPointQueue,prt[x]);
        realPointQueue.Enqueue(x);
    }

    bool pdGround(Vector2 x)
    {
        return (im_.getMp(x) == 0 || im_.getMp(x) == 1);
    }

    void getStart(Vector3 S, Vector3 T)
    {
        Vector2 preS = new Vector2();
        Vector2 slashS = new Vector2();
        Vector3 ds = new Vector3();

        preS.x = fixCoordinate(S.x);
        preS.y = fixCoordinate(S.y);

        if (tagManager.isBox(im_.getMp(preS)))
        {
            prt[preS] = Vector3.zero;
            Heap.Push(preS);
        }

        S.x = Mathf.Round(S.x);
        S.y = Mathf.Round(S.y);

        slashS.x = S.x * 2 - preS.x;
        slashS.y = S.y * 2 - preS.y;
        
        for (int i = 0; i < 4; i++)
        {
            if (i == 0 || i == 1) ds.x = S.x + 0.5f;
            else ds.x = S.x - 0.5f;
            if (i == 0 || i == 2) ds.y = S.y + 0.5f;
            else ds.y = S.y - 0.5f;
            
            if (!pdGround(ds)) continue;
            if ((Vector2) ds == slashS) continue;
            
            ds.z = distance(ds, T);
            prt[ds] = Vector3.zero;
            Heap.Push(ds);
        }
    }

    public bool Astar(Queue<Vector2> realPointQueue, Vector3 S, Vector3 T)
    {
        bool found = false;
        realPointQueue.Clear();
        Heap.Clear();
        prt.Clear();

        T.z = 0;
        getStart(S,T);

        while (Heap.count > 0)
        {
            Vector3 x = Heap.Top();
            
            Heap.Pop();

            if ((Vector2)x == (Vector2)T)
            {
                found = true;
                break;
            }

            for (int i = 0; i < 4; i++)
            {
                Vector3 y = new Vector3();
                y.x = x.x + dx[i];
                y.y = x.y + dy[i];

                if (tagManager.isEmpty(im_.getMp(y))) continue;
                else if (!tagManager.canPass(im_.getMp(y)) && (Vector2)y!=(Vector2)T) continue;
                if (!prt.ContainsKey(y))
                {
                    prt[y] = new Vector3(x.x, x.y, prt[x].z + 1);
                    y.z = distance(y, T);
                    Heap.Push(y);
                }
                else
                {
                    if (prt[y].z > prt[x].z + 1)
                    {
                        prt[y] = new Vector3(x.x, x.y, prt[x].z + 1);
                    }
                }
            }
        }

        if (found) getPrt(realPointQueue, T);
        return found;
    }
}
