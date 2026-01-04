using Unity.Cinemachine;
using UnityEngine;

public class RotationIndicator : MonoBehaviour
{
    public Transform target;
    public Color color;
    public float yRotation;

    private void Start()
    {
        var renderer = GetComponent<CanvasRenderer>();
        renderer.SetColor(color);
        transform.localRotation = Quaternion.Euler(0f, yRotation, 0f);
    }

    private void LateUpdate()
    {
        if (!target)
            return;

        transform.position = target.position;
    }
}
