using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IllustratedBookManager : MonoBehaviour
{
    private static IllustratedBookManager instance;

    public gradualChange OperOREnemyChooseUI;
    public static OperIllustratedBook oib_;
    public static EnemyIllustratedBook eib_;

    
    public GameObject operChoose;
    public GameObject enemyChoose;
    public List<operData> AllOperData = new List<operData>();
    public List<enemyInfo> AllEnemyInfo = new List<enemyInfo>();
    public static List<operData> showingOperList = new List<operData>();
    public static List<enemyInfo> showingEnemyList = new List<enemyInfo>();
    public static List<GameObject> operOBList = new List<GameObject>();
    public static List<GameObject> enemyOBList = new List<GameObject>();

    public gradualChange asyScene;   //加载界面
    public Slider asySlider;   //进度条
    public Text asyText;      //加载进度文本
    public static gradualChange AsyScene;
    public static Slider AsySlider;
    public static Text AsyText;
    
    [Header("精英化和回复释放类型贴图")] 
    public Sprite Elitism0;
    public Sprite Elitism1;
    public Sprite Elitism2;
    public Sprite AutoRecover;
    public Sprite AtkRecover;
    public Sprite BeAtkRecover;
    public Sprite HandRelease;
    public Sprite AutoRelease;
    public Sprite PassiveSkill;
    public static Sprite[] elitismImage = new Sprite[3];
    public static Sprite autoRecover;
    public static Sprite atkRecover;
    public static Sprite beAtkRecover;
    public static Sprite handRelease;
    public static Sprite autoRelease;
    public static Sprite passiveSkill;

    [Header("图鉴")] 
    public gradualChange IllustratedBook;
    public GameObject operChoosePrt;
    public GameObject enemyChoosePrt;
    
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this);
        oib_ = instance.GetComponent<OperIllustratedBook>();
        eib_ = instance.GetComponent<EnemyIllustratedBook>();
        AsyScene = asyScene;
        AsySlider = asySlider;
        AsyText = asyText;
    }
    
    void Start()
    {
        elitismImage[0] = instance.Elitism0;
        elitismImage[1] = instance.Elitism1;
        elitismImage[2] = instance.Elitism2;
        autoRecover = instance.AutoRecover;
        atkRecover = instance.AtkRecover;
        beAtkRecover = instance.BeAtkRecover;
        handRelease = instance.HandRelease;
        autoRelease = instance.AutoRelease;
        passiveSkill = instance.PassiveSkill;
        
        ChangeBookPrefab(instance.AllOperData, instance.AllEnemyInfo);
        SceneManager.sceneLoaded += ChangeScene0;
    }
    public static void ChangeBookPrefab(List<operData> odList, List<enemyInfo> eiList)
    {
        showingOperList.Clear();
        showingEnemyList.Clear();
        foreach (var i in odList)
            showingOperList.Add(i);
        foreach (var i in eiList)
            showingEnemyList.Add(i);

        foreach (var i in operOBList)
            Destroy(i);
        foreach (var i in enemyOBList)
            Destroy(i);
        operOBList.Clear();
        enemyOBList.Clear();
        
        foreach (var i in showingOperList)
        {
            GameObject ch = Instantiate(instance.operChoose, instance.operChoosePrt.transform);
            OperChoose oc = ch.GetComponent<OperChoose>();
            oc.od_ = i;
            ch.transform.SetParent(instance.operChoosePrt.transform);
            operOBList.Add(ch);
        }

        foreach (var i in showingEnemyList)
        {
            GameObject ch = Instantiate(instance.enemyChoose, instance.enemyChoosePrt.transform);
            EnemyChoose ec = ch.GetComponent<EnemyChoose>();
            ec.ei_ = i;
            ch.transform.SetParent(instance.enemyChoosePrt.transform);
            enemyOBList.Add(ch);
        }
    }
    public static void ChangeScene0(Scene arg0, LoadSceneMode arg1)
    {
        if (SceneManager.GetActiveScene().buildIndex != 0) return;
        ChangeBookPrefab(instance.AllOperData, instance.AllEnemyInfo);
    }

    public static Sprite recoverTypeSprite(recoverType type)
    {
        switch (type)
        {
            case recoverType.auto:
                return autoRecover;
            case recoverType.atk:
                return atkRecover;
            case recoverType.beAtk:
                return beAtkRecover;
            default:
                return null;
        }
    }

    public static Sprite releaseTypeSprite(releaseType type)
    {
        switch (type)
        {
            case releaseType.auto:
                return autoRelease;
            case releaseType.hand:
                return handRelease;
            case releaseType.passive:
                return passiveSkill;
            default:
                return null;
        }
    }

    public static void OpenIllustratedBook()
    {
        Time.timeScale = 0;
        oib_.closeAll = false;
        instance.IllustratedBook.Show();
        oib_.CloseOperBook(true);   
        eib_.CloseEnemyBook();
    }
    
    public static void OpenIllustratedBook(operData od_)
    {
        Time.timeScale = 0;
        oib_.closeAll = true;
        instance.IllustratedBook.Show();
        oib_.OpenOperBook(true);
        oib_.ChangeShowingOD(od_);
    }

    public static void CloseIllustratedBook()
    {
        if (gameManager.pause)
            Time.timeScale = 0;
        else if (gameManager.twoFast)
            Time.timeScale = 2;
        else 
            Time.timeScale = 1;
        instance.IllustratedBook.Hide();
    }
    
}
