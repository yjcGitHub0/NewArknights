using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OperChooseLeftInfo : MonoBehaviour
{
    public List<OperChoose> formationChooseList = new List<OperChoose>();
    public GameObject noInfo;

    public operData od_;
    private bool noOD = true;
    public Text NameText;
    public Button bookButton;
    public Image atkRangeImage;
    public Text lifeText;
    public Text atkText;
    public Text defText;
    public Text magicDefText;
    public Text blockText;

    public Image[] skillImage = new Image[3];
    public Text[] initSpText = new Text[3];
    public Text[] maxSpText = new Text[3];
    public Image[] recoverTypeImage = new Image[3];
    public Image[] releaseTypeImage = new Image[3];
    public Text[] durationText = new Text[3];
    public Text[] descriptionText = new Text[3];


    public void ChangeOD(operData newOD)
    {
        od_ = newOD;
        noOD = false;
        noInfo.SetActive(false);

        NameText.text = od_.Name;
        atkRangeImage.sprite = od_.atkRangeImage[0];
        lifeText.text = od_.life.ToString("f0");
        atkText.text = od_.life.ToString("f0");
        defText.text = od_.life.ToString("f0");
        magicDefText.text = od_.life.ToString("f0");
        blockText.text = od_.maxBlock.ToString();

        for (int i = 0; i < 3; i++)
        {
            skillImage[i].sprite = od_.skillImage[i];
            
        }

        initSpText[0].text = od_.initSP0[0].ToString();
        initSpText[1].text = od_.initSP1[0].ToString();
        initSpText[2].text = od_.initSP2[0].ToString();
        maxSpText[0].text = od_.maxSP0[0].ToString();
        maxSpText[1].text = od_.maxSP1[0].ToString();
        maxSpText[2].text = od_.maxSP2[0].ToString();
        recoverTypeImage[0].sprite = IllustratedBookManager.recoverTypeSprite(od_.skill0_recoverType);
        recoverTypeImage[1].sprite = IllustratedBookManager.recoverTypeSprite(od_.skill1_recoverType);
        recoverTypeImage[2].sprite = IllustratedBookManager.recoverTypeSprite(od_.skill2_recoverType);
        releaseTypeImage[0].sprite = IllustratedBookManager.releaseTypeSprite(od_.skill0_releaseType);
        releaseTypeImage[1].sprite = IllustratedBookManager.releaseTypeSprite(od_.skill1_releaseType);
        releaseTypeImage[2].sprite = IllustratedBookManager.releaseTypeSprite(od_.skill2_releaseType);
        durationText[0].text = od_.duration0[0].ToString("f0");
        durationText[1].text = od_.duration1[0].ToString("f0");
        durationText[2].text = od_.duration2[0].ToString("f0");
        descriptionText[0].text = od_.description0[0];
        descriptionText[1].text = od_.description1[0];
        descriptionText[2].text = od_.description2[0];
    }

    public void RemoveOD()
    {
        noOD = true;
        noInfo.SetActive(true);
    }

    public void OpenBook()
    {
        IllustratedBookManager.OpenIllustratedBook(od_);
    }

    public void AdaptFormation()
    {
        foreach (var i in formationChooseList)
        {
            if (gameManager.formation[gameManager.formationNum].Contains(i.od_))
            {
                i.FormationChoose();
            }
            else
            {
                i.FormationRemove();
            }
        }
    }
    
}
