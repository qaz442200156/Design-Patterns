using UnityEngine;
/*
Another special Duck A come from Duck
     */
public class Duck_A : Duck
{
    public Duck_A() {
        flyBehavior = new flyWithWing();
        quackBehavior = new Quack();
    }

    public override void DoDisplay()
    {
        Debug.Log("I am a normal duck! ");
    }
}