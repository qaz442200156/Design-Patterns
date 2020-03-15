using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCrossFade : MonoBehaviour
{
    public Animator animator;
    public float crossStartTime;
    public string crossFadeName;
    public bool isCrossFade;
    public void CrossFadeTo(string name)
    {
        isCrossFade = true;
        crossFadeName = name;
        animator.CrossFade(name,1);
        crossStartTime = Time.time;
        animator.speed = 0;
    }

    private void Update()
    {
        if (isCrossFade)
        {
            float passTime = Time.time - crossStartTime;
            if (passTime <= 1)
            {
                animator.speed = 1;
            }
            else {

                animator.speed = 0;
                isCrossFade = false;
            }
        }
    }

}
