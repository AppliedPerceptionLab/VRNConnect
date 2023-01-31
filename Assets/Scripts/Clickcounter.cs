using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clickcounter : MonoBehaviour
{
    // Start is called before the first frame update
    public UserStudyScript uTest;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float triggerLeft = OVRInput.Get(OVRInput.RawAxis1D.LIndexTrigger); //Between 0-1
        
        float triggerRight = OVRInput.Get(OVRInput.RawAxis1D.RIndexTrigger);
        
        var hand = GetComponent<OVRHand>();
        bool isIndexFingerPinching = hand.GetFingerIsPinching(OVRHand.HandFinger.Index);
        float ringFingerPinchStrength = hand.GetFingerPinchStrength(OVRHand.HandFinger.Ring);
        

        if (triggerLeft > 0.9f || triggerRight > 0.9f)
        {
            uTest.getUserClicks();
        }

        if (isIndexFingerPinching && ringFingerPinchStrength>0.8f)
        {
            uTest.getUserClicks();
        }
    }
}