using UnityEngine;
using System.Collections;

public class Apple : Fruit
{
    public Apple()
    {
        Debug.Log("1st Apple Constructor Called");
    }

    //这些方法是覆盖方法，因此
    //可以覆盖父类中的任何
    //虚方法。
    public override void Chop()
    {
        Debug.Log("The apple has been chopped.");
    }

    public override void SayHello()
    {
        Debug.Log("Hello, I am an apple.");
    }
}