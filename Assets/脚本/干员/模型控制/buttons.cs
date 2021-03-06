using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class buttons : MonoBehaviour
{
    private operControl oc_;
    private operData od_;
    
    void Start()
    {
        oc_ = transform.parent.GetComponent<operControl>();
        od_ = oc_.od_;
    }


    public void retreatBotton()
    {
        oc_.retreat(true, false);
    }

    public void safeBotton()
    {
        oc_.dontCloseSUI();
    }

    public void openBotton()
    {
        oc_.OpenSUI();
        if (od_.Selected.Count == 0) return;
        int index = Random.Range(0, od_.Selected.Count);
        AudioManager.OperatorTalk(od_.Selected[index]);
    }

}
