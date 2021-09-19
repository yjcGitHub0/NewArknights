using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class follow : MonoBehaviour
{
    public Transform tar;
    private Vector3 tarPos;
    private bool isNull;

    public void Init(Transform t)
    {
        tar = t;
        tarPos = t.position;
        isNull = false;
    }
    
    void Update()
    {
        if (!isNull)
        {
            if (tar != null) tarPos = tar.position;
            else isNull = true;
        }
        transform.position = tarPos;
    }
}
