using Assets.Objects.PuzzleObjects;
using Assets.Objects.PuzzleObjects.Optical;
using UnityEngine;

/// <summary>
/// Manages the processing and simulation of light beams within the scene, including handling beam reflection and
/// interaction with optical elements.
/// </summary>
/// <remarks><para> <see cref="BeamManager"/> is implemented as a singleton and can be accessed via the <see
/// cref="Instance"/> property. It is responsible for tracing the path of a beam, applying reflection logic, and
/// interacting with objects that implement the <c>IOpticalElement</c> interface. </para> <para> Attach this component
/// to a GameObject in your scene to enable beam processing functionality. Only one instance of <see
/// cref="BeamManager"/> should exist at a time; additional instances will be destroyed automatically. </para></remarks>
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

    /// <summary>
    /// Traces the path of a light beam through the scene, updating the specified <see cref="Beam"/> with its resulting
    /// segments based on interactions with optical elements and scene geometry.
    /// </summary>
    /// <remarks>The method simulates the propagation of a light beam, including reflections and interactions
    /// with objects implementing <see cref="IOpticalElement"/>. The number of reflections is limited by an internal
    /// maximum. The <c>segments</c> collection of the <paramref name="beam"/> is cleared and then filled with the
    /// resulting beam path segments. If the beam does not intersect any objects within the allowed distance, it is
    /// extended to the maximum range.</remarks>
    /// <param name="beam">The <see cref="Beam"/> instance to process. The method clears and repopulates its <c>segments</c> collection to
    /// represent the computed path of the beam.</param>
    public void ProcessBeam(Beam beam)
    {
        beam.segments.Clear();

        Ray currentRay = beam.lightBeam;
        BeamContext context = new BeamContext(beam.color);

        for (int i = 0; i < maxReflections; i++)
        {
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
