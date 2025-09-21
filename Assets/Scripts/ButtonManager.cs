using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void loader()
    {
        Debug.Log("play button");
        SceneManager.LoadScene("BasicScene");
    }
}
