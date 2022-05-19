using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HalfEdgeManager
{
    public Dictionary<int, HE> dicoHE;
    HE HE1,HE2,HE3,HE4;

    public HalfEdgeManager(Mesh inputMesh)
    {
        dicoHE = new Dictionary<int, HE>();
        var lT = inputMesh.triangles;
        var lV = inputMesh.vertices;

        int edgeCount=0;
        for (int i=0;i<lT.Length;i+=6)
        {
            HE_Face HEF;
            HEF.startingEdge=edgeCount;

            HE_vertex HEV1,HEV2,HEV3,HEV4;
            HEV1.pos=lV[lT[i]];
            HEV1.keyIncidentEdge=edgeCount;
            HEV2.pos=lV[lT[i+1]];
            HEV2.keyIncidentEdge=edgeCount+1;
            HEV3.pos=lV[lT[i+2]];
            HEV3.keyIncidentEdge=edgeCount+2;
            HEV4.pos=lV[lT[i+5]];
            HEV4.keyIncidentEdge=edgeCount+3;

            HE1.originVertex=HEV1;
            HE1.keyNextEdge=edgeCount+1;
            HE1.keyPrevEdge=edgeCount+3;
            HE1.incidentFace=HEF;

            HE2.originVertex=HEV2;
            HE2.keyNextEdge=edgeCount+2;
            HE2.keyPrevEdge=edgeCount;
            HE2.incidentFace=HEF;

            HE3.originVertex=HEV3;
            HE3.keyNextEdge=edgeCount+3;
            HE3.keyPrevEdge=edgeCount+1;
            HE3.incidentFace=HEF;

            HE4.originVertex=HEV4;
            HE4.keyNextEdge=edgeCount;
            HE4.keyPrevEdge=edgeCount+2;
            HE4.incidentFace=HEF;
            
            dicoHE.Add(edgeCount, HE1);
            dicoHE.Add(edgeCount+1, HE2);
            dicoHE.Add(edgeCount+2, HE3);
            dicoHE.Add(edgeCount+3, HE4);
            edgeCount+=4;
        }

        for (int i=0;i<dicoHE.Count;i++)
        {
            for (int j=0;j<dicoHE.Count;j++)
            {
                if(dicoHE[i].originVertex.pos==next(dicoHE[j]).originVertex.pos && next(dicoHE[i]).originVertex.pos==dicoHE[j].originVertex.pos)
                {
                    HE toedit = dicoHE[i];
                    toedit.keyTwinEdge=j;
                    dicoHE[i]=toedit;
                }   
            }
        }
    }
    
    public HE next(HE currentEdge) {return dicoHE[currentEdge.keyNextEdge];}
    public HE prev(HE currentEdge) {return dicoHE[currentEdge.keyPrevEdge];}
    public HE twin(HE currentEdge) {return dicoHE[currentEdge.keyTwinEdge];}

    public Mesh output()
    {
    Mesh mesh = new Mesh();

    int edgeCount = dicoHE.Count;
        List<Vector3> verts = new List<Vector3>();
        List<int> quads = new List<int>();

        Debug.LogWarning(edgeCount);

        for (int i =0; i<edgeCount;i++)
        {
            if(!verts.Contains(dicoHE[i].originVertex.pos))
            {
                verts.Add(dicoHE[i].originVertex.pos);
            }
            int index = verts.IndexOf(dicoHE[i].originVertex.pos);
            quads.Add(index);
        }

         Debug.LogWarning(verts.Count);
          Debug.LogWarning(quads.Count);            
        mesh.vertices = verts.ToArray();
        mesh.SetIndices(quads.ToArray(), MeshTopology.Quads, 0);

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
        return mesh;
    }

    }

public struct HE_vertex{

    public Vector3 pos;
    public int keyIncidentEdge;
}

public struct HE{

    public HE_vertex originVertex;
    public int keyTwinEdge;
    public int keyNextEdge;
    public int keyPrevEdge;

    public HE_Face incidentFace;
}

public struct HE_Face{

    public int startingEdge;
}