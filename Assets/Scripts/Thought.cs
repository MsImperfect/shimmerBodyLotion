using UnityEngine;

public class Thought : MonoBehaviour
{
    public GameObject flowerPrefab;
    private Vector3 originalPos;
    private bool hasSpawned = false;

    private void Start()
    {
        originalPos = transform.position;
        originalPos.y = 0;
    }

    void Update()
    {
        if (transform.position.y < -50 && !hasSpawned)
        {
            if (flowerPrefab != null)
            {
                GameObject flower = Instantiate(flowerPrefab, originalPos, Quaternion.identity);
                hasSpawned = true;
            }
            else
            {
                Debug.LogError("Flower Prefab is not assigned in the Inspector!");
            }
        }
    }
}