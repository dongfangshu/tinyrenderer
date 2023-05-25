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
            //Test1();
            Zbuffer();
        }
        private void Test1()
        {
            Vector2 a = new Vector2(0,0);
            Vector2 b = new Vector2(6,0);
            Vector2 c = new Vector2(0,6);
            //Vector2 p = new Vector2(3,3);
            Box box = Graphics.GetBox(new Vector2[] { a,b,c});
            for (int x = box.minX; x < box.maxX; x++)
            {
                for (int y = box.minY; y < box.maxY; y++)
                {
                    Vector2 p = new Vector2(x,y);
                    Vector3 p_barycentric = Graphics.GetBarycentric(p, a, b, c);
                }
            }


        }
        private static void Zbuffer()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            string modelPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "obj", "african_head.obj");
            var result = FileFormatObj.Load(modelPath, false);
            string uvPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "obj", "african_head_diffuse.tga");
            Image<Rgba32> uvImage = Image<Rgba32>.Load(uvPath) as Image<Rgba32>;
            int width = 800;
            int height = 800;
            float[] zbuffer = new float[width * height];
            Array.Fill(zbuffer, int.MinValue);
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
                        //Vector2[] uv_pos = new Vector2[3];
                        //for (int i = 0; i < model_uv.Length; i++)
                        //{
                        //    float u = model_uv[i].u * uvImage.Width;
                        //    float v = (1-model_uv[i].v) * uvImage.Height;
                        //    uv_pos[i] = new Vector2(u,v);
                        //}


                        //byte r = (byte)Math.Ceiling(intensity * 255);
                        //byte g = (byte)Math.Ceiling(intensity * 255);
                        //byte b = (byte)Math.Ceiling(intensity * 255);
                        //byte a = 255;
                        //Color intensityColor = Color.FromRgba(r, g, b, a);
                        Box box = Graphics.GetBox(points);
                        float uv_min_x = Math.Min(model_uv[0].u, Math.Min(model_uv[1].u, model_uv[2].u));
                        float uv_max_x = Math.Max(model_uv[0].u, Math.Max(model_uv[1].u, model_uv[2].u));

                        float uv_min_y = Math.Min(model_uv[0].v, Math.Min(model_uv[1].v, model_uv[2].v));
                        float uv_max_y = Math.Max(model_uv[0].v, Math.Max(model_uv[1].v, model_uv[2].v));

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
                                if (zbuffer[zindex] > zvalue)
                                {
                                    continue;
                                }
                                zbuffer[zindex] = zvalue;
                                //这里先算插值再得倒实际像素。如果先算像素再算比值会有较大误差，出来倒图像会叠层
                                float box_x_per = (x - box.minX) / (float)box.Width;
                                float box_x_r_per = (uv_max_x - uv_min_x) * box_x_per;
                                float uv_p_x = (box_x_r_per + uv_min_x) * uvImage.Width;
                                int c_x =(int)MathF.Ceiling(uv_p_x);

                                float box_y_per = (y - box.minY) / (float)box.Height;
                                float box_y_r_per = (uv_max_y - uv_min_y) * box_y_per;
                                float uv_p_y = (box_y_r_per + uv_min_y) * uvImage.Height;
                                int c_y = (int)MathF.Ceiling(uv_p_y);
                                //左手右手需要倒一下y
                                Rgba32 color = uvImage[c_x, uvImage.Height-c_y];
                                byte r = (byte)Math.Ceiling(intensity * color.R);
                                byte g = (byte)Math.Ceiling(intensity * color.G);
                                byte b = (byte)Math.Ceiling(intensity * color.B);
                                byte a = 255;
                                image[x, y] = new Rgba32(r,g,b,a);

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

