using Assets.Player.Contracts;
using UnityEngine;

public abstract class Interactable : MonoBehaviour, IObjectInteraction
{
    [Header("Interactable Settings")]
    public string displayName = "Interactable";
    public bool showHighlight = true;

    // Called by PlayerInteraction when the player presses interact.
    public abstract void Interact(GameObject interactor);

    // Optional: Called when player looks at object.
    public virtual void OnFocusEnter() { }

    // Optional: Called when player looks away.
    public virtual void OnFocusExit() { }
}
