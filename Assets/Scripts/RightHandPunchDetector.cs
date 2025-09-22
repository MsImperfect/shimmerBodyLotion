using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class RightHandPunchDetector : MonoBehaviour
{
    public float punchVelocityThreshold = 0.1f;
    public float hapticIntensity = 0.7f;
    public float cooldownTime = 0.5f;
    public Transform controller;
    public InputActionProperty rightHandSelect;
    public XRRayInteractor rightHandInteractor;
    public Vector3 pos;
    public Vector3 velocity;

    private bool isOnCooldown = false;

    public static System.Action OnRightHandPunch;

    private void LateUpdate()
    {
        velocity = (controller.localPosition-pos)/Time.deltaTime;
        Debug.Log(velocity.magnitude);
        pos = controller.localPosition; 
    }
    void OnEnable()
    {
        rightHandSelect.action.Enable();
    }

    void OnDisable()
    {
        rightHandSelect.action.Disable();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Punchable"));
        {
            Debug.Log("Punch detected");
            if (rightHandSelect.action.ReadValue<float>() > 0.1f)
            {
                if (velocity.magnitude > punchVelocityThreshold)
                {
                    Debug.Log("Punch detected!");

                    if (rightHandInteractor != null)
                    {
                        rightHandInteractor.SendHapticImpulse(hapticIntensity, 0.2f);
                    }

                    OnRightHandPunch?.Invoke();

                    isOnCooldown = true;
                    Invoke("ResetCooldown", cooldownTime);
                }
            }
        }
    }

    private void ResetCooldown()
    {
        isOnCooldown = false;
    }
}