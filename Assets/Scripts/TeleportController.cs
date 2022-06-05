using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportController : MonoBehaviour
{
    public VRTeleporter teleporter;

    // Start is called before the first frame update
    void Start()
    {
        teleporter.ToggleDisplay(true);
    }

    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            teleporter.ToggleDisplay(true);
        }

        if (Input.GetMouseButtonUp(0))
        {
            teleporter.Teleport();
            teleporter.ToggleDisplay(false);
        }
    }
}
