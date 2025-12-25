using TMPro;
using Unity.Cinemachine;
using UnityEngine;

/// <summary>
///  This component makes a UI element follow a target in world space and face the main camera.
/// </summary>
public class WorldUIFollow : MonoBehaviour
{
    public Transform target;
    public CinemachineCamera mainCamera;
    public TMP_Text promptText;

    private void Awake()
    {
        promptText = GetComponent<TMP_Text>();
    }

    private void LateUpdate()
    {
        if(!target || !mainCamera)
            return;

        transform.position = target.position;
        transform.forward = mainCamera.transform.forward;
    }
}
