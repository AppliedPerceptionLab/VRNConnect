using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetRig : MonoBehaviour
{
    private Quaternion startingRotation;
    private Vector3 startingPosition;
    // Start is called before the first frame update
    void Start()
    {
        var transform1 = transform;
        startingPosition = transform1.position;
        startingRotation = transform1.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetTransform()
    {
        var rotationAngleY = startingRotation.eulerAngles.y - transform.rotation.eulerAngles.y;
        transform.Rotate(0,rotationAngleY,0);

        var distanceDiff = startingPosition - transform.position;
        transform.position += distanceDiff;
    }
}
