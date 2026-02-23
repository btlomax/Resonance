using Assets.Data.GameManagement;
using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Searcher.Searcher.AnalyticsEvent;

public class GameProgressionTracker : MonoBehaviour
{
    public static GameProgressionTracker Instance;

    private HashSet<GameEvent> triggeredEvents = new HashSet<GameEvent>();

    public static event Action<GameEvent> OnGameEventTriggered;

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

    public void TriggerEvent(GameEvent gameEvent, bool force = false)
    {
        bool isNew = triggeredEvents.Add(gameEvent);

        if (isNew || force)
        {
            Debug.Log($"Event Fired: {gameEvent} (Forced: {force})");
            OnGameEventTriggered?.Invoke(gameEvent);
        }
    }
}
