using UnityEngine;

public class PlayerCameraSetup : MonoBehaviour
{
    public Transform followTarget;
    public float Yaw { get; private set; } = 0f;
    public float Pitch { get; private set; } = 0f;

    public Vector3 offset = new Vector3(0f, 8f, -8f);
    public bool useFixedRotation = true;

    [SerializeField]
    private InputHandler _inputHandler; // assign your InputHandler

    /*
    [SerializeField]
    private float _rotationSpeed = 100f;
    [SerializeField]
    private float pitchMaxAngle = 0f;
    [SerializeField]
    private float pitchMinAngle = 0f;
    */

    private void LateUpdate()
    {
        if(!followTarget) return;

        Vector3 nextPosition = followTarget.position;
        transform.position = Vector3.Lerp(transform.position, nextPosition, Time.deltaTime * 5f);

        if (useFixedRotation)
            transform.rotation = Quaternion.Euler(45f, 0f, 0f);
        else { } // Add rotation here later 

    }
}
