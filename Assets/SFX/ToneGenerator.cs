using UnityEngine;

public class ToneGenerator
{
    public AudioClip CreateSineWave(float frequency, float duration, int sampleRate = 44100)
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

    public float ConvertNoteToFrequency(string note)
    {

        // Note format: "C4", "A#3", etc.
        switch (note)
        {
           case "A3": return 220f;
           case "Bb3": return 233.08f;
           case "B3": return 246.94f;
           case "C4": return 261.63f;
           case "Db4": return 277.18f;
           case "D4": return 293.66f;
           case "Eb4": return 311.13f;
           case "E4": return 329.63f;
           case "F4": return 349.23f;
           case "Gb4": return 369.99f;
           case "G4": return 392.00f;
           case "Ab4": return 415.30f;
           case "A4": return 440f;

            default: return 440f; // Default to A4
        }
    }
}
