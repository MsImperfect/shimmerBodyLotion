using UnityEngine;
using System.IO;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.Collections;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject objectToSpawn;
    public int numberOfObjects = 5;
    public float spawnRadius = 15f;
    public int destroyedObjects = 0;

    public float spawnHeightY = 1f;
    //public float soundVolume = 1.0f;
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

    public void Update()
    {
        if(destroyedObjects == numberOfObjects)
        {
            SceneManager.LoadScene("Final");
        }
    }

    private void SpawnObjects()
    {
        if (objectToSpawn == null)
        {
            Debug.LogError("Object prefab is not assigned!");
            return;
        }

        for (int i = 0; i < numberOfObjects; i++)
        {
            float randomX = Random.Range(-45f, 45f);
            float randomZ = Random.Range(-45f, 45f);
            Vector3 randomPosition = new Vector3(randomX, spawnHeightY, randomZ);
            GameObject spawnedObject = Instantiate(objectToSpawn, randomPosition, Quaternion.identity);

            AudioSource audioSource = spawnedObject.AddComponent<AudioSource>();
            audioSource.clip = loadedClips[Random.Range(0, loadedClips.Count)];
            float vol = Random.Range(0.5f, 1f);
            audioSource.spatialBlend = 1f;
            audioSource.loop = true;

            audioSource.volume = vol;
            audioSource.minDistance = minSoundDistance;
            audioSource.maxDistance = maxSoundDistance;
            audioSource.rolloffMode = AudioRolloffMode.Linear;

            audioSource.Play();
            Debug.Log($"Clip: {audioSource.clip.name}, Pos: {spawnedObject.transform.position}, " + $"CamPos: {Camera.main.transform.position}, " + $"Distance{Vector3.Distance(spawnedObject.transform.position, Camera.main.transform.position)}");
        }
    }
}