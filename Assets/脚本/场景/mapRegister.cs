using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapRegister : MonoBehaviour
{
    [EnumLabel("地形类")]
    public platformType platform;
    private GameObject initManager;
    private initManager im_;
    
    void Awake()
    {
        initManager=GameObject.Find("initManager");
        im_ = initManager.GetComponent<initManager>();
        im_.regist(transform.position, (int)platform);
        /*
        if (transform.CompareTag("ground"))
        {
            im_.regist(transform.position, 0);
        }
        else if (transform.CompareTag("lowground"))
        {
            im_.regist(transform.position, 1);
        }
        else if (transform.CompareTag("wall"))
        {
            im_.regist(transform.position, 2);
        }
        else if (transform.CompareTag("highground"))
        {
            im_.regist(transform.position, 3);
        }
        */
    }

    
}
