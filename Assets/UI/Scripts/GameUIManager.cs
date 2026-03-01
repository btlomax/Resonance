using Assets.Data.GameManagement;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameUIManager : MonoBehaviour
{
    [SerializeField]
    private List<PopupData> popupLibrary = new List<PopupData>();
    [SerializeField]
    private List<PopupData> errorPopupLibrary = new List<PopupData>();
    [SerializeField]
    private Canvas _gameUICanvas;
    [SerializeField]
    private InputHandler _inputHandler;

    public GameObject fadePanel;
    public GameObject bigPopupPanel;
    public GameObject toastPanel;

    [Header("Panel Animation")]
    [SerializeField] private AnimationCurve slideCurve;
    [SerializeField] private float slideDistance = 200f;

    private CanvasGroup _fadePanelCanvasGroup;

    private void OnEnable()
    {
        GameEventDispatcher.OnGameEventTriggered += HandleGameUIEvent;
        GameEventDispatcher.OnFadeScreenEventTriggered += FadePanel;
        GameEventDispatcher.OnGameEvent_ErrorTriggered += HandleGameUIErrorEvent;

        _fadePanelCanvasGroup = fadePanel.GetComponent<CanvasGroup>();

        FadePanel(); // Start with fade panel active to create a fade-in effect when the scene loads
    }

    private void OnDisable()
    {
        GameEventDispatcher.OnGameEventTriggered -= HandleGameUIEvent;
        GameEventDispatcher.OnFadeScreenEventTriggered -= FadePanel;
        GameEventDispatcher.OnGameEvent_ErrorTriggered -= HandleGameUIErrorEvent;
    }
    private void HandleGameUIEvent(GameUI_Event gameEvent)
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

    private void HandleGameUIErrorEvent(GameUI_Errors error)
    {
        PopupData errorData = errorPopupLibrary.Find(p => p.errorEvent == error);

        if(errorData != null)
            ShowToastPanel(errorData);
    }
    private void FadePanel()
    {
        if(_fadePanelCanvasGroup.alpha == 0)
            StartCoroutine(AlphaFadeToBlack());
        else
            StartCoroutine(AlphaFadeFromBlack());
    }

    private void ShowBigPopupPanel(PopupData data)
    {
        if(data.pauseGame)
        {
            Time.timeScale = 0f; // Pause the game
            _inputHandler.Controls.Disable();
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
        _inputHandler.Controls.Enable();

        if (data.pauseGame)
            Time.timeScale = 1f; // Resume the game regardless of popup type, as big popups will pause the game and we want to ensure it resumes after the toast is done
    }

    private IEnumerator AlphaFadeToBlack()
    {
        Debug.Log("Starting fade effect");
        float elapsedTime = 0f;

        while (elapsedTime < 5f)
        {
            elapsedTime += Time.deltaTime;

            float alpha = Mathf.Lerp(0f, 1f, elapsedTime);

            _fadePanelCanvasGroup.alpha = alpha;

            yield return null;
        }

        Debug.Log("Fade effect completed");
        SceneManager.LoadScene(0);
    }

    private IEnumerator AlphaFadeFromBlack()
    {
        Debug.Log("Starting fade effect");
        float elapsedTime = 0f;

        while (elapsedTime < 5f)
        {
            elapsedTime += Time.deltaTime;

            float alpha = Mathf.Lerp(1f, 0f, elapsedTime);

            _fadePanelCanvasGroup.alpha = alpha;

            yield return null;
        }

        Debug.Log("Fade effect completed");
    }
}
