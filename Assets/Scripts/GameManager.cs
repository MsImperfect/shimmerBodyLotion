//using UnityEngine;
//using System.Collections.Generic;

//public class GameManager : MonoBehaviour
//{
//    public GameObject objectToSpawn;
//    public int numberOfObjects = 5;
//    public float spawnRadius = 15f;
//    public VRRecorder VRRecorder;

//    //public List<AudioClip> audioClips;
//    public AudioClip positiveThoughtClip;

//    private void Start()
//    {
//        SpawnObjects();
//        SpawnPositiveThought();
//    }

//    private void SpawnObjects()
//    {
//        if (objectToSpawn == null)
//        {
//            Debug.LogError("Object prefab is not assigned in the Inspector!");
//            return;
//        }

//        if (VRRecorder.Instance.audioClips == null || VRRecorder.Instance.audioClips.Count == 0)
//        {
//            Debug.LogError("AudioClips list is empty!");
//            return;
//        }

//        for (int i = 0; i < numberOfObjects; i++)
//        {
//            Vector3 randomPosition = Random.insideUnitSphere * spawnRadius;
//            GameObject spawnedObject = Instantiate(objectToSpawn, randomPosition, Quaternion.identity);

//            AudioSource audioSource = spawnedObject.AddComponent<AudioSource>();
//            audioSource.clip = VRRecorder.Instance.audioClips[Random.Range(0, VRRecorder.Instance.audioClips.Count)];
//            audioSource.spatialBlend = 1f;
//            audioSource.loop = true;
//            audioSource.Play();
//        }
//    }

//    public void SpawnPositiveThought()
//    {
//        Vector3 randomPos = Random.insideUnitSphere * spawnRadius;
//        GameObject positiveThought = Instantiate(objectToSpawn, randomPos, Quaternion.identity);
//        AudioSource audioSource = positiveThought.AddComponent<AudioSource>();
//        audioSource.clip = positiveThoughtClip;
//        audioSource.spatialBlend= 1f;
//        audioSource.loop = true;
//        audioSource.Play();
//    }
//}
//using UnityEngine;
//using System.Collections.Generic;

//public class GameManager : MonoBehaviour
//{
//    [Header("Prefabs & References")]
//    public GameObject objectToSpawn;
//    public VRRecorder recorder;            // <-- drag your VRRecorder GameObject here

//    [Header("Spawn Settings")]
//    public int numberOfObjects = 5;
//    public float spawnRadius = 15f;

//    [Header("Audio")]
//    public AudioClip positiveThoughtClip;

//    private void Start()
//    {
//        SpawnObjects();
//        SpawnPositiveThought();
//    }

//    private void SpawnObjects()
//    {
//        if (objectToSpawn == null)
//        {
//            Debug.LogError("❌ Object prefab is not assigned in the Inspector!");
//            return;
//        }

//        if (recorder == null)
//        {
//            Debug.LogError("❌ VRRecorder reference missing in GameManager!");
//            return;
//        }

//        if (recorder.audioClips == null || recorder.audioClips.Count == 0)
//        {
//            Debug.LogWarning("⚠ No audio clips loaded in VRRecorder. Spawning objects without audio.");
//        }

//        for (int i = 0; i < numberOfObjects; i++)
//        {
//            Vector3 randomPosition = Random.insideUnitSphere * spawnRadius;
//            GameObject spawnedObject = Instantiate(objectToSpawn, randomPosition, Quaternion.identity);

//            if (recorder.audioClips != null && recorder.audioClips.Count > 0)
//            {
//                AudioSource audioSource = spawnedObject.AddComponent<AudioSource>();
//                audioSource.clip = recorder.audioClips[Random.Range(0, recorder.audioClips.Count)];
//                audioSource.spatialBlend = 1f; // 3D audio
//                audioSource.loop = true;
//                audioSource.Play();
//            }
//        }
//    }

//    public void SpawnPositiveThought()
//    {
//        if (objectToSpawn == null)
//        {
//            Debug.LogError("❌ Object prefab is not assigned!");
//            return;
//        }

//        Vector3 randomPos = Random.insideUnitSphere * spawnRadius;
//        GameObject positiveThought = Instantiate(objectToSpawn, randomPos, Quaternion.identity);

//        if (positiveThoughtClip != null)
//        {
//            AudioSource audioSource = positiveThought.AddComponent<AudioSource>();
//            audioSource.clip = positiveThoughtClip;
//            audioSource.spatialBlend = 1f;
//            audioSource.loop = true;
//            audioSource.Play();
//        }
//        else
//        {
//            Debug.LogWarning("⚠ PositiveThoughtClip is not set.");
//        }
//    }
//}

//using UnityEngine;
//using System.IO;
//using System.Collections.Generic;

//public class GameManager : MonoBehaviour
//{
//    [Header("Recording Folder Settings")]
//    public string recordingsFolderName = "VRRecordings";

//    private List<AudioClip> loadedClips = new List<AudioClip>();

//    private void Start()
//    {
//        LoadAllRecordings();
//        AssignClipsToChildren();
//    }

//    private void LoadAllRecordings()
//    {
//        loadedClips.Clear();
//        string folderPath = Path.Combine(Application.persistentDataPath, recordingsFolderName);

//        if (!Directory.Exists(folderPath))
//        {
//            Debug.LogWarning("Recordings folder not found: " + folderPath);
//            return;
//        }

