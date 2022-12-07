using System.Text;
using TMPro;
using UnityEngine;

public class Scaler : MonoBehaviour
{
    public GameObject brain;
    public TextMeshPro tmpro;
    public UnityEngine.UI.Slider slider;

    private Vector3 localScale = new(1, 1, 1);

    // Start is called before the first frame update
    private void Start()
    {
        localScale = brain.transform.localScale;
    }

    // Update is called once per frame
    private void Update()
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
        brain.transform.localScale = localScale;
        tmpro.SetText("1");
    }
}