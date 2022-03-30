using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectInteraction : MonoBehaviour
{
    public void OnHoverOver()
    {
        GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
    }

    public void OnHoverEnd()
    {
        GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
    }
}
