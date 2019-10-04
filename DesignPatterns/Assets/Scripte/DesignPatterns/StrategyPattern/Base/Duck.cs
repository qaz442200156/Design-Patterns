using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
This script is the all type Duck.cs's main script

*/

public abstract class Duck
{
    public FlyBehavior flyBehavior;
    public QuackBehavior quackBehavior;

    // init you cloud setup the unique Behavior when init
    public Duck() { }

    // could be override at otehr type duck
    public abstract void DoDisplay();

    // Behavior A
    public void doFly() {
        flyBehavior.fly();
    }

    // Behavior B
    public void DoQuack() {
        quackBehavior.quack();
    }

    // Replace behavior A when need
    public void setFlyBehavior(FlyBehavior newFlyBehavior)
    {
        flyBehavior = newFlyBehavior;
    }

    // Replace behavior B when need
    public void setQuackBehavior(QuackBehavior newQuackBehavior)
    {
        quackBehavior = newQuackBehavior;
    }

    // Same Asction
    public void swin()
    {
        Debug.Log("All ducks float, even decoys!");
    }
}