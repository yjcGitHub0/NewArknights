using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AsynchronousSwitch : MonoBehaviour
{
    private float TOLERANCE = 1e-3f;
    private gradualChange image;   //加载界面
    private Slider slider;   //进度条
    private Text text;      //加载进度文本
    public SceneNum sceneID;
    private AsyncOperation operation;
    private bool is90 = false;
    private float maxTime = 2f;

    private void Start()
    {
        image = IllustratedBookManager.AsyScene;
        slider = IllustratedBookManager.AsySlider;
        text = IllustratedBookManager.AsyText;
    }

    public void LoadNextLeaver()
    {
        image.Show();
        is90 = false;
        StartCoroutine(LoadLeaver());
    }
    IEnumerator LoadLeaver()
    {   
        operation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1); //获取当前场景并加一
        operation.allowSceneActivation = false;
        float speed = 0.9f / maxTime;
        float sum = 0.1f;
        while(!operation.isDone)   //当场景没有加载完毕
        {
            if(!is90)
                sum += speed * Time.deltaTime;
            float value = Math.Min(operation.progress + 0.1f, sum);
            slider.value = value;  //进度条与场景加载进度对应
            text.text = (value * 100).ToString("f0") + "%";
            if (!is90 && Math.Abs(value - 1f) < TOLERANCE)
            {
                is90 = true;
                operation.allowSceneActivation = true;
                image.Hide();
            }
            yield return null;
        }
    }

}
