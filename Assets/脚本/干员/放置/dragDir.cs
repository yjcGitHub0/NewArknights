using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class dragDir : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler
{

    public GameObject slot;
    public GameObject imageInDrag;
    public GameObject atkRange;
    public float stripe_lowZ;
    public float stripe_highZ;
    private Vector2 prePos = new Vector2();
    private operData od_;
    private SpriteRenderer sr_;
    private colConrtol cc_;
    public Drag drag_;
    private initManager im_;
    private Vector3 tiltRolNormal = new Vector3(-25f,0f,0f);
    private Vector3 tiltRolBack = new Vector3(25f,180f,0f);
    private Vector3 rolForword = new Vector3(0f,0f,0f);
    private Vector3 rolBack = new Vector3(0f,0f,180f);
    private Vector3 rolLeft = new Vector3(0f,0f,90f);
    private Vector3 rolRight = new Vector3(0f,0f,-90f);
    private Vector3 baseRol = new Vector3();
    private Vector3 rangeRot = new Vector3();
    private Vector3 nowRangeRot = new Vector3();
    public int sta;
    public int preSta;
    
    public GameObject operPrefab;
    public specialEvent se_;

    private bool is3D;
    private bool draging;
    
    private void Awake()
    {
        drag_ = slot.GetComponent<Drag>();
        is3D = drag_.is3D;
        od_ = slot.GetComponent<Drag>().thisOper;
        if(!is3D)
            sr_ = imageInDrag.transform.Find("image").GetComponent<SpriteRenderer>();
        atkRange = drag_.atkRange;
        cc_ = atkRange.GetComponent<colConrtol>();
        im_ = GameObject.Find("initManager").GetComponent<initManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!draging) return;
        pdSta();
        changeImage();
    }
    
    public void getPrePos()
    {
        prePos = transform.position;
    }

    public void updateColControl()
    {
        
        atkRange=drag_.atkRange;
        cc_ = atkRange.GetComponent<colConrtol>();
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        draging = true;
        cc_.showRange = true;
        cc_.showStripe(stripe_highZ,stripe_lowZ);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = fixCoordinate(eventData.position);
        //transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        draging = false;
        if (!pdOut(transform.position))
        {
            transform.position = prePos;
        }
        else//放置成功
        {
            im_.draging = false;
            im_.timeRecover();
            //一些通用限制
            im_.cost_.UseCost(drag_.cd_.cost);
            im_.remainPlace -= od_.consumPlace;
            
            //创建干员prefab
            operPrefab = gameManager.creatOper(drag_.cd_);
            se_ = operPrefab.GetComponent<specialEvent>();
            
            operControl oc_ = operPrefab.GetComponent<operControl>();
            oc_.slot = slot;
            oc_.dr_ = drag_;
            oc_.Data = drag_.Data;
            oc_.cd_ = drag_.cd_;
            oc_.is3D = drag_.is3D;
            oc_.od_ = od_;
            oc_.AppearIn(imageInDrag.transform.position,baseRol,sta == 0,rangeRot);

            drag_.operPrefab = operPrefab;
            
            drag_.Reverse(-1, is3D);
        }
    }

    Vector2 fixCoordinate(Vector2 pos)
    {

        Vector2 dpos = new Vector2();
        dpos.x = Mathf.Abs(pos.x - prePos.x);
        dpos.y = Mathf.Abs(pos.y - prePos.y);

        if (dpos.x + dpos.y < 120f) return pos;

        dpos.x = (120f * dpos.x) / (dpos.x + dpos.y);
        dpos.y = 120f - dpos.x;

        if (pos.x - prePos.x < 0) dpos.x = -dpos.x;
        if (pos.y - prePos.y < 0) dpos.y = -dpos.y;

        dpos += prePos;
        return dpos;
    }

    bool pdOut(Vector2 pos)
    {
        Vector2 dpos = new Vector2();
        dpos.x = Mathf.Abs(pos.x - prePos.x);
        dpos.y = Mathf.Abs(pos.y - prePos.y);

        if (dpos.x + dpos.y < 100f) return false;
        return true;
    }

    void pdSta()
    {
        int dSta;
        float dx = transform.position.x - prePos.x;
        float dy = transform.position.y - prePos.y;
        
        if (dy > dx && dy > -dx) dSta = 0;
        else if (dy > dx && dy < -dx) dSta = 1;
        else if (dy < dx && dy < -dx) dSta = 2;
        else dSta = 3;

        if (dSta != sta)
        {
            preSta = sta;
            sta = dSta;
        }
    }

    void changeImage()
    {
        switch (sta)
        {
            case 0:
                rangeRot = rolLeft;
                if (!is3D)
                    sr_.sprite = od_.imageInDragBack;
                if (preSta == 3) baseRol = tiltRolNormal;
                else baseRol = tiltRolBack;
                break;
            case 1:
                rangeRot = rolBack;
                if (!is3D)
                    sr_.sprite = od_.imageInDrag;
                baseRol = tiltRolBack;
                break;
            case 2:
                rangeRot = rolRight;
                if (!is3D)
                    sr_.sprite = od_.imageInDrag;
                if (preSta == 1) baseRol = tiltRolBack;
                else baseRol = tiltRolNormal;
                break;
            case 3:
                rangeRot = rolForword;
                if (!is3D)
                    sr_.sprite = od_.imageInDrag;
                baseRol = tiltRolNormal;
                break;
            default:
                break;
        }

        if (!is3D)
        {
            imageInDrag.transform.rotation = Quaternion.Euler(baseRol);
        }

        if (nowRangeRot != rangeRot)
        {
            atkRange.transform.rotation = Quaternion.Euler(rangeRot);
            nowRangeRot = rangeRot;
            cc_.showStripe(stripe_highZ, stripe_lowZ);
        }

    }

}
