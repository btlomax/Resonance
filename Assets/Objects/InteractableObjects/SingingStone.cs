using UnityEngine;

public class SingingStone : Interactable
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Interact(GameObject interactor)
    {
        Debug.Log($"The Singing Stone hums a melodious tune as {interactor.name} interacts with it.");
    }
}
