using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public GameObject objectToSpawn;
    public int numberOfObjects = 5;
    public float spawnRadius = 15f;

    public List<AudioClip> audioClips;
    public AudioClip positiveThoughtClip;

    private void Start()
    {
        SpawnObjects();
        SpawnPositiveThought();
    }

    private void SpawnObjects()
    {
        if (objectToSpawn == null)
        {
            Debug.LogError("Object prefab is not assigned in the Inspector!");
            return;
        }

        if (audioClips == null || audioClips.Count == 0)
        {
            Debug.LogError("AudioClips list is empty!");
            return;
        }

        for (int i = 0; i < numberOfObjects; i++)
        {
            Vector3 randomPosition = Random.insideUnitSphere * spawnRadius;
            GameObject spawnedObject = Instantiate(objectToSpawn, randomPosition, Quaternion.identity);

            AudioSource audioSource = spawnedObject.AddComponent<AudioSource>();
            audioSource.clip = audioClips[Random.Range(0, audioClips.Count)];
            audioSource.spatialBlend = 1f;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    public void SpawnPositiveThought()
    {
        Vector3 randomPos = Random.insideUnitSphere * spawnRadius;
        GameObject positiveThought = Instantiate(objectToSpawn, randomPos, Quaternion.identity);
        AudioSource audioSource = positiveThought.AddComponent<AudioSource>();
        audioSource.clip = positiveThoughtClip;
        audioSource.spatialBlend= 1f;
        audioSource.loop = true;
        audioSource.Play();
    }
}