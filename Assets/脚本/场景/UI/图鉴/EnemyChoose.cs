using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyChoose : MonoBehaviour
{
    public enemyInfo ei_;
    private Image enmeyImage;

    private EnemyIllustratedBook eib_;
    
    void Start()
    {
        enmeyImage = GetComponent<Image>();
        enmeyImage.sprite = ei_.headImage;
        eib_ = IllustratedBookManager.eib_;
    }

    public void chooseEnemy()
    {
        eib_.ChangeShowingEI(ei_);
    }
}
