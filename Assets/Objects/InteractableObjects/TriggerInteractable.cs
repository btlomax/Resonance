using UnityEngine;

/// <summary>
/// Need to put this on an object that can trigger a mechanism to activate it via interaction
/// Put the mechanism on the object to be triggered
/// </summary>
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
