using Assets.Data.GameManagement;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameEventDispatcher : MonoBehaviour
{
    public static GameEventDispatcher Instance;

    private HashSet<GameUI_Event> triggeredEvents = new HashSet<GameUI_Event>();

    public static event Action<GameUI_Event> OnGameEventTriggered;
    public static event Action OnFadeScreenEventTriggered;
    public static event Action<GameUI_Errors> OnGameEvent_ErrorTriggered;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void TriggerUIEvent(GameUI_Event gameEvent, bool force = false)
    {
        bool isNew = triggeredEvents.Add(gameEvent);

        if (isNew || force)
        {
            Debug.Log($"Event Fired: {gameEvent} (Forced: {force})");
            OnGameEventTriggered?.Invoke(gameEvent);
        }
    }

    public void TriggerUIErrorEvent(GameUI_Errors errorEvent)
    {
       OnGameEvent_ErrorTriggered?.Invoke(errorEvent);
    }

    public void TriggerFadeScreenEvent()
    {
        OnFadeScreenEventTriggered?.Invoke();
    }
}
