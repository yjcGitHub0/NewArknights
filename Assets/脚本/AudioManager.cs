using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    public AudioMixer Mixer;
    public static bool coolDown;
    private static float coolTime = 0;
    
    public static AudioSource Operator;
    public static AudioSource BGM;
    
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this);
        
    }

    private void Start()
    {   
        coolDown = true;
        Operator = gameObject.AddComponent<AudioSource>();
        BGM = gameObject.AddComponent<AudioSource>();


        Operator.outputAudioMixerGroup = instance.Mixer.FindMatchingGroups("Voice")[0];
        BGM.outputAudioMixerGroup = instance.Mixer.FindMatchingGroups("BGM")[0];
    }
    private void Update()
    {
        if (coolTime > 0)
        {
            coolTime -= Time.deltaTime;
            if (coolTime <= 0)
            {
                coolDown = true;
            }
        }
    }

    public static void OperatorTalk(AudioClip talk)
    {
        Operator.clip = talk;
        Operator.Play();
    }
    
    public static void OperatorTalkAndClod(AudioClip talk)
    {
        Operator.clip = talk;
        Operator.Play();
        coolDown = false;
        coolTime = 5f;
    }


}
