using AK.Wwise;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Responsible for playing sequences of notes for copy puzzles
/// </summary>
public class CopyPuzzleSequencePlayer : MonoBehaviour
{
    private bool _noteFinished = false;
    [Header("Wwise Events")]
    public AK.Wwise.Event playSequenceEvent;

    [Header("Light Settings")]
    public GameObject playArrow;
    public Color playLightEmissionColour;
    public float intensity;
    private Renderer _playLightRenderer;
    private Material _playLightMaterial;

    private void Awake()
    {
        _playLightRenderer = playArrow.GetComponent<Renderer>();

        if (_playLightRenderer != null)
            _playLightMaterial = _playLightRenderer.material;
    }

    public void StartSequenceCoroutine(List<string> notesToPlay)
    {
        StartCoroutine(PlayNoteSequence(notesToPlay));
    }

    private IEnumerator PlayNoteSequence(List<string> notesToPlay)
    {
        Debug.Log("Playing note sequence...");

        _playLightMaterial.EnableKeyword("_EMISSION");

        //_playLightMaterial.SetColor("_EmissionColor", playLightEmissionColour * intensity);

        foreach (var note in notesToPlay)
        {
            _noteFinished = false;

            AkUnitySoundEngine.SetSwitch("Env_Single_A3_A4", note, gameObject);
            Debug.Log($"Event ID : {playSequenceEvent.Id} - Playing note: {note}");
            AkUnitySoundEngine.PostEvent(playSequenceEvent.Id, gameObject, (uint)AkCallbackType.AK_EndOfEvent, NoteFinishedCallback, null);

            yield return new WaitUntil(() => _noteFinished);
        }

        _playLightMaterial.DisableKeyword("_EMISSION");

    }

    private void NoteFinishedCallback(object in_cookie, AkCallbackType type, AkCallbackInfo info)
    {
        if (type == AkCallbackType.AK_EndOfEvent)
            _noteFinished = true;
    }
}
