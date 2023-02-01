using System;
using TMPro;
using UnityEngine;

public class HintTooltipUI : MonoBehaviour
{
    public Vector2 textPadding = new(0.5f, 0.2f);
    private Transform backgroundTransform;
    private Func<string> getTooltipTextFunc;
    private TextMeshPro textMeshPro;
    private bool isHand = false;
    private bool isController = true;
    public static HintTooltipUI instance { get; private set; }

    // Start is called before the first frame update
    private void Start()
    {
        instance = this;

        backgroundTransform = transform.Find("HintTooltipBackground").GetComponent<Transform>();

        textMeshPro = transform.Find("HintText").GetComponent<TextMeshPro>();

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
        textMeshPro.alignment = TextAlignmentOptions.Left;
        textMeshPro.ForceMeshUpdate();

        var localScale = textMeshPro.transform.localScale;
        var renVal = textMeshPro.GetRenderedValues(false);
        var textSize = new Vector2(localScale.x * renVal.x,
            localScale.y * renVal.y);
        // var textSize = new Vector2(localScale.x * textMeshPro.preferredWidth,
        //     localScale.y * textMeshPro.preferredHeight);
        var localPosition = textMeshPro.transform.localPosition;
        var textPos = new Vector2(localPosition.x,
            localPosition.y);
        var padding = textPadding;
        Debug.Log("RenderedValues: GetRenderedValues" + textMeshPro.GetRenderedValues(false));
        Debug.Log("RenderedValues: textSize" + textSize);
        Debug.Log("RenderedValues: padding" + padding);
        backgroundTransform.localScale = new Vector3(textSize.x + padding.x, textSize.y + padding.y, 1f);
        textMeshPro.transform.localPosition = new Vector3(-textSize.x/2, 0f, -0.001f);

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
        if (isHand)
        {
            gameObject.transform.localPosition = new Vector3(-0.1f, 0.1f, 0.1f);
        }
        else
        {
            gameObject.transform.localPosition = new Vector3(0.0f, 0.15f, 0.2f);
        }
        SetText(getTooltipTextFunc());
    }

    public void SetControllerType(bool isController, bool isHand)
    {
        this.isHand = isHand;
        this.isController = isController;
        Debug.unityLogger.Log(LogType.Warning,
            $"TooltipUI Hands = {isHand}, controllers = {isController}");
    }
}