//using UnityEngine;
//using System.IO;
//using System.Collections.Generic;

//public class VRRecorder : MonoBehaviour
//{
//    [Header("Recording Settings")]
//    public string fileName = "MyVRClip";
//    public int recordLength = 5;
//    public int sampleRate = 44100;

//    [HideInInspector]
//    public List<AudioClip> audioClips = new List<AudioClip>();

//    private AudioClip recordedClip;
//    private bool isRecording = false;
//    private string folderPath;

//    void Awake()
//    {
//        folderPath = Path.Combine(Application.persistentDataPath, "VRRecordings");
//        if (!Directory.Exists(folderPath))
//            Directory.CreateDirectory(folderPath);

//        Debug.Log("🎧 VR Recordings folder: " + folderPath);
//    }

//    void Start()
//    {
//        // Load any existing WAVs on startup
//        LoadAllRecordings();
//    }

//    // ------------------------------
//    // Button: Start/Stop recording
//    public void ToggleRecording()
//    {
//        Debug.Log("Button clicked!");
//        if (!isRecording)
//            StartRecording();
//        else
//            StopAndSave();
//    }

//    private void StartRecording()
//    {
//        if (Microphone.devices.Length == 0)
//        {
//            Debug.LogWarning("No microphone detected!");
//            return;
//        }

//        isRecording = true;
//        recordedClip = Microphone.Start(null, false, recordLength, sampleRate);
//        Debug.Log("🎙 Recording started...");
//    }

//    private void StopAndSave()
//    {
//        if (!isRecording) return;
//        isRecording = false;
//        Microphone.End(null);

//        if (recordedClip == null)
//        {
//            Debug.LogWarning("No audio recorded!");
//            return;
//        }

//        string timestamp = System.DateTime.Now.ToString("yyyyMMdd_HHmmss");
//        string fullPath = Path.Combine(folderPath, $"{fileName}_{timestamp}.wav");

//        SaveWav(recordedClip, fullPath);
//        Debug.Log($"✅ Audio saved: {fullPath}");

//        // Load the new clip immediately
//        AudioClip newClip = LoadWav(fullPath);
//        if (newClip != null) audioClips.Add(newClip);
//    }

//    // ------------------------------
//    // Load all existing recordings
//    public void LoadAllRecordings()
//    {
//        audioClips.Clear();
//        if (!Directory.Exists(folderPath)) return;

//        string[] files = Directory.GetFiles(folderPath, "*.wav");
//        foreach (string path in files)
//        {
//            AudioClip clip = LoadWav(path);
//            if (clip != null) audioClips.Add(clip);
//        }
//        Debug.Log($"📂 Loaded {audioClips.Count} clips from {folderPath}");
//    }

//    // ------------------------------
//    // Save WAV (unchanged from before)
//    private void SaveWav(AudioClip clip, string savePath)
//    {
//        float[] samples = new float[clip.samples * clip.channels];
//        clip.GetData(samples, 0);

//        using (MemoryStream stream = new MemoryStream())
//        {
//            byte[] header = new byte[44];
//            stream.Write(header, 0, header.Length);

//            const float rescale = 32767;
//            byte[] bytesData = new byte[samples.Length * 2];
//            for (int i = 0; i < samples.Length; i++)
//            {
//                short val = (short)(samples[i] * rescale);
//                System.BitConverter.GetBytes(val).CopyTo(bytesData, i * 2);
//            }
//            stream.Write(bytesData, 0, bytesData.Length);

//            stream.Seek(0, SeekOrigin.Begin);
//            WriteWavHeader(stream, clip.channels, clip.frequency, clip.samples * clip.channels);
//            File.WriteAllBytes(savePath, stream.ToArray());
//        }
//    }

//    private void WriteWavHeader(Stream stream, int channels, int sampleRate, int sampleCount)
//    {
//        int byteRate = sampleRate * channels * 2;
//        int subChunk2Size = sampleCount * 2;
//        int chunkSize = 36 + subChunk2Size;

//        using (BinaryWriter writer = new BinaryWriter(stream, System.Text.Encoding.UTF8, true))
//        {
//            writer.Write(System.Text.Encoding.UTF8.GetBytes("RIFF"));
//            writer.Write(chunkSize);
//            writer.Write(System.Text.Encoding.UTF8.GetBytes("WAVE"));
//            writer.Write(System.Text.Encoding.UTF8.GetBytes("fmt "));
//            writer.Write(16);
//            writer.Write((short)1);
//            writer.Write((short)channels);
//            writer.Write(sampleRate);
//            writer.Write(byteRate);
//            writer.Write((short)(channels * 2));
//            writer.Write((short)16);
//            writer.Write(System.Text.Encoding.UTF8.GetBytes("data"));
//            writer.Write(subChunk2Size);
//        }
//    }

//    // ------------------------------
//    // Minimal WAV loader (16-bit PCM)
//    private AudioClip LoadWav(string path)
//    {
//        byte[] bytes = File.ReadAllBytes(path);
//        if (bytes.Length < 44) return null;

//        int channels = System.BitConverter.ToInt16(bytes, 22);
//        int freq = System.BitConverter.ToInt32(bytes, 24);
//        int dataStart = 44;
//        int sampleCount = (bytes.Length - dataStart) / 2;

