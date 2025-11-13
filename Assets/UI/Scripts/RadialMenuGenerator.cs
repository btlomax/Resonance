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
    private AudioSource _audioSource;

    [SerializeField]
    private InputHandler _inputHandler;
    [SerializeField]
    private int _highlightedSlice = -1;
    [SerializeField]
    private float _selectionAngle = -1f;

#if UNITY_EDITOR
    [Header("Debug")]
    public bool showDebugGizmos = true;
#endif

    private void Awake()
    {
        RebuildMenu();
        _audioSource = GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();
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

    /// <summary>
    /// Generates the radial menu slices based on unlocked notes.
    /// </summary>
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
            _activeSlices[i].name = $"{unlockedNotes[i].noteTitle}";
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

        if (_inputHandler.MenuSelectInput.sqrMagnitude < 0.05f)
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

        Debug.Log($"Selected slice: {_activeSlices[sliceIndex].name}");

        PlayNote(_activeSlices[sliceIndex].name);
    }

    private void PlayNote(string note)
    {
        Debug.Log($"Converting note {note} to frequency.");

        float frequency = ToneGenerator.ConvertNoteToFrequency(note);

       AudioClip clip = ToneGenerator.CreateSineWave(frequency, 0.5f);

        if (!_audioSource.isPlaying)
        {
            _audioSource.clip = clip;
            _audioSource.Play();
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
