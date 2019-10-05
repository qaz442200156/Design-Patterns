using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
Game start enter
     */
public class DuckMain : MonoBehaviour
{
    public void Start()
    {
        Duck Duck_A = new Duck_A();
        Duck_A.DoDisplay();
        Duck_A.DoQuack();
        Duck_A.doFly();
        Duck_A.swin();
        Debug.Log("----------------");
        Duck Duck_B = new Duck_B();
        Duck_B.DoDisplay();
        Duck_B.DoQuack();
        Duck_B.doFly();
        Debug.Log("Duck A get new flyability !!----------------");
        Duck_A.setFlyBehavior(new RocketFly());
        Duck_A.doFly();
        Duck_B.swin();
    }
}