using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogEventManager : MonoBehaviour
{
    public static LogEventManager Instance;

    public LogEventModal logEventModal = new LogEventModal();
    public List<LogEventListenerBaseEnitity> allLogEventListener;

    private void Awake()
    {
        Instance = this;
        allLogEventListener = new List<LogEventListenerBaseEnitity>();
    }



    private void Start()
    {
        //必ずEventModalの初期化をする
        logEventModal.BindLogEventModal(FailEvent, SuccessedEvent);
        DoActiveLogEventListener();
    }

    void SuccessedEvent()
    {
        Debug.Log("Successed");
    }

    void FailEvent() {
        Debug.Log("Fail");
    }

    /// <summary>
    /// Event Listener登録
    /// </summary>
    /// <param name="logEventListener"></param>
    public void RegisterNewListener(LogEventListenerBaseEnitity logEventListener)
    {
        allLogEventListener.Add(logEventListener);
    }
    
    /// <summary>
    /// Event Log Listenerを初期化して機能起動する
    /// </summary>
    public void DoActiveLogEventListener()
    {
        if (allLogEventListener.Count > 0)
        {
            foreach (LogEventListenerBaseEnitity logEventListener in allLogEventListener)
            {
                logEventListener.DoLogEventListenerInit();
            }
        }
    }

    /// <summary>
    /// Event Logを記録する
    /// </summary>
    /// <param name="eventName"></param>
    public void SendEventLog(LogEventTag.TriggerEventNames eventName)
    {
        if (logEventModal != null)
        {
            logEventModal.TriggeredCheckerRegister(eventName);
        }
    }

    /// <summary>
    /// Event Log Listenerを再起動する
    /// </summary>
    public void DoReactiveLogEventListener()
    {
        if (allLogEventListener.Count > 0)
        {
            foreach (LogEventListenerBaseEnitity logEventListener in allLogEventListener)
            {
                logEventListener.ReactiveListener();
            }
        }
    }
}
