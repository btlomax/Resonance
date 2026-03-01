using Assets.Data.GameManagement;
using Assets.Objects.InteractableObjects.Collectables;
using Assets.Player.Contracts;
using System;
using UnityEngine;

public class PlayerFlute : Interactable
{
    public GameObject fluteModel;
    public GameObject sectionWall;
    public static Action PlayerCollectsFlute;

    public override void Interact(GameObject interactor)
    {
        //fluteModel.SetActive(false);
        GameEventDispatcher.Instance.TriggerUIEvent(GameUI_Event.PickedUpFlute);
        PlayerCollectsFlute?.Invoke();
        this.gameObject.SetActive(false);
        sectionWall.SetActive(false);
    }
}
