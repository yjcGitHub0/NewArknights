using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CalUICon : MonoBehaviour
{
    private initManager im_;
    private UIconnect uc_;
    
    private lifeController life_;
    private spController sp_;
    
    public bool skillDuring;
    private float graySpeed = 0.01f;
    public Slider blueLifeSlider;
    public Slider grayLifeSlider;
    public Slider skillSlider;
    public Image skillSliderFillImage;

    private Color norCol = new Color32(200,255,0,180);
    private Color durCol = new Color32(255,220,0,180);
    
    void Start()
    {
        im_ = gameManager.im_;
        uc_ = im_.uc_;
        Transform prt = transform.parent;
        blueLifeSlider.value = 1f;
        grayLifeSlider.value = 1f;
        skillSlider.value = 0f;
        life_ = GetComponent<lifeController>();
        sp_ = GetComponent<spController>();
    }


    void Update()
    {
        blueLifeSlider.value = life_.maxLife == 0 ? 1 : life_.life / life_.maxLife;
        ChangeGraySliderValue();
        ChangeSkillSliderValue();
        pdSkill();
    }
    
    void pdSkill()
    {
        skillDuring = sp_.skillDuring > 0;
    }
    
    void ChangeGraySliderValue()
    {
        float v = life_.maxLife == 0 ? 1 : life_.life / life_.maxLife;
        if (grayLifeSlider.value - graySpeed >= v)
        {
            grayLifeSlider.value -= graySpeed;
        }
        else grayLifeSlider.value = v;
    }

    void ChangeSkillSliderValue()
    {
        if (skillDuring)
        {
            skillSliderFillImage.color = durCol;
            skillSlider.value = sp_.maxSkillDuring == 0 ? 1 : sp_.skillDuring / sp_.maxSkillDuring;
        }
        else
        {
            skillSliderFillImage.color = norCol;
            skillSlider.value = sp_.maxSP == 0 ? 1 : sp_.sp / sp_.maxSP;
        }
    }
    
}
