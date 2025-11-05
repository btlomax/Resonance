using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RadialMenuGenerator : MonoBehaviour
{
    [Header("Radial Menu Settings")]
    public float radius = 0f;

    public Image slicePrefab;
    public NoteScriptObj[] allNotes;
    public RectTransform radialMenu;
    public VoidEventChannel openRadialMenuEventListener;
    public VoidEventChannel closeRadialMenuEventListener;
    public VoidEventChannel toggleRadialMenuEventListener;

    private Image[] _activeSlices;

    [SerializeField]
    private InputHandler _inputHandler;
    [SerializeField]
    private int _highlightedSlice = -1;

    private void Awake()
    {
        Debug.Log("RadialMenuGenerator Awake called");
        RebuildMenu();
    }

    private void Update()
    {
        if(radialMenu.gameObject.activeSelf)
            HandleRightStickInput();
    }

    private void OnEnable()
    {
        openRadialMenuEventListener.OnEventRaised += ShowMenu;
        closeRadialMenuEventListener.OnEventRaised += HideMenu;
        toggleRadialMenuEventListener.OnEventRaised += ToggleMenu;
        Debug.Log("Subscribed to menu events");
    }

    private void OnDisable()
    {
        openRadialMenuEventListener.OnEventRaised -= ShowMenu;
        closeRadialMenuEventListener.OnEventRaised -= HideMenu;
        toggleRadialMenuEventListener.OnEventRaised -= ToggleMenu;
        Debug.Log("Unsubscribed from menu events");
    }

    #region Show/Hide/Toggle Menu Methods
    private void ShowMenu()
    {
        radialMenu.gameObject.SetActive(true);
    }

    private void HideMenu()
    {
        radialMenu.gameObject.SetActive(false);
    }

    private void ToggleMenu()
    {
        radialMenu.gameObject.SetActive(!radialMenu.gameObject.activeSelf);
    }
    #endregion

    public void RebuildMenu()
    {
        foreach (Transform child in radialMenu)
        {
            Destroy(child.gameObject);
        }

        var unlockedNotes = allNotes.Where(n => n.unlocked).ToArray();
        int sliceCount = unlockedNotes.Length;

        if (sliceCount == 0) return;

        float sliceAngle = 360f / sliceCount;
        float fillAmount = 1f / sliceCount;

        _activeSlices = new Image[sliceCount];

        for (int i = 0; i < sliceCount; i++)
        {
            Image newSlice = Instantiate(slicePrefab, radialMenu);
            newSlice.type = Image.Type.Filled;
            newSlice.fillMethod = Image.FillMethod.Radial360;
            newSlice.fillAmount = fillAmount;

            // Rotate the slice
            float rotationZ = -sliceAngle * i + (sliceAngle / 2f);
            newSlice.transform.localRotation = Quaternion.Euler(0, 0, rotationZ);

            _activeSlices[i] = newSlice;
            _activeSlices[i].gameObject.SetActive(true);
            _activeSlices[i].name = $"{unlockedNotes[i].noteTitle} slice";
        }
    }

    private void HandleRightStickInput()
    {
        if (_inputHandler.MenuSelectInput.sqrMagnitude < 0.1f)
        {
            // Remove highlight from currently selected slice
            if (_highlightedSlice >= 0)
            {
                _activeSlices[_highlightedSlice].transform.localScale = Vector3.one;
                _activeSlices[_highlightedSlice].color = Color.white;
                _highlightedSlice = -1; // no selection
            }

            return;
        }

        float angle = Mathf.Atan2(_inputHandler.MenuSelectInput.y, _inputHandler.MenuSelectInput.x) * Mathf.Rad2Deg;

        angle -= 90;

        angle = 360f - angle;

        angle %= 360f;

        Debug.Log($"Raw angle from right stick: {angle}");

        int sliceIndex = Mathf.FloorToInt(angle / (360f / _activeSlices.Length));
        sliceIndex = Mathf.Clamp(sliceIndex, 0, _activeSlices.Length - 1);

        if (sliceIndex != _highlightedSlice)
        {
            // Remove highlight from old slice
            if (_highlightedSlice >= 0)
            {
                _activeSlices[_highlightedSlice].transform.localScale = Vector3.one;
                _activeSlices[_highlightedSlice].color = Color.white;
            }

            // Apply highlight to new slice
            _activeSlices[sliceIndex].transform.localScale = Vector3.one * 1.2f;
            _activeSlices[sliceIndex].color = Color.yellow;

            _highlightedSlice = sliceIndex;
        }

        Debug.Log($"Selected slice: {_activeSlices[sliceIndex].name}");
    }
}
