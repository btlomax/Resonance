using Assets.Data.GameManagement;
using System;
using System.Collections.Generic;
using UnityEngine;

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

    public void TriggerEvent(GameEvent gameEvent)
    {
        if (!triggeredEvents.Contains(gameEvent))
        {
            triggeredEvents.Add(gameEvent);
            Debug.Log($"Game Event Triggered: {gameEvent}");

            OnGameEventTriggered?.Invoke(gameEvent);
        }
    }
}
