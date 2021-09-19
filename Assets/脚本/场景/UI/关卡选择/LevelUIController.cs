using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUIController : MonoBehaviour
{
    public gradualChange rightUI;
    public gradualChange mapGradual_;
    
    public levelData ld_;

    public Text nameText;
    public Text descriptionText;
    public Image mapImage;

    public void OpenRightUI(levelData newLD)
    {
        ld_ = newLD;
        rightUI.Show();
        nameText.text = ld_.Name;
        descriptionText.text = ld_.Description;
        mapGradual_.Hide();
    }

    public void CloseRightUI()
    {
        rightUI.Hide();
    }

    public void OpenMapUI()
    {
        mapImage.sprite = ld_.map;
        mapGradual_.Show();
    }

    public void CloseMapUI()
    {
        mapGradual_.Hide();
    }
    
}
