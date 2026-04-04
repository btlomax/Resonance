using UnityEngine;

public class StoneBridge : BaseMechanism
{
    public GameObject bridge;

    public override void ActivateMechanism()
    {
        bridge.SetActive(true);
        AudioManager.Instance.MechanismActivateEvent.Post(gameObject);
    }
}
