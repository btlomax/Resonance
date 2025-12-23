using Assets.Objects.InteractableObjects.Collectables;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor.Build;
using UnityEngine;

public class Apple : BaseCollectable
{
    private void Awake() => base.Awake();

    public override void OnNotePlayed(string noteName)
    {
      if(noteName == targetNote)
      {
          Debug.Log("Correct note played! Collecting apple.");
          StartCoroutine(WaitBeforeDrop(3f));
      }
      else
      {
          Debug.Log("Incorrect note played. Apple remains on tree.");
      }
    }

    public override void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            notePlayedEvent.OnNotePlayed += OnNotePlayed;
            player = other.GameObject();
        }
    }

    public override void OnTriggerExit(Collider other)
    {
        notePlayedEvent.OnNotePlayed -= OnNotePlayed;
    }

    public override void TriggerCollection()
    {
        StartCoroutine(DropThenFly());
    }

    private IEnumerator WaitBeforeDrop(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        TriggerCollection();
    }

    public override void Collect()
    {
        Debug.Log("Apple collected!");
        Destroy(gameObject);
    }

    private IEnumerator DropThenFly()
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        // Phase 1: drop
        rb.isKinematic = false;       // enable physics
        rb.useGravity = true;         // let it fall naturally

        // Wait a bit to let it drop
        yield return new WaitForSeconds(0.5f);

        // Phase 2: fly toward player
        rb.useGravity = false;        // optional: fly in straight line
        rb.linearVelocity = Vector3.zero;   // stop current movement

        Vector3 direction = (player.transform.position - transform.position).normalized;

        while (Vector3.Distance(transform.position, player.transform.position) > 0.1f)
        {
            rb.MovePosition(transform.position + direction * flightSpeed * Time.deltaTime);
            yield return null;
        }

        Collect();
    }
}
