using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

[System.Serializable]
public class LogEventModal 
{
    [Header("失敗条件")]
    public List<LogEventTag.TriggerEventNames> FailList = new List<LogEventTag.TriggerEventNames>();

    [Header("成功条件")]
    public List<LogEventTag.TriggerEventNames> SuccessedList = new List<LogEventTag.TriggerEventNames>();

    [Header("現在チェックログ")]
    public List<LogEventTag.TriggerEventNames> logEventCollecter = new List<LogEventTag.TriggerEventNames>();

    public UnityAction failEvent;
    public UnityAction successedEvent;

    [Header("Is End Event Log")]
    [SerializeField]
    private bool isGotResult = false;

    /// <summary>
    /// 初期化　成功のCallと失敗のCallを設定する
    /// </summary>
    /// <param name="newFailEvnet"></param>
    /// <param name="newSuccessedEvent"></param>
    public void BindLogEventModal(UnityAction newFailEvnet = null, UnityAction newSuccessedEvent = null) {
        ClearLogEventCollecter();
        failEvent = newFailEvnet;
        isGotResult = false;
        successedEvent = newSuccessedEvent;
    }

    /// <summary>
    /// チェックリストのEvent Logを消す
    /// </summary>
    public void ClearLogEventCollecter()
    {
        logEventCollecter = new List<LogEventTag.TriggerEventNames>();
    }

    /// <summary>
    /// Event Log がLogリストに追加する時、成功条件と失敗条件をチェックする
    /// </summary>
    /// <param name="triggeredCheckerEventName"></param>
    public void TriggeredCheckerRegister(LogEventTag.TriggerEventNames triggeredCheckerEventName)
    {
        Debug.Log("Log added =>" + triggeredCheckerEventName);
        if (isGotResult == false)
        {
            logEventCollecter.Add(triggeredCheckerEventName);
            checkEventLog();
        }
        else
        {
            Debug.Log("Got Final Result Log Event is Shutdown");
        }
    }
    
    #region EventLog Checker
    void checkEventLog()
    {
        if (isFailCheck())
        {
            if (failEvent != null)
            {
                isGotResult = true;
                failEvent.Invoke();
            }
            return;
        }

        if (isSuccessedCheck())
        {
            if (successedEvent != null)
            {
                isGotResult = true;
                successedEvent.Invoke();
            }
            return;
        }

        Debug.Log("Tutorial is not clear yet");
    }

    bool isFailCheck()
    {
        if(FailList.Count > 0 && logEventCollecter.Count > 0) { 
            if (logEventCollecter.Exists(data => FailList.Exists(failldata => data == failldata)))
            {
                return true;
            }
        }
        return false;
    }

    bool isSuccessedCheck()
    {
        if (SuccessedList.Count > 0 && logEventCollecter.Count > 0)
        {
            int successedCounter = 0;
            List<LogEventTag.TriggerEventNames> tmpList = logEventCollecter.ToList();

            for (int i = 0; i < SuccessedList.Count; i++)
            {
                if(tmpList.Count > 0) { 
                    for (int t = 0; t < tmpList.Count; t++)
                    {
                        if (tmpList[t] == SuccessedList[i])
                        {
                            successedCounter++;
                            tmpList.Remove(tmpList[t]);
                            break;
                        }
                    }
                }
            }
            if (successedCounter == SuccessedList.Count)
            {
                return true;
            }
        }
        return false;
    }
    #endregion
}
