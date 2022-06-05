using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallDeceleration : MonoBehaviour
{
    //public Transform stimpMeter;
    public float friction;

    Rigidbody body;

    private bool impact = false;

    void Awake()
    {
        body = GetComponent<Rigidbody>();
    }

    public void StartRoll(Vector3 direction, float force)
    {
        //Vector3 pushForce = (stimpMeter.forward.normalized * 22.0f);
        Vector3 pushForce = (direction * force);
        body.AddForce(pushForce, ForceMode.Acceleration);

        impact = true;
    }

    void FixedUpdate()
    {
        if (!impact)
            return;

        float dynamicFriction = friction * (.75F / body.velocity.magnitude);

        float curSpeed = body.velocity.magnitude;
        if (curSpeed < 0.05)
        {
            body.velocity = Vector3.zero;
            //body.isKinematic = true;
        }

        if (dynamicFriction > friction)
        {
            dynamicFriction = friction;
        }

        Vector3 opposite = -body.velocity;
        Vector3 brakeForce = opposite.normalized * dynamicFriction;
        body.AddForce(brakeForce * Time.deltaTime);
    }
}
