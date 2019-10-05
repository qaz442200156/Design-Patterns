using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Human
{
    public CharacterBehavior characterBehavior;
    public Ｗeapon weapon;
    public int Hp;
    public Transform selfTransform;
    public Human() {}

    public void Init(CharacterBehavior baseProfession,WeaponBehavior baseWeapon)
    {
        characterBehavior = baseProfession;
        weapon = new Ｗeapon(baseWeapon);
        Hp = characterBehavior.baseHp();
    }

    public void equit(WeaponBehavior newWeapon) {
        weapon.setWeapon(newWeapon);
    }

    public void DoAttack(Human target = null, float targetDistance = -1) {
        if (target != null)
        {
            if (targetDistance >= 0)
            {
                if (targetDistance <= weapon.GetDamageRange()) {
                    if (weapon.isCoolDown)
                    {
                        if (target.Hp > 0)
                        {
                            target.GetDamage(weapon);
                        }
                        else
                        {
                            Debug.Log("Target :"+ target.characterBehavior.display() + " is dead !");
                        }
                    }
                    else
                    {
                        Debug.Log(characterBehavior.display() + "'s weapon is cooling");
                    }
                }
            }
        }
        else
        {
            Debug.Log("Target == null");
        }
    }

    public void GetDamage(Ｗeapon weaponDamage) {
        int getDamage = weaponDamage.DoDamage();
        if (Hp > 0 && getDamage > 0)
        {
            Hp -= getDamage;
            Debug.Log(characterBehavior.display() + " Got " + weaponDamage.weaponBehavior.Display() + " attacked and you got " + getDamage + " damaged !");
            if (Hp <= 0)
            {
                Debug.Log(characterBehavior.display() + " Dead !");
            }
        }
        else
        {
            Debug.Log(characterBehavior.display()+" Got " + weaponDamage.weaponBehavior.Display() + " attacked but you dodged !");
        }
    }

    public void changeProfession(CharacterBehavior newCharacterBehavior)
    {
        characterBehavior = newCharacterBehavior;
        Debug.Log("Change to " + characterBehavior.display());
    }

    public void fight(Human target,bool skipDistance = false) {
        if(Hp > 0) { 
            DoAttack(target, skipDistance ? 1:Vector3.Distance(target.selfTransform.position, selfTransform.position));
        }
    }

    public void CheckInfo() {
        Debug.Log(characterBehavior.display());
        weapon.DoDisplay();
    }
}
