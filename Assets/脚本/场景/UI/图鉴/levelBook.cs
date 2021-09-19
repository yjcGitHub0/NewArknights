using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class levelBook : MonoBehaviour
{
    private Button _button;
    
    void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(() =>
            {
                IllustratedBookManager.OpenIllustratedBook();
            }
        );
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
