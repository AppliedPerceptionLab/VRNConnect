using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction.Input;
using UnityEngine;

public class DetectInput : MonoBehaviour
{
    private bool isHandActive = false;
    private bool isControllerActive = false;
    public TooltipUI tooltip;
    public HintTooltipUI hintTooltipUI;
    void Start()
    {
        isHandActive = OVRInput.GetConnectedControllers() == OVRInput.Controller.Hands;
        isControllerActive = OVRInput.GetConnectedControllers() == OVRInput.Controller.Touch;
    }

    // Update is called once per frame
    void Update()
    {
        bool currentControllerState = OVRInput.GetConnectedControllers() == OVRInput.Controller.Touch;
        bool currentHandState = OVRInput.GetConnectedControllers() == OVRInput.Controller.Hands;
        // Debug.unityLogger.Log(LogType.Warning,
        //     $"Hands = {currentHandState}, controllers = {currentControllerState}");
        switch (currentControllerState)
        {
            case true when !currentHandState:
            {
                if (isControllerActive) return;
                isControllerActive = true;
                isHandActive = false;
                tooltip.SetControllerType(isControllerActive, isHandActive);
                hintTooltipUI.SetControllerType(isControllerActive, isHandActive);
                break;
            }
            case false when currentHandState:
            {
                if (isHandActive) return;
                isControllerActive = false;
                isHandActive = true;
                tooltip.SetControllerType(isControllerActive, isHandActive);
                hintTooltipUI.SetControllerType(isControllerActive, isHandActive);
                break;
            }
        }
    }
}
