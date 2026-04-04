using Assets.Data.GameManagement;
using Assets.Objects.InteractableObjects.Collectables;
using Assets.Player.Contracts;
using System;
using System.Collections;
using UnityEngine;

public class PlayerFlute : Interactable
{
    public GameObject fluteModel;
    public GameObject sectionWall;
    public static Action PlayerCollectsFlute;

    public override void Interact(GameObject interactor)
    {
        AudioManager.Instance.PlayOneShot(AudioManager.Instance.CollectFlute);

        GameEventDispatcher.Instance.TriggerUIEvent(GameUI_Event.PickedUpFlute);
        PlayerCollectsFlute?.Invoke();
        fluteModel.SetActive(false);
        StartCoroutine(WaitBeforePlay(0.5f));
    }

    private IEnumerator WaitBeforePlay(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        AudioManager.Instance.PlayOneShot(AudioManager.Instance.BarrierDeactivate, sectionWall);
        sectionWall.SetActive(false);
        this.gameObject.SetActive(false);
    }
}
