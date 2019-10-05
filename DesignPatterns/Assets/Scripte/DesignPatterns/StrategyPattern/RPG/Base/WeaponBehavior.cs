using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface WeaponBehavior
{
    float DamageRange();
    int DoDamage();
    float CoolDown();
    string Display();
}

public class Hand : WeaponBehavior
{
    public float CoolDown()
    {
        return 0.5f;
    }

    public float DamageRange()
    {
        return 1;
    }

    public string Display()
    {
        return "Hand";
    }

    public int DoDamage()
    {
        return 2;
    }
}

public class Swort : WeaponBehavior
{
    public float CoolDown()
    {
        return 2;
    }

    public float DamageRange()
    {
        return 2;
    }

    public string Display()
    {
        return "Swort";
    }

    public int DoDamage()
    {
        return 5;
    }
}

public class Bow : WeaponBehavior
{
    public float CoolDown()
    {
        return 1.5f;
    }

    public float DamageRange()
    {
        return 6;
    }

    public string Display()
    {
        return "Bow";
    }

    public int DoDamage()
    {
        return 3;
    }
}

public class Dagger : WeaponBehavior
{
    public float CoolDown()
    {
        return 1;
    }

    public float DamageRange()
    {
        return 1;
    }

    public string Display()
    {
        return "Dagger";
    }

    public int DoDamage()
    {
        return 4;
    }
}
