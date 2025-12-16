using UnityEngine;

public class StoneBridge : BaseMechanism
{
    public GameObject bridge;

    public override void ActivateMechanism()
    {
        Debug.Log("The stone bridge has been activated.");
        bridge.SetActive(true);
    }
}
