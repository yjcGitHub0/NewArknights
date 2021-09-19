using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OperChoose : MonoBehaviour
{
    public operData od_;
    private Image operImage;
    public GameObject blueBack;
    public Image professionImage;
    public Text Name;
    
    private OperIllustratedBook oib_;
    public OperChooseLeftInfo oli_ins;
    public bool isChoosed;
    
    void Start()
    {
        operImage = GetComponent<Image>();
        operImage.sprite = od_.illustratedBookImage;
        professionImage.sprite = gameManager.GetProfessionImage(od_.profession);
        Name.text = od_.Name;
        oib_ = IllustratedBookManager.oib_;
        if (oli_ins != null)
            oli_ins.formationChooseList.Add(this);
    }

    public void chooseOper_Book()
    {
        oib_.ChangeShowingOD(od_);
        AudioManager.OperatorTalk(od_.Touch);
    }

    public void chooseOper_formation()
    {
        if (isChoosed)//原被选中，取消选中
        {
            FormationRemove();
            oli_ins.RemoveOD();
            gameManager.formation[gameManager.formationNum].Remove(od_);
        }
        else//原未被选中，现选中
        {
            int index = Random.Range(0, od_.Join.Count);
            AudioManager.OperatorTalk(od_.Join[index]);
            FormationChoose();
            oli_ins.ChangeOD(od_);
            gameManager.formation[gameManager.formationNum].Add(od_);
        }
    }

    public void FormationChoose()
    {
        blueBack.SetActive(true);
        isChoosed = true;
    }
    public void FormationRemove()
    {
        blueBack.SetActive(false);
        isChoosed = false;
    }
    
}
