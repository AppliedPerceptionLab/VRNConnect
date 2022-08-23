using UnityEngine;

public class SelectInteraction : MonoBehaviour
{
    [SerializeField] private GameObject brain;

    public bool nodeSelected = false;
    private Color defaultColor;

    //TODO fix the bug with selection/hovering over 2,3 nodes in a straight line
    public void OnHoverOver()
    {
        GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
        if (!nodeSelected) TooltipUI.ShowTooltip_Static(gameObject.name);
        //brain.GetComponent<ReadCSV>().EnableEdgeOfNode(gameObject.name, true);
        //defaultColor = GetComponent<Renderer>().material.GetColor("_Color");
        //GetComponent<Renderer>().material.SetColor("_Color",Color.red);
    }

    public void OnHoverEnd()
    {
        if (!nodeSelected) GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
        if (!nodeSelected) TooltipUI.HideTooltip_Static();
        //brain.GetComponent<ReadCSV>().EnableEdgeOfNode(gameObject.name, false);
        //GetComponent<Renderer>().material.SetColor("_Color", defaultColor);
    }

    public void OnSelected()
    {
        brain.GetComponent<ReadCSV>().EnableEdgeOfNode(gameObject.name, true);
        var tooltipText = brain.GetComponent<ReadCSV>().GetTooltipTextForNode(gameObject.name);
        brain.GetComponent<ReadCSV>().addNodetoQueue(gameObject.name);
        TooltipUI.ShowTooltip_Static(tooltipText);
        nodeSelected = true;
        //GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
        //GetComponent<Renderer>().material.SetColor("_Color", defaultColor);
    }

    public void OnUnselected()
    {
        //At the moment I am not using this one
        if (nodeSelected) GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
        brain.GetComponent<ReadCSV>().EnableEdgeOfNode(gameObject.name, false);
        TooltipUI.HideTooltip_Static();
        nodeSelected = false;
        //GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
        //GetComponent<Renderer>().material.SetColor("_Color", defaultColor);
    }
}