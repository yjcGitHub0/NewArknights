using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class operDisplayButton : MonoBehaviour
{
    public operData od_;
    public FormationController fc_;
    public GameObject empty;
    public Image operImage;
    public Text Name;
    public Image professionImage;
    private bool isEmpty = true;

    public void PressDown()
    {
        if(isEmpty)
            fc_.OpenChooseUI();
        else 
            fc_.OpenChooseUI(od_);
    }

    public void ChangeOD(operData newOD)
    {
        if (newOD == null)
        {
            empty.SetActive(true);
            operImage.gameObject.SetActive(false);
            isEmpty = true;
            return;
        }
        empty.SetActive(false);
        operImage.gameObject.SetActive(true);
        od_ = newOD;
        operImage.sprite = od_.illustratedBookImage;
        professionImage.sprite = gameManager.GetProfessionImage(od_.profession);
        Name.text = od_.Name;
        isEmpty = false;
    }
    
}
