using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectInteraction : MonoBehaviour
{
    Color defaultColor;
    public void OnHoverOver()
    {
        GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
        //defaultColor = GetComponent<Renderer>().material.GetColor("_Color");
        //GetComponent<Renderer>().material.SetColor("_Color",Color.red);
    }

    public void OnHoverEnd()
    {
        GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
        //GetComponent<Renderer>().material.SetColor("_Color", defaultColor);
    }
}
