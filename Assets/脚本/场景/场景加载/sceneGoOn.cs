using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneGoOn : MonoBehaviour
{
    public SceneNum toScene;
    public void ChangeScene()
    {
        SceneManager.LoadScene((int) toScene);
    }
}

public enum SceneNum : byte
{
    [EnumLabel("主界面")]
    mainScene,
    [EnumLabel("0-1")]
    level_0_1
    
    
}