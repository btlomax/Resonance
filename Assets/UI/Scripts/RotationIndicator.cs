using Unity.Cinemachine;
using UnityEngine;

public class RotationIndicator : MonoBehaviour
{
    public Transform target;
    public Color color;
    public Vector3 direction;

    private void Start()
    {
        var renderer = GetComponent<CanvasRenderer>();
        renderer.SetColor(color);

        transform.rotation = Quaternion.LookRotation(direction, Vector3.up) * Quaternion.Euler(0f, -90f, 0f);
    }

    private void LateUpdate()
    {
        if (!target)
            return;

        transform.position = target.position;
    }
}
