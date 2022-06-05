using System.Collections.Generic;
using UnityEngine;


public class HalfEdgeManager
{
    public Dictionary<int, HE> dicoHE;
    HE HE1, HE2, HE3, HE4;

    public HalfEdgeManager(Mesh inputMesh)
    {
        dicoHE = new Dictionary<int, HE>();
        var lT = inputMesh.triangles;
        var lV = inputMesh.vertices;

        int edgeCount = 0;
        int faceCount = 0;
        for (int i = 0; i < lT.Length; i += 6)
        {
            HE_Face HEF;
            HEF.startingEdge = edgeCount;
            HEF.id = faceCount;
            faceCount++;

            HE_vertex HEV1, HEV2, HEV3, HEV4;
            HEV1.pos = lV[lT[i]];
            HEV1.keyIncidentEdge = edgeCount;
            HEV2.pos = lV[lT[i + 1]];
            HEV2.keyIncidentEdge = edgeCount + 1;
            HEV3.pos = lV[lT[i + 2]];
            HEV3.keyIncidentEdge = edgeCount + 2;
            HEV4.pos = lV[lT[i + 5]];
            HEV4.keyIncidentEdge = edgeCount + 3;

            HE1.originVertex = HEV1;
            HE1.keyTwinEdge=-1;
            HE1.keyNextEdge = edgeCount + 1;
            HE1.keyPrevEdge = edgeCount + 3;
            HE1.incidentFace = HEF;

            HE2.originVertex = HEV2;
            HE2.keyTwinEdge=-1;
            HE2.keyNextEdge = edgeCount + 2;
            HE2.keyPrevEdge = edgeCount;
            HE2.incidentFace = HEF;

            HE3.originVertex = HEV3;
            HE3.keyTwinEdge=-1;
            HE3.keyNextEdge = edgeCount + 3;
            HE3.keyPrevEdge = edgeCount + 1;
            HE3.incidentFace = HEF;

            HE4.originVertex = HEV4;
            HE4.keyTwinEdge=-1;
            HE4.keyNextEdge = edgeCount;
            HE4.keyPrevEdge = edgeCount + 2;
            HE4.incidentFace = HEF;

            dicoHE.Add(edgeCount, HE1);
            dicoHE.Add(edgeCount + 1, HE2);
            dicoHE.Add(edgeCount + 2, HE3);
            dicoHE.Add(edgeCount + 3, HE4);
            edgeCount += 4;
        }

        for (int i = 0; i < dicoHE.Count; i++)
        {
            for (int j = 0; j < dicoHE.Count; j++)
            {
                if (dicoHE[i].originVertex.pos == dicoHE[j].originVertex.pos && next(dicoHE[i]).originVertex.pos == next(dicoHE[j]).originVertex.pos && i!=j)
                {
                    var e1 = dicoHE[i];
                    var e2 = next(e1);
                    var e3 = next(e2);
                    var e4 = next(e3);
                    
                    HE[] toSwap={e1,e2,e2,e4};

                    for(int q=0;q<4;q++)
                    {
                        HE toedit = dicoHE[q];
                        toedit.keyNextEdge = dicoHE[q].keyPrevEdge;
                        toedit.keyPrevEdge = dicoHE[q].keyNextEdge;
                        dicoHE[i+q] = toedit;
                    }
                    
                }
            }
        }

        for (int i = 0; i < dicoHE.Count; i++)
        {
            
            for (int j = 0; j < dicoHE.Count; j++)
            {
                if (dicoHE[i].originVertex.pos == next(dicoHE[j]).originVertex.pos && next(dicoHE[i]).originVertex.pos == dicoHE[j].originVertex.pos)
                {
                    HE toedit = dicoHE[i];
                    toedit.keyTwinEdge = j;
                    dicoHE[i] = toedit;
                }
            }
        }
    }

    public HE next(HE currentEdge) { return dicoHE[currentEdge.keyNextEdge]; }
    public HE prev(HE currentEdge) { return dicoHE[currentEdge.keyPrevEdge]; }
    public HE twin(HE currentEdge) { return dicoHE[currentEdge.keyTwinEdge]; }

