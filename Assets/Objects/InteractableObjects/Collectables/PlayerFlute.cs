using Assets.Data.GameManagement;
using Assets.Objects.InteractableObjects.Collectables;
using Assets.Player.Contracts;
using UnityEngine;

public class PlayerFlute : Interactable
{
    public GameObject fluteModel;
    public GameObject sectionWall;

    public override void Interact(GameObject interactor)
    {
        //fluteModel.SetActive(false);
        GameEventDispatcher.Instance.TriggerEvent(GameUI_Event.PickedUpFlute);
        this.gameObject.SetActive(false);
        sectionWall.SetActive(false);
    }
}
