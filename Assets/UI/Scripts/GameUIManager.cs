using Assets.Data.GameManagement;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class GameUIManager : MonoBehaviour
{
    [SerializeField]
    private List<PopupData> popupLibrary = new List<PopupData>();
    [SerializeField]
    private Canvas _gameUICanvas;

    public GameObject bigPopupPanel;

    private void OnEnable() => GameProgressionTracker.OnGameEventTriggered += HandleProgression;
    private void OnDisable() => GameProgressionTracker.OnGameEventTriggered -= HandleProgression;
    private void HandleProgression(GameEvent gameEvent)
    {
       PopupData data = popupLibrary.Find(p => p.triggerEvent == gameEvent);

       if (data != null)
       {
          ShowPopup(data);
       }
    }

    private void ShowPopup(PopupData data)
    {
        if(data.pauseGame)
        {
            Time.timeScale = 0f; // Pause the game
            bigPopupPanel.SetActive(true);
            StartCoroutine(WaitAndClose(data.displayDuration));

            bigPopupPanel.GetComponentInChildren<TMP_Text>().SetText($"{data.title}\n\n{data.bodyText}");
        }

        Debug.Log($"Showing popup: {data.title} - {data.bodyText}");
    }

    private IEnumerator WaitAndClose(float waitTime)
    {
        yield return new WaitForSecondsRealtime(waitTime);
        bigPopupPanel.SetActive(false);
        Time.timeScale = 1f; // Resume the game
    }
}
