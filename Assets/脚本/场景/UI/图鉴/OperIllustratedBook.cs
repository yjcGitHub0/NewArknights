using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OperIllustratedBook : MonoBehaviour
{
    public operData defaultOperData;
    
    public gradualChange bookChooseUI;
    public gradualChange operBook;
    public Scrollbar leftScrollbar;
    public Scrollbar rightScrollbar;
    
    public operData showing_od;
    public Image operImage;
    public Slider elitismSlider;
    public Image elitismBackGround;
    public Text ChineseNameText;
    public Text EnglishNameText;
    public Text levelText;
    public Slider levelSlider;
    public Text lifeText;
    public Text atkText;
    public Text defText;
    public Text magicDefText;
    public Text blockText;
    public Image atkRangeImage;
    public Text talentText;
    public Button[] skillButton = new Button[3];
    public GameObject[] greenHook = new GameObject[3];
    public Text[] skillLevelText = new Text[3];
    public Slider[] skillLevelSlider = new Slider[3];
    public Image recoverTypeImage;
    public Image releaseTypeImage;
    public Text durationText;
    public Text initSPText;
    public Text maxSPText;
    public Text costNeedText;
    public Text expNeedText;
    public Text skillNameText;
    public Text skillDescriptionText;

    public bool closeAll = false;
    private int elitismLevel;
    private int level;
    private int skillNum;
    private int[] skillLevel = new int[3];

    public void OpenOperBook(bool immediate)
    {
        if (immediate)
        {
            bookChooseUI.ImmediateHide();
            operBook.ImmediateShow();
        }
        else
        {
            bookChooseUI.Hide();
            operBook.Show();
        }
        leftScrollbar.value = rightScrollbar.value = 1;
        ChangeShowingOD(defaultOperData);
    }

    public void CloseOperBook(bool immediate)
    {
        if (closeAll)
        {
            IllustratedBookManager.CloseIllustratedBook();
            return;
        }
        if (immediate)
        {
            bookChooseUI.ImmediateShow();
            operBook.ImmediateHide();
        }
        else
        {
            bookChooseUI.Show();
            operBook.Hide();
        }
    }

    public void ChangeShowingOD(operData newOD)
    {
        showing_od = newOD;
        rightScrollbar.value = 1;
        elitismLevel = level = skillNum = 0;
        skillLevel[0] = skillLevel[1] = skillLevel[2] = 1;

        elitismSlider.value = 0;
        ChangeElitismLevel();
        skillLevelSlider[0].value = skillLevelSlider[1].value = skillLevelSlider[2].value = 1;
        ChineseNameText.text = showing_od.Name;
        EnglishNameText.text = showing_od.EnName;
        SelectSkill(0);
        ChangeLevel();
        skillLevelText[0].text = skillLevelText[1].text = skillLevelText[2].text = "1";
        for(int i=0;i<3;i++)
            skillButton[i].image.sprite = showing_od.skillImage[i];
    }

    public void ChangeElitismLevel()
    {
        elitismLevel = (int) elitismSlider.value;
        elitismBackGround.sprite = IllustratedBookManager.elitismImage[elitismLevel];
        levelSlider.value = 0;
        levelSlider.maxValue = showing_od.maxLevel[elitismLevel];
        ChangeAttributes();
        talentText.text = showing_od.Description[elitismLevel];
        atkRangeImage.sprite = showing_od.atkRangeImage[elitismLevel];
        operImage.sprite = elitismLevel < 2 ? showing_od.operUIImage1 : showing_od.operUIImage2;
    }

    public void ChangeLevel()
    {
        level = (int) levelSlider.value;
        levelText.text = level.ToString();
        ChangeAttributes();
    }

    void ChangeAttributes()
    {
        float life = showing_od.life;
        float atk = showing_od.atk;
        float def = showing_od.def;
        float magicDef = showing_od.magicDef;
        int block = showing_od.maxBlock;
        for (int i = 0; i < elitismLevel; i++)
        {
            life += showing_od.maxLevel[i] * showing_od.growingLife[i] + showing_od.elitismLife[i];
            atk += showing_od.maxLevel[i] * showing_od.growingAtk[i] + showing_od.elitismAtk[i];
            def += showing_od.maxLevel[i] * showing_od.growingDef[i] + showing_od.elitismDef[i];
            magicDef += showing_od.elitismMagicDef[i];
            block += showing_od.elitismBlock[i];
        }
        life += level * showing_od.growingLife[elitismLevel];
        atk += level * showing_od.growingAtk[elitismLevel];
        def += level * showing_od.growingDef[elitismLevel];
        lifeText.text = life.ToString("f0");
        atkText.text = atk.ToString("f0");
        defText.text = def.ToString("f0");
        magicDefText.text = magicDef.ToString("f0");
        blockText.text = block.ToString();
    }

    public void SelectSkill(int num)
    {
        skillNum = num;
        for (int i = 0; i < 3; i++)
        {
            if (i == num) greenHook[i].SetActive(true);
            else greenHook[i].SetActive(false);
        }
        skill_showing_change();
    }

    public void ChangeSkillLevel(int num)
    {
        skillLevel[num] = (int)skillLevelSlider[num].value;
        skillLevelText[num].text = skillLevel[num].ToString();
        skill_showing_change();
    }

    void skill_showing_change()
    {
        int skill_level = skillLevel[skillNum] - 1;
         switch (skillNum)
        {
            case 0:
                recoverTypeImage.sprite = IllustratedBookManager.recoverTypeSprite(showing_od.skill0_recoverType);
                releaseTypeImage.sprite = IllustratedBookManager.releaseTypeSprite(showing_od.skill0_releaseType);
                durationText.text = showing_od.duration0[skill_level].ToString("f0");
                initSPText.text = showing_od.initSP0[skill_level].ToString();
                maxSPText.text = showing_od.maxSP0[skill_level].ToString();
                costNeedText.text = skill_level < 6 ? showing_od.costNeed0[skill_level].ToString() : "0";
                expNeedText.text = skill_level < 6 ? showing_od.expNeed0[skill_level].ToString() : "0";
                skillNameText.text = showing_od.skillName0;
                skillDescriptionText.text = showing_od.description0[skill_level];
                break;
            case 1:
                recoverTypeImage.sprite = IllustratedBookManager.recoverTypeSprite(showing_od.skill1_recoverType);
                releaseTypeImage.sprite = IllustratedBookManager.releaseTypeSprite(showing_od.skill1_releaseType);
                durationText.text = showing_od.duration1[skill_level].ToString("f0");
                initSPText.text = showing_od.initSP1[skill_level].ToString();
                maxSPText.text = showing_od.maxSP1[skill_level].ToString();
                costNeedText.text = skill_level < 6 ? showing_od.costNeed1[skill_level].ToString() : "0";
                expNeedText.text = skill_level < 6 ? showing_od.expNeed1[skill_level].ToString() : "0";
                skillNameText.text = showing_od.skillName1;
                skillDescriptionText.text = showing_od.description1[skill_level];
                break;
            case 2:
                recoverTypeImage.sprite = IllustratedBookManager.recoverTypeSprite(showing_od.skill2_recoverType);
                releaseTypeImage.sprite = IllustratedBookManager.releaseTypeSprite(showing_od.skill2_releaseType);
                durationText.text = showing_od.duration2[skill_level].ToString("f0");
                initSPText.text = showing_od.initSP2[skill_level].ToString();
                maxSPText.text = showing_od.maxSP2[skill_level].ToString();
                costNeedText.text = skill_level < 6 ? showing_od.costNeed2[skill_level].ToString() : "0";
                expNeedText.text = skill_level < 6 ? showing_od.expNeed2[skill_level].ToString() : "0";
                skillNameText.text = showing_od.skillName2;
                skillDescriptionText.text = showing_od.description2[skill_level];
                break;
        }
    }
    
}
