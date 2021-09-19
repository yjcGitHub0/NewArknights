using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tagManager : MonoBehaviour
{
    //判断标号x是否为空（未被收录）
    public static bool isEmpty(int x)
    {
        return (x == -1);
    }
    
    //判断标号x是否为高台（纯高度）
    public static bool isHigh(int x)
    {
        return (x == 2) || (x == 3);
    }
    
    //判断标号x是否为地面（纯高度）
    public static bool isLow(int x)
    {
        return (x == 0) || (x == 1) || (x == 4);
    }
    
    //判断标号x能否放置干员
    public static bool canPut(int x)
    {
        return (x == 1) || (x == 3);
    }
    
    //判断标号x敌人能否通过
    public static bool canPass(int x)
    {
        return (x == 0) || (x == 1);
    }
    
    //判断标号x是否为箱子
    public static bool isBox(int x)
    {
        return (x == 4);
    }
    
    //判断标号x敌人在恐惧状态下能否通过
    public static bool canPassFear(int x)
    {
        return (x == 0) || (x == 1) || (x == 5);
    }
}
