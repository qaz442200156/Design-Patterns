using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface CharacterBehavior
{
    void descriptSelf();
    void weaponLimit();
    void baseSkill();
    void baseProperty();
    string display();
    int baseHp();
}

public class farmer : CharacterBehavior
{
    public int baseHp()
    {
        return 100;
    }

    public void baseProperty()
    {
        Debug.Log("Atk 1 , Int 2 , Avi 3 , Vit 10");
    }

    public void baseSkill()
    {
        Debug.Log("Skill list: Punch ");
    }

    public void descriptSelf()
    {
        Debug.Log("I am a farmer !");
    }

    public string display()
    {
        return "Farmer";
    }

    public void weaponLimit()
    {
        Debug.Log("I can use all type of weapon, I can be any one!");
    }
}

public class fighter : CharacterBehavior
{
    public int baseHp()
    {
        return 50;
    }

    public void baseProperty()
    {
        Debug.Log("Atk 5 , Int 1 , Avi 3 , Vit 5");
    }

    public void baseSkill()
    {
        Debug.Log("Skill list: slash , kick");
    }

    public void descriptSelf()
    {
        Debug.Log("I am a fighter !");
    }

    public string display()
    {
        return "Fighter";
    }

    public void weaponLimit()
    {
        Debug.Log("I can use swort, box , axe, heavy hammer");
    }
}

public class magician : CharacterBehavior
{
    public int baseHp()
    {
        return 20;
    }

    public void baseProperty()
    {
        Debug.Log("Atk 1 , Int 5 , Avi 3 , Vit 3");
    }

    public void baseSkill()
    {
        Debug.Log("Skill list: fire ball , ice shield , heal");
    }

    public void descriptSelf()
    {
        Debug.Log("I am a magician !");
    }

    public string display()
    {
        return "Msagician";
    }

    public void weaponLimit()
    {
        Debug.Log("I can use staff, dagger");
    }
}