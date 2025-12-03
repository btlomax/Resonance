using UnityEngine;

public class LeverAndMechanism : Interactable
{
    public GameObject mechanism;

    public override void Interact(GameObject interactor)
    {
        Debug.Log($"The lever creaks as {interactor.name} pulls it, activating the mechanism.");

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
