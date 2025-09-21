//using UnityEngine;
//using System.IO;

///// <summary>
///// Attach this script to a GameObject with an AudioSource.
///// Give it the name of a WAV file (without path),
///// it will load and play the file from Application.persistentDataPath.
///// </summary>
//[RequireComponent(typeof(AudioSource))]
//public class WavPlayer : MonoBehaviour
//{
//    [Header("WAV Settings")]
//    [Tooltip("C:/Users/Mahesh/AppData/LocalLow/DefaultCompany/Vr_testing\\VRRecordingso\\MyVRClip_20250921_145753.wav")]
//    public string wavFileName;

//    private AudioSource audioSource;

//    void Awake()
//    {
//        audioSource = GetComponent<AudioSource>();
//    }

//    public void PlayWav()
//    {
//        string fullPath = Path.Combine(Application.persistentDataPath, "VRRecordings", wavFileName);
//        //string fullPath = "C:/ Users / Mahesh / AppData / LocalLow / DefaultCompany / Vr_testing\VRRecordingso\MyVRClip_20250921_145753.wav";

//        if (!File.Exists(fullPath))
//        {
//            Debug.LogError("WAV file not found: " + fullPath);
//            return;
//        }

//        AudioClip clip = LoadWav(fullPath);
//        if (clip != null)
//        {
//            audioSource.clip = clip;
//            audioSource.Play();
//            Debug.Log("? Playing WAV: " + fullPath);
//        }
//    }

//    // -------- WAV loader --------
//    private AudioClip LoadWav(string path)
//    {
//        byte[] fileBytes = File.ReadAllBytes(path);
//        if (fileBytes.Length < 44)
//        {
//            Debug.LogError("Invalid WAV file (too small).");
//            return null;
//        }

//        // parse header
//        int channels = System.BitConverter.ToInt16(fileBytes, 22);
//        int sampleRate = System.BitConverter.ToInt32(fileBytes, 24);
//        int dataStartIndex = 44; // standard header size
//        int samples = (fileBytes.Length - dataStartIndex) / 2; // 16-bit PCM

//        float[] data = new float[samples];
//        int offset = dataStartIndex;
//        for (int i = 0; i < samples; i++)
//        {
//            short sample = System.BitConverter.ToInt16(fileBytes, offset);
//            data[i] = sample / 32768f;
//            offset += 2;
//        }

//        AudioClip audioClip = AudioClip.Create(Path.GetFileNameWithoutExtension(path),
//                                               samples / channels,
//                                               channels,
//                                               sampleRate,
//                                               false);
//        audioClip.SetData(data, 0);
//        return audioClip;
//    }
//}
