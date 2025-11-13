using UnityEngine;

public static class ToneGenerator
{
    public static AudioClip CreateSineWave(float frequency, float duration, int sampleRate = 44100)
    {
        int sampleCount = (int)(sampleRate * duration);
        float[] samples = new float[sampleCount];
        for (int i = 0; i < sampleCount; i++)
        {
            samples[i] = Mathf.Sin(2 * Mathf.PI * frequency * i / sampleRate);
        }
        AudioClip audioClip = AudioClip.Create("SineWave", sampleCount, 1, sampleRate, false);
        audioClip.SetData(samples, 0);
        return audioClip;
    }

    public static float ConvertNoteToFrequency(string note)
    {

        // Note format: "C4", "A#3", etc.
        switch (note)
        {
            case "C4": return 261f;
            case "C#4": return 277f;
            case "D4": return 293f;
            case "D#4": return 311f;
            case "E4": return 329f;
            case "F4": return 349f;
            case "F#4": return 369f;
            case "G4": return 392f;
            case "G#4": return 415f;
            case "A4": return 440f;
            case "A#4": return 466f;
            case "B4": return 493f;
            default: return 440f; // Default to A4
        }
    }
}
