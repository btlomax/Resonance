using AK.Wwise;
using System.Collections;
using UnityEngine;

public class EnvironmentMusicPlayer : MonoBehaviour
{
    private bool _noteFinished = false;
    [Header("Wwise Events")]
    public AK.Wwise.Event playSequenceEvent;

    public IEnumerator PlayNoteSequence(NoteScriptObj[] notesToPlay)
    {
        Debug.Log("Playing note sequence...");

        foreach (var note in notesToPlay)
        {
            _noteFinished = false;

            AkUnitySoundEngine.SetSwitch("SequencePlayer", note.noteTitle, gameObject);
            Debug.Log($"Event ID : {playSequenceEvent.Id} - Playing note: {note.noteTitle} (Frequency: {note.noteFrequency} Hz)");
            AkUnitySoundEngine.PostEvent(playSequenceEvent.Id, gameObject, (uint)AkCallbackType.AK_EndOfEvent, NoteFinishedCallback, null);

            yield return new WaitUntil(() => _noteFinished);
        }
    }

    private void NoteFinishedCallback(object in_cookie, AkCallbackType type, AkCallbackInfo info)
    {
        if (type == AkCallbackType.AK_EndOfEvent)
            _noteFinished = true;
    }
}
