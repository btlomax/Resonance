using Assets.Data.GameManagement;
using JetBrains.Annotations;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SingingStone : Interactable
{
    [Header("Singing Stone Settings")]
    [SerializeField] private GameEvent eventToTrigger;

    private void Awake()
    {
    }

    public override void Interact(GameObject interactor)
    {
        Debug.Log($"The Singing Stone hums a melodious tune as {interactor.name} interacts with it.");
        GameProgressionTracker.Instance.TriggerEvent(eventToTrigger, true);
    }
}
