using UnityEngine;
using static UnityEngine.GridBrushBase;

public class MirrorStone : MonoBehaviour
{
    [SerializeField] private GameObject _mirror;
    [SerializeField] private float _rotateAmount = 10;
    [SerializeField] private int _rotationDirection = 0;
    [SerializeField] private GameObject _rotateClockwiseUIPrompt;
    [SerializeField] private GameObject _rotateCounterClockwiseUIPrompt;

    public NotePlayedEventChannel NotePlayed;
    public OnNoteStoppedEvent OnNoteStopped;
    public ScaleDegreeToColour scaleDegreeToColour;
    public MusicalScale musicalScale;
    public string RotateClockwiseNote;
    public string RotateCounterClockwiseNote;
    public Transform RotateClockwiseMarkerLocation;
    public Transform RotateCounterClockwiseMarkerLocation;
   
    public bool CanRotate = true;

    private void Start()
    {
        if(CanRotate)
        {
            for (int i = 0; i < musicalScale.NotesInScale.Length; i++)
            {
                if (musicalScale.NotesInScale[i].noteTitle == RotateClockwiseNote)
                {
                    if(_rotateClockwiseUIPrompt == null)
                    {
                        _rotateClockwiseUIPrompt = WorldUIManager.Instance.CreateRotationIndicators(RotateClockwiseMarkerLocation, scaleDegreeToColour.scaleDegreeColours[i], transform.right);
                        _rotateClockwiseUIPrompt.SetActive(false);
                    }
                }

                if (musicalScale.NotesInScale[i].noteTitle == RotateCounterClockwiseNote)
                {
                    if(_rotateCounterClockwiseUIPrompt == null)
                    {
                        var rotationOffset = (transform.rotation.y + 90) * 57.3f;
                        _rotateCounterClockwiseUIPrompt = WorldUIManager.Instance.CreateRotationIndicators(RotateCounterClockwiseMarkerLocation, scaleDegreeToColour.scaleDegreeColours[i], -transform.right);
                        _rotateCounterClockwiseUIPrompt.SetActive(false);
                    }
                }
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
        if (other.CompareTag("Player") && CanRotate)
        {
            NotePlayed.OnNotePlayed += RotateMirror;
            OnNoteStopped.OnNoteStopped += StopRotating;
            _rotateClockwiseUIPrompt.SetActive(true);
            _rotateCounterClockwiseUIPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        NotePlayed.OnNotePlayed -= RotateMirror;
        OnNoteStopped.OnNoteStopped -= StopRotating;
        _rotateClockwiseUIPrompt.SetActive(false);
        _rotateCounterClockwiseUIPrompt.SetActive(false);
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
