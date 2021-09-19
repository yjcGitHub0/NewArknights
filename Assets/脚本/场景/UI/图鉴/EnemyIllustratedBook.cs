using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyIllustratedBook : MonoBehaviour
{
    public enemyInfo defaultEnemyInfo;
    
    public gradualChange bookChooseUI;
    public gradualChange enemyBook;
    public Scrollbar leftScrollbar;
    public Scrollbar rightScrollbar;

    public enemyInfo showing_enemyInfo;
    public Image enemyImage;
    public Text NameText;
    public Text lifeText;
    public Text atkText;
    public Text defText;
    public Text magicDefText;
    public Text blockText;
    public Text expDropText;
    public Text costDropText;
    public Text speedText;
    public Text resistanceText;
    public Text minIntervalText;
    public Text talentText;
    public Image recoverTypeImage;
    public Image releaseTypeImage;
    public Text durationText;
    public Text initSPText;
    public Text maxSPText;
    public Text skillNameText;
    public Text skillDescriptionText;
    
    public void OpenEnemyBook()
    {
        bookChooseUI.Hide();
        enemyBook.Show();
        leftScrollbar.value = rightScrollbar.value = 1;
        ChangeShowingEI(defaultEnemyInfo);
    }
    
    public void CloseEnemyBook()
    {
        enemyBook.Hide();
        bookChooseUI.Show();
    }
    
    public void ChangeShowingEI(enemyInfo newEI)
    {
        showing_enemyInfo = newEI;
        enemyImage.sprite = newEI.headImage;
        NameText.text = newEI.name;
        lifeText.text = newEI.life.ToString("f0");
        atkText.text = newEI.atk.ToString("f0");
        defText.text = newEI.def.ToString("f0");
        magicDefText.text = newEI.magicDef.ToString("f0");
        blockText.text = newEI.consumeBlock.ToString();
        costDropText.text = newEI.dropCost.ToString();
        expDropText.text = newEI.dropExp.ToString();
        speedText.text = newEI.speed.ToString("f1");
        resistanceText.text = newEI.fearResistance.ToString();
        minIntervalText.text = newEI.minAtkInterval.ToString("f1");
        talentText.text = newEI.talentDescription;
        recoverTypeImage.sprite = IllustratedBookManager.recoverTypeSprite(newEI.skill_recoverType);
        releaseTypeImage.sprite = IllustratedBookManager.releaseTypeSprite(releaseType.auto);
        durationText.text = newEI.during.ToString("f0");
        initSPText.text = newEI.startSP.ToString("f0");
        maxSPText.text = newEI.maxSP.ToString("f0");
        skillNameText.text = newEI.skillName;
        skillDescriptionText.text = newEI.skillDescription;
    }
    
}
