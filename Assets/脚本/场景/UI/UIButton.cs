using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButton : MonoBehaviour
{
    public GameObject IM;
    public GameObject ElitismButtonObj;
    private initManager im_;
    private UIconnect uc_;
    public Image[] greenHook = new Image[3];
    public Text topText;
    public Text nextLevel;
    private costController _costController;
    private expController _expController;
    private elitismMouseEnter eme_;
    
    private void Start()
    {
        im_ = IM.GetComponent<initManager>();
        uc_ = IM.GetComponent<UIconnect>();
        greenHook[0].gameObject.SetActive(true);
        greenHook[1].gameObject.SetActive(false);
        greenHook[2].gameObject.SetActive(false);
        _costController = IM.GetComponent<costController>();
        _expController = IM.GetComponent<expController>();
        eme_ = ElitismButtonObj.GetComponent<elitismMouseEnter>();
    }

    public void LevelUP(int x)//提升x级
    {
        uc_.dontCloseUI = true;
        currentData cd_;
        if (uc_.openingCd) cd_ = uc_.showingOper_cd;
        else cd_ = uc_.showingOper_oc.cd_;
        operData od_ = cd_.od_;

        int dx = Math.Min(x, cd_.maxLevel[cd_.elitismLevel] - cd_.level);
        if (dx == 0 || _costController.cost < dx || _expController.exp < dx) return;
        _costController.UseCost(dx);//消耗资源
        _expController.UseExp(dx);

        cd_.LevelUP(dx);
        if (uc_.openingOc)
        {
            uc_.showingOper_oc.atk_.ChangeBaseAtk(cd_.baseAtk);
            uc_.showingOper_oc.def_.ChangeBaseDef(cd_.baseDef, cd_.baseMagicDef);
            uc_.showingOper_oc.life_.ChangeBaseLife(cd_.baseMaxLife);
        }
        AudioManager.OperatorTalk(od_.LevelUP);
    }

    public void ChooseSkill(int skillNum)
    {
        uc_.dontCloseUI = true;
        if (!uc_.openingCd) return;
        currentData cd_ = uc_.showingOper_cd;
        if (cd_.elitismLevel < skillNum) return;

        cd_.skillNum = skillNum;
        for (int i = 0; i < 3; i++)
        {
            if (i != skillNum) greenHook[i].gameObject.SetActive(false);
            else greenHook[i].gameObject.SetActive(true);
        }
        uc_.ChangeDescriptionText_Skill(false,false);
    }

    public void skillLevelUP(int skillNum)
    {
        uc_.dontCloseUI = true;
        if (!uc_.openingCd) return;
        currentData cd_ = uc_.showingOper_cd;
        if (cd_.elitismLevel < skillNum) return;
        ChooseSkill(skillNum);
        int levelNow = cd_.skillLevel[skillNum];
        if (levelNow >= 6)
        {
            topText.gameObject.SetActive(true);
            nextLevel.gameObject.SetActive(false);
            uc_.ChangeDescriptionText_Skill(true, false);
            return;
        }
        int expNeed = 0, costNeed = 0;

        switch (skillNum)
        {
            case 0:
                expNeed = cd_.od_.expNeed0[levelNow];
                costNeed = cd_.od_.costNeed0[levelNow];
                break;
            case 1:
                expNeed = cd_.od_.expNeed1[levelNow];
                costNeed = cd_.od_.costNeed1[levelNow];
                break;
            case 2:
                expNeed = cd_.od_.expNeed2[levelNow];
                costNeed = cd_.od_.costNeed2[levelNow];
                break;
        }

        if (im_.cost >= costNeed && im_.exp >= expNeed)
        {
            im_.cost -= costNeed;
            im_.exp -= expNeed;
            cd_.skillLevel[skillNum]++;
        }

        if(cd_.skillLevel[skillNum] < 6)
            uc_.ChangeDescriptionText_Skill(false, true);
        else
        {
            topText.gameObject.SetActive(true);
            nextLevel.gameObject.SetActive(false);
            uc_.ChangeDescriptionText_Skill(true, false);
        }
    }

    public void ElitismLevelUP()
    {
        uc_.dontCloseUI = true;
        if (!uc_.openingCd) return;
        currentData cd_ = uc_.showingOper_cd;
        operData od_ = cd_.od_;
        int elitismLevel = cd_.elitismLevel;
        if (elitismLevel >= 2) return;
        if (cd_.level < cd_.maxLevel[elitismLevel]) return;
        int costNeed = od_.elitismCost[elitismLevel];
        int expNeed = od_.elitismExp[elitismLevel];
        if ((im_.cost < costNeed) || (im_.exp < expNeed)) return;
        im_.cost -= costNeed;
        im_.exp -= expNeed;
        cd_.baseAtk += od_.elitismAtk[elitismLevel];
        cd_.baseDef += od_.elitismDef[elitismLevel];
        cd_.baseMagicDef += od_.elitismMagicDef[elitismLevel];
        cd_.baseMaxLife += od_.elitismLife[elitismLevel];
        cd_.baseBlock += od_.elitismBlock[elitismLevel];

        cd_.elitismLevel++;
        uc_.OpenLeftUI(cd_);
        //eme_.ChangeDescriptionNormal();
        cd_.dr_.changeAtkRange();
        cd_.level = 0;
        if (cd_.elitismLevel == 1)
            AudioManager.OperatorTalk(od_.ElitismUP[0]);
        else AudioManager.OperatorTalk(od_.ElitismUP[1]);
    }
}
