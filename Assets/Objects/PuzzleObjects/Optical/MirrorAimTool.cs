using UnityEngine;

public class MirrorAimTool : MonoBehaviour
{
    [Header("Assign these")]
    public Transform beamSource;
    public Transform mirrorA;
    public Transform mirrorB;

    [ContextMenu("Aim Mirror A")]
    void AimMirror()
    {
        if (!beamSource || !mirrorA || !mirrorB)
        {
            Debug.LogError("Missing reference");
            return;
        }

        // Incoming direction (source -> mirror)
        Vector3 incomingDir =
            (mirrorA.position - beamSource.position).normalized;

        // Outgoing direction (mirror -> target mirror)
        Vector3 outgoingDir =
            (mirrorB.position - mirrorA.position).normalized;

        // Mirror normal bisects the angle
        Vector3 desiredNormal =
            (incomingDir + outgoingDir).normalized;

        if (desiredNormal == Vector3.zero)
        {
            Debug.LogWarning("Invalid geometry");
            return;
        }

        // Rotate mirror so its forward axis matches the normal
        mirrorA.rotation =
            Quaternion.LookRotation(desiredNormal, mirrorA.up);
    }

    void OnDrawGizmos()
    {
        if (!beamSource || !mirrorA || !mirrorB) return;

        Gizmos.color = Color.red;
        Gizmos.DrawRay(mirrorA.position,
            (mirrorA.position - beamSource.position).normalized);

        Gizmos.color = Color.green;
        Gizmos.DrawRay(mirrorA.position,
            (mirrorB.position - mirrorA.position).normalized);

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(mirrorA.position, mirrorA.forward);
    }
}
