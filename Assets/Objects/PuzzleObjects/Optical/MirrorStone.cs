using UnityEngine;
using static UnityEngine.GridBrushBase;

public class MirrorStone : MonoBehaviour
{
    [SerializeField] private GameObject _mirror;
    [SerializeField] private float _rotateAmount = 10;
    [SerializeField] private int _rotationDirection = 0;

    public NotePlayedEventChannel NotePlayed;
    public OnNoteStoppedEvent OnNoteStopped;
    public ScaleDegreeToColour scaleDegreeToColour;
    public MusicalScale musicalScale;
    public string RotateClockwiseNote;
    public string RotateCounterClockwiseNote;
    public GameObject RotateClockwiseMarker;
    public GameObject RotateCounterClockwiseMarker;
    public bool CanRotate = true;

    private void Awake()
    {
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

    private void Update()
    {
        if (_rotationDirection == 0) return;

        if(CanRotate)
        {
            _mirror.transform.Rotate(
           Vector3.forward,
           _rotationDirection * _rotateAmount * Time.deltaTime,
           Space.Self
           );
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            NotePlayed.OnNotePlayed += RotateMirror;
            OnNoteStopped.OnNoteStopped += StopRotating;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        NotePlayed.OnNotePlayed -= RotateMirror;
        OnNoteStopped.OnNoteStopped -= StopRotating;
    }

    private void RotateMirror(string incomingNote)
    {
        if (!CanRotate) return;

        if (incomingNote == RotateClockwiseNote)
            _rotationDirection = -1;
        else if (incomingNote == RotateCounterClockwiseNote)
            _rotationDirection = 1;
    }

    private void StopRotating()
    {
        _rotationDirection = 0;
    }
}

// x: 49.391 y: 52.5 z: 78.5