    public Mesh output()
    {
        Mesh mesh = new Mesh();

        int edgeCount = dicoHE.Count;
        List<Vector3> verts = new List<Vector3>();
        List<int> quads = new List<int>();

        for (int i = 0; i < edgeCount; i++)
        {
            if (!verts.Contains(dicoHE[i].originVertex.pos))
            {
                verts.Add(dicoHE[i].originVertex.pos);
            }
            int index = verts.IndexOf(dicoHE[i].originVertex.pos);
            quads.Add(index);
        }
        mesh.vertices = verts.ToArray();
        mesh.SetIndices(quads.ToArray(), MeshTopology.Quads, 0);

        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
        return mesh;
    }

    public void subdivide()
    {
        List<int> exclude = new List<int>();
        Dictionary<int, HE> subDicoHE = new Dictionary<int, HE>();

        List<Vector3> midPointList = new List<Vector3>();

        List<Vector3> cornersList = new List<Vector3>();

        List<Vector3> edgePointList = new List<Vector3>();

        for (int i = 0; i < dicoHE.Count; i++)
        {
            if (!cornersList.Contains(dicoHE[i].originVertex.pos)) cornersList.Add(dicoHE[i].originVertex.pos);

        }


        int idcount = 0;
        for (int i = 0; i < dicoHE.Count; i++)
        {
            if (!exclude.Contains(dicoHE[i].incidentFace.id))
            {
                var e1 = dicoHE[i];
                var e2 = next(e1);
                var e3 = next(e2);
                var e4 = next(e3);

                Vector3 centerVert = (e1.originVertex.pos + e2.originVertex.pos + e3.originVertex.pos + e4.originVertex.pos) / 4.0f;
                Vector3 mid1 = (e1.originVertex.pos + e2.originVertex.pos) / 2;
                Vector3 mid2 = (e2.originVertex.pos + e3.originVertex.pos) / 2;
                Vector3 mid3 = (e3.originVertex.pos + e4.originVertex.pos) / 2;
                Vector3 mid4 = (e4.originVertex.pos + e1.originVertex.pos) / 2;
                midPointList.Add(mid1);
                midPointList.Add(mid2);
                midPointList.Add(mid3);
                midPointList.Add(mid4);

                List<Vector3> l1 = new List<Vector3> { mid4, e1.originVertex.pos, mid1, centerVert };
                List<Vector3> l2 = new List<Vector3> { mid1, e2.originVertex.pos, mid2, centerVert };
                List<Vector3> l3 = new List<Vector3> { mid2, e3.originVertex.pos, mid3, centerVert };
                List<Vector3> l4 = new List<Vector3> { mid3, e4.originVertex.pos, mid4, centerVert };

                List<List<Vector3>> bL = new List<List<Vector3>> { l1, l2, l3, l4 };

                for (int j = 0; j < 4; j++)
                {
                    var cl = bL[j];

                    HE_Face HEF;
                    HEF.startingEdge = subDicoHE.Count / 4;
                    HEF.id = idcount;
                    idcount++;

                    HE_vertex HEV1, HEV2, HEV3, HEV4;

                    var count = subDicoHE.Count;

                    HEV1.pos = cl[0];
                    HEV1.keyIncidentEdge = count;
                    HEV2.pos = cl[1];
                    HEV2.keyIncidentEdge = count + 1;
                    HEV3.pos = cl[2];
                    HEV3.keyIncidentEdge = count + 2;
                    HEV4.pos = cl[3];
                    HEV4.keyIncidentEdge = count + 3;

                    HE1.originVertex = HEV1;
                    HE1.keyNextEdge = count + 1;
                    HE1.keyPrevEdge = count + 3;
                    HE1.incidentFace = HEF;

                    HE2.originVertex = HEV2;
                    HE2.keyNextEdge = count + 2;
                    HE2.keyPrevEdge = count;
                    HE2.incidentFace = HEF;

                    HE3.originVertex = HEV3;
                    HE3.keyNextEdge = count + 3;
                    HE3.keyPrevEdge = count + 1;
                    HE3.incidentFace = HEF;

                    HE4.originVertex = HEV4;
                    HE4.keyNextEdge = count;
                    HE4.keyPrevEdge = count + 2;
                    HE4.incidentFace = HEF;

                    subDicoHE.Add(count, HE1);
                    subDicoHE.Add(count + 1, HE2);
                    subDicoHE.Add(count + 2, HE3);
                    subDicoHE.Add(count + 3, HE4);
                }
                exclude.Add(dicoHE[i].incidentFace.id);
            }

        }

        dicoHE = subDicoHE;

        for (int i = 0; i < dicoHE.Count; i++)
        {
            for (int j = 0; j < dicoHE.Count; j++)
            {
                if (dicoHE[i].originVertex.pos == next(dicoHE[j]).originVertex.pos && next(dicoHE[i]).originVertex.pos == dicoHE[j].originVertex.pos)
                {
                    HE toedit = dicoHE[i];
                    toedit.keyTwinEdge = j;
                    dicoHE[i] = toedit;
                }
            }
        }

        List<Vector3> prevPos = new List<Vector3>();
        List<Vector3> nextPos = new List<Vector3>();

        for (int i = 0; i < dicoHE.Count; i++)
        {

            var e1 = next(dicoHE[i]);
            var e2 = prev(dicoHE[i]);

            List<Vector3> l = new List<Vector3>();
            List<Vector3> opposite = new List<Vector3>();

            for (int j = 0; j < dicoHE.Count; j++)
            {
                if(prevPos.Contains(dicoHE[i].originVertex.pos))break;
                
                if (dicoHE[j].originVertex.pos==dicoHE[i].originVertex.pos && i!=j)
                {
                    if(!l.Contains(dicoHE[j].originVertex.pos) && dicoHE[i].originVertex.pos!=dicoHE[j].originVertex.pos) l.Add(dicoHE[j].originVertex.pos);
                    if(!opposite.Contains(next(dicoHE[j]).originVertex.pos) && dicoHE[i].originVertex.pos!=dicoHE[j].originVertex.pos) opposite.Add(next(dicoHE[j]).originVertex.pos);
                }

                if (dicoHE[dicoHE[j].keyNextEdge].originVertex.pos==dicoHE[i].originVertex.pos && i!=j )
                {
                    if(!l.Contains(dicoHE[j].originVertex.pos) && dicoHE[i].originVertex.pos!=dicoHE[j].originVertex.pos) l.Add(dicoHE[j].originVertex.pos);
                    if(!opposite.Contains(next(dicoHE[j]).originVertex.pos) && dicoHE[i].originVertex.pos!=dicoHE[j].originVertex.pos) opposite.Add(next(dicoHE[j]).originVertex.pos);
                }

                if (dicoHE[dicoHE[j].keyPrevEdge].originVertex.pos==dicoHE[i].originVertex.pos && i!=j)
                {
                    if(!l.Contains(dicoHE[j].originVertex.pos) && dicoHE[i].originVertex.pos!=dicoHE[j].originVertex.pos) l.Add(dicoHE[j].originVertex.pos);
                    if(!opposite.Contains(next(dicoHE[j]).originVertex.pos) && dicoHE[i].originVertex.pos!=dicoHE[j].originVertex.pos) opposite.Add(next(dicoHE[j]).originVertex.pos);
                }
            }
        

            if(l.Count==2)
            {
                prevPos.Add(dicoHE[i].originVertex.pos);
                nextPos.Add((l[0]+l[1])/2);
            }

            if(l.Count==3)
            {
                bool border=false;

                for(int q=0;q<dicoHE.Count;q++)
                {
                    if(dicoHE[q].originVertex.pos==dicoHE[i].originVertex.pos)
                    {
                        if(dicoHE[q].keyTwinEdge==-1)border=true;
                    }
                }

                if(border)
                {
                    prevPos.Add(dicoHE[i].originVertex.pos);
                    nextPos.Add((l[0]+l[1]+l[2]+dicoHE[i].originVertex.pos)/4);
                }
                else
                {
                    prevPos.Add(dicoHE[i].originVertex.pos);
                    nextPos.Add((opposite[0]+opposite[1]+opposite[2])/3);
                }
                
            }

            if(l.Count==4)
            {
                prevPos.Add(dicoHE[i].originVertex.pos);
                nextPos.Add((l[0]+l[1]+l[2]+l[3])/4);
            }
        }

        for (int i = 0; i < prevPos.Count; i++)
        {
            for (int j = 0; j < dicoHE.Count; j++)
            {
                if (dicoHE[j].originVertex.pos == prevPos[i])
                {
                    var swap =dicoHE[j];
                    swap.originVertex.pos=nextPos[i];
                    dicoHE[j]=swap;
                }
            }
        }
    }

}

public struct HE_vertex
{

    public Vector3 pos;
    public int keyIncidentEdge;
}

public struct HE
{

    public HE_vertex originVertex;
    public int keyTwinEdge;
    public int keyNextEdge;
    public int keyPrevEdge;

    public HE_Face incidentFace;
}

public struct HE_Face
{

    public int id;
    public int startingEdge;
}