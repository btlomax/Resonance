using Assets.Objects.PuzzleObjects;
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
        Ray currentRay = beam.lightBeam;

        for (int i = 0; i < maxReflections; i++)
        {
            Debug.DrawRay(currentRay.origin, currentRay.direction * 100f, Color.red, 1f);

            if (!Physics.Raycast(currentRay, out RaycastHit hit, maxBeamDistance, beamLayerMask))
            {
                //Nothing hit, extend beam to max distance and then stop
                beam.segments.Add(new BeamSegment
                {
                    origin = currentRay.origin,
                    end = currentRay.origin + currentRay.direction * maxBeamDistance
                });

                break;
            }

            //Record segment
            beam.segments.Add(new BeamSegment
            {
                origin = currentRay.origin,
                end = hit.point
            });

            IOpticalElement opticalElement = hit.collider.GetComponent<IOpticalElement>();

            //Hit something non-optical, stop the beam here
            if (opticalElement == null)
                break;

            //Interact with the optical element to get the outgoing beam
            if(!opticalElement.Interact(currentRay, hit, beam, out Ray outgoingBeam))
                break;

            currentRay = outgoingBeam;
        }
    }
}