//        float[] data = new float[sampleCount];
//        int offset = dataStart;
//        for (int i = 0; i < sampleCount; i++)
//        {
//            short s = System.BitConverter.ToInt16(bytes, offset);
//            data[i] = s / 32768f;
//            offset += 2;
//        }

//        AudioClip clip = AudioClip.Create(Path.GetFileNameWithoutExtension(path),
//                                          sampleCount / channels,
//                                          channels,
//                                          freq,
//                                          false);
//        clip.SetData(data, 0);
//        return clip;
//    }
//}
using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class VRRecorder : MonoBehaviour
{
    [Header("Recording Settings")]
    public string fileName = "MyVRClip";
    public int recordLength = 5;
    public int sampleRate = 44100;

    [HideInInspector]
    public List<AudioClip> audioClips = new List<AudioClip>();

    private AudioClip recordedClip;
    private bool isRecording = false;
    private string folderPath;

    void Awake()
    {
        folderPath = Path.Combine(Application.persistentDataPath, "VRRecordings");
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        Debug.Log("🎧 VR Recordings folder: " + folderPath);
    }

    void Start()
    {
        LoadAllRecordings();
    }

    // ------------------------------
    // Button: Start/Stop recording
    public void ToggleRecording()
    {
        Debug.Log("Button clicked!");
        if (!isRecording)
            StartRecording();
        else
            StopRecordingAndSave();
    }

    private void StartRecording()
    {
        if (Microphone.devices.Length == 0)
        {
            Debug.LogWarning("No microphone detected!");
            return;
        }

        isRecording = true;
        Debug.Log("🎙 Recording started...");
        StartCoroutine(RecordAndSaveCoroutine());
    }

    private void StopRecordingAndSave()
    {
        if (!isRecording) return;
        isRecording = false;
        Microphone.End(null);
        Debug.Log("⏹ Recording stopped manually.");
    }

    private IEnumerator RecordAndSaveCoroutine()
    {
        recordedClip = Microphone.Start(null, false, recordLength, sampleRate);

        float elapsedTime = 0f;
        while (elapsedTime < recordLength && isRecording)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (!isRecording)
        {
            yield break;
        }

        SaveClip();
    }

    private void SaveClip()
    {
        if (recordedClip == null)
        {
            Debug.LogWarning("No audio recorded!");
            return;
        }

        string timestamp = System.DateTime.Now.ToString("yyyyMMdd_HHmmss");
        string fullPath = Path.Combine(folderPath, $"{fileName}_{timestamp}.wav");

        SaveWav(recordedClip, fullPath);
        Debug.Log($"✅ Audio saved: {fullPath}");

        AudioClip newClip = LoadWav(fullPath);
        if (newClip != null) audioClips.Add(newClip);

        isRecording = false;
    }

    // ------------------------------
    // Load all existing recordings
    public void LoadAllRecordings()
    {
        audioClips.Clear();
        if (!Directory.Exists(folderPath)) return;

        string[] files = Directory.GetFiles(folderPath, "*.wav");
        foreach (string path in files)
        {
            AudioClip clip = LoadWav(path);
            if (clip != null) audioClips.Add(clip);
        }
        Debug.Log($"📂 Loaded {audioClips.Count} clips from {folderPath}");
    }

    // ------------------------------
    // Save WAV
    private void SaveWav(AudioClip clip, string savePath)
    {
        float[] samples = new float[clip.samples * clip.channels];
        clip.GetData(samples, 0);

        using (MemoryStream stream = new MemoryStream())
        {
            byte[] header = new byte[44];
            stream.Write(header, 0, header.Length);

            const float rescale = 32767;
            byte[] bytesData = new byte[samples.Length * 2];
            for (int i = 0; i < samples.Length; i++)
            {
                short val = (short)(samples[i] * rescale);
                System.BitConverter.GetBytes(val).CopyTo(bytesData, i * 2);
            }
            stream.Write(bytesData, 0, bytesData.Length);

            stream.Seek(0, SeekOrigin.Begin);
            WriteWavHeader(stream, clip.channels, clip.frequency, clip.samples * clip.channels);
            File.WriteAllBytes(savePath, stream.ToArray());
            Debug.Log("Audio Saved");
        }
    }

    private void WriteWavHeader(Stream stream, int channels, int sampleRate, int sampleCount)
    {
        int byteRate = sampleRate * channels * 2;
        int subChunk2Size = sampleCount * 2;
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

    // ------------------------------
    // Minimal WAV loader (16-bit PCM)
    private AudioClip LoadWav(string path)
    {
        try
        {
            byte[] bytes = File.ReadAllBytes(path);
            if (bytes.Length < 44) return null;

            int channels = System.BitConverter.ToInt16(bytes, 22);
            int freq = System.BitConverter.ToInt32(bytes, 24);
            int dataStart = 44;
            int sampleCount = (bytes.Length - dataStart) / 2;

            float[] data = new float[sampleCount];
            int offset = dataStart;
            for (int i = 0; i < sampleCount; i++)
            {
                short s = System.BitConverter.ToInt16(bytes, offset);
                data[i] = s / 32768f;
                offset += 2;
            }

            AudioClip clip = AudioClip.Create(Path.GetFileNameWithoutExtension(path),
                                              sampleCount / channels,
                                              channels,
                                              freq,
                                              false);
            clip.SetData(data, 0);
            return clip;
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to load WAV file at {path}: {ex.Message}");
        }

        return null;
    }
}