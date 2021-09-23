using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class redDoorUI : MonoBehaviour
{
    public Button startButton;

    private initManager im_;
    public Image sliderImage;
    public gradualChange gc_;
    public Text detailText;
    public Canvas can;
    
    private void Awake()
    {
        im_ = GameObject.Find("initManager").GetComponent<initManager>();
        im_.redDoorWaveList.Add(this);
        im_.redDoorSliderImage.Add(sliderImage);
        gc_.ImmediateHide();
        can.worldCamera = im_.orthoCamera;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
