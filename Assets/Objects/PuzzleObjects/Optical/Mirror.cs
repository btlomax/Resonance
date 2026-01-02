using Assets.Objects.PuzzleObjects;
using Assets.Objects.PuzzleObjects.Optical;
using UnityEngine;

public class Mirror : MonoBehaviour, IOpticalElement
{
    public bool Interact(Ray incomingRay, RaycastHit hit, BeamContext beam, out Ray outgoingRay)
    {
        Vector3 normal = transform.up;
        Vector3 reflectedDir = Vector3.Reflect(incomingRay.direction, normal);

        outgoingRay = new Ray(transform.position, reflectedDir);
        Debug.DrawRay(outgoingRay.origin, outgoingRay.direction * 10f, beam.color, 1f);

        return true;
    }
}
