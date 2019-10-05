using UnityEngine;
/*
Another special Duck B come from Duck
     */
public class Duck_B : Duck
{
    public Duck_B(){
        flyBehavior = new noWayFly();
        quackBehavior = new Squack();
    }
    public override void DoDisplay()
    {
        Debug.Log("Hello! I am Duck B !");
    }
}