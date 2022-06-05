using UnityEngine;
using UnityEngine.InputSystem;

public class BallSpawner : MonoBehaviour
{
    public GameObject prefab;
    public Transform pointer;
    public GameObject ramp;
    public GameObject powerIndicator;

    public Transform startPosition;
    public InputActionReference onTrigger;
    public InputActionReference onPowerUp;
    public InputActionReference onPowerDown;

    private float click_degrees = 5f;
    private Quaternion current_rotation;

    private float MAX_POWER = 140f;
    private float MIN_POWER = 50f;
    private float current_power = 70f;

    private Vector3 position_on_ramp;
    private Vector3 ramp_normal;

    void Start()
    {
        current_rotation = pointer.rotation;

        onTrigger.action.Enable();
        onTrigger.action.performed += (ctx) =>
        {
            OnTrigger();
        };

        onPowerUp.action.Enable();
        onPowerUp.action.started += (ctx) =>
        {
            pointer.RotateAround(pointer.position, transform.up, -click_degrees);

            if(current_power < MAX_POWER)
            {
                current_power += click_degrees;
            }

            adjustPower();
        };

        onPowerDown.action.Enable();
        onPowerDown.action.started += (ctx) =>
        {
            pointer.RotateAround(pointer.position, transform.up, click_degrees);

            if (current_power > MIN_POWER)
            {
                current_power -= click_degrees;
            }

            adjustPower();
        };

        adjustPower();
    }

    private void adjustPower()
    {
        RaycastHit hit;

        int layerMask = 1 << 5;

        if (Physics.Raycast(pointer.position, pointer.forward, out hit, Mathf.Infinity, layerMask))
        {
            position_on_ramp = hit.point;
            ramp_normal = hit.normal;
            powerIndicator.transform.position = position_on_ramp + ramp_normal * 0.02f;
        }
    }

    private void OnTrigger()
    {
        GameObject ball = Instantiate(prefab, startPosition.position, Quaternion.identity) as GameObject;

        Vector3 direction = transform.right.normalized;

        ball.GetComponent<BallDeceleration>().StartRoll(direction, current_power);
    }
}
