using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class EnemyFarAtk : MonoBehaviour
{
    private Transform prt;
    private enemyControl ec_;
    private enemyFight ef_;
    
    void Start()
    {
        prt = transform.parent;
        ec_ = prt.GetComponent<enemyControl>();
        ef_ = prt.GetComponent<enemyFight>();
        ef_.efa_ = this;
    }

    public void pretendEnter(GameObject oper)
    {
        if (oper.CompareTag("operator"))
        {
            if (ef_.atkRangeList.Contains(oper)) return;
            operControl oc_ = oper.GetComponent<operControl>();

            if (ef_.atkRangeListOC.Count > 0 && ef_.atkRangeListOC[0].priority < oc_.priority)
            {
                ef_.atkRangeList.Add(ef_.atkRangeList[0]);
                ef_.atkRangeListOC.Add(ef_.atkRangeListOC[0]);
                ef_.atkRangeList[0] = oper;
                ef_.atkRangeListOC[0] = oc_;
            }
            else
            {
                ef_.atkRangeList.Add(oper);
                ef_.atkRangeListOC.Add(oc_);
            }
            oc_.operDie += DoWhenOperDie_FarAtk;
        }
    }

    public void pretendExit(GameObject oper)
    {
        if (!oper.transform.CompareTag("operator")) return;
        if (ef_.atkRangeList.Contains(oper.gameObject))
        {
            int id = ef_.atkRangeList.IndexOf(oper);
            operControl oc_ = ef_.atkRangeListOC[id];
            oc_.operDie -= DoWhenOperDie_FarAtk;
            
            if (ef_.fightTar == oper.gameObject)
            {
                ef_.fightTar = ef_.Empty;
            }
            
            if (id != 0)
            {
                ef_.atkRangeList.RemoveAt(id);
                ef_.atkRangeListOC.RemoveAt(id);
            }
            else
            {
                ZeroDelAndFindMaxFollow();
            }
        }
    }

    public void ZeroDelAndFindMaxFollow()
    {
        int i, maxid = 0, maxx = -1;
        for (i = 1; i < ef_.atkRangeListOC.Count; i++)
        {
            if (ef_.atkRangeListOC[i].priority > maxx)
            {
                maxx = ef_.atkRangeListOC[i].priority;
                maxid = i;
            }
        }
        ef_.atkRangeList[0] = ef_.atkRangeList[maxid];
        ef_.atkRangeListOC[0] = ef_.atkRangeListOC[maxid];
        ef_.atkRangeList.RemoveAt(maxid);
        ef_.atkRangeListOC.RemoveAt(maxid);
    }

    private void OnTriggerEnter(Collider other)
    {
        pretendEnter(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        pretendExit(other.gameObject);
    }
    
    public void DoWhenOperDie_FarAtk(operControl oc)
    {
        if (ef_.tarOC_ == oc)
        {
            ef_.FightTar_Empty();
        }
        ef_.atkRangeList.Remove(oc.gameObject);
        ef_.atkRangeListOC.Remove(oc);
    }
    
}
