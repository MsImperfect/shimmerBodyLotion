using UnityEngine;
using System.Collections.Generic;

public class Thought : MonoBehaviour
{
    public GameObject flowerPrefab;
    private Vector3 originalPos;
    private AudioSource audioSource;

    public GameManager manager;

    void Start()
    {
        originalPos = transform.position;
        originalPos.y = 0;
        audioSource = GetComponent<AudioSource>();
    }

    //private void OnDestroy()
    //{
    //    if (flowerPrefab != null)
    //    {
    //        Instantiate(flowerPrefab, originalPos, Quaternion.identity);
    //        Debug.Log("Flower spawned at " + originalPos);
    //    }
    //    else
    //    {
    //        Debug.LogError("Flower Prefab is not assigned in the Inspector!");
    //    }

    //    if (audioSource != null)
    //    {
    //        audioSource.Stop();
    //    }
    //}

    public void SilenceThoughtAndSpawnFlower()
    {
        if (flowerPrefab != null)
        {
            Instantiate(flowerPrefab, originalPos, Quaternion.identity);
            Debug.Log("Flower spawned at " + originalPos);
        }

        if (audioSource != null)
            audioSource.Stop();

        manager.destroyedObjects++;
        Destroy(gameObject); // Destroy after spawning
    }

}