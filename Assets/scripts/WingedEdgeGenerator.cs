// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;


// public class Vertex
// {
//     public int index;
//     public Vector3 pos;

//     public Vertex(int index, Vector3 pos)
//     {
//         this.index = index;
//         this.pos = pos;
//     }
// }

// public class Face
// {
//     public int index;
//     //public WingedEdge edge;

//     //public Face(int index, WingedEdge edge)
//     //{
//     //    this.index = index;
//     //    this.edge = edge;
//     //}

//     public Face(int index)
//     {
//         this.index = index;
//     }
// }


// public class WingedEdge
// {
//     public int index;

//     public Vertex startVertex;
//     public Vertex endVertex;

//     public WingedEdge startCW;
//     public WingedEdge startCCW;

//     public WingedEdge endCW;
//     public WingedEdge endCCW;

//     public Face rightFace;
//     public Face leftFace;

//     public WingedEdge(int index, Vertex startVertex, Vertex endVertex, WingedEdge startCW, WingedEdge startCCW, WingedEdge endCW, WingedEdge endCCW, Face rightFace, Face leftFace)
//     {
//         this.index = index;
//         this.startVertex = startVertex;
//         this.endVertex = endVertex;
//         this.startCW = startCW;
//         this.startCCW = startCCW;
//         this.endCW = endCW;
//         this.endCCW = endCCW;
//         this.rightFace = rightFace;
//         this.leftFace = leftFace;
//     }

// }


// public class WingedEdgeMesh
// {
//     public List<Vertex> vertices;
//     public List<WingedEdge> edges;
//     public List<Face> faces;

//     public WingedEdgeMesh(Mesh mesh)
//     {
//         Vector3[] vfVertices = mesh.vertices;
//         int[] vfQuads = mesh.GetIndices(0);

//         //vertices
//         vertices = new List<Vertex>();
//         for (int i = 0; i < vfVertices.Length; i++)
//         {
//             vertices.Add(new Vertex(i, vfVertices[i]));
//         }

//         //faces
//         for(int i = 0;  i < vfQuads.Length/4; i++)
//         {
//             Face face = new Face(i);

//             //edges
//             for(int j = 0; j< 4; j++)
//             {
//                 int index1 = vfQuads[4 * i + j];
//                 int index2 = vfQuads[4 * i + (j + 1) % 4];

//                 // WingedEdge edge = new WingedEdge(i, );
//                 // edges.Add(edge);
//             }
//         }

//     }

// }
