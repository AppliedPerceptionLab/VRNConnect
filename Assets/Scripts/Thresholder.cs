using System.Text;
using TMPro;
using UnityEngine;

public class Thresholder : MonoBehaviour
{
    public GameObject brain;
    public TextMeshPro tmpro;
    public UnityEngine.UI.Slider slider;

    public float threshold = 0.05f;

    // Start is called before the first frame update
    private void Start()
    {
        brain.GetComponent<ReadCSV>().threshold = threshold;
        brain.GetComponent<ReadCSV>().OnThresholdChange();
        tmpro.SetText(new StringBuilder().Append((threshold*100).ToString("0.00")).Append("%").ToString());
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void OnValueChanged(float newValue)
    {
        float currentThreshold;
        // if (newValue > 50)
        // {
        //     currentThreshold = (float)Math.Log10(newValue * 10) / 10;
        //     Debug.Log("currentThreshold = " + currentThreshold);
        // }
        // else
        // {
            currentThreshold = newValue / 500;
        // }
        
        Debug.Log("currentThreshold = " + currentThreshold);
        brain.GetComponent<ReadCSV>().threshold = currentThreshold;
        brain.GetComponent<ReadCSV>().OnThresholdChange();
        tmpro.SetText(new StringBuilder().Append((currentThreshold*100).ToString("0.00")).Append("%").ToString());
    }

    public void OnReset()
    {
        slider.SetValueWithoutNotify(25);
        brain.GetComponent<ReadCSV>().threshold = threshold;
        brain.GetComponent<ReadCSV>().OnThresholdChange();
        tmpro.SetText(new StringBuilder().Append((threshold*100).ToString("0.00")).Append("%").ToString());
    }
}