using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class ReadCSV : MonoBehaviour
{
    public const int numberOfSelections = 2;
    public static List<Node> selectedNodes = new();
    public static List<Edge> highlightedEdge = new();
    public static List<Node> nodes = new();

    public static List<Edge> edges = new();

    //private Graph<Vector3, float> graph;
    //public Vector3 CenterOfMass = new Vector3(0.0f, 0.0f, 0.0f);
    public Vector3 scale = new(50.0f, 50.0f, 50.0f);

    // public Vector3 rotate = new(90.0f, 0.0f, 0.0f);
    public Vector3 sphereSize = new(0.05f, 0.05f, 0.05f);
    public bool edgeColoring = false;
    public bool shadowEffect = false;
    public float threshold = 0.05f;
    private string shortestPathAlg = "Hops";
    public Color defaultColor = Color.black;

    [Tooltip("The file that contains algorithm results")]
    private TextAsset algResults;

    [Tooltip("The file that contains the reference atlas of the brain")]
    private TextAsset atlas;

    [Tooltip("The file that contains the reference atlas of the brain")]
    private TextAsset atlasColors;

    //[Tooltip("The Prefabs for each object")]
    //public List<GameObject> objects = new List<GameObject>();

    [Tooltip("The file that contains structural connectivity")]
    private TextAsset brainSC;

    [Tooltip("The file that contains clustering coefficients")]
    private TextAsset clusteringCoef;

    [Tooltip("The file that contains shortest distances")]
    private TextAsset distFile;
    
    [Tooltip("The file that contains Floyd alg results for shortest paths length")]
    private TextAsset floydsplFile;
    
    [Tooltip("The file that contains Floyd alg results for pmat")]
    private TextAsset floydpmatFile;
    
    [Tooltip("The file that contains Floyd alg results for hops")]
    private TextAsset floydhopsFile;

    private float maxEdgeSize = 0f;

    private int nodesSelected = 0;
    /*private float weiAss;
    private float binAss;
    private float minDist;
    private float maxDist;*/

    private bool showAllEdges = true;
    public bool ShownFlag { get; private set; } = false;

    // Start is called before the first frame update
    private void Start()
    {
        brainSC = Resources.Load<TextAsset>("hcpmmp1");
        atlas = Resources.Load<TextAsset>("HCP-MMP1_UniqueRegionList");
        atlasColors = Resources.Load<TextAsset>("HCP-MMP1_RegionColor");

        var Node = brainSC.text.Split(new char[] { '\n' });
        var AtlasNode = atlas.text.Split(new char[] { '\n' });
        var AtlasNodeColor = atlasColors.text.Split(new char[] { '\n' });
        // Debug.Log(Node.Length);

        for (var i = 0; i < Node.Length - 1; i++)
        {
            var Node2 = Node[i].Split(new char[] { ' ' });
            var n = new Node();
            n.nodeId = i + 1;
            n.NodeConnection = Node2;
            nodes.Add(n);
        }

        for (var i = 1; i < AtlasNode.Length - 1; i++)
        {
            var atlasRow = AtlasNode[i].Split(new char[] { ',' });
            nodes[i - 1].regionName = atlasRow[0];
            nodes[i - 1].regionLongName = atlasRow[1];
            nodes[i - 1].regionIdLabel = atlasRow[2];
            nodes[i - 1].LR = atlasRow[3];
            nodes[i - 1].region = atlasRow[4];
            nodes[i - 1].Lobe = atlasRow[5];
            nodes[i - 1].cortex = atlasRow[6];
            int.TryParse(atlasRow[7], out nodes[i - 1].regionID);
            int.TryParse(atlasRow[8], out nodes[i - 1].CortexID);
            float.TryParse(atlasRow[9], out nodes[i - 1].xCog);
            float.TryParse(atlasRow[10], out nodes[i - 1].yCog);
            float.TryParse(atlasRow[11], out nodes[i - 1].zCog);
            int.TryParse(atlasRow[12], out nodes[i - 1].volume);
        }

        for (var i = 1; i < AtlasNodeColor.Length - 1; i++)
        {
            var colorRow = AtlasNodeColor[i].Split(new char[] { ',' });
            float.TryParse(colorRow[2], out nodes[i - 1].Red);
            float.TryParse(colorRow[3], out nodes[i - 1].Green);
            float.TryParse(colorRow[4], out nodes[i - 1].Blue);
            float.TryParse(colorRow[5], out nodes[i - 1].Alpha);
        }

        CreateGraph();
        RepositionBrain();
        ReadMeasurements();
        //TooltipRes.ShowTooltip_Static(gameObject.name);
    }

    // Update is called once per frame

    private void Update()
    {
    }

    private void RepositionBrain()
    {
        //Vector3 COM = FindCenterOfMass();
        //Vector3 axis = Vector3.zero;
        /*if(vect == 0)
        {
            axis = new Vector3(COM.x, 0, 0);
        } else if (vect == 1)
        {
            axis = new Vector3(0, COM.y, 0);
        } else if (vect == 2)
        {
            axis = new Vector3(0, 0, COM.z);
        }*/
        gameObject.transform.Rotate(-90f, -90f, 90f, Space.World);
        gameObject.transform.Translate(new Vector3(0f, -0.4f, 4f), Space.World);
    }

    private Vector3 FindCenterOfMass()
    {
        var com = Vector3.zero;
        foreach (var n in nodes)
            //remove regions > 360 -> cerebellum
            if (n.nodeId < 361)
            {
                com.x += n.xCog / 360;
                com.y += n.yCog / 360;
                com.z += n.zCog / 360;
            }

        return com;
    }

    private void CreateGraph()
    {
        //Create the gameObject for each Node
        generateNodes();

        //Create the gameObject for each Edge
        generateEdges();
    }

    private void generateNodes()
    {
        //if (showNodes)
        //{
        var sampleNode = GameObject.Find("Node");
        foreach (var n in nodes)
        {
            GameObject temp;
            //remove regions > 360 -> cerebellum
            if (n.nodeId < 361)
            {
                //temp = Instantiate(objects[n.nodeId%2]);
                //temp = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                temp = Instantiate(sampleNode);
                temp.name = n.regionName;
                //temp.GetComponent<MeshRenderer>().material = Resources.Load("Materials/SphereB.mat", typeof(Material)) as Material;
                //Using a material from assets with GPU instancing on
                temp.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/SphereB");
                temp.GetComponent<MeshRenderer>().material.SetColor("_Color",
                    new Color(n.Red / 255f, n.Green / 255f, n.Blue / 255f, n.Alpha / 255f));
                //temp.GetComponent<MeshRenderer>().receiveShadows = false;
                temp.transform.localScale = getSphereSize();
                temp.transform.position = scaleVector3(new Vector3(n.xCog, n.yCog, n.zCog));
                //make each node a child of the current GameObject
                temp.transform.parent = gameObject.transform;
                //temp.transform.Rotate(rotation.x, rotation.y, rotation.z, Space.World);
                //objects.Add(temp);
                // n.gameObject = temp;
            }
            //Debug.Log(n.nodeId + " : " + n.regionName + " (" + n.xCog + "," + n.yCog + "," + n.zCog + ")" + n.NodeConnection.Length);
        }

        sampleNode.SetActive(false);
        //}
    }

    private Vector3 getSphereSize()
    {
        //TODO implement the sphere size
        return sphereSize;
    }

    private Vector3 scaleVector3(Vector3 Value)
    {
        return new Vector3(Value.x / scale.x, Value.y / scale.y, Value.z / scale.z);
    }

    private void generateEdges()
    {
        Vector3 start, end, offset;
        var sampleEdge = GameObject.Find("Edge");
        var id = 1;
        for (var i = 0; i < nodes.Count && i < 360; i++)
        {
            var n = nodes[i];
            start = scaleVector3(new Vector3(n.xCog, n.yCog, n.zCog));
            for (var j = i + 1; j < n.NodeConnection.Length && j < 360; j++)
            {
                float.TryParse(n.NodeConnection[j], out var strength);
                //Debug.Log(n.NodeConnection[j]);
                //if they have connection
                if (strength != 0.0f)
                {
                    //Debug.Log(i + ":" + j + "->" + strength) ;
                    var e = nodes[j];
                    end = scaleVector3(new Vector3(e.xCog, e.yCog, e.zCog));
                    offset = end - start;
                    var edge = new Edge();
                    edge.edgeId = id++;
                    edge.node1Id = n.nodeId;
                    edge.node2Id = e.nodeId;
                    var color1 = new Color(n.Red / 255f, n.Green / 255f, n.Blue / 255f, n.Alpha / 255f);
                    var color2 = new Color(e.Red / 255f, e.Green / 255f, e.Blue / 255f, e.Alpha / 255f);
                    edge.edgeColor = CreateGradient(color1, color2);
                    edge.start = start;
                    edge.end = end;
                    edge.offset = offset;
                    edge.strenght = strength;
                    edge.name = n.nodeId + ":" + e.nodeId;
                    drawEdge(edge, sampleEdge);
                }
            }
        }

        maxEdgeSize = FindMax();
        sampleEdge.SetActive(false);
        //Enabling edges depending on strenght*threshold
        //calculating node degree
        for (var i = 0; i < nodes.Count && i < 360; i++)
        {
            var n = nodes[i];
            nodes[i].nodeDegree = (int)calculateNodeDegree(n, true);
            nodes[i].nodeStrength = calculateNodeDegree(n, false);
            EnableEdgeOfNode(n.regionName, showAllEdges);
        }
    }

    private float calculateNodeDegree(Node n, bool isNodedegree)
    {
        float res = 0;
        foreach (var con in n.NodeConnection)
        {
            float.TryParse(con, out var strength);
            if (strength >= threshold * maxEdgeSize)
            {
                if (isNodedegree)
                    res++;
                else
                    res += strength;
            }
        }

        return res;
    }

    private void drawEdge(Edge edge, GameObject sampleEdge)
    {
        //GameObject edge = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        var edgeObj = Instantiate(sampleEdge);
        //edge.GetComponent<MeshRenderer>().material = Resources.Load("Materials/SphereR.mat", typeof(Material)) as Material;
        //Using a material from assets with GPU instancing on
        edgeObj.SetActive(true);
        edgeObj.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/EdgeR");
        edgeObj.GetComponent<MeshRenderer>().material.SetColor("_Color", defaultColor);
        edgeObj.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        edgeObj.GetComponent<MeshRenderer>().receiveShadows = false;
        edgeObj.name = edge.name;
        edgeObj.transform.position = edge.start + edge.offset / 2.0f;
        //edge.transform.localScale = new Vector3(strength, offset.magnitude, strength);
        edgeObj.transform.up = edge.offset;
        edgeObj.transform.localScale = new Vector3(0.001f, edge.offset.magnitude / 2.0f, 0.001f);
        edgeObj.transform.parent = gameObject.transform;
        //objects.Add(edge);
        edge.gameObject = edgeObj;
        edges.Add(edge);
        edgeObj.SetActive(false);
    }

    private float CalculateWidth(float strength)
    {
        if (strength >= threshold * maxEdgeSize) return strength * 0.04f / maxEdgeSize;

        return 0f;
    }

    private float FindMax()
    {
        var max = 0f;
        foreach (var e in edges)
            if (max < e.strenght)
                max = e.strenght;
        Debug.Log("Max =" + max);
        return max;
    }

    public string GetTooltipTextForNode(string name)
    {
        var tooltipText = "";
        var node = FindNode(name);
        //NodeID found
        if (node != null) tooltipText = node.GetTooltipText();
        //tooltipText = node.ToString();
        return tooltipText;
    }

    public void EnableEdgeOfNode(string nodeName, bool enable)
    {
        Debug.Log(nodeName + enable);
        var nodeID = FindNodeID(nodeName);
        //NodeID found
        if (nodeID != -1)
            foreach (var e in edges)
                if (e.node1Id == nodeID || e.node2Id == nodeID)
                {
                    var width = CalculateWidth(e.strenght);
                    if (width > 0f)
                    {
                        e.gameObject.transform.localScale = new Vector3(width, e.offset.magnitude / 2.0f, width);
                        e.gameObject.GetComponent<MeshRenderer>().material.DisableKeyword("_EMISSION");
                        if (showAllEdges)
                        {
                            if (edgeColoring)
                                e.gameObject.GetComponent<MeshRenderer>().material.SetColor("_Color", e.edgeColor);
                            else
                                e.gameObject.GetComponent<MeshRenderer>().material.SetColor("_Color", defaultColor);
                        }
                        else
                        {
                            if (enable)
                            {
                                e.gameObject.GetComponent<MeshRenderer>().material.SetColor("_Color", e.edgeColor);
                                if (shadowEffect)
                                    e.gameObject.GetComponent<MeshRenderer>().shadowCastingMode =
                                        UnityEngine.Rendering.ShadowCastingMode.On;
                            }
                            else
                            {
                                e.gameObject.GetComponent<MeshRenderer>().material.SetColor("_Color", defaultColor);
                                e.gameObject.GetComponent<MeshRenderer>().shadowCastingMode =
                                    UnityEngine.Rendering.ShadowCastingMode.Off;
                            }
                        }

                        e.gameObject.SetActive(enable || showAllEdges);
                    }
                    else
                    {
                        e.gameObject.SetActive(false); //Destroy gameObject
                        e.gameObject.GetComponent<MeshRenderer>().material.DisableKeyword("_EMISSION");
                    }
                }
    }

    private Node FindNode(string nodeName)
    {
        foreach (var n in nodes)
            if (n.regionName.Equals(nodeName))
                return n;

        //Node not found
        return null;
    }

    private int FindNodeID(string nodeName)
    {
        foreach (var n in nodes)
            if (n.regionName.Equals(nodeName))
                return n.nodeId;

        //Node not found
        return -1;
    }
    
    private Node FindNodeByID(int nodeID)
    {
        foreach (var n in nodes)
            if (n.nodeId.Equals(nodeID))
                return n;

        //Node not found
        return null;
    }

    private Color CreateGradient(Color color1, Color color2)
    {
        GradientColorKey[] colorKey;
        GradientAlphaKey[] alphaKey;


        var gradient = new Gradient();

        // Populate the color keys at the relative time 0 and 1 (0 and 100%)
        colorKey = new GradientColorKey[2];
        colorKey[0].color = color1;
        colorKey[0].time = 0.0f;
        colorKey[1].color = color2;
        colorKey[1].time = 1.0f;

        // Populate the alpha  keys at relative time 0 and 1  (0 and 100%)
        alphaKey = new GradientAlphaKey[2];
        alphaKey[0].alpha = 1.0f;
        alphaKey[0].time = 0.0f;
        alphaKey[1].alpha = 0.0f;
        alphaKey[1].time = 1.0f;

        gradient.SetKeys(colorKey, alphaKey);

        return gradient.Evaluate(1f);
    }

    private void ReadMeasurements()
    {
        algResults = Resources.Load<TextAsset>("algorithm_results");
        //distFile = Resources.Load<TextAsset>("breadth_distance");
        clusteringCoef = Resources.Load<TextAsset>("clustering_coef");

        var results = algResults.text.Split(new char[] { '\n' });
        //string[] AtlasNode = atlas.text.Split(new char[] { '\n' });
        var clustCoef = clusteringCoef.text.Split(new char[] { '\n' });

        for (var i = 1; i < clustCoef.Length - 1; i++)
        {
            var Row = clustCoef[i].Split(new char[] { ',' });
            var nodeID = 0;
            int.TryParse(Row[0], out nodeID);
            float.TryParse(Row[1], out nodes[nodeID].clusteringCoef);
        }

        var sb = new StringBuilder();
        for (var i = 0; i < results.Length - 1; i++)
            if (i == 0)
            {
                sb.AppendLine(results[i]);
                sb.AppendLine("***********************");
            }
            else
            {
                var Row = results[i].Split(new char[] { ',' });
                sb.AppendLine(Row[0] + ":\t" + Row[1] + "\t" + Row[2]);
            }

        sb.ToString();
    }

    public void OnThresholdChange()
    {
        shortestPathAlg = "Hops";
        resetNodesEmission();
        for (var i = 0; i < nodes.Count && i < 360; i++)
        {
            var n = nodes[i];
            nodes[i].nodeDegree = (int)calculateNodeDegree(n, true);
            nodes[i].nodeStrength = calculateNodeDegree(n, false);
            GameObject.Find(n.regionName).GetComponent<MeshRenderer>().material.DisableKeyword("_EMISSION");
            EnableEdgeOfNode(n.regionName, showAllEdges);
        }
    }

    public void addNodetoQueue(string nodeName)
    {
        Debug.unityLogger.Log(LogType.Warning, nodeName);
        if (nodesSelected < numberOfSelections)
        {
            var n = FindNode(nodeName);
            selectedNodes.Add(n);
            nodesSelected++;
        }
        else
        {
            resetNodesEmission();
            ShownFlag = false;
            addNodetoQueue(nodeName);
        }

        if (nodesSelected == numberOfSelections) ShownFlag = showPath();

        if (nodesSelected == 1)
        {
            //diableAllOtherEdges();
            //EnableEdgeOfNode(nodeName,true);
        }
    }

    private bool showPath()
    {
        //TODO have to modify this to show only shortest path
        
        Debug.unityLogger.Log(LogType.Error, selectedNodes.Count);
        foreach (var node in selectedNodes) Debug.unityLogger.Log(LogType.Warning, node.regionName);
        if (shortestPathAlg.Equals("Hops"))
        {
            floydhopsFile = Resources.Load<TextAsset>("floyd_shortest_paths_hops_hops");
            floydsplFile = Resources.Load<TextAsset>("floyd_shortest_paths_hops_spl");
            floydpmatFile = Resources.Load<TextAsset>("floyd_shortest_paths_hops_pmat");
        }
        else
        {
            floydhopsFile = Resources.Load<TextAsset>("floyd_shortest_paths_dist_hops");
            floydsplFile = Resources.Load<TextAsset>("floyd_shortest_paths_dist_spl");
            floydpmatFile = Resources.Load<TextAsset>("floyd_shortest_paths_dist_pmat");
        }
        diableAllOtherEdges();
        var startingNode = selectedNodes[0].nodeId-1;
        var targetNode = selectedNodes[1].nodeId-1;
        var spl = floydsplFile.text.Split(new char[] { '\n' });
        var sourceRow = spl[startingNode+1].Split(new char[] { ',' });
        if (!sourceRow[targetNode+1].Equals("inf"))
        {
            calculatePath(startingNode, targetNode);
            
            var hops = floydhopsFile.text.Split(new char[] { '\n' });
            var hopsRow = hops[startingNode+1].Split(new char[] { ',' });
            Debug.unityLogger.Log(LogType.Warning, "Number of hops" + hopsRow[targetNode+1]);
        }
        else
        {
            Debug.unityLogger.Log(LogType.Error, "There is no path between selected nodes" + startingNode + "and " + targetNode);
        }
        //showSelectedNodesEdges();

        return true;
    }

    private void calculatePath(int startingNode, int targetNode)
    {
        int nodeToCheck = startingNode;
        List<Node> nodesInPath = new();
        nodesInPath.Add(FindNodeByID(startingNode+1));
        var pmat = floydpmatFile.text.Split(new char[] { '\n' });
        
        while (nodeToCheck != targetNode)
        {
            var pmatRow = pmat[nodeToCheck+1].Split(new char[] { ',' });
            int.TryParse(pmatRow[targetNode+1], out nodeToCheck);
            nodesInPath.Add(FindNodeByID(nodeToCheck+1));
        }

        drawShortestPath(nodesInPath);

    }

    private void drawShortestPath(List<Node> nodesInPath)
    {
        for (int i = 0; i < nodesInPath.Count; i++)
        {
            if (i != nodesInPath.Count - 1)
            {
                foreach (var e in edges)
                {
                    if (e.node1Id == nodesInPath[i].nodeId && e.node2Id == nodesInPath[i+1].nodeId)
                    {
                        var width = CalculateWidth(e.strenght);
                        e.gameObject.transform.localScale = new Vector3(width, e.offset.magnitude / 2.0f, width);
                        e.gameObject.GetComponent<MeshRenderer>().material.SetColor("_Color", e.edgeColor);
                        e.gameObject.SetActive(true);
                        e.gameObject.GetComponent<MeshRenderer>().material.EnableKeyword("_EMISSION");
                        highlightedEdge.Add(e);
                    }

                    if (e.node1Id == nodesInPath[i+1].nodeId && e.node2Id == nodesInPath[i].nodeId)
                    {
                        var width = CalculateWidth(e.strenght);
                        e.gameObject.transform.localScale = new Vector3(width, e.offset.magnitude / 2.0f, width);
                        e.gameObject.GetComponent<MeshRenderer>().material.SetColor("_Color", e.edgeColor);
                        e.gameObject.SetActive(true);
                        e.gameObject.GetComponent<MeshRenderer>().material.EnableKeyword("_EMISSION");
                        highlightedEdge.Add(e);
                    }
                }
            }
            // nodesInPath[i].gameObject.GetComponent<MeshRenderer>().material.EnableKeyword("_EMISSION");
            // nodesInPath[i].gameObject.GetComponent<MeshRenderer>().transform.localScale *= 2;
        }
    }

    //Not used for now
    private void showSelectedNodesEdges()
    {
        foreach (var e in edges)
        {
            if (e.node1Id == selectedNodes[0].nodeId || e.node2Id == selectedNodes[1].nodeId)
            {
                var width = CalculateWidth(e.strenght);
                e.gameObject.transform.localScale = new Vector3(width, e.offset.magnitude / 2.0f, width);
                e.gameObject.GetComponent<MeshRenderer>().material.SetColor("_Color", e.edgeColor);
                e.gameObject.SetActive(true);
            }

            if (e.node1Id == selectedNodes[1].nodeId || e.node2Id == selectedNodes[0].nodeId)
            {
                var width = CalculateWidth(e.strenght);
                e.gameObject.transform.localScale = new Vector3(width, e.offset.magnitude / 2.0f, width);
                e.gameObject.GetComponent<MeshRenderer>().material.SetColor("_Color", e.edgeColor);
                e.gameObject.SetActive(true);
            }

            if (e.node1Id == selectedNodes[0].nodeId && e.node2Id == selectedNodes[1].nodeId)
            {
                e.gameObject.GetComponent<MeshRenderer>().material.EnableKeyword("_EMISSION");
                highlightedEdge.Add(e);
            }

            if (e.node1Id == selectedNodes[1].nodeId && e.node2Id == selectedNodes[0].nodeId)
            {
                e.gameObject.GetComponent<MeshRenderer>().material.EnableKeyword("_EMISSION");
                highlightedEdge.Add(e);
            }
        }
    }

    private void diableAllOtherEdges()
    {
        foreach (var e in edges)
        {
            e.gameObject.SetActive(false);
            e.gameObject.GetComponent<MeshRenderer>().material.DisableKeyword("_EMISSION");
        }
    }

    private void resetNodesEmission()
    {
        nodesSelected = 0;
        foreach (var node in selectedNodes)
        {
            GameObject.Find(node.regionName).GetComponent<MeshRenderer>().material.DisableKeyword("_EMISSION");
            GameObject.Find(node.regionName).GetComponent<SelectInteraction>().nodeSelected = false;
            GameObject.Find(node.regionName).GetComponent<MeshRenderer>().transform.localScale = getSphereSize();
        }

        foreach (var e in highlightedEdge)
        {
            e.gameObject.GetComponent<MeshRenderer>().material.DisableKeyword("_EMISSION");
        }

        selectedNodes.Clear();
        highlightedEdge.Clear();
    }

    public void ToggleShowAllEdges(bool toggle)
    {
        showAllEdges = toggle;
        OnThresholdChange();
    }
    
    public void DropDownAlgChanged(TMP_Dropdown d)
    {
        switch (d.options[d.value].text)
        {
            case "Hops":
                shortestPathAlg = "Hops";
                break;
            case "Distance":
                shortestPathAlg = "Dist";
                break;
        }
    }
}