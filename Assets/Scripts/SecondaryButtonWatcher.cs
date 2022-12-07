using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;

[System.Serializable]
public class SecondaryButtonEvent : UnityEvent<bool>
{
}

public class SecondaryButtonWatcher : MonoBehaviour
{
    public SecondaryButtonEvent ButtonPress;
    private List<InputDevice> devicesWithSecondaryButton;

    private bool lastButtonState = false;

    private void Awake()
    {
        if (ButtonPress == null) ButtonPress = new SecondaryButtonEvent();

        devicesWithSecondaryButton = new List<InputDevice>();
    }

    private void Update()
    {
        var tempState = false;
        foreach (var device in devicesWithSecondaryButton)
        {
            var ButtonState = false;
            tempState =
                (device.TryGetFeatureValue(CommonUsages.secondaryButton, out ButtonState) // did get a value
                 && ButtonState) // the value we got
                || tempState; // cumulative result from other controllers
        }

        if (tempState != lastButtonState) // Button state changed since last frame
        {
            ButtonPress.Invoke(tempState);
            lastButtonState = tempState;
        }
    }

    private void OnEnable()
    {
        var allDevices = new List<InputDevice>();
        InputDevices.GetDevices(allDevices);
        foreach (var device in allDevices)
            InputDevices_deviceConnected(device);

        InputDevices.deviceConnected += InputDevices_deviceConnected;
        InputDevices.deviceDisconnected += InputDevices_deviceDisconnected;
    }

    private void OnDisable()
    {
        InputDevices.deviceConnected -= InputDevices_deviceConnected;
        InputDevices.deviceDisconnected -= InputDevices_deviceDisconnected;
        devicesWithSecondaryButton.Clear();
    }

    private void InputDevices_deviceConnected(InputDevice device)
    {
        bool discardedValue;
        if (device.TryGetFeatureValue(CommonUsages.secondaryButton, out discardedValue))
            devicesWithSecondaryButton.Add(device); // Add any devices that have a secondary button.
    }

    private void InputDevices_deviceDisconnected(InputDevice device)
    {
        if (devicesWithSecondaryButton.Contains(device))
            devicesWithSecondaryButton.Remove(device);
    }
}