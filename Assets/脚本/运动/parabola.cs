using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parabola : MonoBehaviour
{
    private Transform target;
    private Vector3 tarPos;
    public float speed;
    public float maxHeight;
    public bool getIt;
    private const float g = 30f;
    private float verticalSpeed;
    private bool isNull;
    private float time;
    private Vector3 xV;
    private Vector3 yV = new Vector3(0, 1, 0);
    private Vector3 zV = new Vector3(0, 0, 1);
    private float t1, t2;
    private float t;
    
    public void Init(Transform tarT, Vector3 tarP, bool nu)
    {
        target = tarT;
        tarPos = tarP;
        isNull = nu;
        var position = transform.position;
        float dis = Math.Abs(tarPos.x - position.x);
        if (dis > 0.2f) speed = (float) Math.Log(dis + 1.01, 1.3);
        else speed = Math.Abs(tarPos.y - position.y) + 2;

        xV = new Vector3(tarPos.x > position.x ? 1 : -1, 0, 0);

        if (dis > 0.2f) t = dis / speed;
        else t = 1;
        float H = Math.Abs(tarPos.y - position.y);
        float h = (g * (t * t - 2 * H / g) * (t * t - 2 * H / g)) / (8 * t * t);
        t1 = (float) (tarPos.y > position.y ? Math.Sqrt(2 * (H + h) / g) : Math.Sqrt(2 * (h) / g));
        t2 = (float) (tarPos.y > position.y ? Math.Sqrt(2 * (h) / g) : Math.Sqrt(2 * (H + h) / g));

        verticalSpeed = t1 * g;
        getIt = false;
        time = 0;
        t = t1 + t2;
    }


    

    void Update()
    {
        if (!isNull)
        {
            tarPos = target.position;
        }
        time += Time.deltaTime;  
        float test = verticalSpeed - g*time;
        test = Math.Max(-25, test);
        float dx = (tarPos.x - transform.position.x) * xV.x;
        if (dx > 0)
        {
            transform.Translate(xV * (speed * Time.deltaTime), Space.World);
        }
        if (time < t)
        {
            transform.Translate(yV * (test * Time.deltaTime), Space.World);
        }

        if (dx <= 0 && time >= t)
        {
            getIt = true;
        }
    }

    public void DoWhenTarNull(operControl oc)
    {
        isNull = true;
    }
    
}
