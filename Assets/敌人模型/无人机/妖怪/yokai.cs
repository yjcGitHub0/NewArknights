using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class yokai : MonoBehaviour
{
    public float skillSpead;

    private GameObject fa;
    private enemyControl ec_;
    private enemyFight ef_;
    private spController sp_;
    private Animator anim;

    private float skillTime;
    private float maxSkillTime;

    void Start()
    {
        fa = transform.parent.gameObject;
        ec_ = fa.GetComponent<enemyControl>();
        ef_ = fa.GetComponent<enemyFight>();
        anim = GetComponent<Animator>();
        sp_ = ec_.sp_;
        maxSkillTime = ec_.ei_.during;
    }

    private void Update()
    {
        if (sp_.sp == sp_.maxSP)
        {
            sp_.useSkill(maxSkillTime);
            skillTime = maxSkillTime;
            ef_.unBlock++;
            ef_.FightTar_Empty();
            anim.SetInteger("sta", 1);
            ec_.speed = skillSpead;
        }
        if (skillTime > 0)
        {
            skillTime -= Time.deltaTime;
            if (skillTime <= 0)
            {
                anim.SetInteger("sta", 0);
                ec_.speed = ec_.ei_.speed;
                ef_.unBlock--;
            }
        }
    }

    public void attack()
    {
        ef_.atk_.causePhyDamage(ef_.atk_.atk, ef_.tarOC_.life_, true);
        GameObject boom = poolManager.kroos_norBoom();
        follow f_ = boom.GetComponent<follow>();
        f_.Init(ef_.tarOC_.transform);
    }
}
