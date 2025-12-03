using Assets.Player.Contracts;
using UnityEditor;
using UnityEngine;

public abstract class Interactable : MonoBehaviour, IObjectInteraction
{
    [Header("Interactable Settings")]
    public string displayName = "Interactable";
    public bool showHighlight = true;

    public GameObject Highlighter;

    // Called by PlayerInteraction when the player presses interact.
    public abstract void Interact(GameObject interactor);

    // Optional: Called when player looks at object.
    public virtual void OnFocusEnter()
    {
        Highlighter.SetActive(true);
    }

    public virtual void OnFocusExit()
    {
       Highlighter.SetActive(false);
    }
}
