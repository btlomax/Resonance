using Assets.Data.GameManagement;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPopup", menuName = "Scriptable Objects/PopupContent")]
public class PopupData : ScriptableObject
{
    public PopupType popupType;
    public GameEvent triggerEvent;
    public string title;
    [TextArea(5, 10)] public string bodyText;
    public float displayDuration = 5f;
    public bool pauseGame = false;
    //public AudioClip appearanceSound; // Bonus: easily add sounds!
}

public enum PopupType
{
    Big,
    Toast,
    Tutorial
}