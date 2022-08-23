using System;
using TMPro;
using UnityEngine;

public class TooltipUI : MonoBehaviour
{
    public Vector2 textPadding = new(0.5f, 0.2f);
    private Transform backgroundTransform;
    private Func<string> getTooltipTextFunc;
    private TextMeshPro textMeshPro;
    public static TooltipUI instance { get; private set; }

    // Start is called before the first frame update
    private void Start()
    {
        instance = this;

        backgroundTransform = transform.Find("TooltipBackground").GetComponent<Transform>();

        textMeshPro = transform.Find("TooltipText").GetComponent<TextMeshPro>();

        //SetText("Hello World from Unity Object");
        HideTooltip();
    }

    // Update is called once per frame
    private void Update()
    {
        SetText(getTooltipTextFunc());
    }

    private void SetText(string tooltipText)
    {
        textMeshPro.SetText(tooltipText);
        textMeshPro.ForceMeshUpdate();

        var textSize = new Vector2(textMeshPro.transform.localScale.x * textMeshPro.preferredWidth,
            textMeshPro.transform.localScale.y * textMeshPro.preferredHeight);
        var padding = textPadding;
        Debug.Log("RenderedValues: " + textMeshPro.GetRenderedValues());
        backgroundTransform.localScale = new Vector3(textSize.x + padding.x, textSize.y + padding.y, 1f);

        //TODO check if the size(height) is not bigger than transform y value
    }

    public static void ShowTooltip_Static(string tooltipText)
    {
        instance.ShowTooltip(tooltipText);
    }

    private void ShowTooltip(string tooltipText)
    {
        ShowTooltip(() => tooltipText);
    }

    public static void HideTooltip_Static()
    {
        instance.HideTooltip();
    }

    private void HideTooltip()
    {
        gameObject.SetActive(false);
    }

    public static void ShowTooltip_Static(Func<string> getTooltipTextFunc)
    {
        instance.ShowTooltip(getTooltipTextFunc);
    }

    private void ShowTooltip(Func<string> getTooltipTextFunc)
    {
        this.getTooltipTextFunc = getTooltipTextFunc;
        gameObject.SetActive(true);
        SetText(getTooltipTextFunc());
    }
}