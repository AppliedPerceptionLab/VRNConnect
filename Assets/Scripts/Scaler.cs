using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class Scaler : MonoBehaviour
{
    public GameObject brain;
    public TextMeshPro tmpro;
    public UnityEngine.UI.Slider slider;
    Vector3 localScale = new Vector3(1, 1, 1);
    // Start is called before the first frame update
    void Start()
    {
        localScale = brain.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void OnValueChanged(float newValue)
    {
        var currentScale = localScale;
        Debug.Log("CurrentScale = " + currentScale);
        var scaleMultiplier = newValue / 10;
        Debug.Log("scaleMultiplier = " + scaleMultiplier);
        currentScale = new Vector3(scaleMultiplier, scaleMultiplier, scaleMultiplier);
        brain.transform.localScale = currentScale;
        Debug.Log("localScale = " + currentScale);
        tmpro.SetText(new StringBuilder().Append(scaleMultiplier).ToString());
    }

    public void OnReset()
    {
        slider.SetValueWithoutNotify(10);
        tmpro.SetText("1");
        brain.transform.localScale = localScale;
        
    }
}
