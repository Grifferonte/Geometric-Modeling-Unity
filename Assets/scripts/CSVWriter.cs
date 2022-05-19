using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

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
        tw.WriteLine(toWrite[i].originVertex.pos.ToString()+";"+toWrite[i].keyTwinEdge+";"+toWrite[i].keyNextEdge.ToString()+";"+toWrite[i].keyPrevEdge.ToString());
    }
    tw.Close();

    Debug.LogWarning("Path = "+filename);
}

public Mesh ReadCSVHE()
{
    Mesh mesh = new Mesh();

    var lineCount = File.ReadLines(filename).Count();
        List<Vector3> verts = new List<Vector3>();
        List<int> quads = new List<int>();
    
    Debug.LogWarning(lineCount);

    using(var reader = new StreamReader(filename))
    {
        
        
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            for (int i =0; i<(lineCount-1)/4;i+=4)
            {
                line = reader.ReadLine();
                var values = line.Split(';');
                float[] newPosCoordinates = values[0].Split(new string[] { ", " }, System.StringSplitOptions.None).Select(x => float.Parse(x)).ToArray();
                Vector3 newpos = new Vector3(newPosCoordinates[0], newPosCoordinates[1], newPosCoordinates[2]);
                if(!verts.Contains(newpos))
                {
                    verts.Append(newpos);
                }
                int index = verts.IndexOf(newpos);
                quads.Add(index);
            }
            
        }
    }
        mesh.vertices = verts.ToArray();
        mesh.SetIndices(quads.ToArray(), MeshTopology.Quads, 0);

        return mesh;
}

/*
public Mesh constructMes()
{
    Mesh mesh = new Mesh();

    var lineCount = File.ReadLines(filename).Count();
        List<Vector3> verts = new List<Vector3>();
        List<int> quads = new List<int>();
    
    Debug.LogWarning(lineCount);

    using(var reader = new StreamReader(filename))
    {
        
        
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            for (int i =0; i<(lineCount-1)/4;i+=4)
            {
                line = reader.ReadLine();
                var values = line.Split(';');
                float[] newPosCoordinates = values[0].Split(new string[] { ", " }, System.StringSplitOptions.None).Select(x => float.Parse(x)).ToArray();
                Vector3 newpos = new Vector3(newPosCoordinates[0], newPosCoordinates[1], newPosCoordinates[2]);
                if(!verts.Contains(newpos))
                {
                    verts.Append(newpos);
                }
                int index = verts.IndexOf(newpos);
                quads.Add(index);
            }
            
        }
    }
        mesh.vertices = verts.ToArray();
        mesh.SetIndices(quads.ToArray(), MeshTopology.Quads, 0);

        return mesh;
}*/

}

