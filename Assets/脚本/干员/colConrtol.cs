using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colConrtol : MonoBehaviour
{
    public bool showRange;
    public GameObject stripe;
    public List<Vector2> detaxy = new List<Vector2>();
    private List<GameObject> showing = new List<GameObject>();
    private initManager im_;
    private GameObject fa;
    private operFight of_;
    private operControl oc_;
    private operData od_;
    private bool ofIsNull;
    
    // Start is called before the first frame update
    void Start()
    {
        im_ = GameObject.Find("initManager").GetComponent<initManager>();
        ofIsNull = true;
        if (transform.parent.name != "verify")
        {
            fa = transform.parent.gameObject;
            oc_ = fa.GetComponent<operControl>();
            of_ = oc_.of_;
            od_ = oc_.od_;
            ofIsNull = false;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    float fixCoordinate(float x)
    {
        if (x > 0) return (int) x + 0.5f;
        else return (int) x - 0.5f;
    }
    
    public void showStripe(float highZ, float lowZ)
    {
        Vector3 pos = transform.position;
        pos.x = fixCoordinate(transform.position.x);
        pos.y = fixCoordinate(transform.position.y);
        transform.position = pos;
        
        delStripe();

        foreach (var i in detaxy)
        {
            
            GameObject newStripe = Instantiate(stripe, transform, true);
            newStripe.transform.localPosition = i;
            pos = newStripe.transform.position;

            if (tagManager.isEmpty(im_.getMp(pos))) 
            {
                
                Destroy(newStripe);
                continue;
            }

            if (tagManager.isHigh(im_.getMp(pos))) 
            {
                pos.z = highZ;
            }
            else pos.z = lowZ;
            
            newStripe.transform.position = pos;
            
            showing.Add(newStripe);
        }
    }

    public void delStripe()
    {
        foreach (var i in showing)
        {
            Destroy(i);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (ofIsNull) return;
        if (other.CompareTag("enemy"))
        {
            enemyControl ec_ = other.GetComponent<enemyControl>();
            enemyFight ef_ = ec_.ef_;
            if (of_.tarEnemyList.Contains(ef_)) return;
            enemyInfo ei_ = ec_.ei_;
            if (ei_.isDrone && !od_.canAtkDrone) return;
            of_.tarEnemyList.Add(ef_);
            ec_.enemyDie += of_.DoWhenEnemyDie;
        }
        else if (other.CompareTag("operator"))
        {
            operControl tarOC = other.GetComponent<operControl>();
            of_.tarOperList.Add(tarOC);
            tarOC.operDie += of_.DoWhenTarOperDie;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (ofIsNull) return;
        if (other.CompareTag("enemy"))
        {
            enemyControl ec_ = other.GetComponent<enemyControl>();
            enemyFight ef_ = ec_.ef_;
            of_.tarEnemyList.Remove(ef_);
            ec_.enemyDie -= of_.DoWhenEnemyDie;
        }
        else if (other.CompareTag("operator"))
        {
            operControl tarOC = other.GetComponent<operControl>();
            of_.tarOperList.Remove(tarOC);
            tarOC.operDie -= of_.DoWhenTarOperDie;
        }
    }
}
