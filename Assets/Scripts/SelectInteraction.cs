using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectInteraction : MonoBehaviour
{
    Color defaultColor;
    public GameObject brain;
    public void OnHoverOver()
    {
        GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
        //brain.GetComponent<ReadCSV>().EnableEdgeOfNode(gameObject.name, true);
        //defaultColor = GetComponent<Renderer>().material.GetColor("_Color");
        //GetComponent<Renderer>().material.SetColor("_Color",Color.red);
    }

    public void OnHoverEnd()
    {
        GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
        //brain.GetComponent<ReadCSV>().EnableEdgeOfNode(gameObject.name, false);
        //GetComponent<Renderer>().material.SetColor("_Color", defaultColor);
    }

    public void OnSelected()
    {
        brain.GetComponent<ReadCSV>().EnableEdgeOfNode(gameObject.name, true);
        //GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
        //GetComponent<Renderer>().material.SetColor("_Color", defaultColor);
    }

    public void OnUnselected()
    {
        brain.GetComponent<ReadCSV>().EnableEdgeOfNode(gameObject.name, false);
        //GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
        //GetComponent<Renderer>().material.SetColor("_Color", defaultColor);
    }
}
