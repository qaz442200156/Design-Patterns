using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ｗeapon
{
    public WeaponBehavior weaponBehavior;
    public bool isCoolDown { get { return (Time.time - lastDamageTime) >= weaponBehavior.CoolDown();} }
    public float lastDamageTime = 0;

    public Ｗeapon(WeaponBehavior defaultWeapon) { weaponBehavior = defaultWeapon; }
    // Do damage will need wait cooldown
    public int DoDamage() {
        if (isCoolDown)
        {
            lastDamageTime = Time.time;
            return weaponBehavior.DoDamage();
        }
        Debug.Log("still cooling!");
        return 0;
    }

    // Get attack range
    public float GetDamageRange() {
       return weaponBehavior.DamageRange();
    }

    // shwo weapon name
    public void DoDisplay()
    {
        Debug.Log(weaponBehavior.Display());
    }

    public void setWeapon(WeaponBehavior newWeapon) {
        weaponBehavior = newWeapon;
        lastDamageTime = Time.time;

        Debug.Log(weaponBehavior.Display() + " Equited!");
    }
}
