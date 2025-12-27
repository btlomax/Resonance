using Assets.Objects.PuzzleObjects;
using Assets.Objects.PuzzleObjects.Optical;
using UnityEngine;

public class Mirror : MonoBehaviour, IOpticalElement
{
    [SerializeReference]
    private GameObject _mirror;

    [Header("Mirror Settings")]
    public float mirrorZAngle = 0f;

    private void Awake()
    {
        mirrorZAngle = transform.rotation.eulerAngles.z;
    }

    public bool Interact(Ray incomingRay, RaycastHit hit, BeamContext beam, out Ray outgoingRay)
    {
        Vector3 normal = _mirror.transform.up;
        Vector3 reflectedDir = Vector3.Reflect(incomingRay.direction, normal);

        outgoingRay = new Ray(hit.point, reflectedDir);
        Debug.DrawRay(outgoingRay.origin, outgoingRay.direction * 10f, beam.color, 1f);

        return true;
    }
}
