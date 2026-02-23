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
    public GameObject toastPanel;

    [Header("Panel Animation")]
    [SerializeField] private AnimationCurve slideCurve;
    [SerializeField] private float slideDistance = 200f;

    private void OnEnable() => GameProgressionTracker.OnGameEventTriggered += HandleProgression;
    private void OnDisable() => GameProgressionTracker.OnGameEventTriggered -= HandleProgression;
    private void HandleProgression(GameEvent gameEvent)
    {
       PopupData data = popupLibrary.Find(p => p.triggerEvent == gameEvent);

       if (data != null)
       {
          switch(data.popupType)
          {
              case PopupType.Big:
                  ShowBigPopupPanel(data);
                  break;
              case PopupType.Toast:
                  ShowToastPanel(data);
                  break;
            }
        }
    }

    private void ShowBigPopupPanel(PopupData data)
    {
        if(data.pauseGame)
        {
            Time.timeScale = 0f; // Pause the game
        }

        bigPopupPanel.GetComponentInChildren<TMP_Text>().SetText($"{data.title}\n\n{data.bodyText}");
        StartCoroutine(SlidePanelIn(data, bigPopupPanel));

        Debug.Log($"Showing popup: {data.title} - {data.bodyText}");
    }

    private void ShowToastPanel(PopupData data)
    {
        toastPanel.GetComponentInChildren<TMP_Text>().SetText($"{data.title}\n\n{data.bodyText}");
        StartCoroutine(SlidePanelIn(data, toastPanel));

        Debug.Log($"Showing toast: {data.title} - {data.bodyText}");
    }

    private IEnumerator SlidePanelIn(PopupData data, GameObject panel)
    {
        RectTransform rect = panel.GetComponent<RectTransform>();
        float panelHeight = rect.rect.height;

        Vector2 hiddenPos = new Vector2(0, panelHeight + slideDistance);
        Vector2 visiblePos = Vector2.zero;

        rect.anchoredPosition = hiddenPos;
        panel.SetActive(true);

        // 2. Slide In
        float t = 0;
        while (t < 1)
        {
            t += Time.unscaledDeltaTime * 2f; // Fast slide
            rect.anchoredPosition = Vector2.Lerp(hiddenPos, visiblePos, slideCurve.Evaluate(t));
            yield return null;
        }

        // 3. Wait
        yield return new WaitForSecondsRealtime(data.displayDuration);

        // 4. Slide Out
        t = 0;
        while (t < 1)
        {
            t += Time.unscaledDeltaTime * 2f;
            rect.anchoredPosition = Vector2.Lerp(visiblePos, hiddenPos, slideCurve.Evaluate(t));
            yield return null;
        }

        panel.SetActive(false);

        if(data.pauseGame)
            Time.timeScale = 1f; // Resume the game regardless of popup type, as big popups will pause the game and we want to ensure it resumes after the toast is done
    }
}
