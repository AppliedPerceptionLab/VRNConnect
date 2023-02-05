using Oculus.Interaction.Input;
using UnityEngine;

public class Clickcounter : MonoBehaviour
{
    // Start is called before the first frame update
    public UserStudyScript uTest;
    public GameObject handR;
    private Hand hand;
    private bool controller_triggered = false;
    private bool pinch_triggered = false;
    void Start()
    {
        hand = handR.GetComponent<Hand>();
    }

    // Update is called once per frame
    void Update()
    {
        float triggerLeft = OVRInput.Get(OVRInput.RawAxis1D.LIndexTrigger); //Between 0-1
        
        float triggerRight = OVRInput.Get(OVRInput.RawAxis1D.RIndexTrigger);

        // Debug.unityLogger.Log(LogType.Error,$"left:{triggerLeft} - right:{triggerRight}");
        
        // var hand = GetComponent<OVRHand>();
        float ringFingerPinchStrength = hand.GetFingerPinchStrength(HandFinger.Ring);
        float indexFingerPinchStrength = hand.GetFingerPinchStrength(HandFinger.Index);
        // Debug.unityLogger.Log(LogType.Error,$"ringFinger:{ringFingerPinchStrength} - indexFinger:{indexFingerPinchStrength}");
        

        if (triggerLeft > 0.9f || triggerRight > 0.9f)
        {
            if (!controller_triggered)
            {
                uTest.getUserClicks();
                controller_triggered = true;   
            }
        }
        else if (triggerLeft < 0.5f || triggerRight < 0.5f)
        {
            controller_triggered = false;
        }

        if (indexFingerPinchStrength > 0.8f || ringFingerPinchStrength > 0.8f)
        {
            if (!pinch_triggered)
            {
                uTest.getUserClicks();
                pinch_triggered = true;
            }
        }
        else if (indexFingerPinchStrength<0.5f || ringFingerPinchStrength < 0.5f)
        {
            pinch_triggered = false;
        }
    }
}