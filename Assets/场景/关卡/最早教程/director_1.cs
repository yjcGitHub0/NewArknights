using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class director_1 : MonoBehaviour
{
    public operData noircOD;
    public operData kroosOD;
    public operData stewardOD;
    public operData anselOD;
    
    public initManager im_;
    public GameObject dialog;
    public Image amiyaImage;
    public Image noircImage;
    public Image kroosImage;
    public Image stewardImage;
    public Image anselImage;
    public Text dialogText;
    public GameObject circle_1;
    public GameObject circle_2;
    public GameObject circle_3;
    public guidePoint guidePoint_1;
    public enemyControl thug_1;
    public GameObject blackHole_1;
    public GameObject blackHole_2;

    private int index = 0;
    private string dialogString;
    private int sPoint;
    private int textDelay;
    private bool indexFinish;
    private Action staAction;
    
    void Start()
    {
        index = 0;
        staAction = Sta_1;
    }

    // Update is called once per frame
    void Update()
    {
        staAction();
        UpdateDialogText();
    }

    void ChangeHeadImage(Image showing)
    {
        amiyaImage.gameObject.SetActive(false);
        noircImage.gameObject.SetActive(false);
        kroosImage.gameObject.SetActive(false);
        stewardImage.gameObject.SetActive(false);
        anselImage.gameObject.SetActive(false);
        showing.gameObject.SetActive(true);
    }

    void UpdateDialogText()
    {
        if (sPoint >= dialogString.Length) return;
        if (Input.GetMouseButtonDown(0))
        {
            dialogText.text = dialogString;
            sPoint = dialogString.Length;
            return;
        }
        textDelay++;
        if (textDelay <= 12) return;
        textDelay = 0;
        dialogText.text += dialogString[sPoint];
        sPoint++;
    }
    void ChangeDialogText(string s)
    {
        dialogString = s;
        sPoint = 0;
        dialogText.text = "";
        textDelay = 10;
    }
    bool IndexFrame(Action toDo, Func<bool> nxt)
    {
        if (!indexFinish)
        {
            toDo();
            indexFinish = true;
        }
        if (nxt())
        {
            index++;
            indexFinish = false;
            return true;
        }
        return false;
    }



    void Sta_1()
    {
        switch (index)
        {
            case 0:
                IndexFrame(
                    () =>
                    {
                        ChangeDialogText("?????????????????????????????????");
                        gameManager.ForcePause();
                        im_.redDoorWaveList[0].gc_.ImmediateHide();
                    },
                    () => (Input.GetMouseButtonDown(0) && sPoint >= dialogString.Length));
                break;
            case 1:
                IndexFrame(
                    () => { ChangeDialogText("???????????????????????????????????????????????????????????????????????????????????????"); },
                    () => (Input.GetMouseButtonDown(0) && sPoint >= dialogString.Length));
                break;
            case 2:
                IndexFrame(
                    () =>
                    {
                        ChangeDialogText("????????????????????????");
                        circle_1.SetActive(true);
                    },
                    () => (Input.GetMouseButtonDown(0) && sPoint >= dialogString.Length));
                break;
            case 3:
                IndexFrame(
                    () =>
                    {
                        ChangeDialogText("?????????????????????????????????????????????????????????????????????????????????");
                        circle_1.SetActive(false);
                        circle_2.SetActive(true);
                    },
                    () => (Input.GetMouseButtonDown(0) && sPoint >= dialogString.Length));
                break;
            case 4:
                IndexFrame(
                    () =>
                    {
                        ChangeDialogText("?????????????????????");
                        circle_2.SetActive(false);
                        im_.NextWaveStart();
                        gameManager.EndForcePause();
                    },
                    () => (sPoint >= dialogString.Length && 
                           (Vector2)guidePoint_1.transform.position == new Vector2(5.5f,5.5f)));
                break;
            case 5:
                IndexFrame(
                    () =>
                    {
                        dialog.SetActive(false);
                        index = -1;
                        staAction = Sta_2;
                    },
                    () => (true));
                break;
        }
    }
    void Sta_2()
    {
        switch (index)
        {
            case 0:
                IndexFrame(
                    () => {},
                    () => (thug_1.transform.position.x > -4.49f));
                break;
            case 1:
                IndexFrame(
                    () =>
                    {
                        dialog.SetActive(true);
                        ChangeDialogText("???????????????????????????????????????????????????????????????");
                        circle_3.SetActive(true);
                        gameManager.ForcePause();
                    },
                    () => (Input.GetMouseButtonDown(0) && sPoint >= dialogString.Length));
                break;
            case 2:
                IndexFrame(
                    () =>
                    {
                        ChangeDialogText("??????????????????????????????????????????");
                        circle_3.SetActive(false);
                    },
                    () => (Input.GetMouseButtonDown(0) && sPoint >= dialogString.Length));
                break;
            case 3:
                IndexFrame(
                    () =>
                    {
                        ChangeHeadImage(noircImage);
                        ChangeDialogText("????????????????????????????????????????????????????????????");
                    },
                    () => (Input.GetMouseButtonDown(0) && sPoint >= dialogString.Length));
                break;
            case 4:
                IndexFrame(
                    () =>
                    {
                        ChangeHeadImage(amiyaImage);
                        ChangeDialogText("???????????????????????????????????????");
                        im_.AddNewOperSlot(noircOD);
                    },
                    () => (Input.GetMouseButtonDown(0) && sPoint >= dialogString.Length));
                break;
            case 5:
                IndexFrame(
                    () =>
                    {
                        blackHole_1.SetActive(true);
                        ChangeDialogText("????????????????????????????????????????????????????????????");
                    },
                    () => (Input.GetMouseButtonDown(0) && sPoint >= dialogString.Length));
                break;
            case 6:
                IndexFrame(
                    () =>
                    {
                        blackHole_1.SetActive(false);
                        blackHole_2.SetActive(true);
                        ChangeDialogText("?????????????????????????????????");
                    },
                    () => (Input.GetMouseButtonDown(0) && sPoint >= dialogString.Length));
                break;
            case 7:
                IndexFrame(
                    () =>
                    {
                        blackHole_2.SetActive(false);
                        ChangeDialogText("??????????????????????????????????????????????????????????????????");
                    },
                    () => (Input.GetMouseButtonDown(0) && sPoint >= dialogString.Length));
                break;
        }
    }
    
    
}
