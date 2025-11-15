using Assets.SFX;
using UnityEngine;

public class AudioManager : MonoBehaviour, IPlayNote
{
    public static AudioManager Instance { get; private set; }

    [Header("Wwise Events")]
    public AK.Wwise.Event playToneEvent;

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

    public void PlayNote(float note, float duration)
    {
        AkUnitySoundEngine.SetRTPCValue("Note_Frequency", note);
        playToneEvent.Post(gameObject);
    }
}
