using UnityEngine;

public class MirrorStone : MonoBehaviour
{
    [SerializeField] private GameObject _mirror;
    [SerializeField] private float _mirrorXAngle = 0f;
    [SerializeField] private float _mirrorZAngle = 0f;
    [SerializeField] private float _rotateAmount = 45f;

    public NotePlayedEventChannel NotePlayed;
    public ScaleDegreeToColour scaleDegreeToColour;
    public MusicalScale musicalScale;
    public string RotateClockwiseNote;
    public string RotateCounterClockwiseNote;
    public GameObject RotateClockwiseMarker;
    public GameObject RotateCounterClockwiseMarker;

    private void Awake()
    {
        for(int i = 0; i < musicalScale.NotesInScale.Length; i++)
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
        if(incomingNote == RotateClockwiseNote)
        {
            _mirrorZAngle -= _rotateAmount;
        }
        else if (incomingNote == RotateCounterClockwiseNote)
        {
            _mirrorZAngle += _rotateAmount;
        }
        else
        {
            return;
        }

        _mirror.transform.rotation = Quaternion.Euler(_mirrorXAngle, 0f, _mirrorZAngle);
    }
}
