using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
   public Dictionary<string, int> playerInventory = new Dictionary<string, int>();

    public void AddItem(string itemName, int quantity)
    {
        Debug.Log($"Item {itemName} x {quantity} added to inventory.");

        if (playerInventory.ContainsKey(itemName))
        {
            playerInventory[itemName] += quantity;
        }
        else
        {
            playerInventory[itemName] = quantity;
        }

        foreach (var item in playerInventory)
        {
            Debug.Log($"Inventory contains: {item.Key} x {item.Value}");
        }
    }
}
