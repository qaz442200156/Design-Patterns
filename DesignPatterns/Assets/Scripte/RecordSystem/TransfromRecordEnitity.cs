public class TransfromRecordEnitity : RecordEnitity
{
    public override void Start()
    {
        base.Start();
    }

    public override void PerBeginRecord()
    {
        base.PerBeginRecord();
    }

    public override void Record(float time)
    {
        Data.Add(transform, time);
    }

    public override void PerReplayInit()
    {
        base.PerReplayInit();
    }

    public override void RePlay(float time, float timeScale)
    {
        Data.Set(time, this.transform);
    }
}