using FileFormatWavefront.Model;
using FileFormatWavefront;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Numerics;

namespace tiny_renderer.Lesson
{
    public class Lesson3:ILesson
    {
        public Lesson3()
        {
        }

        public void Start()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            string modelPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "obj", "african_head.obj");
            var result = FileFormatObj.Load(modelPath, false);
            string uvPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "obj", "african_head_diffuse.tga");
            Image<Rgba32> uvImage = Image<Rgba32>.Load(uvPath) as Image<Rgba32>;
            int width = 800;
            int height = 800;
            float[] zbuffer= new float[width * height];
            Array.Fill(zbuffer,int.MinValue);
            Vector3 light_dir = new Vector3(0, 0, -1);
            Image<Rgba32> image = new Image<Rgba32>(width, height, Color.Black);
            foreach (Group group in result.Model.Groups)
            {

                foreach (Face face in group.Faces)
                {
                    //Color color = RandomColor();
                    Vector2[] points = new Vector2[3];
                    Vector3[] world_points = new Vector3[3];
                    UV[] model_uv = new UV[3];
                    for (int i = 0; i < 3; i++)
                    {
                        int index = face.Indices[i].vertex;
                        int? vtIndex = face.Indices[i].uv;
                        Vertex v0 = result.Model.Vertices[index];
                        if (vtIndex.HasValue)
                        {
                            model_uv[i] = result.Model.Uvs[vtIndex.Value];
                        }
                        int x0 = (int)Math.Ceiling((v0.x * width / 2) + width / 2);
                        int y0 = (int)Math.Ceiling((v0.y * height / 2) + height / 2);
                        points[i] = new Vector2(x0, y0);
                        world_points[i] = new Vector3(v0.x, v0.y, v0.z);
                    }

                    Vector3 n = Vector3.Cross(world_points[2] - world_points[0], world_points[1] - world_points[0]);
                    Vector3 normalize = Vector3.Normalize(n);
                    //光照负方向*法线方向*cosθ 来表示光照强度
                    float intensity = Vector3.Dot(normalize, light_dir);
                    if (intensity > 0)
                    {
                        Rgba32[] color_array = new Rgba32[3];
                        for (int i = 0; i < model_uv.Length; i++)
                        {
                            float u = model_uv[i].u * uvImage.Width;
                            float v = model_uv[i].v * uvImage.Height;
                            var color = uvImage[(int)Math.Ceiling(u), (int)Math.Ceiling(v)];
                            color_array[i] = color;
                        }


                        byte r = (byte)Math.Ceiling(intensity * 255);
                        byte g = (byte)Math.Ceiling(intensity * 255);
                        byte b = (byte)Math.Ceiling(intensity * 255);
                        byte a = 255;
                        Color intensityColor = Color.FromRgba(r, g, b, a);

                        Box box =Graphics.GetBox(points);
                        for (int x = box.minX; x < box.maxX; x++)
                        {
                            for (int y = box.minY; y < box.maxY; y++)
                            {
                                Vector2 point = new Vector2(x, y);
                                Vector3 u = Graphics.GetBarycentric(point, points[0], points[1], points[2]);
                                if (u.X < 0 || u.Y < 0 || u.Z < 0)
                                {
                                    continue;
                                }
                                //计算zbuffer，并且每个顶点的z值乘上对应的质心坐标分量
                                float zvalue = world_points[0].Z * u.X + world_points[1].Z * u.Y + world_points[2].Z * u.Z;
                                int zindex = x + width * y;
                                if (zbuffer[zindex]> zvalue)
                                {
                                    continue;
                                }
                                zbuffer[zindex] = zvalue;

                                image[x, y] = intensityColor;

                            }
                        }
                    }
                }
            }
            string path = Environment.CurrentDirectory + "/Lesson3_model_zbuffer.png";
            image.SaveAsPng(path);
            stopwatch.Stop();
            Console.WriteLine("DrawModelZbuffer:" + stopwatch.ElapsedMilliseconds + "ms");
        }
    }
}

