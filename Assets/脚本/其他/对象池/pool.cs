using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

[System.Serializable]
public class pool
{
    [SerializeField] GameObject prefab;
    private bool isInit = false;
    private Transform poolPrt;
    private Queue<GameObject> q;

    void Initialize()
    {
        q = new Queue<GameObject>();
        poolPrt = new GameObject("pool:" + prefab.name).transform;
        q.Enqueue(Copy());
        isInit = true;
        SceneManager.sceneLoaded += Clear;
    }

    GameObject Copy()
    {
        GameObject copy = GameObject.Instantiate(prefab, poolPrt, true);
        copy.SetActive(false);
        return copy;
    }

    GameObject AvailableObject()
    {
        GameObject availableObject = null;
        if (!isInit) Initialize();
        if (q.Count > 0 && !q.Peek().activeSelf)
        {
            availableObject = q.Dequeue();
        }
        else
        {
            availableObject = Copy();
        }

        q.Enqueue(availableObject);
        
        return availableObject;
    }

    public GameObject PrepareObject()
    {
        GameObject prepareObject = AvailableObject();
        prepareObject.SetActive(true);
        return prepareObject;
    }

    public void Clear(Scene arg0, LoadSceneMode arg1)
    {
        q.Clear();
        q.Enqueue(Copy());
    }
}
