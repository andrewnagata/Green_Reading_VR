using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class RightHandController : MonoBehaviour
{
    public GameObject _character = null;

    public InputActionReference show_teleport;

    public VRTeleporter _teleport;

    public GameObject stimpmeter = null;

    private ActionBasedController _controller;

    private bool isPositioningRamp = false;
    private bool groundDetected = false;

    private Vector3 groundPos; // detected ground position
    private Vector3 lastNormal; // detected surface normal

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Hello start this");

        _controller = gameObject.GetComponent<ActionBasedController>();

        show_teleport.action.Enable();
        show_teleport.action.started += (ctx) =>
        {
            var control = show_teleport.action.activeControl;
            if (null == control)
                return;
            
            _teleport.ToggleDisplay(true);

            GetComponent<XRInteractorLineVisual>().enabled = false;
        };
        
        show_teleport.action.canceled += (ctx) =>
        {
            var control = show_teleport.action.activeControl;
            if (null == control)
                return;

            _teleport.ToggleDisplay(false);

            if(_character)
            {
                _character.gameObject.transform.position = _teleport.GetGroundPosition();
            }

            GetComponent<XRInteractorLineVisual>().enabled = true;
        };
    }

    public void OnRampGrabbed()
    {
        isPositioningRamp = true;
    }

    public void OnRampDropped()
    {
        isPositioningRamp = false;
        stimpmeter.transform.position = groundPos + lastNormal * 0.01f;
        stimpmeter.transform.LookAt(groundPos);
    }

    private void positionRamp()
    {
        groundDetected = false;

        RaycastHit hit;

        int layerMask = 1 << 3;

        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, layerMask))
        {
            groundDetected = true;
            groundPos = hit.point;
            lastNormal = hit.normal;
        }

        if (groundDetected)
        {
            stimpmeter.transform.position = groundPos + lastNormal * 0.01f;
            stimpmeter.transform.LookAt(groundPos);
        }
    }

    void FixedUpdate()
    {
        if(isPositioningRamp)
        {
            // move ramp to ground hit point
            positionRamp();
        }
    }
}
