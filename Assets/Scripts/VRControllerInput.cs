using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class VRControllerInput : MonoBehaviour
{
    public XRController leftController;      // assign your left-hand controller
    public VRRecorder vrRecorder;            // your VRRecorder script

    // Choose the button to press (primary button on left controller)
    public InputHelpers.Button triggerButton = InputHelpers.Button.Trigger;
    public float activationThreshold = 0.1f; // threshold for trigger press

    private bool isPressed = false;

    private void Update()
    {
        if (leftController == null || vrRecorder == null) return;

        bool triggerValue;
        leftController.inputDevice.IsPressed(triggerButton, out triggerValue, activationThreshold);

        // On trigger press
        if (triggerValue && !isPressed)
        {
            isPressed = true;
            vrRecorder.ToggleRecording();   // call the recording button
            Debug.Log("🎙 Left controller triggered recording!");
        }
        // On trigger release
        else if (!triggerValue && isPressed)
        {
            isPressed = false;
        }
    }
}
