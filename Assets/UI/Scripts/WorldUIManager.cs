using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class WorldUIManager : MonoBehaviour
{
   public static WorldUIManager Instance { get; private set; }

    [SerializeField]
    private Canvas worldCanvas;
    [SerializeField]
    private CinemachineCamera mainCamera;
    [SerializeField]
    private GameObject resonatorUI;
    [SerializeField]
    private GameObject mirrorArrow;

    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// Create a world space UI element that follows a target transform and displays prompt text on the object.
    /// Displays text passed in promptText parameter.
    /// </summary>
    /// <param name="target"></param>
    /// <param name="promptText"></param>
    /// <returns></returns>
    public GameObject CreateResonatorUI(Transform target, string promptText, Color textColor)
    {
        GameObject worldUIInstance = Instantiate(resonatorUI, worldCanvas.transform);
        WorldUIFollowText followComponent = worldUIInstance.GetComponent<WorldUIFollowText>();

        if (followComponent != null)
        {
            followComponent.target = target;
            followComponent.mainCamera = mainCamera;
            followComponent.promptText.text = promptText;
            followComponent.promptText.color = textColor;

        }

        return worldUIInstance;
    }

    public GameObject CreateRotationIndicators(Transform target, Color arrowColour, Vector3 arrowRotation)
    {
        GameObject rotationIndicator = Instantiate(mirrorArrow, worldCanvas.transform);
        RotationIndicator rotationComponent = rotationIndicator.GetComponent<RotationIndicator>();

        if(rotationComponent != null)
        {
            rotationComponent.target = target;
            rotationComponent.color = arrowColour;
            rotationComponent.direction = arrowRotation;
        }

        return rotationIndicator;
    }

    public void HidePrompt(GameObject ui)
    {
        Destroy(ui); // later replace with pooling
    }
}
