using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableObject : MonoBehaviour
{
    private PlayerGrabbing grabbedByPlayer;

    public void Grab(Transform grabPoint)
    {
        transform.position = grabPoint.position;
        transform.rotation = grabPoint.rotation;
    }
}
