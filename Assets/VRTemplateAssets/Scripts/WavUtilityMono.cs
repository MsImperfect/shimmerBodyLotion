using System;
using System.IO;
using UnityEngine;

public class WavUtilityMono : MonoBehaviour
{
    /// <summary>
    /// Converts an AudioClip to WAV byte[] and writes to disk.
    /// </summary>
    /// <param name="clip">The recorded AudioClip</param>
    /// <param name="savePath">Full path including .wav filename</param>
    public void SaveWavFile(AudioClip clip, string savePath)
    {
        if (clip == null)
        {
            Debug.LogError("❌ Cannot save WAV: AudioClip is null.");
            return;
        }

        try
        {
            byte[] wavData = ConvertToWav(clip);
            File.WriteAllBytes(savePath, wavData);
            Debug.Log($"✅ WAV saved at: {savePath}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"❌ Failed to save WAV: {ex.Message}");
        }
    }

    /// <summary>
    /// Converts an AudioClip into a byte array in WAV format.
    /// </summary>
    public byte[] ConvertToWav(AudioClip clip)
    {
        using (MemoryStream stream = new MemoryStream())
        {
            int sampleCount = clip.samples * clip.channels;
            int frequency = clip.frequency;

            float[] samples = new float[sampleCount];
            clip.GetData(samples, 0);

            // Convert float [-1,1] to Int16 PCM
            Int16[] intData = new Int16[sampleCount];
            byte[] bytesData = new byte[sampleCount * 2];
            const float rescaleFactor = 32767;

            for (int i = 0; i < samples.Length; i++)
            {
                intData[i] = (short)(samples[i] * rescaleFactor);
                byte[] byteArr = BitConverter.GetBytes(intData[i]);
                byteArr.CopyTo(bytesData, i * 2);
            }

            WriteWavHeader(stream, clip.channels, frequency, sampleCount);
            stream.Write(bytesData, 0, bytesData.Length);

            return stream.ToArray();
        }
    }

    /// <summary>
    /// Writes the WAV header into the stream.
    /// </summary>
    private void WriteWavHeader(Stream stream, int channels, int sampleRate, int sampleCount)
    {
        int byteRate = sampleRate * channels * 2;
        int subChunk2Size = sampleCount * channels * 2;
        int chunkSize = 36 + subChunk2Size;

        using (BinaryWriter writer = new BinaryWriter(stream, System.Text.Encoding.UTF8, true))
        {
            writer.Write(System.Text.Encoding.UTF8.GetBytes("RIFF"));
            writer.Write(chunkSize);
            writer.Write(System.Text.Encoding.UTF8.GetBytes("WAVE"));

            writer.Write(System.Text.Encoding.UTF8.GetBytes("fmt "));
            writer.Write(16);
            writer.Write((short)1);
            writer.Write((short)channels);
            writer.Write(sampleRate);
            writer.Write(byteRate);
            writer.Write((short)(channels * 2));
            writer.Write((short)16);

            writer.Write(System.Text.Encoding.UTF8.GetBytes("data"));
            writer.Write(subChunk2Size);
        }
    }
}
