using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lockLife : MonoBehaviour
{
    private lifeController life_;

    private float lockLine;
    private float duration;
    
    
    void Start()
    {
        
    }

    public void Init(lifeController Life_, float LockLine, float Duration)
    {
        lockLine = LockLine;
        duration = Duration;
        
        life_ = Life_;
        life_.SetLockLine(lockLine);
    }

    // Update is called once per frame
    void Update()
    {
        duration -= Time.deltaTime;
        if (duration <= 0)
            Invalid();
    }

    void Invalid()
    {
        life_.SetLockLine(0);
        gameObject.SetActive(false);
    }
}
