using UnityEngine;

public static class ToneGenerator
{
    public static AudioClip CreateSineWave(float frequency, float duration = 0.4f, int sampleRate = 44100)
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
}
