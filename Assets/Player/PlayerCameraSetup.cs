using UnityEngine;

public class PlayerCameraSetup : MonoBehaviour
{
    public float Yaw { get; private set; } = 0f;
    public float Pitch { get; private set; } = 0f;

    [SerializeField]
    private InputHandler _inputHandler; // assign your InputHandler
    [SerializeField]
    private float _rotationSpeed = 100f;
    [SerializeField]
    private float pitchMaxAngle = 0f;
    [SerializeField]
    private float pitchMinAngle = 0f;

    private void LateUpdate()
    {
        Vector2 look = _inputHandler.LookInput;

        Yaw += look.x * _rotationSpeed * Time.deltaTime;
        Pitch -= look.y * _rotationSpeed * Time.deltaTime;

        Pitch = Mathf.Clamp(Pitch, pitchMinAngle, pitchMaxAngle); // limit vertical look angle

        transform.Rotate(look.y * _rotationSpeed * Time.deltaTime, look.x * _rotationSpeed * Time.deltaTime, 0);
    }
}
