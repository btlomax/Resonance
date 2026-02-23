using Assets.Data.GameManagement;
using Assets.Objects.InteractableObjects.Collectables;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor.Build;
using UnityEngine;

public class Apple : BaseCollectable
{
    private void Awake() => base.Awake();


    public override void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            notePlayedEvent.OnNotePlayed += OnNotePlayed;
            player = other.gameObject;

            GameEventDispatcher.Instance.TriggerEvent(GameUI_Event.ReachedApples);

            // Start glowing here
        }
    }

    public override void OnTriggerExit(Collider other)
    {
        notePlayedEvent.OnNotePlayed -= OnNotePlayed;
    }

    public override void OnNotePlayed(string noteName)
    {
        if (noteName == targetNote)
        {
            Debug.Log("Correct note played! Collecting apple.");
            StartCoroutine(WaitBeforeDrop(0.5f));
        }
        else
        {
            Debug.Log("Incorrect note played. Apple remains on tree.");
        }
    }

    private IEnumerator WaitBeforeDrop(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        TriggerCollection();
    }

    public override void TriggerCollection()
    {
        StartCoroutine(DropThenFly());
    }
    private IEnumerator DropThenFly()
    {
        foreach(var obj in numberOfCollectables)
        {
            Rigidbody rb = obj.GetComponent<Rigidbody>();

            // Drop
            rb.isKinematic = false;       // enable physics
            rb.useGravity = true;         // let it fall naturally

            // Wait a bit to let it drop
            yield return new WaitForSeconds(0.3f);

            // Fly toward player
            rb.useGravity = false;        // optional: fly in straight line
            rb.linearVelocity = Vector3.zero;   // stop current movement

            if(player != null)
            {
                while (Vector3.Distance(obj.transform.position, player.transform.position) > 1f)
                {
                    Vector3 direction = (player.transform.position - obj.transform.position).normalized;
                    rb.MovePosition(obj.transform.position + direction * flightSpeed * Time.deltaTime);
                    yield return null;
                }

                Collect(obj);
            }
        }

        GameEventDispatcher.Instance.TriggerEvent(GameUI_Event.ApplesCollected);
    }

    public override void Collect(GameObject gameObject)
    {
        Debug.Log("Apple collected!");

        player.GetComponent<PlayerInventory>().AddItem("Apple", 1);

        gameObject.SetActive(false); // hide the apple object
        Destroy(gameObject);
    }
}
