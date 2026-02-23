using Assets.Data.GameManagement;
using UnityEngine;

public class UITrigger : MonoBehaviour
{
    public GameUI_Event triggerEvent;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameEventDispatcher.Instance.TriggerEvent(triggerEvent);
        }
    }
}
