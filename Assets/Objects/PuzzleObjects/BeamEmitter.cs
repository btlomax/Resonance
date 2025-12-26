using UnityEngine;

public class BeamEmitter : MonoBehaviour
{
    [Header("Beam Settings")]
    [SerializeField] private Transform beamOrigin;
    [SerializeField] private Color beamColor = Color.white;
    public string resonantNote;
    public NotePlayedEventChannel onNotePlayed;

    private void Update()
    {
        
    }

    public void Emit(string incomingNote)
    {
        if(beamOrigin == null)
        {
            Debug.LogError("Beam Origin is not assigned.");
            return;
        }

        if(incomingNote != resonantNote || string.IsNullOrEmpty(incomingNote))
        {
            Debug.Log("Incoming note " + incomingNote + " does not match resonant note " + resonantNote + ". Beam not emitted.");
            return;
        }

        Ray ray = new Ray(beamOrigin.position, beamOrigin.forward);
        Debug.DrawRay(beamOrigin.position, beamOrigin.forward * 10f, beamColor);
        Beam emittedBeam = new Beam(ray, beamColor);

        Debug.Log("Emitting beam from " + beamOrigin.position + " in direction " + beamOrigin.forward);

        BeamManager.Instance.ProcessBeam(emittedBeam);
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("Player entered BeamEmitter trigger. Subscribing to note played events.");
            onNotePlayed.OnNotePlayed += Emit;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        Debug.Log("Player exited BeamEmitter trigger. Unsubscribing from note played events.");
        onNotePlayed.OnNotePlayed -= Emit;
    }
}
