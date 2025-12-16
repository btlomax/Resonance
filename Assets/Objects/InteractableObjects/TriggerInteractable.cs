using UnityEngine;

public class TriggerInteractable : MonoBehaviour
{
    public BaseMechanism mechanism;

    public string InteractMessage = string.Empty;

    public void TriggerAction(string action)
    {
       Debug.Log($"TriggerInteractable received action: {action}");
        
       switch(action)
       {
           case "ActivateMechanism":
               if(mechanism != null)
               {
                    mechanism.ActivateMechanism();
                }
               else
               {
                   Debug.LogWarning("No mechanism assigned to TriggerInteractable.");
               }
               break;
            default:
               Debug.LogWarning($"Unknown action: {action}");
               break;
        }
    }
}
