using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class RampGrabController : XRBaseInteractable
{
    public Transform _parent;

    public InputActionReference rotateBase;
    public float objRotationSpeed = 20f;
    private Quaternion objRotation;
    private float startValue = 0;
    private float lastValue = 0;

    private void Start()
    {
        firstHoverEntered.AddListener(onHover);
        lastHoverExited.AddListener(onExit);
        selectEntered.AddListener(onGrab);
        selectExited.AddListener(onDrop);

        objRotation = _parent.rotation;
    }

    private void onHover(HoverEnterEventArgs args0)
    {
        rotateBase.action.Enable();
        rotateBase.action.performed += (ctx) =>
        {
            Vector2 reading = ctx.ReadValue<Vector2>();
            
            if(reading.x != lastValue)
            {
                float value = startValue - reading.x;
                objRotation = Quaternion.AngleAxis(value, Vector3.forward);
                _parent.rotation *= objRotation;

                lastValue = reading.x;
            }
        };

        rotateBase.action.started += (ctx) =>
        {
            Vector2 reading = ctx.ReadValue<Vector2>();

            startValue = reading.x;
        };
    }

    private void onExit(HoverExitEventArgs args0)
    {
        rotateBase.action.Disable();
    }

    private void onGrab(SelectEnterEventArgs args0)
    {
        args0.interactorObject.transform.gameObject.GetComponent<RightHandController>().OnRampGrabbed();
    }

    private void onDrop(SelectExitEventArgs args0)
    {
        args0.interactorObject.transform.gameObject.GetComponent<RightHandController>().OnRampDropped();
    }
}