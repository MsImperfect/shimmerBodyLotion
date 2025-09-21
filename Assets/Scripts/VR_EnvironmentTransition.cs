using UnityEngine;
using System.Collections;

public class VR_EnvironmentTransition : MonoBehaviour
{
    [Header("Skybox Settings")]
    public Material startSkybox;
    public Material endSkybox;

    [Header("Lighting Settings")]
    public Light directionalLight;              // Sun / main light
    public Color startAmbient = Color.gray;
    public Color endAmbient = Color.white;
    public Color startFogColor = Color.gray;
    public Color endFogColor = Color.blue;
    public float startLightIntensity = 1f;
    public float endLightIntensity = 1f;

    [Header("Transition Settings")]
    public float transitionDuration = 5f;

    private void Start()
    {
        // Initialize start settings
        if (startSkybox != null)
            RenderSettings.skybox = startSkybox;

        RenderSettings.ambientLight = startAmbient;
        RenderSettings.fogColor = startFogColor;

        if (directionalLight != null)
            directionalLight.intensity = startLightIntensity;

        // Start the transition
        StartCoroutine(TransitionEnvironment());
    }

    private IEnumerator TransitionEnvironment()
    {
        if (startSkybox == null || endSkybox == null)
        {
            Debug.LogWarning("Skyboxes not assigned!");
            yield break;
        }

        float t = 0f;

        // Create a temporary material to blend skyboxes
        Material blendedSkybox = new Material(startSkybox);

        while (t < transitionDuration)
        {
            t += Time.deltaTime;
            float lerp = t / transitionDuration;

            // -----------------------
            // Skybox blending
            if (startSkybox.HasProperty("_Tint") && endSkybox.HasProperty("_Tint"))
            {
                Color startColor = startSkybox.GetColor("_Tint");
                Color endColor = endSkybox.GetColor("_Tint");
                blendedSkybox.SetColor("_Tint", Color.Lerp(startColor, endColor, lerp));
            }

            if (startSkybox.HasProperty("_Exposure") && endSkybox.HasProperty("_Exposure"))
            {
                float startExp = startSkybox.GetFloat("_Exposure");
                float endExp = endSkybox.GetFloat("_Exposure");
                blendedSkybox.SetFloat("_Exposure", Mathf.Lerp(startExp, endExp, lerp));
            }

            RenderSettings.skybox = blendedSkybox;

            // -----------------------
            // Ambient light
            RenderSettings.ambientLight = Color.Lerp(startAmbient, endAmbient, lerp);

            // Fog
            RenderSettings.fogColor = Color.Lerp(startFogColor, endFogColor, lerp);

            // Directional light intensity
            if (directionalLight != null)
                directionalLight.intensity = Mathf.Lerp(startLightIntensity, endLightIntensity, lerp);

            yield return null;
        }

        // Final assignment
        RenderSettings.skybox = endSkybox;
        RenderSettings.ambientLight = endAmbient;
        RenderSettings.fogColor = endFogColor;

        if (directionalLight != null)
            directionalLight.intensity = endLightIntensity;

        Debug.Log("🌌 Environment transition complete!");
    }
}
