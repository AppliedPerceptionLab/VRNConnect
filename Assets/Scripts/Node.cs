using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
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
    public int nodeDegree;
    public int nodeStrenght;



    public float ConvertNodeConnection(string c)
    {
        float res;
        float.TryParse(c, result: out res);
        return res;
        
    }

    internal string GetTooltipText()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("***" + regionName + "***");
        sb.AppendLine("Full Region Name : " + regionLongName);
        //sb.AppendLine("regionIdLabel : " + regionIdLabel);
        sb.AppendLine("Lobe : " + Lobe);
        sb.AppendLine("Left/Right: "+ LR);
        sb.AppendLine("Region : " + region);
        sb.AppendLine("Cortex : " + cortex);
        //sb.AppendLine("regionID : " + regionID.ToString());
        sb.AppendLine("CortexID : " + CortexID.ToString());
        //sb.AppendLine(xCog.ToString());
        //sb.AppendLine(yCog.ToString());
        //sb.AppendLine(zCog.ToString());
        //sb.AppendLine("volume : " + volume.ToString());
        //sb.AppendLine(Red.ToString());
        //sb.AppendLine(Green.ToString());
        //sb.AppendLine(Blue.ToString());
        //sb.AppendLine(Alpha.ToString());
        sb.AppendLine("nodeDegree : " + nodeDegree.ToString());
        sb.AppendLine("nodeStrenght : " + nodeStrenght.ToString());

        return sb.ToString();
    }
}
