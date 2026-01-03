using Assets.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Cinemachine;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class RadialMenuGenerator : MonoBehaviour
{
    #region Fields and Properties
    [Header("Radial Menu Settings")]
    public Image slicePrefab;
    public float deadZone = 0.25f;

    public RectTransform radialMenu;

    public NoteScriptObj[] allNotes;
    public ScaleDegreeToColour ScaleDegreeColourLookup;

    public MusicalScale currentMajorScale;
    public MusicalScale currentMinorScale;
    public MusicalScale currentScale;
    public MusicalScale[] allUnlockedScales;

    public TMP_Text scaleNameText;
    public TMP_Text noteNameText;

    [Header("Event Channels")]
    public VoidEventChannel openRadialMenuEventListener;
    public VoidEventChannel closeRadialMenuEventListener;
    public VoidEventChannel toggleRadialMenuEventListener;
    public VoidEventChannel toggleMajorMinor;
    public NotePlayedEventChannel notePlayedEvent;
    public OnNoteStoppedEvent onNoteStoppedEvent;

    [SerializeField]
    private InputHandler _inputHandler;
    [SerializeField]
    private CinemachineInputAxisController _cameraInput;
    [SerializeField]
    private float _sliceHysteresis = 0.2f;

    private int _highlightedSlice = -1;
    private float _selectionAngle = -1f;
    private Image[] _activeSlices;

#if UNITY_EDITOR
    [Header("Debug")]
    public bool showDebugGizmos = true;
#endif
    #endregion

    private void Awake()
    {
        currentMajorScale = allUnlockedScales.First();

        GetRelativeMinor();

        currentScale = currentMajorScale;

        RebuildMenu();
    }

    private void Update()
    {
        HandleRightStickInput();
    }

    #region OnEnable/OnDisable
    private void OnEnable()
    {
        openRadialMenuEventListener.OnEventRaised += ShowMenu;
        closeRadialMenuEventListener.OnEventRaised += HideMenu;
        toggleRadialMenuEventListener.OnEventRaised += ToggleMenu;
        toggleMajorMinor.OnEventRaised += OnToggleMajorMinor;
    }

    private void OnDisable()
    {
        openRadialMenuEventListener.OnEventRaised -= ShowMenu;
        closeRadialMenuEventListener.OnEventRaised -= HideMenu;
        toggleRadialMenuEventListener.OnEventRaised -= ToggleMenu;
        toggleMajorMinor.OnEventRaised -= OnToggleMajorMinor;
    }
    #endregion

    #region Show/Hide/Toggle Menu Methods
    private void ShowMenu()
    {
        radialMenu.gameObject.SetActive(true);
        _cameraInput.enabled = false;
    }

    private void HideMenu()
    {
        radialMenu.gameObject.SetActive(false);
        _cameraInput.enabled = true;
    }

    private void ToggleMenu()
    {
        radialMenu.gameObject.SetActive(!radialMenu.gameObject.activeSelf);
        _cameraInput.enabled = !radialMenu.gameObject.activeSelf;
    }
    #endregion

    /// <summary>
    /// Generates the radial menu slices based on unlocked notes.
    /// </summary>
    public void RebuildMenu()
    {
        foreach (Transform child in radialMenu)
        {
            Destroy(child.gameObject);
        }

        if(currentScale.NotesInScale.Length == 0 ) return;

        _activeSlices = new Image[currentScale.NotesInScale.Length];

        float sliceAngle = 360f / currentScale.NotesInScale.Length;
        float fillAmount = 1f / currentScale.NotesInScale.Length; 

        for (int i = 0; i < currentScale.NotesInScale.Length; i++)
        {
            Image newSlice = Instantiate(slicePrefab, radialMenu);
            newSlice.type = Image.Type.Filled;
            newSlice.fillMethod = Image.FillMethod.Radial360;
            newSlice.fillAmount = fillAmount;

            TMP_Text scaleDegreeText = Instantiate(scaleNameText, newSlice.transform);
            scaleDegreeText.text = (i + 1).ToString();

            TMP_Text noteNameTextInstance = Instantiate(noteNameText, newSlice.transform);
            noteNameTextInstance.text = currentScale.NotesInScale[i].noteTitle;

            // Rotate the slice
            float rotationZ = -sliceAngle * i + (sliceAngle / 2f);
            newSlice.transform.localRotation = Quaternion.Euler(0, 0, rotationZ);

            _activeSlices[i] = newSlice;
            _activeSlices[i].gameObject.SetActive(true);

            // Set slice name and color with alpha, otherwise it appears fully opaque
            _activeSlices[i].name = $"{currentScale.NotesInScale[i].noteTitle}";
            _activeSlices[i].color = ScaleDegreeColourLookup.Get(i);
        }
    }

    /// <summary>
    /// Uses right stick input to determine which slice is selected.
    /// </summary>

    private void HandleRightStickInput()
    {
       if(radialMenu.gameObject.activeSelf)
       {
           _selectionAngle = CalculateAngleFromStickInput( _inputHandler.MenuSelectInput);
           UpdateDirectionIndicator(_selectionAngle);

            SelectNoteSlice(_activeSlices, _selectionAngle);
       }
    }

    /// <summary>
    /// Calculates the angle in degrees from the right stick input vector.
    /// </summary>
    /// <param name="stickInput"></param>
    /// <returns></returns>
    private float CalculateAngleFromStickInput(Vector2 stickInput)
    {
        float angle = Mathf.Atan2(stickInput.x, stickInput.y) * Mathf.Rad2Deg;
        return (angle + 360f) % 360f;
    }

    /// <summary>
    /// Selects the note slice based on the given angle.
    /// </summary>
    /// <param name="activeSlices"></param>
    /// <param name="angle"></param>
    private void SelectNoteSlice(Image[] activeSlices, float angle)
    {
        float sliceAngle = 360f / activeSlices.Length;

        int sliceIndex = GetSliceIndexWithHysteresis(angle, _highlightedSlice, activeSlices.Length);
        sliceIndex = Mathf.Clamp(sliceIndex, 0, _activeSlices.Length - 1);

        // When stick is not being moved
        if (_inputHandler.MenuSelectInput.magnitude < deadZone)
        {
            // Clear highlight
            if (_highlightedSlice >= 0)
            {
                _activeSlices[_highlightedSlice].transform.localScale = Vector3.one;
                _activeSlices[_highlightedSlice].color = ScaleDegreeColourLookup.Get(_highlightedSlice);

                _highlightedSlice = -1;
            }

            StopNote();
            return;
        }

        if (sliceIndex != _highlightedSlice)
        {
            if (_highlightedSlice >= 0)
            {
                _activeSlices[_highlightedSlice].transform.localScale = Vector3.one;
                _activeSlices[_highlightedSlice].color = ScaleDegreeColourLookup.Get(_highlightedSlice);
            }

            _activeSlices[sliceIndex].transform.localScale = Vector3.one * 1.2f;
            _activeSlices[sliceIndex].color = ScaleDegreeColourLookup.Get(sliceIndex) * 1.5f;

            _highlightedSlice = sliceIndex;

            StopNote(); // important — ends old note
            PlayNote(_activeSlices[sliceIndex].name);
        }

        //Debug.Log($"Selected slice: {_activeSlices[sliceIndex].name}");

        return;
    }

    private int GetSliceIndexWithHysteresis(float angle, int currentSlice, int sliceCount)
    {
        float sliceSize = 360f / sliceCount;

        int rawIndex = Mathf.FloorToInt(angle / sliceSize);
        rawIndex = (rawIndex + sliceCount) % sliceCount;

        if (currentSlice < 0)
            return rawIndex;

        float sliceCenter = (currentSlice + 0.5f) * sliceSize;
        float delta = Mathf.DeltaAngle(sliceCenter, angle);

        float hysteresisAngle = sliceSize * _sliceHysteresis;

        if (Mathf.Abs(delta) < sliceSize / 2f + hysteresisAngle)
            return currentSlice;

        return rawIndex;
    }

    private void PlayNote(string note)
    {
       AudioManager.Instance.Player_PlayLoopingNote(note);

       notePlayedEvent.RaiseEvent(note);
    }

    private void StopNote()
    {
        AudioManager.Instance.Player_StopLoopingNote();

        onNoteStoppedEvent.RaiseEvent();
    }

    private void GetRelativeMinor()
    {
        if (!currentMinorScale)
        {
            currentMinorScale = ScriptableObject.CreateInstance<MusicalScale>();
            currentMinorScale.ScaleName = currentMajorScale.GetNameOfMinorScale();
            currentMinorScale.NotesInScale = currentMajorScale.GetRelativeMinorScale();
        }
        else
        {
            currentMinorScale.ScaleName = currentMajorScale.GetNameOfMinorScale();
            currentMinorScale.NotesInScale = currentMajorScale.GetRelativeMinorScale();
        }

        currentMinorScale.name = currentMinorScale.ScaleName;
    }

    private void OnToggleMajorMinor()
    {
        if(currentScale == currentMajorScale)
        {
            currentScale = currentMinorScale;
        }
        else
        {
            currentScale = currentMajorScale;
        }

        RebuildMenu();
    }
}
