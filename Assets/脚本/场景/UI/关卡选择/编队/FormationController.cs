using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FormationController : MonoBehaviour
{
    public gradualChange displayUI;
    public gradualChange chooseUI;
    public OperChooseLeftInfo oci_;
    public operDisplayButton[] displayButtons = new operDisplayButton[12];
    public Button[] formationButtons = new Button[4];
    private Color blue = new Color(0, 0.666f, 1, 1);

    public void ChooseFormation(int num)
    {
        formationButtons[gameManager.formationNum].image.color = Color.white;
        formationButtons[num].image.color = blue;
        gameManager.formationNum = num;
        for (int i = 0; i < 12; i++)
        {
            if (i >= gameManager.formation[num].Count)
                displayButtons[i].ChangeOD(null);
            else
                displayButtons[i].ChangeOD(gameManager.formation[num][i]);
        }
    }

    public void LeftFormation()
    {
        if (gameManager.formationNum == 0) return;
        ChooseFormation(gameManager.formationNum - 1);
    }
    public void RightFormation()
    {
        if (gameManager.formationNum == 3) return;
        ChooseFormation(gameManager.formationNum + 1);
    }

    public void OpenChooseUI()
    {
        displayUI.Hide();
        chooseUI.Show();
        oci_.RemoveOD();
        oci_.AdaptFormation();
    }
    public void OpenChooseUI(operData od)
    {
        displayUI.Hide();
        chooseUI.Show();
        oci_.ChangeOD(od);
        oci_.AdaptFormation();
    }
    
    public void CloseChooseUI()
    {
        displayUI.Show();
        chooseUI.Hide();
        ChooseFormation(gameManager.formationNum);
    }
    
    
}
