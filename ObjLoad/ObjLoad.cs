//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace tiny_renderer
//{
//    class ObjLoadResult
//    {
//        public List<Vertex> Vertices = new List<Vertex>();
//        public List<Face> Faces = new List<Face>();
//    }
//    internal class ObjLoad
//    {
//        public static ObjLoadResult Load(string path)
//        {
//            ObjLoadResult result = new ObjLoadResult();
//            string[] lines = File.ReadAllLines(path, Encoding.UTF8);
//            foreach (string line in lines) {
//                if (line.StartsWith("v"))
//                {
//                    float[] vertexArray = new float[3];
//                    string[] vertexs = line.Split(' ');
//                    for (int i = 1; i < vertexs.Length; i++)
//                    {
//                        string vertexValue = vertexs[i];
//                        vertexArray[i-1] = float.Parse(vertexValue);
//                    }
//                    Vertex vertex = new Vertex(vertexArray);
//                    result.Vertices.Add(vertex);
//                }
//                else if (line.StartsWith("f"))
//                {
//                    Face face = new Face();
//                    string[] array1 = line.Split(' ');
//                    for (int i = 1; i < array1.Length; i++)
//                    {
//                        string[] array2 = array1[i].Split('/');
//                        int[] vertexindex = new int[array2.Length];
//                        for (int j = 0; j < array2.Length; j++)
//                        {
//                            vertexindex[j] = int.Parse(array2[j]);
//                        }
//                        face.VertexIndex.Add(vertexindex);
//                    }
//                    result.Faces.Add(face);
//                }
//            }
//            return result;
//        }
//    }
//}
