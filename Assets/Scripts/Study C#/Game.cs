using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour
{
    private void Start()
    {
        Apple myApple = new Apple();

        //请注意，Apple 版本的方法
        //将覆盖 Fruit 版本。另外请注意，
        //由于 Apple 版本使用“base”关键字
        //来调用 Fruit 版本，因此两者都被调用。
        myApple.SayHello();
        myApple.Chop();

        //“覆盖”在多态情况下也很有用。
        //由于 Fruit 类的方法是“虚”的，
        //而 Apple 类的方法是“覆盖”的，因此
        //当我们将 Apple 向上转换为 Fruit 时，
        //将使用 Apple 版本的方法。
        Fruit myFruit = new Apple();
        myFruit.SayHello();
        myFruit.Chop();
    }
}