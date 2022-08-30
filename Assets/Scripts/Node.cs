using System.Text;

public class Node
{
    public float Alpha;
    public float Blue;
    public float clusteringCoef;
    public string cortex;
    public int CortexID;
    public float Green;
    public string Lobe;
    public string LR;
    public string[] NodeConnection;

    public int nodeDegree;

    //From nodeId 361-379 are not needed
    public int nodeId;
    public float nodeStrength;
    public float Red;
    public string region;
    public int regionID;
    public string regionIdLabel;
    public string regionLongName;
    public string regionName;
    public int volume;
    public float xCog;
    public float yCog;
    public float zCog;


    public float ConvertNodeConnection(string c)
    {
        float res;
        float.TryParse(c, out res);
        return res;
    }

    internal string GetTooltipText()
    {
        var sb = new StringBuilder();
        sb.AppendLine("****" + regionName + "****");
        sb.AppendLine("Full Region Name : \t" + regionLongName);
        //sb.AppendLine("regionIdLabel : " + regionIdLabel);
        sb.AppendLine("Lobe : \t\t\t" + Lobe);
        sb.AppendLine("Left/Right: \t\t\t" + LR);
        sb.AppendLine("Region : \t\t\t" + region);
        sb.AppendLine("Cortex : \t\t\t" + cortex);
        //sb.AppendLine("regionID : " + regionID.ToString());
        sb.AppendLine("CortexID : \t\t\t" + CortexID.ToString());
        //sb.AppendLine(xCog.ToString());
        //sb.AppendLine(yCog.ToString());
        //sb.AppendLine(zCog.ToString());
        //sb.AppendLine("volume : " + volume.ToString());
        //sb.AppendLine(Red.ToString());
        //sb.AppendLine(Green.ToString());
        //sb.AppendLine(Blue.ToString());
        //sb.AppendLine(Alpha.ToString());
        sb.AppendLine("Degree : \t\t\t" + nodeDegree.ToString());
        sb.AppendLine("Strength : \t\t\t" + nodeStrength.ToString());
        sb.AppendLine("Clustering Coefficient : \t" + clusteringCoef.ToString());

        return sb.ToString();
    }
}