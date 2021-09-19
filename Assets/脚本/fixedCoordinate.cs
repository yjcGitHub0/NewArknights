using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fixedCoordinate : MonoBehaviour
{
    public Vector3 fixPos = new Vector3();
    public Vector3 fixRol = new Vector3();
    private Quaternion rol;
    private Transform tr;

    private void Awake()
    {
        rol=Quaternion.Euler(fixRol);
        tr = transform;
    }

    void Update()
    {
        tr.localPosition = fixPos;
        tr.localRotation = rol;
    }
}
