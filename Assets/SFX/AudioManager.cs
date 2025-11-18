using Assets.SFX;
using UnityEngine;

public class AudioManager : MonoBehaviour, IPlayNote
{
    public static AudioManager Instance { get; private set; }

    [Header("Wwise Events")]
    public AK.Wwise.Event playToneEvent;

    [SerializeField]
    private AK.Wwise.RTPC noteFrequencyRTPC = null;

    private uint _currentNote = 0;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        Debug.Log("AudioManager initialized.");
    }

    public void PlayNote(float note)
    {
        Debug.Log($"Playing note with frequency: {note} Hz");
       
        if(_currentNote == 0)
        {
            AkUnitySoundEngine.SetRTPCValue("Note_Frequency", note);
            _currentNote = playToneEvent.Post(gameObject);
        }
    }

    public void StopNote()
    {
        playToneEvent.Stop(gameObject);
        _currentNote = 0;
    }
}
