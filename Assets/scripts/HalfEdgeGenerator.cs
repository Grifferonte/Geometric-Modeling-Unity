// using System.Collections;
// using System.Collections.Generic;
// using UnityEditor;
// using UnityEngine;
// public class Quad
// {
//     public int[] indices = new int[4];    
//     public Quad(int[] indices) {
//         this.indices = indices;
//     }
// }

// public class Face
// {
//     public HalfEdge edge;

//     public Face(HalfEdge edge) {
//         this.edge = edge;
//     }
// }

// public class HalfEdge
// {
//     public Vector3 origin;
//     public HalfEdge twin;
//     public Face IncidentFace;
//     public HalfEdge next;
//     public HalfEdge prev;

//     public HalfEdge(Vector3 origin, HalfEdge twin, Face IncidentFace, HalfEdge next, HalfEdge prev) {
//         this.origin = origin;
//         this.twin = twin;
//         this.IncidentFace = IncidentFace;
//         this.next = next;
//         this.prev = prev;
//     }
// }

// public class HalfEdgeGenerator : MonoBehaviour
// {
//     [SerializeField] MeshFilter m_Mf;
//     [SerializeField] int size;

//     Mesh m_QuadMesh;

//     private void Awake()
//     {
//         if (!m_Mf) m_Mf = GetComponent<MeshFilter>();
//         m_QuadMesh = createPlan(new Vector3(4, 0, 2));
//         m_Mf.mesh = m_QuadMesh;
//     }

//     Mesh createPlan(Vector3 gridOffset) 
//     {
//         Mesh mesh = new Mesh();
//         Quad[] quads = new Quad[size*4];
//         Vertex[] vertices = new Vertex[(size+1)**2];

//         int index = 0;

//         for (int i = 0; i < size; i++)
//         {
//             for (int j = 0; j < size; j++)
//             {
//                 vertices[index] = new vector3(gridOffset);
//             }
                
//         }

//         for (int i = 0; i < quads.Length; i++)
//         {
//         }
        
//         // foreach (Quad quad in quads)
//         // {
//         //     quad.vertices[0] = new Vector3(size.x,0, size.z);
//         //     quad.vertices[1] = new Vector3(-size.x,0,size.z);
//         //     quad.vertices[2] = new Vector3(-size.x,0,-size.z);
//         //     quad.vertices[3] = new Vector3(size.x,0,-size.z);
//         //     size /= 2;
//         // }
        
//         // mesh.vertices = ;
//         // mesh.SetIndices(quads, MeshTopology.Quads, 0);

//         return mesh;
//     }

//     // Mesh CreateQuad(Vector3 size)
//     // {
//     //     Mesh mesh = new Mesh();

//     //     Vector3[] vertices = new Vector3[4];
//     //     int[] quads = new int[4];

//     //     Vector3 halfSize = size * .5f;

//     //     vertices[0] = new Vector3(halfSize.x, 0, halfSize.z);
//     //     vertices[1] = new Vector3(-halfSize.x, 0, halfSize.z);
//     //     vertices[2] = new Vector3(-halfSize.x, 0, -halfSize.z);
//     //     vertices[3] = new Vector3(+halfSize.x, 0, -halfSize.z);

//     //     quads[0] = 0;
//     //     quads[1] = 3;
//     //     quads[2] = 2;
//     //     quads[3] = 1;

//     //     mesh.vertices = vertices;
//     //     mesh.SetIndices(quads, MeshTopology.Quads, 0);

//     //     return mesh;
//     // }

//     private void OnDrawGizmos()
//     {
//         if (!m_QuadMesh) return;

//         GUIStyle guiStyle = new GUIStyle();
//         guiStyle.fontSize = 24;
//         guiStyle.normal.textColor = Color.red;

//         Vector3[] vertices = m_QuadMesh.vertices;
//         int[] quads = m_QuadMesh.GetIndices(0);

//         for (int i = 0; i < vertices.Length; i++)
//         {
//             Vector3 pos = transform.TransformPoint(vertices[i]);
//             Handles.Label(pos, new GUIContent(i.ToString()), guiStyle);
//         }

//         Gizmos.color = Color.black;

//         guiStyle.normal.textColor = Color.blue;
//         for (int i = 0; i < quads.Length; i += 4)
//         {
//             string str = (i / 4).ToString() + "(";
//             Vector3 centroidPos = Vector3.zero;
//             for (int j = 0; j < 4; j++)
//             {
//                 str += quads[i + j].ToString() + ((j < 3) ? "," : "");
//                 Vector3 pos = transform.TransformPoint(vertices[quads[i + j]]);
//                 Vector3 nextPos = transform.TransformPoint(vertices[quads[(i + j + 1) % 4]]);
//                 centroidPos += pos;
//                 Gizmos.DrawLine(pos, nextPos);
//             }
//             str += ")";
//             centroidPos *= .25f;

//             Handles.Label(centroidPos, new GUIContent(str), guiStyle);
//         }
//     }

//     // private void CreateHalfEdgeGenerator() {
//     //     HalfEdge[] halfEdges = new HalfEdge[8];

//     //     for (int i = 0; i < halfEdges.Length; i++)
//     //     {
//     //         halfEdges[i] = vertices[i];
//     //         halfEdges[i].twin;
//     //         halfEdges[i].IncidentFace;
//     //         halfEdges[i].next;
//     //         halfEdges[i].prev;
//     //     }

//     //     Face[] faces = new Face[4];

//     //     faces[0] = halfEdges[0];
//     // }
// }


