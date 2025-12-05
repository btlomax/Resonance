using UnityEngine;

public class TriggerInteractable : Interactable
{
    public GameObject mechanism;

    public string InteractMessage = string.Empty;

    public override void Interact(GameObject interactor)
    {
        Debug.Log($"{InteractMessage}");

        if(mechanism != null)
            mechanism.SetActive(!mechanism.activeSelf);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
