using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New levelData",menuName = "myScript/levelData")]
public class levelData : ScriptableObject
{
    [TextArea] 
    public string Name;
    [TextArea] 
    public string Description;

    [Header("地图快照")] 
    public Sprite map;

}
