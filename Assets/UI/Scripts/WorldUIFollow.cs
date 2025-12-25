using TMPro;
using Unity.Cinemachine;
using UnityEngine;

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
