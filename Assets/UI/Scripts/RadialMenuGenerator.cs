using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class RadialMenuGenerator : MonoBehaviour
{
    public Image slicePrefab;
    public NoteScriptObj[] allNotes;
    public RectTransform radialMenuParent;
    public VoidEventChannel openRadialMenuEvent;

    private Image[] _activeSlices;

    private void Awake()
    {
        RebuildMenu();
    }

    private void OnEnable()
    {
        openRadialMenuEvent.OnEventRaised += ShowMenu;
        Debug.Log("Subscribed to OpenRadialMenuEvent");
    }

    private void OnDisable()
    {
        openRadialMenuEvent.OnEventRaised -= ShowMenu;
        Debug.Log("Unsubscribed from OpenRadialMenuEvent");
    }

    private void ShowMenu()
    {
        Debug.Log("Toggling Radial Menu");
        radialMenuParent.gameObject.SetActive(!radialMenuParent.gameObject.activeSelf);
    }

    public void RebuildMenu()
    {
        foreach (Transform child in radialMenuParent)
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
            Image newSlice = Instantiate(slicePrefab, radialMenuParent);
            newSlice.type = Image.Type.Filled;
            newSlice.fillMethod = Image.FillMethod.Radial360;
            newSlice.fillAmount = 1f / sliceCount;

            newSlice.transform.localRotation = Quaternion.Euler(0, 0, -sliceAngle * i);

            _activeSlices[i] = newSlice;
            _activeSlices[i].gameObject.SetActive(true);
        }
    }
}
