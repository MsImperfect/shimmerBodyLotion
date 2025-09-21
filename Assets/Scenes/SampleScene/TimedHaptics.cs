using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class TimedHaptics : MonoBehaviour
{
    public float interval = 5f;   // Seconds between vibrations
    public float amplitude = 0.5f; // 0.0 to 1.0 strength
    public float duration = 0.2f; // Seconds vibration lasts

    private float timer = 0f;
    private InputDevice device;

    void Start()
    {
        // Example: get right hand controller
        var inputDevices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.RightHand, inputDevices);
        if (inputDevices.Count > 0)
            device = inputDevices[0];
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= interval)
        {
            timer = 0f;
            if (device.isValid)
            {
                // Send a haptic impulse
                device.SendHapticImpulse(0u, amplitude, duration);
            }
        }
    }
}
