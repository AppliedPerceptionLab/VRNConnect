using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class Thresholder : MonoBehaviour
{
    public GameObject brain;
    public TextMeshPro tmpro;
    public UnityEngine.UI.Slider slider;
    public float threshold = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        brain.GetComponent<ReadCSV>().threshold = 0.1f;
        brain.GetComponent<ReadCSV>().OnThresholdChange();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void OnValueChanged(float newValue)
    {
        if (newValue > 50)
        {
            float currentThreshold = (float) Math.Log10(newValue*10)/10;
            Debug.Log("currentThreshold = " + currentThreshold);
            brain.GetComponent<ReadCSV>().threshold = currentThreshold;
            brain.GetComponent<ReadCSV>().OnThresholdChange();
            tmpro.SetText(new StringBuilder().Append(currentThreshold*100 + "%").ToString());
        }
        else
        {
            var currentThreshold = newValue/500;
            Debug.Log("currentThreshold = " + currentThreshold);
            brain.GetComponent<ReadCSV>().threshold = currentThreshold;
            brain.GetComponent<ReadCSV>().OnThresholdChange();
            tmpro.SetText(new StringBuilder().Append(currentThreshold + "%").ToString());
        }
        
        
    }

    public void OnReset()
    {
        slider.SetValueWithoutNotify(50);
        tmpro.SetText("10%");
        brain.GetComponent<ReadCSV>().threshold = 0.1f;
        brain.GetComponent<ReadCSV>().OnThresholdChange();
    }
}
