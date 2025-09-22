using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class RightHandShooter : MonoBehaviour
{
    public float maxLaserDistance = 30f;
    public Color hitColor = Color.red;
    public Color noHitColor = Color.green;

    // The input action for the trigger button
    public InputActionProperty rightHandSelect;


    // The number of hits required to destroy an object
    public int hitsToSilence = 3;

    private LineRenderer lineRenderer;
    private XRController rightController;

    private GameObject currentHitObject;
    private int currentHitCount = 0;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }

        lineRenderer.enabled = false;
        rightController = GetComponentInParent<XRController>();
    }

    void Update()
    {
        bool triggerValue = rightHandSelect.action.ReadValue<float>() > 0.1f;

        if (triggerValue)
        {
            ShootLaser();
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }

    private void ShootLaser()
    {
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, transform.position);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, maxLaserDistance))
        {
            lineRenderer.SetPosition(1, hit.point);
            lineRenderer.startColor = hitColor;
            lineRenderer.endColor = hitColor;

            // Check if the raycast hit a new object
            if (hit.collider.gameObject != currentHitObject)
            {
                currentHitObject = hit.collider.gameObject;
                currentHitCount = 0;
            }

            // Check if the object is a "Punchable" thought and increment the counter
            if (hit.collider.CompareTag("Punchable"))
            {
                currentHitCount++;
                Debug.Log("Hit registered! Hits on " + currentHitObject.name + ": " + currentHitCount);

                if (currentHitCount >= hitsToSilence)
                {
                    // Get the Thought component
                    Thought thought = currentHitObject.GetComponent<Thought>();
                    if (thought != null)
                    {
                        thought.SilenceThoughtAndSpawnFlower(); // <- Safe flower spawn + destroy
                    }
                    else
                    {
                        Destroy(currentHitObject); // fallback if no Thought script attached
                    }

                    currentHitObject = null;
                    Debug.Log("Thought destroyed by laser!");
                }
            }
        }
        else
        {
            // If the laser is not hitting anything, reset the hit counter
            currentHitObject = null;
            currentHitCount = 0;

            lineRenderer.SetPosition(1, transform.position + transform.forward * maxLaserDistance);
            lineRenderer.startColor = noHitColor;
            lineRenderer.endColor = noHitColor;
        }
    }

    void OnEnable()
    {
        rightHandSelect.action.Enable();
    }

    void OnDisable()
    {
        rightHandSelect.action.Disable();
    }
}