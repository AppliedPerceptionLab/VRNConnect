using System;
using System.Collections.Generic;
using UnityEngine;

public class ReadCSV : MonoBehaviour
{
    //private Graph<Vector3, float> graph;
    public Vector3 CenterOfMass = new Vector3(0.0f, 0.0f, 0.0f);
    public Vector3 scale = new Vector3(50.0f, 50.0f, 50.0f);
    public Vector3 rotation = new Vector3(90.0f, 0.0f, 0.0f);
    public Vector3 sphereSize = new Vector3(0.05f, 0.05f, 0.05f);
    public List<Node> nodes = new List<Node>();
    public Boolean showNodes = true;
    public Boolean showEdges = true;

    private Dictionary<string, GameObject> prefabs = new Dictionary<string, GameObject>();

    //[Tooltip("The Prefabs for each object")]
    //public List<GameObject> objects = new List<GameObject>();

    [Tooltip("The file that contains structural connectivity")]
    TextAsset brainSC;

    [Tooltip("The file that contains the reference atlas of the brain")]
    TextAsset atlas;

    [Tooltip("The file that contains the reference atlas of the brain")]
    TextAsset atlasColors;

    /*void awake()
    {
        foreach (var obj in objects)
        {
            prefabs[obj.name] = obj;
        }
    }*/

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
    }

    private void CreateGraph()
    {
        //graph = new Graph<Vector3, float>();

        /*var edge1 = new Edge<float, Vector3>()
        {
            Value = 1.0f,
            From = node1,
            To = node2,
            EdgeColor = Color.yellow
        };*/

        //TODO add nodes
        /*foreach (Nodes n in nodes)
        {
            Vector3 nd = new Vector3(n.xCog, n.yCog, n.zCog);
            //TODO get colors from list.ordered RGBA values
            System.Random rnd = new System.Random();
            Color randomColor = new Color(rnd.Next(255), rnd.Next(255), rnd.Next(255));
            var node = new Node<Vector3>() { Value = nd, NodeColor = randomColor };
            graph.Nodes.Add(node);
        }*/
        //TODO add edges
        //graph.Edges.Add(edge1);

        //Create the gameObject for each Node
        generateNodes();

        //Create the gameObject for each Edge
        generateEdges();
    }

    private void generateEdges()
    {
        Vector3 start, end, offset;
        for (int i = 0; i< nodes.Count && i<360; i++)
        {
            Node n = nodes[i];
            start = scaleVector3(new Vector3(n.xCog, n.yCog, n.zCog));
            for (int j = i+1; j<n.NodeConnection.Length && j < 360; j++)
            {
                
                float.TryParse(n.NodeConnection[j], out float strength);
                //Debug.Log(n.NodeConnection[j]);
                //if they have connection
                if (strength != 0.0f) {
                    //Debug.Log(i + ":" + j + "->" + strength) ;
                    Node e = nodes[j];
                    end = scaleVector3(new Vector3(e.xCog, e.yCog, e.zCog));
                    offset = end - start;
                    drawEdge(start,end,offset,strength, nodes[i].nodeId + ":" + nodes[j].nodeId);
                }
            }
            
        }
    }

    private void drawEdge(Vector3 start, Vector3 end, Vector3 offset, float strength, string name)
    {
        if (showEdges)
        {
            GameObject edge = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            edge.GetComponent<Renderer>().material.SetColor("_Color", Color.black);
            edge.name = name;
            edge.transform.position = start + (offset / 2.0f);
            //edge.transform.localScale = new Vector3(strength, offset.magnitude, strength);
            edge.transform.up = offset;
            edge.transform.localScale = new Vector3(0.001f, offset.magnitude / 2.0f, 0.001f);
            edge.transform.parent = gameObject.transform;
            //objects.Add(edge);
        }
    }

    private void generateNodes()
    {
        if (showNodes)
        {
            foreach (Node n in nodes)
            {
                GameObject temp;
                //remove regions > 360 -> cerebellum
                if (n.nodeId < 361)
                {
                    //temp = Instantiate(objects[n.nodeId%2]);
                    temp = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    temp.name = n.regionName;
                    temp.GetComponent<Renderer>().material.SetColor("_Color", new Color(n.Red / 255f, n.Green / 255f, n.Blue / 255f, n.Alpha / 255f));
                    temp.transform.localScale = sphereSize;
                    temp.transform.position = scaleVector3(new Vector3(n.xCog, n.yCog, n.zCog));
                    //make each node a child of the current GameObject
                    temp.transform.parent = gameObject.transform;
                    temp.transform.Rotate(rotation.x, rotation.y, rotation.z, Space.World);
                    //objects.Add(temp);
                }
                //Debug.Log(n.nodeId + " : " + n.regionName + " (" + n.xCog + "," + n.yCog + "," + n.zCog + ")" + n.NodeConnection.Length);
            }
        }
    }

    private void OnDrawGizmos()
    {

       /* if (graph == null)
        {
            Start();
        }

        Debug.Log("Drawing your gizmos" + graph.Nodes.Count);
        //Drawing Nodes
        foreach (var node in graph.Nodes)
        {

            Gizmos.color = node.NodeColor;
            //TODO: change node size
            Gizmos.DrawSphere(scaleVector3(node.Value), 0.125f);
        }*/

        //Drawing Edges
        /*foreach (var edge in graph.Edges)
        {
            Gizmos.color = edge.EdgeColor;
            Gizmos.DrawLine(edge.From.Value, edge.To.Value);
        }*/
    }

    private Vector3 scaleVector3(Vector3 Value)
    {
        return new Vector3(Value.x / scale.x, Value.y / scale.y, Value.z / scale.z);
    }

    // Update is called once per frame
    void Update()
    {
        /*foreach(GameObject obj in objects)
        {
            if(obj.GetType().Equals(PrimitiveType.Sphere))
            {
                obj.SetActive(showNodes);
            } else if (obj.GetType().Equals(PrimitiveType.Cylinder))
            {
                obj.SetActive(showEdges);
            }
        }*/
        //gameObject.transform.position = CenterOfMass;
    }
}
