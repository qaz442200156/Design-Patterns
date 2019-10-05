using UnityEngine;
/*
All fly behaviors in the duck world
*/

public interface FlyBehavior
{
    void fly();
}

public class flyWithWing : FlyBehavior
{
    public void fly()
    {
        Debug.Log("fly with wings");
    }
}

public class noWayFly : FlyBehavior
{
    public void fly()
    {
        Debug.Log("can't fly");
    }
}

public class RocketFly : FlyBehavior
{
    public void fly()
    {
        Debug.Log("Fly with a rocket !");
    }
}
