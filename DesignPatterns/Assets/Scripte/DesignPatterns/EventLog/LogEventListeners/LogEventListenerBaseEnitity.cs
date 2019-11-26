using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LogEventListenerBaseEnitity : MonoBehaviour
{
    public LogEventTag.TriggerEventNames logEventType;
    [Header("------ Base Enitity Bool------")]
    public bool destoryWithGameobjectWhenListenerDestorty;
    public bool deactiveLogEventListenerWhenSended;
    public bool deactiveMeshRenderWhenSended;
    private Collider collider_;
    private MeshRenderer meshRender;
    public bool deactiveSpriteRenderWhenSended;
    private SpriteRenderer spriteRenderer;
    private bool isAllowToSendLog = true;
    public UnityAction<LogEventTag.TriggerEventNames> reportAction;

    public void Start()
    {
        // Need create manager to  Register All listener
        LogEventManager.Instance.RegisterNewListener(this);
        checkIfUseDeactiveObjectRenderAndSetup();
        collider_ = GetComponent<Collider>();
    }

    /// <summary>
    /// Object Render系初期化処理
    /// </summary>
    void checkIfUseDeactiveObjectRenderAndSetup()
    {
        if (deactiveMeshRenderWhenSended)
        {
            meshRender = GetComponent<MeshRenderer>();
        }
        if (deactiveSpriteRenderWhenSended)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }

    /// <summary>
    /// EventLog　機能再起動の処理
    /// </summary>
    public void ReactiveListener() {
        enabled = true;
        collider_.enabled = true;
        isAllowToSendLog = true;
        checkIfNeedUseReActiveObjectRender();
    }


    /// <summary>
    /// 初期化処理
    /// </summary>
    public void DoLogEventListenerInit()
    {
        if (isCanUseEventLog() == false)
        {
            if (destoryWithGameobjectWhenListenerDestorty)
            {
                Destroy(gameObject);
            }
            else { 
                Destroy(this);
            }
        }
        else
        {
            // ここで初期化のActionを指定する
            Debug.Log(this.gameObject.name+"Added");
            reportAction = LogEventManager.Instance.SendEventLog;
        }
    }

    bool isCanUseEventLog() {
        // ここで他の判断を足す
        return true;
    }

    /// <summary>
    /// Event Log を発送する
    /// </summary>
    public void Send()
    {
        if (reportAction != null)
        {
            if(isAllowToSendLog){
                reportAction(logEventType);
                if (deactiveLogEventListenerWhenSended)
                {
                    enabled = false;
                    isAllowToSendLog = false;
                }
                collider_.enabled = false;
                checkIfNeedUseDeactiveObjectRender();
            }
        }
        else
        {
            Debug.LogWarning(" Fail to send Log event because report action is null ");
        }
    }

    /// <summary>
    /// 機能有効化する時Object　Renderの処理
    /// </summary>
    void checkIfNeedUseReActiveObjectRender()
    {
        if (deactiveMeshRenderWhenSended)
        {
            meshRender.enabled = true;
        }
        if (deactiveSpriteRenderWhenSended)
        {
            spriteRenderer.enabled = true;
        }
    }

    /// <summary>
    /// 機能無効化する時Object　Renderの処理
    /// </summary>
    void checkIfNeedUseDeactiveObjectRender()
    {
        if (deactiveMeshRenderWhenSended)
        {
            meshRender.enabled = false;
        }
        if (deactiveSpriteRenderWhenSended)
        {
            spriteRenderer.enabled = false;
        }
    }
}
