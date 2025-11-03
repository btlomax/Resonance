using System;
using System.Linq;
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
    public VoidEventChannel toggleRadialMenuEventListener
        ;

    private Image[] _activeSlices;

    [SerializeField]
    private InputHandler _inputHandler;

    private void Awake()
    {
        RebuildMenu();
    }

    private void Update()
    {
      /*  if (!radialMenu.gameObject.activeSelf)
        {
            Console.WriteLine("Radial menu is not active; skipping input handling.");
            return;
        }

        Console.WriteLine("Radial menu is active; handling right stick input.");*/
      Console.WriteLine("Updating radial menu input handling.");
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
            float rotationZ = -sliceAngle * i;
            newSlice.transform.localRotation = Quaternion.Euler(0, 0, rotationZ);

            _activeSlices[i] = newSlice;
            _activeSlices[i].gameObject.SetActive(true);
            _activeSlices[i].name = $"{unlockedNotes[i].noteTitle} slice";
        }
    }

    private void HandleRightStickInput()
    {
        Console.WriteLine($"Right stick input vector: {_inputHandler.MenuSelectInput}");

        if (_inputHandler.MenuSelectInput.sqrMagnitude < 0.1f)
            return;

        Console.WriteLine($"Right stick input: {_inputHandler.MenuSelectInput}");

        float angle = Mathf.Atan2(_inputHandler.MenuSelectInput.y, _inputHandler.MenuSelectInput.x) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360f;

        int sliceIndex = Mathf.FloorToInt(angle / (360f / _activeSlices.Length));
        sliceIndex = Mathf.Clamp(sliceIndex, 0, _activeSlices.Length - 1);

        Console.WriteLine($"Selected slice index: {sliceIndex}");
    }
}
