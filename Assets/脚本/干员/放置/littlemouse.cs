using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class littlemouse : MonoBehaviour
{
    public bool getIt;
    public bool dragEnd;
    public float lowGround_z;
    public float highGround_z;
    public GameObject verify;
    public GameObject slot;

    private initManager im_;
    private Drag drag_;
    private operData od_;
    private Vector3 shouldBe = new Vector3();
    private float px, py;
    private Vector3 screenPosition = new Vector3();
    private Vector3 mousePositionOnScreen = new Vector3();
    private Vector3 mousePositionInWorld = new Vector3();
    

    // Start is called before the first frame update
    void Start()
    {
        getIt = false;
        dragEnd = false;
        drag_ = slot.GetComponent<Drag>();
        od_ = drag_.thisOper;
        im_ = GameObject.Find("initManager").GetComponent<initManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (dragEnd)
        {
            if (Input.GetMouseButtonDown(0))
            {
                px = mousePositionInWorld.x;
                py = mousePositionInWorld.y;
                if (!CheckClick())
                {
                    //im_.draging = false;
                    drag_.Reverse(0, false);
                }
            }
        }
        screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        mousePositionOnScreen = Input.mousePosition;
        mousePositionOnScreen.z = screenPosition.z;
        mousePositionInWorld = Camera.main.ScreenToWorldPoint(mousePositionOnScreen);
        mousePositionInWorld.z = -0.9f;
        
        if (dragEnd) return;
        
        transform.position = mousePositionInWorld;
        pdCanPut(transform.position);
        
        if (!getIt)
        {
            shouldBe.x = transform.position.x;
            shouldBe.y = transform.position.y;
        }
        else
        {
            shouldBe.x = fixCoordinate(transform.position.x);
            shouldBe.y = fixCoordinate(transform.position.y);
        }
        verify.transform.position = shouldBe;
    }

    bool CheckClick()
    {
        float x = Mathf.Abs(px - shouldBe.x);
        float y = Mathf.Abs(py - shouldBe.y);
        if (x + y <= 4) return true;
        else return false;
    }

    float fixCoordinate(float x)
    {
        if (x > 0) return (int) x + 0.5f;
        else return (int) x - 0.5f;
    }

    void pdCanPut(Vector2 x)
    {
        x.x = fixCoordinate(x.x);
        x.y = fixCoordinate(x.y);
        if (tagManager.isEmpty(im_.getMp(x)))  
        {
            getIt = false;
            return;
        }

        if (tagManager.canPut(im_.getMp(x)))
        {
            getIt = false;
            if (tagManager.isHigh(im_.getMp(x)) && !od_.banHighGround)
            {
                shouldBe.z = highGround_z;
                getIt = true;
            }
            if (tagManager.isLow(im_.getMp(x)) && !od_.banLowGround)
            {
                shouldBe.z = lowGround_z;
                getIt = true;
            }
        }
        else
        {
            getIt = false;
        }
    }
}
