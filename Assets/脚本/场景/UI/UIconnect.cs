using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class UIconnect : MonoBehaviour
{
    private initManager im_;
    private costController cost_;
    private expController exp_;
    private Color grayColor;
    public currentData showingOper_cd;//同时只能有一个有效
    public operControl showingOper_oc;
    public bool openingCd = false;
    public bool openingOc = false;
    public int closeDelay;
    public bool dontCloseUI;
    
    public spController showingSP_;
    public Text showingSPText;
    public Slider showingGreenSlider;
    public Slider showingYellowSlider;

    [Header("常驻UI")]
    public GameObject messagePanel;
    public Text messageText;
    public Text costText;
    public Text expText;
    public Text placeText;
    public Text lifeText;
    public Text enemyText;
    public Slider costSlider;

    [Header("技能UI")]
    public GameObject LeftUIPanel;
    public Image operImage;
    public Button elitismButton;
    public Text nameText;
    public Text EnNameText;
    public Text levelText;
    public Text atkText;
    public Text defText;
    public Text magicDefText;
    public Text blockText;
    public Slider lifeSlider;
    public Text lifeSliderText;
    public Button[] skillChoose = new Button[3];
    public Button[] skillLevelUP = new Button[3];
    public Image[] greenHook = new Image[3];
    public Text[] skillLevelText = new Text[3];
    public Text descriptionText;


    [Header("底部描述")] 
    public Image recoverTypeImage;
    public Image releaseTypeImage;
    public Text topNameText;
    public Text bottomDescriptionText;
    public Text duringText;
    public Text expNeedText;
    public Text costNeedText;
    public Text leftInitSPText;
    public Text leftMaxSPText;
    
    
    
    void Start()
    {
        im_ = GetComponent<initManager>();
        cost_ = GetComponent<costController>();
        exp_ = GetComponent<expController>();
        grayColor.b = grayColor.g = grayColor.r = 0.25f;
        grayColor.a = 1f;
        //初始化text
        costText.text = im_.cost.ToString();
        expText.text = im_.exp.ToString();
        lifeText.text = im_.life.ToString();
        placeText.text = im_.remainPlace.ToString();
    }

    private void Update()
    {
        //如果左侧UI打开，判断是否该关闭
        if ((openingCd && im_.draging == false) || openingOc)
        {
            
            if (Input.GetMouseButtonUp(0))
            {
                closeDelay = 1;
            }
            else if (closeDelay > 0)
            {
                closeDelay++;
            }

            if (closeDelay >= 2)
            {
                if (!dontCloseUI)
                {
                    if (openingCd) CloseLeftUI(showingOper_cd);
                    else CloseLeftUI(showingOper_oc);
                }
                else dontCloseUI = false;
                closeDelay = 0;
            }            
        }
        
        //如果左侧UI打开，更新左侧UI
        if (openingOc)
        {//干员放置中
            levelText.text = showingOper_oc.cd_.level + "/" + showingOper_oc.cd_.maxLevel[showingOper_oc.cd_.elitismLevel];
            
            atkText.text = showingOper_oc.atk_.atk.ToString("f0");
            defText.text = showingOper_oc.def_.def.ToString("f0");
            magicDefText.text = showingOper_oc.def_.magicDef.ToString("f0");
            blockText.text = showingOper_oc.of_.maxBlock.ToString();
            lifeSlider.value = showingOper_oc.life_.life/showingOper_oc.life_.maxLife;
            lifeSliderText.text = showingOper_oc.life_.life.ToString("f0") + "/" + showingOper_oc.life_.maxLife.ToString("f0");
            
            showingSPText.text = showingSP_.sp.ToString("f0") + "/" + showingSP_.maxSP.ToString("f0");
            showingGreenSlider.value = showingSP_.maxSP == 0 ? 1 : showingSP_.sp / showingSP_.maxSP;
            showingYellowSlider.value =
                showingSP_.maxSkillDuring == 0 ? 1 : showingSP_.skillDuring / showingSP_.maxSkillDuring;
            ChangeSkillBar();
        }
        else if (openingCd)
        {//干员在干员栏
            levelText.text = showingOper_cd.level + "/" + showingOper_cd.maxLevel[showingOper_cd.elitismLevel];
            
            atkText.text = showingOper_cd.baseAtk.ToString("f0");
            defText.text = showingOper_cd.baseDef.ToString("f0");
            magicDefText.text = showingOper_cd.baseMagicDef.ToString("f0");
            blockText.text = showingOper_cd.baseBlock.ToString();
            lifeSlider.value = 1;
            lifeSliderText.text = showingOper_cd.baseMaxLife.ToString("f0") + "/" + showingOper_cd.baseMaxLife.ToString("f0");
        }
    }

    void LateUpdate()
    {
        //更新常驻UI
        expText.text = im_.exp.ToString();
        costText.text = im_.cost.ToString();
        lifeText.text = im_.life.ToString();
        placeText.text = im_.remainPlace.ToString();
        enemyText.text = im_.defeatEnemy + "/" + im_.totEnemy.ToString();
        costSlider.value = cost_.costTime / cost_.maxCostTime;
    }
    
    public void ShowMessage(string s)
    {
        messageText.text = s;
        messagePanel.SetActive(true);
        Invoke(nameof(CloseMessage),1f);
    }
    
    void CloseMessage()
    {
        messagePanel.SetActive(false);
    }

    public void OpenLeftUI(currentData oper)
    {
        CloseLeftUI(showingOper_cd);
        CloseLeftUI(showingOper_oc);
        dontCloseUI = true;

        showingOper_cd = oper;
        openingCd = true;
        
        im_.timeSlowDrag();
        
        operData od_ = oper.od_;
        LeftUIPanel.SetActive(true);
        
        if (oper.elitismLevel == 0)
        {
            operImage.sprite = od_.operUIImage1;
            elitismButton.image.sprite = IllustratedBookManager.elitismImage[0];
            skillChoose[1].image.color = skillChoose[2].image.color = grayColor;
            descriptionText.text = od_.Description[0];
        }
        else if (oper.elitismLevel == 1)
        {
            operImage.sprite = od_.operUIImage1;
            elitismButton.image.sprite = IllustratedBookManager.elitismImage[1];
            skillChoose[1].image.color = Color.white;
            skillChoose[2].image.color = grayColor;
            descriptionText.text = od_.Description[1];
        }
        else if (oper.elitismLevel == 2)
        {
            operImage.sprite = od_.operUIImage2;
            elitismButton.image.sprite = IllustratedBookManager.elitismImage[2];
            skillChoose[1].image.color = skillChoose[2].image.color = Color.white;
            descriptionText.text = od_.Description[2];
        }

        nameText.text = od_.Name;
        EnNameText.text = od_.EnName;
        levelText.text = oper.level + "/" + oper.maxLevel[oper.elitismLevel];

        atkText.text = oper.baseAtk.ToString("f0");
        defText.text = oper.baseDef.ToString("f0");
        magicDefText.text = oper.baseMagicDef.ToString("f0");
        blockText.text = oper.baseBlock.ToString();

        lifeSlider.value = 1f;
        lifeSliderText.text = oper.baseMaxLife.ToString("f0") + "/" + oper.baseMaxLife.ToString("f0");
        
        OptionsWhenOpenUI_CD(oper);
    }
    
    public void OpenLeftUI(operControl oper)
    {
        CloseLeftUI(showingOper_cd);
        CloseLeftUI(showingOper_oc);
        dontCloseUI = true;
        
        showingOper_oc = oper;
        openingOc = true;

        im_.timeSlowpick(oper.gameObject);
        
        operData od_ = oper.od_;
        LeftUIPanel.SetActive(true);
        
        if (oper.cd_.elitismLevel == 0)
        {
            operImage.sprite = od_.operUIImage1;
            elitismButton.image.sprite = IllustratedBookManager.elitismImage[0];
            descriptionText.text = od_.Description[0];
        }
        else if (oper.cd_.elitismLevel == 1)
        {
            operImage.sprite = od_.operUIImage1;
            elitismButton.image.sprite = IllustratedBookManager.elitismImage[1];
            descriptionText.text = od_.Description[1];
        }
        else if (oper.cd_.elitismLevel == 2)
        {
            operImage.sprite = od_.operUIImage2;
            elitismButton.image.sprite = IllustratedBookManager.elitismImage[2];
            descriptionText.text = od_.Description[2];
        }

        nameText.text = od_.Name;
        EnNameText.text = od_.EnName;
        levelText.text = oper.cd_.level + "/" + oper.cd_.maxLevel[oper.cd_.elitismLevel];

        atkText.text = oper.atk_.atk.ToString("f0");
        defText.text = oper.def_.def.ToString("f0");
        magicDefText.text = oper.def_.magicDef.ToString("f0");
        blockText.text = oper.of_.maxBlock.ToString();

        lifeSlider.value = oper.life_.life / oper.life_.maxLife;
        lifeSliderText.text = oper.life_.life.ToString("f0") + "/" + oper.life_.maxLife.ToString("f0");
        
        //改变技能
        OptionsWhenOpenUI_OC(oper);
    }

    private void OptionsWhenOpenUI_CD(currentData oper)
    {
        operData od_ = oper.od_;
        for (int i = 0; i < 3; i++)
        {
            skillChoose[i].image.sprite = od_.skillImage[i];
            skillChoose[i].gameObject.SetActive(true);
            skillLevelUP[i].gameObject.SetActive(true);
            greenHook[i].gameObject.SetActive(i == oper.skillNum);
            skillLevelText[i].text = (oper.skillLevel[i] + 1).ToString();
        }
        ChangeDescriptionText_Skill(false,false);
    }
    
    public void OptionsWhenOpenUI_OC(operControl oper)
    {
        operData od_ = oper.od_;
        for (int i = 0; i < 3; i++)
        {
            skillChoose[i].image.sprite = od_.skillImage[i];
            if (i != oper.cd_.skillNum) skillChoose[i].gameObject.SetActive(false);
            skillLevelUP[i].gameObject.SetActive(false);
            greenHook[i].gameObject.SetActive(false);
        }
        showingSP_ = oper.sp_;
        oper.skillImage.sprite = oper.od_.skillImage[oper.cd_.skillNum];
        showingSPText = oper.spText;
        showingSPText.text = showingSP_.sp.ToString("f0") + "/" + oper.sp_.maxSP.ToString("f0");
        showingGreenSlider = oper.skillGreenSlider;
        showingYellowSlider = oper.skillYellowSlider;
        ChangeSkillBar();
        ChangeDescriptionText_Skill(false,false);
    }

    public void ChangeDescriptionText_Skill(bool showIsMaxLevel, bool levelPlus)
    {
        operData od_ = openingCd ? showingOper_cd.od_ : showingOper_oc.od_;
        currentData cd_ = openingCd ? showingOper_cd : showingOper_oc.cd_;
        int skillNum = cd_.skillNum;
        int level = cd_.skillLevel[skillNum] + (levelPlus ? 1 : 0);
        skillLevelText[skillNum].text = (cd_.skillLevel[skillNum] + 1).ToString();
        recoverType showingRecoverType = recoverType.auto;
        releaseType showingReleaseType = releaseType.hand;
        switch (skillNum)
        {
            case 0:
                if(!showIsMaxLevel)topNameText.text = od_.skillName0;
                bottomDescriptionText.text = od_.description0[level];
                duringText.text = od_.duration0[level].ToString("f0");
                leftInitSPText.text = od_.initSP0[level].ToString();
                leftMaxSPText.text = od_.maxSP0[level].ToString();
                if (levelPlus)
                {
                    expNeedText.text = od_.expNeed0[level - 1].ToString();
                    costNeedText.text = od_.costNeed0[level - 1].ToString();
                }
                showingRecoverType = od_.skill0_recoverType;
                showingReleaseType = od_.skill0_releaseType;
                break;
            case 1:
                if(!showIsMaxLevel)topNameText.text = od_.skillName1;
                duringText.text = od_.duration1[level].ToString("f0");
                bottomDescriptionText.text = od_.description1[level];
                leftInitSPText.text = od_.initSP1[level].ToString();
                leftMaxSPText.text = od_.maxSP1[level].ToString();
                if (levelPlus)
                {
                    expNeedText.text = od_.expNeed1[level - 1].ToString();
                    costNeedText.text = od_.costNeed1[level - 1].ToString();
                }
                showingRecoverType = od_.skill1_recoverType;
                showingReleaseType = od_.skill1_releaseType;
                break;
            case 2:
                if(!showIsMaxLevel)topNameText.text = od_.skillName2;
                duringText.text = od_.duration2[level].ToString("f0");
                bottomDescriptionText.text = od_.description2[level];
                leftInitSPText.text = od_.initSP1[level].ToString();
                leftMaxSPText.text = od_.maxSP1[level].ToString();
                if (levelPlus)
                {
                    expNeedText.text = od_.expNeed2[level - 1].ToString();
                    costNeedText.text = od_.costNeed2[level - 1].ToString();
                }
                showingRecoverType = od_.skill2_recoverType;
                showingReleaseType = od_.skill2_releaseType;
                break;
        }
        switch (showingRecoverType)
        {
            case recoverType.auto: 
                recoverTypeImage.sprite = IllustratedBookManager.autoRecover;
                break;
            case recoverType.atk:
                recoverTypeImage.sprite = IllustratedBookManager.atkRecover;
                break;
            case recoverType.beAtk:
                recoverTypeImage.sprite = IllustratedBookManager.beAtkRecover;
                break;
        }
        switch (showingReleaseType)
        {
            case releaseType.hand: 
                releaseTypeImage.sprite = IllustratedBookManager.handRelease;
                break;
            case releaseType.auto:
                releaseTypeImage.sprite = IllustratedBookManager.autoRelease;
                break;
            case releaseType.passive:
                releaseTypeImage.sprite = IllustratedBookManager.passiveSkill;
                break;
        }
        if (showIsMaxLevel) topNameText.text = "已达到最高等级";
    }
    
    public void ChangeDescriptionText_elitism()
    {
        operData od_ = openingCd ? showingOper_cd.od_ : showingOper_oc.od_;
        currentData cd_ = openingCd ? showingOper_cd : showingOper_oc.cd_;

        int elitismLevel = cd_.elitismLevel;
        if (elitismLevel >= 2)
        {
            topNameText.text = "已达到最高精英化等级";
            bottomDescriptionText.text = "";
            return;
        }
        expNeedText.text = od_.elitismExp[elitismLevel].ToString();
        costNeedText.text = od_.elitismCost[elitismLevel].ToString();
        bottomDescriptionText.text = "生命：" + cd_.baseMaxLife.ToString("f0") + " -> " + 
                                     (cd_.baseMaxLife + od_.elitismLife[elitismLevel]).ToString("f0") + "\n";
        bottomDescriptionText.text += "攻击：" + cd_.baseAtk.ToString("f0") + " -> " + 
                                      (cd_.baseAtk + od_.elitismAtk[elitismLevel]).ToString("f0") + "\n";
        bottomDescriptionText.text += "防御：" + cd_.baseDef.ToString("f0") + " -> " +
                                      (cd_.baseDef + od_.elitismDef[elitismLevel]).ToString("f0") + "\n";
        bottomDescriptionText.text += "法抗：" + cd_.baseMagicDef.ToString("f0") + " -> " +
                                      (cd_.baseMagicDef + od_.elitismMagicDef[elitismLevel]).ToString("f0") + "\n";
        bottomDescriptionText.text += "阻挡：" + cd_.baseBlock.ToString("f0") + " -> " +
                                      (cd_.baseBlock + od_.elitismBlock[elitismLevel]).ToString("f0") + "\n";
    }
    
    public void CloseLeftUI(currentData oper)
    {
        if (!openingCd || showingOper_cd != oper) return;
        LeftUIPanel.SetActive(false);
        openingCd = false;
        showingOper_cd = null;
        im_.timeRecover();
    }

    public void CloseLeftUI(operControl oper)
    {
        if (!openingOc || showingOper_oc != oper) return;
        LeftUIPanel.SetActive(false);
        oper.sUI.SetActive(false);
        openingOc = false;
        showingOper_oc = null;
        im_.timeRecover();
    }

    public void ChangeSkillBar()
    {
        if (openingOc)
        {
            if (showingOper_oc.cuc_.skillDuring)
            {
                showingGreenSlider.gameObject.SetActive(false);
                showingYellowSlider.gameObject.SetActive(true);
            }
            else
            {
                showingGreenSlider.gameObject.SetActive(true);
                showingYellowSlider.gameObject.SetActive(false);
            }
        }
    }

}
