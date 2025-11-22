using AK.Wwise;
using System.Collections;
using UnityEngine;

/// <summary>
/// Responsible for playing sequences of notes for copy puzzles
/// </summary>
public class CopyPuzzleSequencePlayer : MonoBehaviour
{
    private bool _noteFinished = false;
    [Header("Wwise Events")]
    public AK.Wwise.Event playSequenceEvent;

    public void StartSequenceCoroutine(string[] notesToPlay)
    {
        StartCoroutine(PlayNoteSequence(notesToPlay));
    }

    private IEnumerator PlayNoteSequence(string[] notesToPlay)
    {
        Debug.Log("Playing note sequence...");

        foreach (var note in notesToPlay)
        {
            _noteFinished = false;

            AkUnitySoundEngine.SetSwitch("Env_A3_A4", note, gameObject);
            Debug.Log($"Event ID : {playSequenceEvent.Id} - Playing note: {note}");
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
