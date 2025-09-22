using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneCleaner : MonoBehaviour
{
    void OnEnable()
    {
        SceneManager.sceneUnloaded += CleanFlowers;
    }

    void OnDisable()
    {
        SceneManager.sceneUnloaded -= CleanFlowers;
    }

    void CleanFlowers(Scene scene)
    {
        var flowers = GameObject.FindGameObjectsWithTag("Flower");
        foreach (var flower in flowers)
        {
            Destroy(flower);
        }
    }
}
