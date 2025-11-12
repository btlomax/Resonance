using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RadialMenuGenerator : MonoBehaviour
{
    [Header("Radial Menu Settings")]
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

#if UNITY_EDITOR
    [Header("Debug")]
    public bool showDebugGizmos = true;
#endif

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
        Debug.Log($"Rebuilding radial menu with {unlockedNotes.Length} unlocked notes.");

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

            Debug.Log($"Slice {i}: fillAmount={newSlice.fillAmount}, expected={fillAmount}");

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
            // Clear highlight
            if (_highlightedSlice >= 0)
            {
                _activeSlices[_highlightedSlice].transform.localScale = Vector3.one;
                _activeSlices[_highlightedSlice].color = Color.white;
                _highlightedSlice = -1;
            }
            return;
        }

        float sliceAngle = 360f / _activeSlices.Length;

        float angle = Mathf.Atan2(_inputHandler.MenuSelectInput.y, _inputHandler.MenuSelectInput.x) * Mathf.Rad2Deg;
        angle -= 90;
        angle = (360f - angle + sliceAngle / 2f) % 360f;

        int sliceIndex = Mathf.FloorToInt(angle / sliceAngle);
        sliceIndex = Mathf.Clamp(sliceIndex, 0, _activeSlices.Length - 1);

        if (sliceIndex != _highlightedSlice)
        {
            if (_highlightedSlice >= 0)
            {
                _activeSlices[_highlightedSlice].transform.localScale = Vector3.one;
                _activeSlices[_highlightedSlice].color = Color.white;
            }

            _activeSlices[sliceIndex].transform.localScale = Vector3.one * 1.2f;
            _activeSlices[sliceIndex].color = Color.yellow;

            _highlightedSlice = sliceIndex;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!showDebugGizmos || _activeSlices == null || _activeSlices.Length == 0)
            return;

        // Gizmo setup
        Vector3 center = radialMenu != null ? radialMenu.position : transform.position;
        float radiusGizmo = 150f; // purely visual — not related to UI radius

        float sliceAngle = 360f / _activeSlices.Length;

        for (int i = 0; i < _activeSlices.Length; i++)
        {
            float startAngle = -90f - (i * sliceAngle); // -90 = top of circle
            float endAngle = startAngle - sliceAngle;

            // Convert to direction vectors
            Vector3 startDir = new Vector3(Mathf.Cos(startAngle * Mathf.Deg2Rad), Mathf.Sin(startAngle * Mathf.Deg2Rad), 0);
            Vector3 endDir = new Vector3(Mathf.Cos(endAngle * Mathf.Deg2Rad), Mathf.Sin(endAngle * Mathf.Deg2Rad), 0);

            // Pick color
            Gizmos.color = (i == _highlightedSlice) ? Color.yellow : new Color(0, 1, 1, 0.25f);

            // Draw arc segment (approximate with two lines)
            Vector3 outerStart = center + startDir * radiusGizmo;
            Vector3 outerEnd = center + endDir * radiusGizmo;

            Gizmos.DrawLine(center, outerStart);
            Gizmos.DrawLine(center, outerEnd);
            Gizmos.DrawLine(outerStart, outerEnd);

            // Label the slice index
#if UNITY_EDITOR
            UnityEditor.Handles.color = Color.white;
            Vector3 midDir = new Vector3(
                Mathf.Cos((startAngle - sliceAngle / 2f) * Mathf.Deg2Rad),
                Mathf.Sin((startAngle - sliceAngle / 2f) * Mathf.Deg2Rad),
                0
            );
            Vector3 labelPos = center + midDir * (radiusGizmo * 1.1f);
            UnityEditor.Handles.Label(labelPos, i.ToString());
#endif
        }
    }
#endif

}
