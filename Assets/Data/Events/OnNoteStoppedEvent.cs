using System;
using UnityEngine;

[CreateAssetMenu(fileName = "OnNoteStopped", menuName = "Scriptable Objects/OnNoteStopped")]
public class OnNoteStoppedEvent : ScriptableObject
{
    public Action OnNoteStopped;

    public void RaiseEvent()
    {
        OnNoteStopped?.Invoke();
    }
}

