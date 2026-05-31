using Assets.Objects.PuzzleObjects;
using Assets.Objects.PuzzleObjects.Optical;
using UnityEngine;

public class Mirror : MonoBehaviour, IOpticalElement
{
    /// <summary>
    /// Takes the incoming ray and reflects it based on the mirror's orientation. The reflected ray is then returned as the outgoing ray.
    /// </summary>
    /// <param name="incomingRay">The incoming ray to be reflected.</param>
    /// <param name="hit">The RaycastHit information from the raycast.</param>
    /// <param name="beam">The context of the beam interacting with the mirror.</param>
    /// <param name="outgoingRay">The resulting reflected ray.</param>
    /// <returns>True if the interaction was successful, false otherwise.</returns>
    public bool Interact(Ray incomingRay, RaycastHit hit, BeamContext beam, out Ray outgoingRay)
    {
        Vector3 normal = transform.up;
        Vector3 reflectedDir = Vector3.Reflect(incomingRay.direction, normal);

        outgoingRay = new Ray(transform.position, reflectedDir);
        Debug.DrawRay(outgoingRay.origin, outgoingRay.direction * 10f, beam.color, 1f);

        return true;
    }
}
