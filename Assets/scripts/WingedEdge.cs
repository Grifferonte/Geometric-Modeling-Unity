using System.Collections.Generic;
using UnityEngine;

//Classe pour gerer nos WingedEdge et nos dictionnaires
public class WingedEdgeManager
{
    public Dictionary<int, WE> dicoWE;
    public List<WE_Face> listFWE;

    public WingedEdgeManager(Mesh inputMesh)
    {
        dicoWE = new Dictionary<int, WE>();
        listFWE = new List<WE_Face>();
        var lT = inputMesh.triangles;
        var lV = inputMesh.vertices;

        int edgeCount = 0;
        int id = 0;

        //On convertit notre Face Vertex en Winged Edge avec des check selon les sens d enregistrement des vertex
        for (int i = 0; i < lT.Length; i += 6) 
        {
            WE_vertex WEV1, WEV2, WEV3, WEV4;

            WEV1.pos = lV[lT[i]];
            WEV1.keyIncidentEdge = edgeCount;
            WEV2.pos = lV[lT[i + 1]];
            WEV2.keyIncidentEdge = edgeCount + 1;
            WEV3.pos = lV[lT[i + 2]];
            WEV3.keyIncidentEdge = edgeCount + 2;
            WEV4.pos = lV[lT[i + 5]];
            WEV4.keyIncidentEdge = edgeCount + 3;

            WE_Face WEF;
            WEF.id = id;

            int[] t = { -1, -1, -1, -1 };

            int[] o = { dicoWE.Count, dicoWE.Count + 1, dicoWE.Count + 2, dicoWE.Count + 3 };

            int order = 0;
            Vector3[] clV = { WEV1.pos, WEV2.pos, WEV3.pos, WEV4.pos };

            var cd = (i / 6) % 2 == 0 ? 1 : 2;
            int[] direction = { cd, cd, cd, cd };

            for (int j = 0; j < dicoWE.Count; j++)
            {
                if (dicoWE[j].checkPoints(WEV1.pos, WEV2.pos)) { t[0] = j; o[0] = j; o[1]--; o[2]--; o[3]--; direction[0] = dicoWE[j].checkState(); }
                if (dicoWE[j].checkPoints(WEV2.pos, WEV1.pos)) { t[0] = j; o[0] = j; o[1]--; o[2]--; o[3]--; direction[0] = dicoWE[j].checkState(); }
                if (dicoWE[j].checkPoints(WEV3.pos, WEV2.pos)) { t[1] = j; o[1] = j; o[2]--; o[3]--; direction[1] = dicoWE[j].checkState(); }
                if (dicoWE[j].checkPoints(WEV2.pos, WEV3.pos)) { t[1] = j; o[1] = j; o[2]--; o[3]--; direction[1] = dicoWE[j].checkState(); }
                if (dicoWE[j].checkPoints(WEV4.pos, WEV3.pos)) { t[2] = j; o[2] = j; o[3]--; direction[2] = dicoWE[j].checkState(); ; }
                if (dicoWE[j].checkPoints(WEV3.pos, WEV4.pos)) { t[2] = j; o[2] = j; o[3]--; direction[2] = dicoWE[j].checkState(); }
                if (dicoWE[j].checkPoints(WEV1.pos, WEV4.pos)) { t[3] = j; o[3] = j; direction[3] = dicoWE[j].checkState(); }
                if (dicoWE[j].checkPoints(WEV4.pos, WEV1.pos)) { t[3] = j; o[3] = j; direction[3] = dicoWE[j].checkState(); }
            }

            for (int j = 0; j < 4; j++)
            {
                o[j] = t[j] != -1 ? t[j] : o[j];
            }

            WE_vertex[] vClock = { WEV1, WEV2, WEV3, WEV4 };

            int[] vOffPrev = { 3, -1, -1, -1 };
            int[] vOffNext = { 1, 1, 1, -3 };

            WEF.associatedEdges = o;

            if (listFWE.Count > 0)
            {
                foreach (int edge in listFWE[listFWE.Count - 1].associatedEdges)
                {
                    if (edge == o[0]) { order = dicoWE[edge].checkOrder(clV); break; }
                    if (edge == o[1]) { order = dicoWE[edge].checkOrder(clV); break; }
                    if (edge == o[2]) { order = dicoWE[edge].checkOrder(clV); break; }
                    if (edge == o[3]) { order = dicoWE[edge].checkOrder(clV); break; }
                }
            }

            for (int j = 0; j < 4; j++)
            {
                WE_vertex v1 = new WE_vertex();
                WE_vertex v2 = new WE_vertex();
                int n = -1;
                int p = -1;

                if (j == 0) { v1 = vClock[0]; v2 = vClock[1]; p = 3; n = 1; }
                if (j == 1) { v1 = vClock[1]; v2 = vClock[2]; p = 0; n = 2; }
                if (j == 2) { v1 = vClock[2]; v2 = vClock[3]; p = 1; n = 3; }
                if (j == 3) { v1 = vClock[3]; v2 = vClock[0]; p = 2; n = 0; }

                if (order == 1 && cd == 1) { var s = v1; v1 = v2; v2 = s; }

                var vPrev = t[p] != -1 ? t[p] : o[p];
                var vNext = t[n] != -1 ? t[n] : o[n];

                if (t[j] == -1)
                {
                    WE cWe = new WE();
                    if (direction[j] == 1)
                    {
                        if (cd == 2 || cd - 1 != order) { var s = vPrev; vPrev = vNext; vNext = s; }
                        cWe.setLeft(v1, v2, WEF.id, vPrev, vNext);
                    }
                    else cWe.setRight(v1, v2, WEF.id, vPrev, vNext);
                    dicoWE.Add(dicoWE.Count, cWe);
                }
                else
                {
                    if (direction[j] == 1)
                    {
                        if (cd == 2 || cd - 1 != order) { var s = vPrev; vPrev = vNext; vNext = s; }
                        dicoWE[t[j]].setLeft(v1, v2, WEF.id, vPrev, vNext);

                    }
                    else dicoWE[t[j]].setRight(v1, v2, WEF.id, vPrev, vNext);
                }
            }



            listFWE.Add(WEF);
            edgeCount += 4;
            id++;
        }

    }

