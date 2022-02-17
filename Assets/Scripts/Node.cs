using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    //From nodeId 361-379 are not needed
    public int nodeId;
    public string[] NodeConnection;
    public string regionName;
    public string regionLongName;
    public string regionIdLabel;
    public string LR;
    public string region;
    public string Lobe;
    public string cortex;
    public int regionID;
    public int CortexID;
    public float xCog;
    public float yCog;
    public float zCog;
    public int volume;
    public float Red;
    public float Green;
    public float Blue;
    public float Alpha;


    public float ConvertNodeConnection(string c)
    {
        float res;
        float.TryParse(c, result: out res);
        return res;
        
    }
}
