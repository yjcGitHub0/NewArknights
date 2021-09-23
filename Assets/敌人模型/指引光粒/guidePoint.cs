using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public class guidePoint : MonoBehaviour
{
    public int wave;
    public float appearTime;
    public List<Vector3> pointList = new List<Vector3>();
    private float eps = 1e-3f;
    private Queue<Vector2> pointQueue = new Queue<Vector2>();
    private Queue<Vector2> realPointQueue = new Queue<Vector2>();
    private Vector3 nxtPoint;
    private Vector2 endPoint;
    private float defaultZ = -0.6f;
    private Axing ax_;
    private bool isArrive = false;
    private initManager im_;

    private void Awake()
    {
        im_ = GameObject.Find("initManager").GetComponent<initManager>();
        while (im_.guideList.Count <= wave) im_.guideList.Add(new List<guidePoint>());
        im_.guideList[wave].Add(this);
    }

    void Start()
    {
        ax_ = GetComponent<Axing>();
        foreach (var i in pointList)
        {
            pointQueue.Enqueue(i);
        }
        Vector3 pos = pointQueue.Dequeue();
        pos.z = defaultZ;
        transform.position = nxtPoint = pos;
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position == nxtPoint)
        {
            if (realPointQueue.Count != 0)
            {
                nxtPoint = realPointQueue.Dequeue();
                nxtPoint.z = defaultZ;
            }
            else
            {
                gotoNextPoint();
            }
        }
        
        transform.position = Vector3.MoveTowards(transform.position, nxtPoint, 15 * Time.deltaTime);

    }
    bool vc2Equal(Vector2 x, Vector2 y)
    {
        return (Mathf.Abs(x.x - y.x) < eps) && (Mathf.Abs(x.y - y.y) < eps);
    }
    void gotoNextPoint()
    {
        if (pointQueue.Count == 0)
        {
            if (!isArrive)
            {
                Invoke(nameof(arrive), 0.5f);
                isArrive = true;
            }
            return;
        }
        endPoint = pointQueue.Dequeue();
        changeRoute();
    }
    public bool changeRoute()
    {
        bool found = ax_.Astar(realPointQueue, transform.position, endPoint);
        
        if (!found) return false;
        nxtPoint = realPointQueue.Dequeue();
        nxtPoint.z = defaultZ;
        return true;
    }

    void arrive()
    {
        Destroy(gameObject);
    }
    
    void Registe()
    {
        while (im_.guideList.Count <= wave) im_.guideList.Add(new List<guidePoint>());
        im_.guideList[wave].Add(this);
    }
    
}
