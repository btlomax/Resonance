using System;
using System.Linq;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class RadialMenuGenerator : MonoBehaviour
{
    [Header("Radial Menu Settings")]
    public Image slicePrefab;
    public NoteScriptObj[] allNotes;
    public RectTransform radialMenu;
    public MusicalScale currentMusicalScale;

    [Header("Event Channels")]
    public VoidEventChannel openRadialMenuEventListener;
    public VoidEventChannel closeRadialMenuEventListener;
    public VoidEventChannel toggleRadialMenuEventListener;
    public NotePlayedEventChannel notePlayedEvent;

    [SerializeField]
    private Image[] _activeSlices;
    [SerializeField]
    private InputHandler _inputHandler;
    [SerializeField]
    private int _highlightedSlice = -1;
    [SerializeField]
    private float _selectionAngle = -1f;
    [SerializeField]
    private CinemachineInputAxisController _cameraInput;

#if UNITY_EDITOR
    [Header("Debug")]
    public bool showDebugGizmos = true;
#endif

    private void Awake()
    {
        RebuildMenu();
    }

    private void Update()
    {
        HandleRightStickInput();
    }
   
    private void OnEnable()
    {
        openRadialMenuEventListener.OnEventRaised += ShowMenu;
        closeRadialMenuEventListener.OnEventRaised += HideMenu;
        toggleRadialMenuEventListener.OnEventRaised += ToggleMenu;
    }

    private void OnDisable()
    {
        openRadialMenuEventListener.OnEventRaised -= ShowMenu;
        closeRadialMenuEventListener.OnEventRaised -= HideMenu;
        toggleRadialMenuEventListener.OnEventRaised -= ToggleMenu;
    }

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

        /* var unlockedNotes = allNotes.Where(n => n.unlocked).ToArray();

        int sliceCount = unlockedNotes.Length; */

        if (currentMusicalScale.NotesInScale.Length == 0) return;

        _activeSlices = new Image[currentMusicalScale.NotesInScale.Length];

        float sliceAngle = 360f / currentMusicalScale.NotesInScale.Length;
        float fillAmount = 1f / currentMusicalScale.NotesInScale.Length; 

        for (int i = 0; i < currentMusicalScale.NotesInScale.Length; i++)
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
            _activeSlices[i].name = $"{currentMusicalScale.NotesInScale[i].noteTitle}";
            _activeSlices[i].color = new Color(currentMusicalScale.NotesInScale[i].noteColor.r,
                currentMusicalScale.NotesInScale[i].noteColor.g,
                currentMusicalScale.NotesInScale[i].noteColor.b, 0.5f);
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
        float angle = Mathf.Atan2(stickInput.y, stickInput.x) * Mathf.Rad2Deg;

        angle -= 90;
        angle = 360f - angle;
        angle %= 360f;

        return angle;
    }

    /// <summary>
    /// Selects the note slice based on the given angle.
    /// </summary>
    /// <param name="activeSlices"></param>
    /// <param name="angle"></param>
    private void SelectNoteSlice(Image[] activeSlices, float angle)
    {
        float sliceAngle = 360f / activeSlices.Length;

        int sliceIndex = Mathf.FloorToInt(angle / sliceAngle);
        sliceIndex = Mathf.Clamp(sliceIndex, 0, _activeSlices.Length - 1);

        // When stick is not being moved
        if (_inputHandler.MenuSelectInput.sqrMagnitude < 0.05f)
        {
            // Clear highlight
            if (_highlightedSlice >= 0)
            {
                _activeSlices[_highlightedSlice].transform.localScale = Vector3.one;
                new Color(_activeSlices[_highlightedSlice].color.r,
                    _activeSlices[_highlightedSlice].color.g,
                    _activeSlices[_highlightedSlice].color.b, 0.5f);

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
                _activeSlices[_highlightedSlice].color = new Color(currentMusicalScale.NotesInScale[_highlightedSlice].noteColor.r,
                    currentMusicalScale.NotesInScale[_highlightedSlice].noteColor.g,
                    currentMusicalScale.NotesInScale[_highlightedSlice].noteColor.b, 1);
            }

            _activeSlices[sliceIndex].transform.localScale = Vector3.one * 1.2f;
            _activeSlices[sliceIndex].color = new Color(currentMusicalScale.NotesInScale[sliceIndex].noteColor.r,
                    currentMusicalScale.NotesInScale[sliceIndex].noteColor.g,
                    currentMusicalScale.NotesInScale[sliceIndex].noteColor.b, 0.5f);

            _highlightedSlice = sliceIndex;

            StopNote(); // important — ends old note
            PlayNote(_activeSlices[sliceIndex].name);
        }

        //Debug.Log($"Selected slice: {_activeSlices[sliceIndex].name}");

        return;
    }

    private void PlayNote(string note)
    {
       AudioManager.Instance.Player_PlayLoopingNote(note);

       notePlayedEvent.RaiseEvent(note);
    }

    private void StopNote()
    {
        AudioManager.Instance.Player_StopLoopingNote();
    }
}
