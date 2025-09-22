//using UnityEngine;
//using UnityEngine.XR;
//using System.Collections.Generic;

//public class PunchableObject : MonoBehaviour
//{
//    public int hitsToSilence = 3;
//    public float hapticIntensity = 0.7f;
//    public float cooldownTime = 0.5f;

//    private int currentHits = 0;
//    private bool isOnCooldown = false;

//    private AudioSource audioSource;

//    public static System.Action OnThoughtSilenced;

//    void Start()
//    {
//        audioSource = GetComponent<AudioSource>();
//    }

//    private void OnTriggerEnter(Collider other)
//    {
//        if (other.CompareTag("Hand") && !isOnCooldown)
//        {
//            InputDevice device = new InputDevice();

//            // Try to get a device reference from the hand that hit the object
//            if (other.transform.parent.name.Contains("Left"))
//            {
//                InputDevices.GetDeviceAtXRNode(XRNode.LeftHand, out device);
//            }
//            else if (other.transform.parent.name.Contains("Right"))
//            {
//                InputDevices.GetDeviceAtXRNode(XRNode.RightHand, out device);
//            }

//            if (device.isValid)
//            {
//                currentHits++;
//                Debug.Log("Hit registered! Hits on " + gameObject.name + ": " + currentHits);

//                device.SendHapticImpulse(0, hapticIntensity, 0.2f);

//                isOnCooldown = true;
//                Invoke("ResetCooldown", cooldownTime);

//                if (currentHits >= hitsToSilence)
//                {
//                    if (audioSource != null)
//                    {
//                        audioSource.Stop();
//                    }
//                    Destroy(gameObject);

//                    OnThoughtSilenced?.Invoke();
//                }
//            }
//            else
//            {
//                // This is the message you'll see if the controller isn't ready
//                Debug.LogWarning("Hand device not valid for collision.");
//            }
//        }
//    }

//    private void ResetCooldown()
//    {
//        isOnCooldown = false;
//    }
//}