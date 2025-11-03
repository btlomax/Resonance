using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class RadialMenuGenerator : MonoBehaviour
{
    public Image slicePrefab;
    public NoteScriptObj[] allNotes;
    public RectTransform radialMenu;
    public VoidEventChannel openRadialMenuEvent;
    public VoidEventChannel closeRadialMenuEvent;
    public VoidEventChannel toggleRadialMenuEvent;

    private Image[] _activeSlices;

    private void Awake()
    {
        RebuildMenu();
    }

    private void OnEnable()
    {
        openRadialMenuEvent.OnEventRaised += ShowMenu;
        closeRadialMenuEvent.OnEventRaised += HideMenu;
        toggleRadialMenuEvent.OnEventRaised += ToggleMenu;
        Debug.Log("Subscribed to menu events");
    }

    private void OnDisable()
    {
        openRadialMenuEvent.OnEventRaised -= ShowMenu;
        closeRadialMenuEvent.OnEventRaised -= HideMenu;
        toggleRadialMenuEvent.OnEventRaised -= ToggleMenu;
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

        if(sliceCount == 0) return;

        float sliceAngle = 360f / sliceCount;

        _activeSlices = new Image[sliceCount];

        for (int i = 0; i < sliceCount; i++)
        {
            Image newSlice = Instantiate(slicePrefab, radialMenu);
            newSlice.type = Image.Type.Filled;
            newSlice.fillMethod = Image.FillMethod.Radial360;
            newSlice.fillAmount = 1f / sliceCount;

            newSlice.transform.localRotation = Quaternion.Euler(0, 0, -sliceAngle * i);

            _activeSlices[i] = newSlice;
            _activeSlices[i].gameObject.SetActive(true);
        }
    }
}