    //Envoie le mesh correspondant a notre WingedEdge
    public Mesh output()
    {
        Mesh mesh = new Mesh();

        List<Vector3> verts = new List<Vector3>();
        List<int> quads = new List<int>();

        WE_Face face;

        for (int i = 0; i < 4; i++) //listFWE.Count
        {
            face = listFWE[i];
            List<int> edges = new List<int>(face.associatedEdges);
            List<Vector3> prevVertex = new List<Vector3>();
            List<Vector3> lv = new List<Vector3>();

            var edge=dicoWE[face.associatedEdges[0]];

            var edgenext=dicoWE[face.associatedEdges[3]];
            Vector3 currentvert;
            if(edge.startVertex.pos==edgenext.endVertex.pos || edge.startVertex.pos==edgenext.startVertex.pos)
            {
                currentvert=edge.startVertex.pos;
            }
            else
            {
                currentvert=edge.endVertex.pos;
            }
            lv.Add(currentvert);

            if(edgenext.startVertex.pos!=currentvert )
            {
                currentvert=edgenext.startVertex.pos;
            }
            else
            {
                currentvert=edgenext.endVertex.pos;
            }
            lv.Add(currentvert);

            edgenext=dicoWE[face.associatedEdges[2]];
            if(edgenext.startVertex.pos!=currentvert )
            {
                currentvert=edgenext.startVertex.pos;
            }
            else
            {
                currentvert=edgenext.endVertex.pos;
            }
            lv.Add(currentvert);

            edgenext=dicoWE[face.associatedEdges[1]];
            if(edgenext.startVertex.pos!=currentvert )
            {
                currentvert=edgenext.startVertex.pos;
            }
            else
            {
                currentvert=edgenext.endVertex.pos;
            }
            lv.Add(currentvert);

            for (int j = 0; j < 4; j++)
            {
                if (verts.Contains(lv[j]))
                {
                    quads.Add(verts.IndexOf(lv[j]));
                }
                else
                {
                    verts.Add(lv[j]);
                    quads.Add(verts.Count - 1);
                }
            }

        }

        mesh.SetVertices(verts);
        mesh.SetIndices(quads, MeshTopology.Quads, 0);

        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
        return mesh;
    }
}

//classe WingedEdge contenant des methodes pour checker les directions et etats
public class WE
{

    public WE_vertex startVertex;
    public WE_vertex endVertex;

    public int faceLeft; 
    public int faceRight;

    public int keyPrevLE; 
    public int keyNextLE; 
    public int keyPrevRE;
    public int keyNextRE;

    public void setLeft(WE_vertex pSV, WE_vertex pEV, int pFaceLeft, int pkPrevLE, int pkNextLE)
    {
        if (startVertex.pos == Vector3.zero) startVertex = pSV;
        if (endVertex.pos == Vector3.zero) endVertex = pEV;

        faceLeft = pFaceLeft;
        keyPrevLE = pkPrevLE;
        keyNextLE = pkNextLE;
    }
    public void setRight(WE_vertex pSV, WE_vertex pEV, int pFaceRight, int pkPrevRE, int pkNextRE)
    {
        if (startVertex.pos == Vector3.zero) startVertex = pSV;
        if (endVertex.pos == Vector3.zero) endVertex = pEV;

        faceRight = pFaceRight;
        keyPrevRE = pkPrevRE;
        keyNextRE = pkNextRE;
    }
    public bool checkPoints(Vector3 start, Vector3 end)
    {
        if (start == startVertex.pos && end == endVertex.pos) return true;
        return false;
    }

    public int checkState()
    {
        if (keyPrevLE == 0 && keyNextLE == 0)
        {
            if (keyPrevRE == 0 && keyNextRE == 0)
            {
                return -1;
            }
            return 1;
        }
        if (keyPrevRE == 0 && keyNextRE == 0)
        {
            if (keyPrevLE == 0 && keyNextLE == 0)
            {
                return -1;
            }
            return 2;
        }
        return 0;
    }

    public int checkOrder(Vector3[] lV)
    {
        int p1 = -1;
        int p2 = -1;

        for (int i = 0; i < 4; i++)
        {
            if (lV[i] == startVertex.pos) p1 = i;
            if (lV[i] == endVertex.pos) p2 = i;
        }
        return (p2 > p1) ? 0 : 1;
    }
}

public struct WE_vertex
{

    public Vector3 pos;
    public int keyIncidentEdge;

}

public struct WE_Face
{
    public int id;
    public int[] associatedEdges;

}