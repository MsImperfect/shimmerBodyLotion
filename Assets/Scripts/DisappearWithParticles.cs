using UnityEngine;
using UnityEngine.InputSystem;

public class DisappearWithParticles : MonoBehaviour
{
    [Tooltip("Particle system prefab to spawn when this object disappears")]
    public GameObject particlePrefab;

    [Tooltip("Optional: delay before destroying the original object")]
    public float destroyDelay = 0f;

    private bool hasTriggered = false;

    private void Update()
    {
        // Mouse click in Editor (new Input System)
        if (!hasTriggered &&
            Mouse.current != null &&
            Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    TriggerEffect();
                }
            }
        }
    }

    /// <summary>
    /// Makes the object disappear and spawns particles.
    /// Safe against multiple triggers.
    /// </summary>
    public void TriggerEffect()
    {
        if (hasTriggered) return;
        hasTriggered = true;

        if (particlePrefab != null)
        {
            // Spawn at same world position / rotation
            Instantiate(particlePrefab, transform.position, transform.rotation);
        }
        else
        {
            Debug.LogWarning("[DisappearWithParticles] No particle prefab assigned!");
        }

        // Hide or destroy the original object
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (var r in renderers) r.enabled = false;

        Collider col = GetComponent<Collider>();
        if (col) col.enabled = false;

        Destroy(gameObject, destroyDelay);
    }
}
