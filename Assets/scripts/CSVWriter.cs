using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;


//classe pour enregistrer nos CSV
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
        tw.WriteLine("Vertex;");
        tw.Close();

        tw = new StreamWriter(filename, true);

        for(int i =0;i<vertex.Length;i++)
        {
            tw.WriteLine(vertex[i].ToString()+";");
        }
        tw.WriteLine("Quads;");
        for(int i =0;i<quads.Length;i+=6)
        {
            tw.WriteLine("("+quads[i].ToString()+","+quads[i+1].ToString()+","+quads[i+2].ToString()+","+quads[i+5].ToString()+");");
        }
        tw.Close();

        foreach(int child in quads)
        {
            Debug.LogWarning(child);
        }
        Debug.LogWarning("Path = "+filename);
    }

    //Methode enregistrement CSV Half Edge
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

    //Methode enregistrement CSV Winged Edge
    public void WriteCSWE(Dictionary<int, WE> toWrite, List<WE_Face>lFace, Vector3[] vertex)
    {
        TextWriter tw = new StreamWriter(filename, false);
        tw.WriteLine(("startVertex;endVertex;faceLeft;faceRight;keyPrevLE;keyNextLE;keyPrevRE;keyNextRE"));
        tw.Close();

        tw = new StreamWriter(filename, true);

        for (int i=0;i<toWrite.Count;i++)
        {
            tw.WriteLine(convertVertex( toWrite[i].startVertex.pos,vertex).ToString()+";"+
            convertVertex( toWrite[i].endVertex.pos,vertex).ToString()+";"+
            toWrite[i].faceLeft.ToString()+";"+
            toWrite[i].faceRight.ToString()+";"+
            toWrite[i].keyPrevLE.ToString()+";"+
            toWrite[i].keyNextLE.ToString()+";"+
            toWrite[i].keyPrevRE.ToString()+";"+
            toWrite[i].keyNextRE.ToString()+";")
            ;
        }
        tw.WriteLine((";"));
        tw.WriteLine(("FaceIndex;EdgeList"));
        foreach(WE_Face face in lFace)
        {
            tw.WriteLine((face.id+";("+face.associatedEdges[0]+","+face.associatedEdges[1]+","+face.associatedEdges[2]+","+face.associatedEdges[3]+")"));
        }
    

        tw.Close();

        Debug.LogWarning("Path = "+filename);
    }

    public int convertVertex(Vector3 pV,Vector3[] vertex )
    {
        for(int i=0;i<vertex.Length;i++)
        {
            if(vertex[i]==pV)return i;
        }
        return -1;
    }

    //Methode enregistrement CSV Half Edge
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
}

