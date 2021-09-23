using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MolotovCocktail : MonoBehaviour
{
    private enemyControl ec_;
    private enemyFight ef_;
    private operControl tarOC;
    private bool atkSp;

    private parabola _parabola;
    private bool tarIsNull;
    private Vector3 tarPos = new Vector3();
    public Transform target_trans;
    
    void Awake()
    {
        _parabola = GetComponent<parabola>();
    }

    public void Init(enemyControl EC,operControl TarOC, bool AtkSp)
    {
        ec_ = EC;
        ef_ = ec_.ef_;
        tarOC = TarOC;
        atkSp = AtkSp;
        if (tarOC == null)
        {
            tarIsNull = true;
            gameObject.SetActive(false);
        }
        else
        {
            target_trans = tarOC.transform;
            tarPos = target_trans.position;
            tarIsNull = false;
            _parabola.Init(target_trans, tarPos, tarIsNull);
            tarOC.operDie += _parabola.DoWhenTarNull;
        }
        
    }
    
    void Update()
    {
        if (_parabola.getIt)
        {
            ef_.atk_.causePhyDamage(ef_.atk_.atk, tarOC.life_, atkSp);
            tarOC.operDie -= _parabola.DoWhenTarNull;
            gameObject.SetActive(false);
        }
    }
}
