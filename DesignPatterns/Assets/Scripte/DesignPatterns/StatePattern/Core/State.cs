public class State : object
{
    protected string StateID;

    virtual public void PreUpdate(StateSetter setter)
    {
    }

    virtual public void Update(StateSetter setter)
    {
    }

    virtual public void PostUpdate(StateSetter setter)
    {
    }
}