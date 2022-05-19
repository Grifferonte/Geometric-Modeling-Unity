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
        foreach(Vector3 child in lV)
        {
            Debug.LogWarning(child);
        }
        
        for (int i=0;i<lT.Length/4;i+=4)
        {
            HE_Face HEF;
            HEF.startingEdge=i;

            HE_vertex HEV1,HEV2,HEV3,HEV4;
            HEV1.pos=lV[i];
            HEV1.keyIncidentEdge=i;
            HEV2.pos=lV[i+1];
            HEV2.keyIncidentEdge=i+1;
            HEV3.pos=lV[i+2];
            HEV3.keyIncidentEdge=i+2;
            HEV4.pos=lV[i+3];
            HEV4.keyIncidentEdge=i+3;

            HE1.originVertex=HEV1;
            HE1.keyNextEdge=i+1;
            HE1.keyPrevEdge=i+3;
            HE1.incidentFace=HEF;

            HE2.originVertex=HEV2;
            HE2.keyNextEdge=i+2;
            HE2.keyPrevEdge=i;
            HE2.incidentFace=HEF;

            HE3.originVertex=HEV3;
            HE3.keyNextEdge=i+3;
            HE3.keyPrevEdge=i+1;
            HE3.incidentFace=HEF;

            HE4.originVertex=HEV4;
            HE4.keyNextEdge=i;
            HE4.keyPrevEdge=i+2;
            HE4.incidentFace=HEF;
            
            dicoHE.Add(i, HE1);
            dicoHE.Add(i+1, HE2);
            dicoHE.Add(i+2, HE3);
            dicoHE.Add(i+3, HE4);
        }

        for (int i=0;i<dicoHE.Count;i++)
        {
            for (int j=0;j<dicoHE.Count;j++)
            {
                if(dicoHE[i].keyNextEdge==dicoHE[j].keyPrevEdge && dicoHE[i].keyPrevEdge==dicoHE[j].keyNextEdge)
                {
                    HE toedit = dicoHE[i];
                    toedit.keyTwinEdge=j;
                    dicoHE[i]=toedit;
                }   
            }
        }
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