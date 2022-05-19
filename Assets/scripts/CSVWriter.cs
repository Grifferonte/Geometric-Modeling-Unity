using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CSVWriter
{
    public string filename ="";
    
    public CSVWriter(string name)
    {
        filename = Application.dataPath+"/"+name+".csv";
    }

public void WriteCSVMesh(Vector3[] vertex ,int[] quads)
{
    TextWriter tw = new StreamWriter(filename, false);
    tw.WriteLine("Vertex;Quads");
    tw.Close();

    tw = new StreamWriter(filename, true);

    for(int i =0;i<vertex.Length;i++)
    {
        tw.WriteLine(vertex[i].ToString()+";"+quads[i].ToString());
    }
    tw.Close();

    Debug.LogWarning("Path = "+filename);
}

public void WriteCSVHE(Dictionary<int, HE> toWrite)
{
    TextWriter tw = new StreamWriter(filename, false);
    tw.WriteLine("originVertex;keyTwinEdge;keyNextEdge;keyPrevEdge");
    tw.Close();

    tw = new StreamWriter(filename, true);

    for (int i=0;i<toWrite.Count;i++)
    {
        Debug.LogWarning(toWrite[i].keyNextEdge.ToString());
        tw.WriteLine(toWrite[i].originVertex.pos.ToString()+";"+toWrite[i].keyTwinEdge+";"+toWrite[i].keyNextEdge.ToString()+";"+toWrite[i].keyPrevEdge.ToString());
    }
    tw.Close();

    Debug.LogWarning("Path = "+filename);
}


}

