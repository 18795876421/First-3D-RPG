using UnityEngine;
using System.Collections;

public class Enemy : Humanoid
{
    public static int x = 1;
    //这会隐藏 Humanoid 版本。
    new public void Yell()
    {
        Debug.Log("Enemy version of the Yell() method");
    }
}