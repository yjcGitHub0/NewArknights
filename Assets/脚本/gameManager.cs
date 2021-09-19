using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class gameManager : MonoBehaviour
{
    private static gameManager instance;
    public static int formationNum;
    public static initManager im_;
    public Sprite[] professionImage = new Sprite[8];
    public static bool pause;
    public static bool twoFast;
    public static List<operData>[] formation = new List<operData>[4];
///////////////////////////////bug表演系要///////////////////////////
    /// /////////////////////////////////////////////////////////////////

private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this);
        formationNum = 0;
        for (int i = 0; i < 4; i++)
            formation[i] = new List<operData>();
    }

    void Start()
    {
        SceneManager.sceneLoaded += EndTwoFast;
        SceneManager.sceneLoaded += EndPause;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static GameObject creatOper(currentData thisOper)
    {
        //初始化干员prefab
        GameObject operPrefab;
        GameObject atkRange;
        GameObject operAnim;
        operPrefab = Instantiate(thisOper.od_.operPrefab, null, true);

        //初始化攻击范围
        atkRange = Instantiate(thisOper.od_.atkRange[thisOper.elitismLevel], operPrefab.transform, true);
        atkRange.name = "atkRange";
        Vector3 atkPos = Vector3.zero;
        
        atkRange.transform.localPosition = atkPos;

        operPrefab.GetComponent<operControl>().atkRange = atkRange;
        
        return operPrefab;
    }

    public static GameObject creatEnemy(enemyInfo thisEnemy)
    {
        GameObject enemyPrefab;

        //初始化敌人prefab
        enemyPrefab = Instantiate(thisEnemy.enemyPrefab, null, true);
        enemyPrefab.GetComponent<enemyControl>().ei_ = thisEnemy;

        return enemyPrefab;
    }

    public static Sprite GetProfessionImage(ProfessionType t)
    {
        return instance.professionImage[(int) t];
    }

    public static void Pause()
    {
        pause = true;
        Time.timeScale = 0;
    }
    public static void EndPause()
    {
        pause = false;
        Time.timeScale = twoFast ? 2 : 1;
    }
    public static void EndPause(Scene arg0, LoadSceneMode arg1)
    {
        EndPause();
    }
    public static void TwoFast()
    {
        twoFast = true;
        if (pause) return;
        Time.timeScale = 2;
    }
    public static void EndTwoFast()
    {
        twoFast = false;
        if (pause) return;
        Time.timeScale = 1;
    }
    public static void EndTwoFast(Scene arg0, LoadSceneMode arg1)
    {
        EndTwoFast();
    }
}

public enum ProfessionType : byte
{
    [EnumLabel("先锋")]
    xianfeng,
    [EnumLabel("狙击")]
    juji,
    [EnumLabel("医疗")]
    yiliao,
    [EnumLabel("术士")]
    shushi,
    [EnumLabel("近卫")]
    jinwei,
    [EnumLabel("重装")]
    zhongzhuang,
    [EnumLabel("辅助")]
    fuzhu,
    [EnumLabel("特种")]
    tezhong
}