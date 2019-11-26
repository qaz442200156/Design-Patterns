public class Attribute : object
{
    protected string AttributeID;
    protected System.Type Type;

    public Attribute(string iID)
    {
        AttributeID = iID;
    }

    ~Attribute()
    {
    }

    public string GetID()
    {
        return AttributeID;
    }

    public System.Type GetAttType()
    {
        return Type;
    }
}

public class AttTemplate<T> : Attribute
{
    protected T MaxValue;

    public T Max
    {
        get { return MaxValue; }
    }

    protected T MinValue;

    public T Min
    {
        get { return MinValue; }
    }

    protected T CurrentValue;

    public T Current
    {
        get { return CurrentValue; }
        set { CurrentValue = value; }
    }

    public AttTemplate(string iID, T iMax, T iMin) : base(iID)
    {
        MaxValue = CurrentValue = iMax;
        MinValue = iMin;
        Type = this.GetType();
    }

    ~AttTemplate()
    { }
}