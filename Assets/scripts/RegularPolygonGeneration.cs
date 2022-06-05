using System.Collections.Generic;
using UnityEngine;
public class RegularPolygonGeneration : MonoBehaviour
{
    [SerializeField] MeshFilter m_Mf;

    [SerializeField] Vector3 gridOffset;
    [SerializeField] int numberVertices;

    [SerializeField] float radius;

    [SerializeField] int numberSubdivison;
    Mesh m_QuadMesh;

    private void Awake()
    {
        if (!m_Mf) m_Mf = GetComponent<MeshFilter>();
        m_QuadMesh = CreateRelugarPolygone();
    
        HalfEdgeManager HEM = new HalfEdgeManager(m_QuadMesh);
        //WingedEdgeManager WEM = new WingedEdgeManager(sphere.GetComponent<MeshFilter>().mesh);

        for (int i = 0; i <= numberSubdivison; i++)
        {
            HEM.subdivide();
        }

        m_Mf.mesh=HEM.output();

        m_QuadMesh=HEM.output();
    }

    Mesh CreateRelugarPolygone() {
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[(numberVertices * 2 + 1)];
        int[] regularPolygone = new int[(numberVertices * 4)];

        float angle = 2 * Mathf.PI / numberVertices;

        //Vertices table filling

        //Center point
        vertices[0] = gridOffset;

        int h = 1; 
        for (int i = 0; i < numberVertices; i++)
        {
            if (h == numberVertices * 2 - 1) {
                //Last vertex
                vertices[h+1] = Vector3.Lerp(vertices[h], vertices[1], 0.5f);
            }
            else {
                if (h == 1) {
                    //First vertex
                    vertices[h] = new Vector3(Mathf.Sin(i*angle), 0, Mathf.Cos(i*angle))*radius + gridOffset;
                }
                //Following vertex
                vertices[h+2] = new Vector3(Mathf.Sin((i+1)*angle), 0, Mathf.Cos((i+1)*angle))*radius + gridOffset;
                //Center of the 2 vertices
                vertices[h+1] = Vector3.Lerp(vertices[h], vertices[h+2], 0.5f);
            }
            h+=2;
        }

        //Quads table filling 
        //Assign each point to face vertices          

        int k = 0;
        for (int i = 0; i < (numberVertices * 4); i+=4)
        {
            //First quad
            if (i == 0) {
                regularPolygone[0] = k;
                regularPolygone[i] = (numberVertices * 2);
                regularPolygone[i+1] = k+1;
                regularPolygone[i+2] = k+2;
            }
            //Others quads
            else {
                regularPolygone[i] = 0;
                regularPolygone[i+1] = k;
                regularPolygone[i+2] = k+1;
                regularPolygone[i+3] = k+2;
            }
            k+=2;
        }

        mesh.vertices = vertices;
        mesh.SetIndices(regularPolygone, MeshTopology.Quads, 0);

        return mesh;
    }

    //Draw Lines between each vertex

    private void OnDrawGizmos()
    {
        if (!m_QuadMesh) return;

        //GUIStyle guiStyle = new GUIStyle();
        // guiStyle.fontSize = 24;
        // guiStyle.normal.textColor = Color.red;

        Vector3[] vertices = m_QuadMesh.vertices;
        int[] quads = m_QuadMesh.GetIndices(0);

        // for (int i = 0; i < vertices.Length; i++)
        // {
        //     Vector3 pos = transform.TransformPoint(vertices[i]);
        //     Handles.Label(pos, new GUIContent(i.ToString()), guiStyle);
        // }

        Gizmos.color = Color.black;

        //guiStyle.normal.textColor = Color.blue;
        for (int i = 0; i < quads.Length; i += 4)
        {
            // string str = (i / 4).ToString() + "(";
            // Vector3 centroidPos = Vector3.zero;
            for (int j = 0; j < 4; j++)
            {
                // str += quads[i + j].ToString() + ((j < 3) ? "," : "");
                Vector3 pos = transform.TransformPoint(vertices[quads[i + j]]);
                Vector3 nextPos = transform.TransformPoint(vertices[quads[i + (j + 1) % 4]]);
                // centroidPos += pos;
                Gizmos.DrawLine(pos, nextPos);
            }
            // str += ")";
            // centroidPos *= .25f;

            // Handles.Label(centroidPos, new GUIContent(str), guiStyle);
        }
    }

    string ExportMeshToCSV(Mesh mesh)
    {
        List<string> lines = new List<string>();
        lines.Add("Vertex Table \t\t\t\tQuads table");

        Vector3[] vertices = mesh.vertices;
        int[] quads = mesh.GetIndices(0);

        int nLines = Mathf.Max(vertices.Length, quads.Length / 4);
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 vertex = vertices[i];
            lines.Add($"{i}\t{vertex.x:N2}\t{vertex.y:N2}\t{vertex.z:N2}");
        }
        for (int i = 0; i < nLines - vertices.Length; i++)
        {
            lines.Add("\t\t\t\t");
        }
        for (int i = 0; i < quads.Length / 4; i++)
        {
            lines[i + 1] += $"{i}\t{quads[4 * i]}\t{quads[4 * i + 1]}\t{quads[4 * i + 2]}\t{quads[4 * i + 3]}";
        }

        return string.Join("\n", lines);
    }
}
