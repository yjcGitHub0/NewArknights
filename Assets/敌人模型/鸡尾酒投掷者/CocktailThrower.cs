using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CocktailThrower : MonoBehaviour
{
    private GameObject fa;
    private enemyControl ec_;
    private enemyFight ef_;
    private spController sp_;

    void Start()
    {
        fa = transform.parent.gameObject;
        ec_ = fa.GetComponent<enemyControl>();
        ef_ = fa.GetComponent<enemyFight>();
        sp_ = ec_.sp_;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void attack()
    {
        if (sp_.sp < sp_.maxSP)
        {
            GameObject cock = poolManager.Molotov_Cocktail();
            cock.transform.position = transform.position;
            MolotovCocktail mo_ = cock.GetComponent<MolotovCocktail>();
            mo_.Init(ec_, ef_.tarOC_, true);
        }
        else
        {
            sp_.useSkill(0);
            for (int i = 0; i < ef_.atkRangeListOC.Count; i++)
            {
                operControl tar = ef_.atkRangeListOC[i];
                GameObject cock = poolManager.Molotov_Cocktail();
                cock.transform.position = transform.position;
                MolotovCocktail mo_ = cock.GetComponent<MolotovCocktail>();
                mo_.Init(ec_, tar, false);
            }
            
        }
    }
}
