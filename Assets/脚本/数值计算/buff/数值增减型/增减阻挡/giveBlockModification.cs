using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class giveBlockModification : MonoBehaviour
{
    private operFight of_;
    private int deBlock;
    private float duration;
    
    
    // Start is called before the first frame update
    void Start()
    {
        of_.maxBlock = of_.maxBlock + deBlock < 0 ? 0 : of_.maxBlock + deBlock;
    }

    public void Init(operFight OF, int DeBlock, float Duration)
    {
        of_ = OF;
        deBlock = DeBlock;
        duration = Duration;
        
        of_.maxBlock = of_.maxBlock + deBlock < 0 ? 0 : of_.maxBlock + deBlock;
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
        of_.maxBlock = of_.maxBlock - deBlock > of_.cd_.baseBlock ? of_.cd_.baseBlock : of_.maxBlock - deBlock;
        gameObject.SetActive(false);
    }
    
    
}
