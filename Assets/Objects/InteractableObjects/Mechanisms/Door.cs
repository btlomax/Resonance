using Assets.Data.GameManagement;
using Assets.Player.Contracts;
using UnityEngine;

public class Door : BaseMechanism, IObjectInteraction
{
    public GameObject door;
    public override void ActivateMechanism()
    {
        door.SetActive(false);
    }

    public void Interact(GameObject interactor)
    {
        GameEventDispatcher.Instance.TriggerEvent(GameUI_Event.LevelCompleted);

        // Fade out and back to main menu
    }
}
