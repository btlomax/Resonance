using Assets.SFX;
using UnityEngine;


/// <summary>
/// I need several "channels" to play different notes simultaneously. Each note played should be independent, allowing for overlapping sounds without cutting each other off.
/// In WWise, I have several events set up to play tones, each with its own RTPC for frequency control. These are my channels.
/// When an object needs to play a tone, it just calls the AudioManager and gives it the name of the event to post ie "Kick" "Bass" "Lead" etc.
/// </summary>
public class AudioManager : MonoBehaviour, IPlayNote
{
    public static AudioManager Instance { get; private set; }

    [Header("Wwise Events")]
    public AK.Wwise.Event playToneEvent;

    [SerializeField]
    private AK.Wwise.RTPC noteFrequencyRTPC = null;

    private uint _currentNote = 0;
    private ToneGenerator _toneGenerator;
    private EnvironmentMusicPlayer _environmentMusicPlayer;

    public AudioManager()
    {
        _toneGenerator = new ToneGenerator();
        Debug.Log("Tone generator created.");
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); 
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        _environmentMusicPlayer = GetComponent<EnvironmentMusicPlayer>();

        Debug.Log("AudioManager initialized.");
    }

    public void PlayLoopingNote(string note)
    {
        float frequency = _toneGenerator.ConvertNoteToFrequency(note);
        Debug.Log($"Playing note with frequency: {note} Hz");
       
        if(_currentNote == 0)
        {
            AkUnitySoundEngine.SetRTPCValue("Note_Frequency", frequency);
            _currentNote = AkUnitySoundEngine.PostEvent(playToneEvent.Id, gameObject);
        }
    }

    public void StopLoopingNote()
    {
        playToneEvent.Stop(gameObject);
        _currentNote = 0;
    }

    public void PlayEnvironmentNoteSequence(NoteScriptObj[] section)
    {
        Debug.Log("Entered PlayEnvironmentNoteSequence in AudioManager.");

        try
        {
            _environmentMusicPlayer.PlayNoteSequence(section);
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error playing environment note sequence: {ex.Message}");
        }
    }
}
