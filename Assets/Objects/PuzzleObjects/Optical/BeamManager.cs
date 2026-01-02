using Assets.Objects.PuzzleObjects;
using Assets.Objects.PuzzleObjects.Optical;
using UnityEngine;

public class BeamManager : MonoBehaviour
{
   public static BeamManager Instance { get; private set; }

    [SerializeField]
    private LayerMask beamLayerMask;

    [SerializeField]
    [Range(10f, 500f)]
    private float maxBeamDistance = 100f;

    [SerializeField]
    private int maxReflections = 10;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void ProcessBeam(Beam beam)
    {
        beam.segments.Clear();

        Ray currentRay = beam.lightBeam;
        BeamContext context = new BeamContext(beam.color);

        for (int i = 0; i < maxReflections; i++)
        {
            //Debug.DrawRay(currentRay.origin, currentRay.direction * 100f, Color.red, 1f);

            if (!Physics.Raycast(currentRay, out RaycastHit hit, maxBeamDistance, beamLayerMask))
            {
                //Nothing hit, extend beam to max distance and then stop
                beam.segments.Add(new BeamSegment(
                    currentRay.origin,
                    currentRay.origin + currentRay.direction * maxBeamDistance));

                break;
            }

            //Record segment
            beam.segments.Add(new BeamSegment(
                currentRay.origin,
                hit.point
            ));

            if (!hit.collider.TryGetComponent<IOpticalElement>(out var optical))
                break;

            //Hit something non-optical, stop the beam here
            if (optical == null)
                break;

            //Interact with the optical element to get the outgoing beam
            if(!optical.Interact(currentRay, hit, context, out Ray outgoingBeam))
                break;

            currentRay = outgoingBeam;
        }
    }
}
