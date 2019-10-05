using UnityEngine;
/*
All Quack behaviors in the duck world
*/
public interface QuackBehavior
{
    void quack();
}

public class Quack : QuackBehavior
{
    public void quack()
    {
        Debug.Log("Quack !");
    }
}

public class MuteQuack : QuackBehavior
{
    public void quack()
    {
        Debug.Log("<< Silence >>");
    }
}

public class Squack : QuackBehavior
{
    public void quack()
    {
        Debug.Log("Squack !");
    }
}