//        string[] files = Directory.GetFiles(folderPath, "*.wav");
//        foreach (string path in files)
//        {
//            AudioClip clip = LoadWav(path);
//            if (clip != null) loadedClips.Add(clip);
//        }

//        Debug.Log($"📂 Loaded {loadedClips.Count} clips from disk.");
//    }

//    private void AssignClipsToChildren()
//    {
//        if (loadedClips.Count == 0)
//        {
//            Debug.LogWarning("No clips to assign!");
//            return;
//        }

//        Transform[] children = GetComponentsInChildren<Transform>();
//        int clipIndex = 0;

//        foreach (Transform child in children)
//        {
//            if (child == transform) continue;

//            AudioSource source = child.GetComponent<AudioSource>();
//            if (source == null) source = child.gameObject.AddComponent<AudioSource>();

//            source.clip = loadedClips[clipIndex % loadedClips.Count];
//            source.spatialBlend = 1f;
//            source.loop = true;
//            source.Play();

//            clipIndex++;
//        }
//    }

//    // Minimal WAV loader
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
//using UnityEngine;
//using System.IO;
//using System.Collections;
//using System.Collections.Generic;

//public class ClipAssigner : MonoBehaviour
//{
//    [Header("Recording Folder Settings")]
//    public string recordingsFolderName = "VRRecordings";

//    [Header("Playback Settings")]
//    public float delayBetweenClips = 0.5f; // seconds

//    private List<AudioClip> loadedClips = new List<AudioClip>();

//    private void Start()
//    {
//        LoadAllRecordings();
//        StartCoroutine(AssignClipsWithDelay());
//    }

//    private void LoadAllRecordings()
//    {
//        loadedClips.Clear();
//        string folderPath = Path.Combine(Application.persistentDataPath, recordingsFolderName);

//        if (!Directory.Exists(folderPath))
//        {
//            Debug.LogWarning("Recordings folder not found: " + folderPath);
//            return;
//        }

//        string[] files = Directory.GetFiles(folderPath, "*.wav");
//        foreach (string path in files)
//        {
//            AudioClip clip = LoadWav(path);
//            if (clip != null) loadedClips.Add(clip);
//        }

//        Debug.Log($"📂 Loaded {loadedClips.Count} clips from disk.");
//    }

//    private IEnumerator AssignClipsWithDelay()
//    {
//        if (loadedClips.Count == 0)
//        {
//            Debug.LogWarning("No clips to assign!");
//            yield break;
//        }

//        Transform[] children = GetComponentsInChildren<Transform>();
//        int clipIndex = 0;

//        foreach (Transform child in children)
//        {
//            if (child == transform) continue;

//            AudioSource source = child.GetComponent<AudioSource>();
//            if (source == null) source = child.gameObject.AddComponent<AudioSource>();

//            source.clip = loadedClips[clipIndex % loadedClips.Count];
//            source.spatialBlend = 1f;
//            source.loop = true;

//            yield return new WaitForSeconds(delayBetweenClips); // <-- delay before playing
//            source.Play();

//            clipIndex++;
//        }
//    }

//    // -----------------------------
//    // Minimal WAV loader
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

public class ClipAssigner : MonoBehaviour
{
    [Header("Recording Folder Settings")]
    public string recordingsFolderName = "VRRecordings";

    [Header("Playback Settings")]
    public float delayBetweenClips = 0.5f; // seconds
    [Range(0f, 1f)]
    public float defaultVolume = 1f;       // default volume if individual volumes not set

    [Header("Optional: Individual Volumes for Each Child")]
    public List<float> childVolumes;       // size = number of children, 0-1

    private List<AudioClip> loadedClips = new List<AudioClip>();

    private void Start()
    {
        LoadAllRecordings();
        StartCoroutine(AssignClipsWithDelay());
    }

    private void LoadAllRecordings()
    {
        loadedClips.Clear();
        string folderPath = Path.Combine(Application.persistentDataPath, recordingsFolderName);

        if (!Directory.Exists(folderPath))
        {
            Debug.LogWarning("Recordings folder not found: " + folderPath);
            return;
        }

        string[] files = Directory.GetFiles(folderPath, "*.wav");
        foreach (string path in files)
        {
            AudioClip clip = LoadWav(path);
            if (clip != null) loadedClips.Add(clip);
        }

        Debug.Log($"📂 Loaded {loadedClips.Count} clips from disk.");
    }

    private IEnumerator AssignClipsWithDelay()
    {
        if (loadedClips.Count == 0)
        {
            Debug.LogWarning("No clips to assign!");
            yield break;
        }

        Transform[] children = GetComponentsInChildren<Transform>();
        int clipIndex = 0;
        int childIndex = 0;

        foreach (Transform child in children)
        {
            if (child == transform) continue;

            AudioSource source = child.GetComponent<AudioSource>();
            if (source == null) source = child.gameObject.AddComponent<AudioSource>();

            // assign clip
            source.clip = loadedClips[clipIndex % loadedClips.Count];
            source.spatialBlend = 1f; // 3D audio
            source.loop = true;

            // assign volume
            if (childVolumes != null && childIndex < childVolumes.Count)
                source.volume = Mathf.Clamp01(childVolumes[childIndex]);
            else
                source.volume = defaultVolume;

            // delay before playing
            yield return new WaitForSeconds(delayBetweenClips);
            source.Play();

            clipIndex++;
            childIndex++;
        }
    }

    // -----------------------------
    // Minimal WAV loader
    private AudioClip LoadWav(string path)
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
}
