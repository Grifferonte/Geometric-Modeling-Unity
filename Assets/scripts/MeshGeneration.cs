using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MeshGeneration : MonoBehaviour
{
    [SerializeField] MeshFilter m_Mf;
    [SerializeField] Vector3 gridSize;
    [SerializeField] Vector3 gridOffset;
    [SerializeField] Vector3 cellSize;
    [SerializeField] int numberVertices;

    [SerializeField] float radius;

    Mesh m_QuadMesh;

    private void Awake()
    {
        if (!m_Mf) m_Mf = GetComponent<MeshFilter>();
        m_QuadMesh = CreateRelugarPolygone();
        m_Mf.mesh = m_QuadMesh;
        Debug.Log(ExportMeshToCSV(m_QuadMesh));
    }

    Mesh CreateQuad()
    {
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[4];
        int[] quads = new int[4];

        Vector3 halfSize = cellSize * .5f;

        vertices[0] = new Vector3(-halfSize.x, 0, -halfSize.z) + gridOffset;
        vertices[1] = new Vector3(-halfSize.x, 0, halfSize.z) + gridOffset;
        vertices[2] = new Vector3(halfSize.x, 0, halfSize.z) + gridOffset;
        vertices[3] = new Vector3(+halfSize.x, 0, -halfSize.z) + gridOffset;

        quads[0] = 0;
        quads[1] = 1;
        quads[2] = 2;
        quads[3] = 3;

        mesh.vertices = vertices;
        mesh.SetIndices(quads, MeshTopology.Quads, 0);

        return mesh;
    }

    Mesh CreateGrid() {
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[((int)gridSize.x + 1) * ((int)gridSize.z + 1)];
        int[] quads = new int[(int)gridSize.x * (int)gridSize.z  * 4];

        Vector3 halfSize = cellSize * .5f;

        int f = 0;

        for (int x = 0; x <= gridSize.x; x++)
        {
            for (int z = 0; z <= gridSize.z; z++)
            {
                vertices[f] = new Vector3((x * cellSize.x) - halfSize.x , 0, (z * cellSize.z) - halfSize.z) + gridOffset;
                f++;
            }
        }

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

    Mesh CreateCube() {
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[8];
        int[] quads = new int[24];

        Vector3 halfSize = cellSize * .5f;

        vertices[0] = new Vector3(-halfSize.x, 0, -halfSize.z) + gridOffset;
        vertices[1] = new Vector3(-halfSize.x, 0, halfSize.z) + gridOffset;
        vertices[2] = new Vector3(halfSize.x, 0, halfSize.z) + gridOffset;
        vertices[3] = new Vector3(+halfSize.x, 0, -halfSize.z) + gridOffset;

        vertices[4] = new Vector3(-halfSize.x, halfSize.y, -halfSize.z) + gridOffset;
        vertices[5] = new Vector3(-halfSize.x, halfSize.y, halfSize.z) + gridOffset;
        vertices[6] = new Vector3(halfSize.x, halfSize.y, halfSize.z) + gridOffset;
        vertices[7] = new Vector3(+halfSize.x, halfSize.y, -halfSize.z) + gridOffset;

        //BOTTOM
        quads[0] = 0;
        quads[1] = 1;
        quads[2] = 2;
        quads[3] = 3;

        //RIGHT
        quads[4] = 3;
        quads[5] = 7;
        quads[6] = 6;
        quads[7] = 2;


        //TOP
        quads[8] = 4;
        quads[9] = 5;
        quads[10] = 6;
        quads[11] = 7;

        //BACK
        quads[12] = 2;
        quads[13] = 6;
        quads[14] = 5;
        quads[15] = 1;

        //LEFT
        quads[16] = 1;
        quads[17] = 5;
        quads[18] = 4;
        quads[19] = 0;

        //FRONT
        quads[20] = 0;
        quads[21] = 4;
        quads[22] = 7;
        quads[23] = 3;

        mesh.vertices = vertices;
        mesh.SetIndices(quads, MeshTopology.Quads, 0);

        return mesh;
    }

     Mesh CreateShip() {
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[8];
        int[] quads = new int[12];

        Vector3 halfSize = cellSize * .5f;

        vertices[0] = new Vector3(-halfSize.x, 0, -halfSize.z) + gridOffset;
        vertices[1] = new Vector3(-halfSize.x, 0, halfSize.z) + gridOffset;
        vertices[2] = new Vector3(halfSize.x, 0, halfSize.z) + gridOffset;
        vertices[3] = new Vector3(+halfSize.x, 0, -halfSize.z) + gridOffset;

        vertices[4] = new Vector3(-halfSize.x, halfSize.y, -halfSize.z) + gridOffset;
        vertices[5] = new Vector3(-halfSize.x, halfSize.y, halfSize.z) + gridOffset;
        vertices[6] = new Vector3(halfSize.x, halfSize.y, halfSize.z) + gridOffset;
        vertices[7] = new Vector3(+halfSize.x, halfSize.y, -halfSize.z) + gridOffset;

        //BOTTOM
        quads[0] = 0;
        quads[1] = 1;
        quads[2] = 2;
        quads[3] = 3;

        //TOP
        quads[4] = 4;
        quads[5] = 5;
        quads[6] = 6;
        quads[7] = 7;

        //LEFT
        quads[8] = 1;
        quads[9] = 5;
        quads[10] = 4;
        quads[11] = 0;

        mesh.vertices = vertices;
        mesh.SetIndices(quads, MeshTopology.Quads, 0);

        return mesh;
    }

    Mesh CreateRelugarPolygone() {
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[(numberVertices * 2 + 1)];
        int[] regularPolygone = new int[(numberVertices * 4)];

        float angle = 2 * Mathf.PI / numberVertices;

        vertices[0] = gridOffset;

        int h = 1; 
        for (int i = 0; i < numberVertices; i++)
        {
            if (h == numberVertices * 2 - 1) {
                vertices[h+1] = Vector3.Lerp(vertices[h], vertices[1], 0.5f) + gridOffset;
            }
            else {
                if (h == 1) {
                    vertices[h] = new Vector3(Mathf.Sin(i * angle), 0, Mathf.Cos(i*angle))*radius + gridOffset;
                }
                vertices[h+2] = new Vector3(Mathf.Sin((i+1) * angle), 0, Mathf.Cos((i+1)*angle))*radius + gridOffset;
                vertices[h+1] = Vector3.Lerp(vertices[h], vertices[h+2], 0.5f) + gridOffset;
            }
            h+=2;
        }          

        int k = 0;
        for (int i = 0; i < (numberVertices * 4); i+=4)
        {
            if (i == 0) {
                regularPolygone[0] = k;
                regularPolygone[i] = (numberVertices * 2);
                regularPolygone[i+1] = k+1;
                regularPolygone[i+2] = k+2;
            }
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

    private void OnDrawGizmos()
    {
        if (!m_QuadMesh) return;

        GUIStyle guiStyle = new GUIStyle();
        guiStyle.fontSize = 24;
        guiStyle.normal.textColor = Color.red;

        Vector3[] vertices = m_QuadMesh.vertices;
        int[] quads = m_QuadMesh.GetIndices(0);

        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 pos = transform.TransformPoint(vertices[i]);
            Handles.Label(pos, new GUIContent(i.ToString()), guiStyle);
        }

        Gizmos.color = Color.black;

        guiStyle.normal.textColor = Color.blue;
        for (int i = 0; i < quads.Length; i += 4)
        {
            string str = (i / 4).ToString() + "(";
            Vector3 centroidPos = Vector3.zero;
            for (int j = 0; j < 4; j++)
            {
                str += quads[i + j].ToString() + ((j < 3) ? "," : "");
                Vector3 pos = transform.TransformPoint(vertices[quads[i + j]]);
                Vector3 nextPos = transform.TransformPoint(vertices[quads[i + (j + 1) % 4]]);
                centroidPos += pos;
                Gizmos.DrawLine(pos, nextPos);
            }
            str += ")";
            centroidPos *= .25f;

            Handles.Label(centroidPos, new GUIContent(str), guiStyle);
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
