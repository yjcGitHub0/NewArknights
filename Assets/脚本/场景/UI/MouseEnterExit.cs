using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class MouseEnterExit : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int thisNum;
    public GameObject InitManager;
    public Text topText;
    public Text nextLevel;
    private UIconnect uc_;
    private void Start()
    {
        uc_ = InitManager.GetComponent<UIconnect>();
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        currentData cd_ = uc_.openingCd ? uc_.showingOper_cd : uc_.showingOper_oc.cd_;
        if (cd_.skillNum != thisNum) return;
        if (cd_.skillLevel[cd_.skillNum] < 6)
        {
            topText.gameObject.SetActive(false);
            nextLevel.gameObject.SetActive(true);
            uc_.ChangeDescriptionText_Skill(false, true);
        }
        else
        {
            topText.gameObject.SetActive(true);
            nextLevel.gameObject.SetActive(false);
            uc_.ChangeDescriptionText_Skill(true, false);
        }
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        topText.gameObject.SetActive(true);
        nextLevel.gameObject.SetActive(false);
        uc_.ChangeDescriptionText_Skill(false, false);
    }
    
}
