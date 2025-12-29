using UnityEngine;

public class MirrorStone : MonoBehaviour
{
    [SerializeField] private GameObject _mirror;
    [SerializeField] private float _mirrorXAngle = 0f;
    [SerializeField]  private float _mirrorYAngle = 0f;
    [SerializeField] private float _mirrorZAngle = 0f;
    [SerializeField] private float _rotateAmount = 45f;

    public NotePlayedEventChannel NotePlayed;
    public ScaleDegreeToColour scaleDegreeToColour;
    public MusicalScale musicalScale;
    public string RotateClockwiseNote;
    public string RotateCounterClockwiseNote;
    public GameObject RotateClockwiseMarker;
    public GameObject RotateCounterClockwiseMarker;
    public bool CanRotate = true;


    private void Awake()
    {
        _mirrorYAngle = _mirror.transform.rotation.y;

        for (int i = 0; i < musicalScale.NotesInScale.Length; i++)
        {
            if (musicalScale.NotesInScale[i].noteTitle == RotateClockwiseNote)
            {
                var renderer = RotateClockwiseMarker.GetComponentInChildren<Renderer>();
                renderer.material.SetColor("_BaseColour", scaleDegreeToColour.scaleDegreeColours[i]);

            }
            if (musicalScale.NotesInScale[i].noteTitle == RotateCounterClockwiseNote)
            {
                var renderer = RotateCounterClockwiseMarker.GetComponentInChildren<Renderer>();
                renderer.material.SetColor("_BaseColour", scaleDegreeToColour.scaleDegreeColours[i]);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            NotePlayed.OnNotePlayed += RotateMirror;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        NotePlayed.OnNotePlayed -= RotateMirror;
    }

    private void RotateMirror(string incomingNote)
    {
        if (!CanRotate)
            return;

        if (incomingNote != RotateClockwiseNote &&
            incomingNote != RotateCounterClockwiseNote)
            return;

        float delta =
            incomingNote == RotateClockwiseNote
                ? -_rotateAmount
                : _rotateAmount;

        _mirror.transform.Rotate(Vector3.forward, delta, Space.Self);
    }
}
