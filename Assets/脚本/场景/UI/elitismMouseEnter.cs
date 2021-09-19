using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class elitismMouseEnter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject Miscellaneous;
    public GameObject InitManager;
    public Text topText;
    public Text nextLevel;
    private UIconnect uc_;
    void Start()
    {
        uc_ = InitManager.GetComponent<UIconnect>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!uc_.openingCd) return;
        currentData cd_ = uc_.showingOper_cd;
        if (cd_.level < cd_.maxLevel[cd_.elitismLevel]) return;
        Miscellaneous.SetActive(false);
        if (cd_.elitismLevel < 2)
        {
            topText.gameObject.SetActive(false);
            nextLevel.gameObject.SetActive(true);
            uc_.descriptionText.text = cd_.od_.Description[cd_.elitismLevel + 1];
            uc_.ChangeDescriptionText_elitism();
        }
        else
        {
            topText.gameObject.SetActive(true);
            nextLevel.gameObject.SetActive(false);
            uc_.ChangeDescriptionText_elitism();
        }
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        ChangeDescriptionNormal();
    }

    public void ChangeDescriptionNormal()
    {
        if (!uc_.openingCd) return;
        currentData cd_ = uc_.showingOper_cd;
        Miscellaneous.SetActive(true);
        topText.gameObject.SetActive(true);
        nextLevel.gameObject.SetActive(false);
        uc_.descriptionText.text = cd_.od_.Description[cd_.elitismLevel];
        uc_.ChangeDescriptionText_Skill(false, false);
    }
}
