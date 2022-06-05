using System.Collections.Generic;
using UnityEngine;
public class GridGeneration : MonoBehaviour
{
    [SerializeField] MeshFilter m_Mf;

    [SerializeField] Vector3 gridOffset;

    [SerializeField] Vector3 gridSize;

    [SerializeField] Vector3 cellSize;

    [SerializeField] int numberSubdivison;
    Mesh m_QuadMesh;

    private void Awake()
    {
        if (!m_Mf) m_Mf = GetComponent<MeshFilter>();
        m_QuadMesh = CreateGrid();
    
        HalfEdgeManager HEM = new HalfEdgeManager(m_QuadMesh);
        //WingedEdgeManager WEM = new WingedEdgeManager(sphere.GetComponent<MeshFilter>().mesh);

        for (int i = 0; i <= numberSubdivison; i++)
        {
            HEM.subdivide();
        }

        m_Mf.mesh=HEM.output();

        m_QuadMesh=HEM.output();
    }

    Mesh CreateGrid() {
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[((int)gridSize.x + 1) * ((int)gridSize.z + 1)];
        int[] quads = new int[(int)gridSize.x * (int)gridSize.z  * 4];

        Vector3 halfSize = cellSize * .5f;

        int f = 0;

        //Vertices table filling

        for (int x = 0; x <= gridSize.x; x++)
        {
            for (int z = 0; z <= gridSize.z; z++)
            {
                vertices[f] = new Vector3((x * cellSize.x) - halfSize.x , 0, (z * cellSize.z) - halfSize.z) + gridOffset;
                f++;
            }
        }

        //Quads table filling 
        //Assign each point to face vertices

        int h=0;
        int p = 0;

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int z = 0; z < gridSize.z; z++)
            {
                quads[h] = p;
                quads[h+1] = p+1;
                quads[h+2] = p+((int)gridSize.z + 1)+1;
                quads[h+3] = p+((int)gridSize.x + 1);
                p++;
                h+=4;
            }
            p++;
        }

        mesh.vertices = vertices;
        mesh.SetIndices(quads, MeshTopology.Quads, 0);

        return mesh;
    }

    //Draw Lines between each vertex

    private void OnDrawGizmos()
    {
        if (!m_QuadMesh) return;

        GUIStyle guiStyle = new GUIStyle();
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

        guiStyle.normal.textColor = Color.blue;
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

    //Export mesh to csv

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
