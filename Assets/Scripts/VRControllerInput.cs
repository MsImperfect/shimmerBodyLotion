//using UnityEngine;
//using UnityEngine.InputSystem;

//public class VRLeftTriggerRecorder : MonoBehaviour
//{
//    [Header("Assign VR Input Actions")]
//    public VRInputActions inputActions;    // the auto-generated class
//    public VRRecorder vrRecorder;          // your recording script

//    private void OnEnable()
//    {
//        if (inputActions == null) inputActions = new VRInputActions();

//        inputActions.LeftHand.TriggerPress.performed += OnTriggerPressed;
//        inputActions.LeftHand.Enable();
//    }

//    private void OnDisable()
//    {
//        inputActions.LeftHand.TriggerPress.performed -= OnTriggerPressed;
//        inputActions.LeftHand.Disable();
//    }

//    private void OnTriggerPressed(InputAction.CallbackContext ctx)
//    {
//        vrRecorder.ToggleRecording();
//        Debug.Log("🎙 Left-hand trigger pressed, recording toggled!");
//    }
//}
