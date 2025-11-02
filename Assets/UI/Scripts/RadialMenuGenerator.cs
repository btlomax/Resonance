using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class RadialMenuGenerator : MonoBehaviour
{
    public Image slicePrefab;
    public NoteScriptObj[] allNotes;
    public RectTransform radialMenuParent;

    private Image[] _activeSlices;

    private void Awake()
    {
        RebuildMenu();
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
