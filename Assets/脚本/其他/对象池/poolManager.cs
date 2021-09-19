using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class poolManager : MonoBehaviour
{
    private static poolManager instance;
    
    [SerializeField] private pool kroos_arrow_pool;
    [SerializeField] private pool kroos_norBoom_pool;
    [SerializeField] private pool kroos_Crit_yellow_pool;
    [SerializeField] private pool Molotov_Cocktail_pool;
    [SerializeField] private pool Magic_bullet_pool;
    [SerializeField] private pool Ansel_Bandage_pool;
    [SerializeField] private pool Shining_Defense_pool;
    [SerializeField] private pool Track_Magic_Fire_pool;
    [SerializeField] private pool Surrounding_Particles_pool;
    [SerializeField] private pool Big_Track_Magic_Fire_pool;
    [SerializeField] private pool Steward_Boom_pool;
    [SerializeField] private pool Healing_pool;

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

    public static GameObject kroos_arrow()
    {
        return instance.kroos_arrow_pool.PrepareObject();
    }
    public static GameObject kroos_norBoom()
    {
        return instance.kroos_norBoom_pool.PrepareObject();
    }
    public static GameObject kroos_Crit_yellow()
    {
        return instance.kroos_Crit_yellow_pool.PrepareObject();
    }
    public static GameObject Molotov_Cocktail()
    {
        return instance.Molotov_Cocktail_pool.PrepareObject();
    }
    public static GameObject Magic_Bullet()
    {
        return instance.Magic_bullet_pool.PrepareObject();
    }
    public static GameObject Ansel_Bandage()
    {
        return instance.Ansel_Bandage_pool.PrepareObject();
    }
    public static GameObject Shining_Defense()
    {
        return instance.Shining_Defense_pool.PrepareObject();
    }
    public static GameObject Track_Magic_Fire()
    {
        return instance.Track_Magic_Fire_pool.PrepareObject();
    }
    public static GameObject Surrounding_Particles()
    {
        return instance.Surrounding_Particles_pool.PrepareObject();
    }
    public static GameObject Big_Track_Magic_Fire()
    {
        return instance.Big_Track_Magic_Fire_pool.PrepareObject();
    }
    public static GameObject Steward_Boom()
    {
        return instance.Steward_Boom_pool.PrepareObject();
    }
    public static GameObject Healing()
    {
        return instance.Healing_pool.PrepareObject();
    }
}
