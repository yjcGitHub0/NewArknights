using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Drag : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler
{
    public operData thisOper;
    public Image imageInQueue;
    public GameObject imageInDrag;
    public GameObject littleMouse;
    public GameObject verify;
    public GameObject frame;
    public GameObject squArr;
    public Text numText;
    public Text reTimeTest;
    public Text costText;
    public GameObject atkRange;
    public GameObject operPrefab;
    private Vector3 tiltRolNormal = new Vector3(-25f,0f,0f);
    
    public GameObject Data;
    public currentData cd_;
    private initManager im_;
    private UIconnect uc_;
    private littlemouse lm_;
    private dragDir dd_;
    private colConrtol cc_;
    private operControl oc_;
    private SpriteRenderer imageInDrag_sr_;
    private float reTime = 0;
    public bool is3D;

    private IBeginDragHandler m_BeginDragHandlerImplementation;
    private IDragHandler m_DragHandlerImplementation;
    private IEndDragHandler m_EndDragHandlerImplementation;

    private void Awake()
    {
        //初始化信息
        Data = transform.Find("Data").gameObject;
        cd_ = Data.GetComponent<currentData>();
        //cd_.od_ = thisOper;
    }

    private void Start()
    {
        
        is3D = thisOper.is3D;
        if(!is3D)
            imageInDrag_sr_ = imageInDrag.transform.Find("image").GetComponent<SpriteRenderer>();

        //初始化队列内图片以及拖拽时图片
        imageInQueue.sprite = thisOper.imageInQueue;
        littleMouse.transform.SetParent(null);
        littleMouse.transform.localScale=Vector3.one;
        if (!is3D)
            imageInDrag_sr_.sprite = thisOper.imageInDrag;
        
        //初始化攻击范围
        atkRange = Instantiate(thisOper.atkRange[cd_.elitismLevel], verify.transform, true);
        atkRange.transform.position = verify.transform.position;

        lm_ = littleMouse.GetComponent<littlemouse>();
        dd_ = squArr.GetComponent<dragDir>();
        cc_ = atkRange.GetComponent<colConrtol>();
        im_ = GameObject.Find("initManager").GetComponent<initManager>();
        uc_ = im_.uc_;
        
        lm_.dragEnd = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (reTime == 0)
        {
            imageInQueue.color = Color.white;
            reTimeTest.gameObject.SetActive(false);
        }
        else
        {
            reTime -= Time.deltaTime;
            if (reTime < 0) reTime = 0;
            reTimeTest.text = reTime.ToString("f2");
        }
    }

    public void changeAtkRange()
    {
        Destroy(atkRange);
        atkRange = Instantiate(thisOper.atkRange[cd_.elitismLevel], verify.transform, true);
        atkRange.transform.position = verify.transform.position;
        cc_ = atkRange.GetComponent<colConrtol>();
        dd_.drag_ = this;
        dd_.updateColControl();
    }

    public void changeText()
    {
        //改变numtext
        if (cd_.num == 1) numText.gameObject.SetActive(false);
        else numText.gameObject.SetActive(true);
        numText.text = cd_.num.ToString();
        
        //改cost
        costText.text = cd_.cost.ToString();
    }

    bool canPut()
    {
        if (reTime > 0) return false;
        if (im_.cost < cd_.cost) return false;
        if (im_.remainPlace < thisOper.consumPlace) return false;
        return true;
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!canPut()) return;
        uc_.OpenLeftUI(cd_);
        im_.draging = true;
        
        if (lm_.dragEnd) return;
        imageInQueue.gameObject.SetActive(false);
        littleMouse.SetActive(true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!canPut()) return;
        if (lm_.dragEnd) return;
        frame.transform.position = eventData.position;

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!canPut()) return;
        if (lm_.dragEnd) return;
        
        if (lm_.getIt)
        {
            lm_.dragEnd = true;
            squArr.transform.position = frame.transform.position;
            dd_.getPrePos();
            //cc_.showRange = true;
            frame.SetActive(true);
            squArr.SetActive(true);
        }
        else
        {
            //im_.draging = false;
            Reverse(0, false);
        }
    }

    public void Reverse(int detaNum,bool willRedeploy)
    {
        uc_.CloseLeftUI(cd_);
        im_.draging = false;

        imageInQueue.gameObject.SetActive(true);
        littleMouse.SetActive(false);
        frame.SetActive(false);
        squArr.SetActive(false);
        lm_.dragEnd = false;
        lm_.getIt = false;
        cc_.showRange = false;
        cc_.delStripe();
        if (!is3D)
        {
            imageInDrag_sr_.sprite = thisOper.imageInDrag;
            imageInDrag.transform.rotation = Quaternion.Euler(tiltRolNormal);
        }

        //再部署倒计时
        if (willRedeploy) Redeploy();


            if (detaNum == -1)
        {
            cd_.num--;
            changeText();
            if (cd_.num == 0)
            {
                gameObject.transform.SetParent(null);
                gameObject.SetActive(false);
            }
        }
        else if (detaNum == 1)
        {
            gameObject.SetActive(true);
            cd_.num++;
            changeText();
            gameObject.transform.SetParent(im_.operPanel.transform);
        }
    }

    public void Redeploy()
    {
        reTime = thisOper.reTime;
        reTimeTest.gameObject.SetActive(true);
        imageInQueue.color = Color.red;
    }

    public void pushDown()
    {
        uc_.OpenLeftUI(cd_);
    }
    
}
