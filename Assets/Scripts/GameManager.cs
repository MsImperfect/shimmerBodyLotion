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
//        audioSource.spatialBlend = 1f;
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

// kinnda okay 
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
//was working until parneeca came 
//using UnityEngine;
//using System.IO;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine.Networking;

//public class ClipAssigner : MonoBehaviour
//{
//    [Header("Recording Folder Settings")]
//    public string recordingsFolderName = "VRRecordings";

//    [Header("Playback Settings")]
//    public float delayBetweenClips = 0.5f; // seconds
//    [Range(0f, 1f)]
//    public float defaultVolume = 1f;       // default volume if individual volumes not set

//    [Header("Optional: Individual Volumes for Each Child")]
//    public List<float> childVolumes;       // size = number of children, 0-1

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

//            // assign clip
//            source.clip = loadedClips[clipIndex % loadedClips.Count];
//            source.spatialBlend = 1f; // 3D audio
//            source.loop = true;

//            // 🔊 Force maximum volume
//            source.volume = 1f;

//            // delay before playing
//            yield return new WaitForSeconds(delayBetweenClips);
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


// latest
//using UnityEngine;
//using System.IO;
//using System.Collections.Generic;
//using UnityEngine.Networking;
//using System.Collections;
//using System.Linq;

//public class GameManager : MonoBehaviour
//{
//    public GameObject objectToSpawn;
//    public int numberOfObjects = 5;
//    public float spawnRadius = 15f;

//    public float spawnHeightY = 1.5f;
//    public float soundVolume = 1.0f;
//    public float minSoundDistance = 0f;
//    public float maxSoundDistance = 10000f;

//    private List<AudioClip> loadedClips = new List<AudioClip>();

//    private void Start()
//    {
//        StartCoroutine(LoadAllRecordingsAndSpawn());
//    }

//    private IEnumerator LoadAllRecordingsAndSpawn()
//    {
//        string folderPath = Path.Combine(Application.persistentDataPath, "VRRecordings");

//        if (!Directory.Exists(folderPath))
//        {
//            Debug.LogError("Recordings folder not found at: " + folderPath);
//            yield break;
//        }

//        string[] files = Directory.GetFiles(folderPath, "*.wav").OrderBy(f => f).ToArray();
//        if (files.Length == 0)
//        {
//            Debug.LogError("No .wav files found in the recordings folder!");
//            yield break;
//        }

//        foreach (string path in files)
//        {
//            using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("file://" + path, AudioType.WAV))
//            {
//                yield return www.SendWebRequest();
//                if (www.result == UnityWebRequest.Result.Success)
//                {
//                    AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
//                    if (clip != null)
//                    {
//                        clip.name = Path.GetFileNameWithoutExtension(path); // ✅ Assign readable name
//                        loadedClips.Add(clip);
//                    }
//                }
//                else
//                {
//                    Debug.LogError("Failed to load audio from path: " + path + ". Error: " + www.error);
//                }
//            }
//        }

//        Debug.Log($"Loaded {loadedClips.Count} clips from disk.");

//        if (loadedClips.Count > 0)
//        {
//            SpawnObjects();
//        }
//    }

//    private void SpawnObjects()
//    {
//        if (objectToSpawn == null)
//        {
//            Debug.LogError("Object prefab is not assigned!");
//            return;
//        }

//        Vector3 camPos = Camera.main.transform.position;
//        if (Camera.main != null)
//        {
//            camPos = Camera.main.transform.position;
//        }
//        else
//        {
//            Debug.LogWarning("No MainCamera found. Using Vector3.zero for spawn position.");
//        }

//        for (int i = 0; i < numberOfObjects && i < loadedClips.Count; i++)
//        {
//            Vector3 randomPosition = camPos + new Vector3(Random.Range(-2f, 2f), spawnHeightY, Random.Range(-2f, 2f));
//            GameObject spawnedObject = Instantiate(objectToSpawn, randomPosition, Quaternion.identity);

//            AudioSource audioSource = spawnedObject.AddComponent<AudioSource>();
//            //audioSource.clip = loadedClips[i];  // pick sequential clip
//            //audioSource.spatialBlend = 0f;      // force 2D for testing
//            //audioSource.loop = true;
//            //audioSource.volume = soundVolume;
//            audioSource.spatialBlend = 1f;                  // 3D sound
//            audioSource.rolloffMode = AudioRolloffMode.Linear;
//            audioSource.minDistance = 2f;                   // Louder close up
//            audioSource.maxDistance = 20f;                  // Fully silent after 20 units


//            Debug.Log($"Clip: {audioSource.clip.name}, Pos: {spawnedObject.transform.position}, " +$"CamPos: {Camera.main.transform.position}, " +$"Distance{Vector3.Distance(spawnedObject.transform.position, Camera.main.transform.position)}");

//            audioSource.Play();
//        }
//    }
//}
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.Collections;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public GameObject objectToSpawn;
    public int numberOfObjects = 5;
    public float spawnRadius = 15f;

    public float spawnHeightY = 1.5f;
    public float soundVolume = 1.0f;
    public float minSoundDistance = 0f;
    public float maxSoundDistance = 10000f;

    private List<AudioClip> loadedClips = new List<AudioClip>();

    private void Start()
    {
        StartCoroutine(LoadAllRecordingsAndSpawn());
    }

    private IEnumerator LoadAllRecordingsAndSpawn()
    {
        string folderPath = Path.Combine(Application.persistentDataPath, "VRRecordings");

        if (!Directory.Exists(folderPath))
        {
            Debug.LogError("Recordings folder not found at: " + folderPath);
            yield break;
        }

        string[] files = Directory.GetFiles(folderPath, "*.wav").OrderBy(f => f).ToArray();
        if (files.Length == 0)
        {
            Debug.LogError("No .wav files found in the recordings folder!");
            yield break;
        }

        foreach (string path in files)
        {
            using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("file://" + path, AudioType.WAV))
            {
                yield return www.SendWebRequest();
                if (www.result == UnityWebRequest.Result.Success)
                {
                    AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
                    if (clip != null) loadedClips.Add(clip);
                }
                else
                {
                    Debug.LogError("Failed to load audio from path: " + path + ". Error: " + www.error);
                }
            }
        }

        Debug.Log($"Loaded {loadedClips.Count} clips from disk.");

        if (loadedClips.Count > 0)
        {
            SpawnObjects();
        }
    }

    private void SpawnObjects()
    {
        if (objectToSpawn == null)
        {
            Debug.LogError("Object prefab is not assigned!");
            return;
        }

        for (int i = 0; i < numberOfObjects && i < loadedClips.Count; i++)
        {
            Vector3 randomPosition = Random.insideUnitSphere * spawnRadius;
            GameObject spawnedObject = Instantiate(objectToSpawn, randomPosition, Quaternion.identity);

            AudioSource audioSource = spawnedObject.AddComponent<AudioSource>();
            audioSource.clip = loadedClips[Random.Range(0, loadedClips.Count)];
            audioSource.spatialBlend = 1f;
            audioSource.loop = true;

            audioSource.volume = soundVolume;
            audioSource.minDistance = minSoundDistance;
            audioSource.maxDistance = maxSoundDistance;
            audioSource.rolloffMode = AudioRolloffMode.Linear;

            audioSource.Play();
            Debug.Log($"Clip: {audioSource.clip.name}, Pos: {spawnedObject.transform.position}, " + $"CamPos: {Camera.main.transform.position}, " + $"Distance{Vector3.Distance(spawnedObject.transform.position, Camera.main.transform.position)}");
        }
    }
}