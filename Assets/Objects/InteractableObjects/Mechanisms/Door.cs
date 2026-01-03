using UnityEngine;

public class Door : BaseMechanism
{
    public GameObject door;
    public override void ActivateMechanism()
    {
        door.SetActive(false);
    }
}
