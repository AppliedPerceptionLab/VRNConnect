using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TooltipUI : MonoBehaviour
{
    private Transform backgroundTransform;
    private TextMeshPro textMeshPro;
    private System.Func<string> getTooltipTextFunc;
    public Vector2 textPadding = new Vector2(0.5f, 0.2f);
    public static TooltipUI instance { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        backgroundTransform = transform.Find("TooltipBackground").GetComponent<Transform>();
        
        textMeshPro = transform.Find("TooltipText").GetComponent<TextMeshPro>();

        //SetText("Hello World from Unity Object");
        HideTooltip();
    }

    private void SetText(string tooltipText)
    {
        textMeshPro.SetText(tooltipText);
        textMeshPro.ForceMeshUpdate();

        Vector2 textSize = new Vector2(textMeshPro.transform.localScale.x * textMeshPro.preferredWidth, textMeshPro.transform.localScale.y * textMeshPro.preferredHeight);
        Vector2 padding = textPadding;
        Debug.Log("RenderedValues: "+textMeshPro.GetRenderedValues());
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

    public static void ShowTooltip_Static(System.Func<string> getTooltipTextFunc)
    {
        instance.ShowTooltip(getTooltipTextFunc);
    }

    private void ShowTooltip(Func<string> getTooltipTextFunc)
    {
        this.getTooltipTextFunc = getTooltipTextFunc;
        gameObject.SetActive(true);
        SetText(getTooltipTextFunc());
    }

    // Update is called once per frame
    void Update()
    {
        SetText(getTooltipTextFunc());
    }
}
