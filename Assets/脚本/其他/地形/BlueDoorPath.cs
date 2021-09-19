using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueDoorPath : MonoBehaviour
{
    public initManager im_;
    public Vector2 pos;
    public Dictionary<Vector2, int> disMap = new Dictionary<Vector2, int>();
    private Queue<Vector3> q = new Queue<Vector3>();
    private float[] fx = new[] {1f, 0f, -1f, 0f};
    private float[] fy = new[] {0f, 1f, 0f, -1f};
    
    void Awake()
    {
        im_ = GameObject.Find("sceneInit").transform.Find("initManager").gameObject.GetComponent<initManager>();
        pos = initManager.FixCoordinate(transform.position);
    }

    private void Start()
    {
        im_.blueDoorPathList.Add(this);
        FindPath();
    }

    public bool FindPath()
    {
        disMap.Clear();
        Vector3 S = new Vector3();
        S.x = initManager.FixCoordinate(transform.position.x);
        S.y = initManager.FixCoordinate(transform.position.y);
        S.z = 0;
        q.Enqueue(S);

        while (q.Count > 0)
        {
            Vector3 x = q.Dequeue();
            for (int i = 0; i < 4; i++)
            {
                Vector3 nxtPos = x;
                nxtPos.x += fx[i];
                nxtPos.y += fy[i];
                nxtPos.z++;
                if (tagManager.canPass(im_.getMp(nxtPos)) && !disMap.ContainsKey(nxtPos))
                {
                    disMap.Add(nxtPos, (int) nxtPos.z);
                    q.Enqueue(nxtPos);
                }
            }
        }
        foreach (var i in im_.startPointList)
        {
            if (!disMap.ContainsKey(i)) return false;
        }

        return true;
    }
}
