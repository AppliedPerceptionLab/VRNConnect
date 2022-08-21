using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrainRotator : MonoBehaviour
{
    //[SerializeField] private GameObject RightController;
    //[SerializeField] private GameObject LeftController;

    //public static BrainManipulation instance { get; private set; }
    public PrimaryButtonWatcher watcher;
    public Vector3 rotationAngle = new Vector3(45, 0, 0);
    public Quaternion FirstPosition;
    public float rotationDuration = 0.25f; // seconds
    private Quaternion offRotation;
    private Quaternion onRotation;
    private Coroutine rotator;

    // Start is called before the first frame update
    void Start()
    {
        //instance = this;
        watcher.primaryButtonPress.AddListener(onPrimaryButtonEvent);
        offRotation = this.transform.rotation;
        FirstPosition = offRotation;
        onRotation = Quaternion.Euler(rotationAngle) * offRotation;
    }

    public void onPrimaryButtonEvent(bool pressed)
    {
        if (rotator != null)
            StopCoroutine(rotator);
        if (pressed)
            rotator = StartCoroutine(AnimateRotation(this.transform.rotation, onRotation));
        /*else
            rotator = StartCoroutine(AnimateRotation(this.transform.rotation, offRotation));*/
    }

    private IEnumerator AnimateRotation(Quaternion fromRotation, Quaternion toRotation)
    {
        //float t = 0;
        while (true)
        {
            transform.Rotate(new Vector3(Time.deltaTime * 20, 0, 0), Space.World);
            yield return null;
         // transform.rotation = Quaternion.Lerp(fromRotation, toRotation, t / rotationDuration);
         // t += Time.deltaTime;
         // yield return null;
        }
       // onRotation = Quaternion.Euler(rotationAngle) * onRotation;
       // transform.Rotate(new Vector3(0, Time.deltaTime *20 ,0));
       // yield return null;
    }

    public void OnReset()
    {
        this.transform.rotation = FirstPosition;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
