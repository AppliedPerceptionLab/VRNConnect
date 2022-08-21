using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ReadCSV : MonoBehaviour
{
    //private Graph<Vector3, float> graph;
    //public Vector3 CenterOfMass = new Vector3(0.0f, 0.0f, 0.0f);
    public Vector3 scale = new Vector3(50.0f, 50.0f, 50.0f);
    public Vector3 rotate = new Vector3(90.0f, 0.0f, 0.0f);
    public Vector3 sphereSize = new Vector3(0.05f, 0.05f, 0.05f);
    public static List<Node> nodes = new List<Node>();
    public static List<Edge> edges = new List<Edge>();
    public Boolean showAllNodes = true;
    public Boolean showAllEdges = false;
    public Boolean edgeColoring = false;
    public Boolean shadowEffect = false;
    private float maxEdgeSize = 0f;
    public float threshold = 0.1f;
    public Color defaultColor = Color.black;
    private string graphResTooltipText = "";
    /*private float weiAss;
    private float binAss;
    private float minDist;
    private float maxDist;*/

    private Dictionary<string, GameObject> prefabs = new Dictionary<string, GameObject>();

    //[Tooltip("The Prefabs for each object")]
    //public List<GameObject> objects = new List<GameObject>();

    [Tooltip("The file that contains structural connectivity")]
    TextAsset brainSC;

    [Tooltip("The file that contains the reference atlas of the brain")]
    TextAsset atlas;

    [Tooltip("The file that contains the reference atlas of the brain")]
    TextAsset atlasColors;

    [Tooltip("The file that contains algorithm results")]
    TextAsset algResults;

    [Tooltip("The file that contains shortest distances")]
    TextAsset distFile;

    [Tooltip("The file that contains clustering coefficients")]
    TextAsset clusteringCoef;

    // Start is called before the first frame update
    void Start()
    {
        brainSC = Resources.Load<TextAsset>("hcpmmp1");
        atlas = Resources.Load<TextAsset>("HCP-MMP1_UniqueRegionList");
        atlasColors = Resources.Load<TextAsset>("HCP-MMP1_RegionColor");

        string[] Node = brainSC.text.Split(new char[] { '\n' });
        string[] AtlasNode = atlas.text.Split(new char[] { '\n' });
        string[] AtlasNodeColor = atlasColors.text.Split(new char[] { '\n' });
        // Debug.Log(Node.Length);

        for(int i=0; i < Node.Length-1 ; i++)
        {
            string[] Node2 = Node[i].Split(new char[] { ' ' });
            Node n = new Node();
            n.nodeId = i + 1;
            n.NodeConnection = Node2;
            nodes.Add(n);
        }
        for(int i=1; i < AtlasNode.Length-1 ; i++)
        {
            string[] atlasRow = AtlasNode[i].Split(new char[] { ',' });
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
        for(int i=1; i<AtlasNodeColor.Length-1; i++)
        {
            string[] colorRow = AtlasNodeColor[i].Split(new char[] { ',' });
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
        gameObject.transform.Rotate(-90f , -90f , 90f , Space.World);
        gameObject.transform.Translate (new Vector3(0f,-0.4f,4f) , Space.World);
    }

    private Vector3 FindCenterOfMass()
    {
        Vector3 com = Vector3.zero;
        foreach (Node n in nodes)
        {
            //remove regions > 360 -> cerebellum
            if (n.nodeId < 361)
            {
                com.x += n.xCog/360;
                com.y += n.yCog/360;
                com.z += n.zCog/360;
            }
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
        GameObject sampleNode = GameObject.Find("Node");
        foreach (Node n in nodes)
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
                temp.GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(n.Red / 255f, n.Green / 255f, n.Blue / 255f, n.Alpha / 255f));
                //temp.GetComponent<MeshRenderer>().receiveShadows = false;
                temp.transform.localScale = getSphereSize();
                temp.transform.position = scaleVector3(new Vector3(n.xCog, n.yCog, n.zCog));
                //make each node a child of the current GameObject
                temp.transform.parent = gameObject.transform;
                //temp.transform.Rotate(rotation.x, rotation.y, rotation.z, Space.World);
                //objects.Add(temp);
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
        GameObject sampleEdge = GameObject.Find("Edge");
        int id = 1;
        for (int i = 0; i < nodes.Count && i < 360; i++)
        {
            Node n = nodes[i];
            start = scaleVector3(new Vector3(n.xCog, n.yCog, n.zCog));
            for (int j = i + 1; j < n.NodeConnection.Length && j < 360; j++)
            {

                float.TryParse(n.NodeConnection[j], out float strength);
                //Debug.Log(n.NodeConnection[j]);
                //if they have connection
                if (strength != 0.0f)
                {
                    //Debug.Log(i + ":" + j + "->" + strength) ;
                    Node e = nodes[j];
                    end = scaleVector3(new Vector3(e.xCog, e.yCog, e.zCog));
                    offset = end - start;
                    Edge edge = new Edge();
                    edge.edgeId = id++;
                    edge.node1Id = n.nodeId;
                    edge.node2Id = e.nodeId;
                    Color color1 = new Color(n.Red / 255f, n.Green / 255f, n.Blue / 255f, n.Alpha / 255f);
                    Color color2 = new Color(e.Red / 255f, e.Green / 255f, e.Blue / 255f, e.Alpha / 255f);
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
        for (int i = 0; i < nodes.Count && i < 360; i++)
        {
            Node n = nodes[i];
            nodes[i].nodeDegree = (int)(calculateNodeDegree(n, true));
            nodes[i].nodeStrength = calculateNodeDegree(n, false);
            EnableEdgeOfNode(n.regionName, showAllEdges);
        }
    }

    private float calculateNodeDegree(Node n, bool isNodedegree)
    {
        float res = 0;
        foreach(string con in n.NodeConnection)
        {
            float.TryParse(con, out float strength);
            if (strength >= threshold * maxEdgeSize)
            {
                if (isNodedegree)
                {
                    res++;
                } else
                {
                    res += strength;
                }
            }
        }
        return res;
    }

    private void drawEdge(Edge edge, GameObject sampleEdge)
    {
        //GameObject edge = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        GameObject edgeObj = Instantiate(sampleEdge);
        //edge.GetComponent<MeshRenderer>().material = Resources.Load("Materials/SphereR.mat", typeof(Material)) as Material;
        //Using a material from assets with GPU instancing on
        edgeObj.SetActive(true);
        edgeObj.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/EdgeR");
        edgeObj.GetComponent<MeshRenderer>().material.SetColor("_Color", defaultColor);
        edgeObj.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        edgeObj.GetComponent<MeshRenderer>().receiveShadows = false;
        edgeObj.name = edge.name;
        edgeObj.transform.position = edge.start + (edge.offset / 2.0f);
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
        if (strength >= threshold * maxEdgeSize)
        {
            return (strength * 0.04f) / maxEdgeSize;
        }
        
        return 0f;
    }

    private float FindMax()
    {
        float max = 0f;
        foreach(Edge e in edges)
        {
            if(max < e.strenght)
            {
                max = e.strenght;
            }
        }
        Debug.Log("Max =" + max);
        return max;
    }

    public string GetTooltipTextForNode(string name)
    {
        String tooltipText = "";
        Node node = FindNode(name);
        //NodeID found
        if (node != null)
        {
            tooltipText = node.GetTooltipText();
            //tooltipText = node.ToString();
        }
        return tooltipText;
    }

    public void EnableEdgeOfNode(string nodeName,bool enable)
    {
        Debug.Log(nodeName + enable);
        int nodeID = FindNodeID(nodeName);
        //NodeID found
        if(nodeID != -1)
        {
            foreach (Edge e in edges)
            {
                if (e.node1Id == nodeID || e.node2Id == nodeID)
                {
                    float width = CalculateWidth(e.strenght);
                    if (width > 0f)
                    {
                        e.gameObject.transform.localScale = new Vector3(width, e.offset.magnitude / 2.0f, width);
                        if(showAllEdges)
                        {
                            if (edgeColoring)
                            {
                                e.gameObject.GetComponent<MeshRenderer>().material.SetColor("_Color", e.edgeColor);
                            }
                            else
                            {
                                e.gameObject.GetComponent<MeshRenderer>().material.SetColor("_Color", defaultColor);
                            }
                        } else
                        {
                            if (enable)
                            {
                                e.gameObject.GetComponent<MeshRenderer>().material.SetColor("_Color", e.edgeColor);
                                if (shadowEffect)
                                {
                                    e.gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                                }
                            }
                            else
                            {
                                e.gameObject.GetComponent<MeshRenderer>().material.SetColor("_Color", defaultColor);
                                e.gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                            }
                        }
                        
                        e.gameObject.SetActive(enable || showAllEdges);
                    } else
                    {
                        e.gameObject.SetActive(false); //Destroy gameObject
                    }
                }
            }
        }
    }

    private Node FindNode(string nodeName)
    {
        foreach (Node n in nodes)
        {
            if(n.regionName.Equals(nodeName))
            {
                return n;
            }
        }

        //Node not found
        return null;
    }

    private int FindNodeID(string nodeName)
    {
        foreach (Node n in nodes)
        {
            if (n.regionName.Equals(nodeName))
            {
                return n.nodeId;
            }
        }

        //Node not found
        return -1;
    }

    private Color CreateGradient(Color color1, Color color2)
    {
        GradientColorKey[] colorKey;
        GradientAlphaKey[] alphaKey;

        
         Gradient gradient = new Gradient();

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

        string[] results = algResults.text.Split(new char[] { '\n' });
        //string[] AtlasNode = atlas.text.Split(new char[] { '\n' });
        string[] clustCoef = clusteringCoef.text.Split(new char[] { '\n' });

        for (int i = 1; i < clustCoef.Length - 1; i++)
        {
            string[] Row = clustCoef[i].Split(new char[] { ',' });
            int nodeID = 0;
            int.TryParse(Row[0], out nodeID);
            float.TryParse(Row[1], out nodes[nodeID].clusteringCoef);
        }

        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < results.Length - 1; i++)
        {
            if(i==0)
            {
                sb.AppendLine(results[i]);
                sb.AppendLine("***********************");
            } 
            else
            {
                string[] Row = results[i].Split(new char[] { ',' });
                sb.AppendLine(Row[0] + ":\t" + Row[1] + "\t" + Row[2]);
            }
        }
        graphResTooltipText = sb.ToString();
    }
    
    public void OnThresholdChange()
    {
        for (int i = 0; i < nodes.Count && i < 360; i++)
        {
            Node n = nodes[i];
            nodes[i].nodeDegree = (int)(calculateNodeDegree(n, true));
            nodes[i].nodeStrength = calculateNodeDegree(n, false);
            EnableEdgeOfNode(n.regionName, showAllEdges);
        }
    }

    private void RunPythonScript()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
