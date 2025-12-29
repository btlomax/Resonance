using UnityEngine;

public class MirrorAimTool : MonoBehaviour
{
    public Transform beamSource;
    public Transform mirrorA;
    public Transform mirrorB;

    [ContextMenu("Aim Mirror A")]
    void AimMirror()
    {
        Vector3 incomingDir =
            (mirrorA.position - beamSource.position).normalized;

        Vector3 outgoingDir =
            (mirrorB.position - mirrorA.position).normalized;

        Vector3 desiredNormal =
            (incomingDir + outgoingDir).normalized;

        if (desiredNormal == Vector3.zero)
        {
            Debug.LogWarning("Invalid reflection geometry");
            return;
        }

        mirrorA.rotation =
            Quaternion.LookRotation(desiredNormal, mirrorA.up);
    }
}

