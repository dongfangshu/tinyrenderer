﻿using System;
using System.Diagnostics;
using System.Numerics;
using FileFormatWavefront;
using FileFormatWavefront.Model;

namespace tiny_renderer
{
    public class Lesson2:ILesson
    {
        public Lesson2()
        {
        }

        public void Start()
        {
            //Test2();//三角面 扫线法
            //Test3();//三角面 叉乘
            DrawModelByLineSweeping();
            DrawModelByCross();
        }
        static void Test2()
        {
            int width = 800;
            int height = 800;
            Image<Rgba32> image = new Image<Rgba32>(width, height, Color.Black);
            Vector2[] v1 = new Vector2[3] { new Vector2(10, 70), new Vector2(50, 160), new Vector2(70, 80) };
            Vector2[] v2 = new Vector2[3] { new Vector2(180, 50), new Vector2(150, 1), new Vector2(70, 180) };
            Vector2[] v3 = new Vector2[3] { new Vector2(180, 150), new Vector2(120, 160), new Vector2(130, 180) };
            Graphics.DrawTriangle(image, v1[0], v1[1], v1[2], Color.White);
            Graphics.DrawTriangle(image, v2[0], v2[1], v2[2], Color.White);
            Graphics.DrawTriangle(image, v3[0], v3[1], v3[2], Color.White);
            string path = Environment.CurrentDirectory + "/Lesson2_Line_sweeping.png";
            image.SaveAsPng(path);
        }
        static void Test3()
        {
            int width = 800;
            int height = 800;
            Image<Rgba32> image = new Image<Rgba32>(width, height, Color.Black);
            Vector2[] v1 = new Vector2[3] { new Vector2(10, 70), new Vector2(50, 160), new Vector2(70, 80) };
            Vector2[] v2 = new Vector2[3] { new Vector2(180, 50), new Vector2(150, 1), new Vector2(70, 180) };
            Vector2[] v3 = new Vector2[3] { new Vector2(180, 150), new Vector2(120, 160), new Vector2(130, 180) };
            Graphics.DrawTriangle2(image, v1[0], v1[1], v1[2], Color.White);
            Graphics.DrawTriangle2(image, v2[0], v2[1], v2[2], Color.White);
            Graphics.DrawTriangle2(image, v3[0], v3[1], v3[2], Color.White);
            string path = Environment.CurrentDirectory + "/Lesson2_cross.png";
            image.SaveAsPng(path);
        }
        static void DrawModelByLineSweeping()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            string modelPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "obj", "african_head.obj");
            var result = FileFormatObj.Load(modelPath, false);
            int width = 800;
            int height = 800;
            Image<Rgba32> image = new Image<Rgba32>(width, height, Color.Black);
            foreach (Group group in result.Model.Groups)
            {

                foreach (Face face in group.Faces)
                {
                    Color color = RandomColor();
                    Vector2[] points = new Vector2[3];
                    for (int i = 0; i < 3; i++)
                    {
                        int index = face.Indices[i].vertex;
                        Vertex v0 = result.Model.Vertices[index];
                        int x0 = (int)Math.Ceiling((v0.x * width / 2) + width / 2);
                        int y0 = (int)Math.Ceiling((v0.y * height / 2) + height / 2);
                        points[i] = new Vector2(x0, y0);
                    }
                    Graphics.DrawTriangle(image, points[0], points[1], points[2],color);
                }
            }
            string path = Environment.CurrentDirectory + "/Lesson2_model.png";
            image.SaveAsPng(path);
            stopwatch.Stop();
            Console.WriteLine("DrawModelByLineSweeping:"+ stopwatch.ElapsedMilliseconds+"ms");
        }
        static void DrawModelByCross()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            string modelPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "obj", "african_head.obj");
            var result = FileFormatObj.Load(modelPath, false);
            int width = 800;
            int height = 800;
            Image<Rgba32> image = new Image<Rgba32>(width, height, Color.Black);
            foreach (Group group in result.Model.Groups)
            {

                foreach (Face face in group.Faces)
                {
                    Color color = RandomColor();
                    Vector2[] points = new Vector2[3];
                    for (int i = 0; i < 3; i++)
                    {
                        int index = face.Indices[i].vertex;
                        Vertex v0 = result.Model.Vertices[index];
                        int x0 = (int)Math.Ceiling((v0.x * width / 2) + width / 2);
                        int y0 = (int)Math.Ceiling((v0.y * height / 2) + height / 2);
                        points[i] = new Vector2(x0, y0);
                    }
                    Graphics.DrawTriangle2(image, points[0], points[1], points[2], color);
                }
            }
            string path = Environment.CurrentDirectory + "/Lesson2_model_cross.png";
            image.SaveAsPng(path);
            stopwatch.Stop();
            Console.WriteLine("DrawModelByCross:" + stopwatch.ElapsedMilliseconds + "ms");
        }
        private static Color RandomColor()
        {
            Random random = new Random();
            return Color.FromRgb((byte)random.Next(0,256), (byte)random.Next(0, 256), (byte)random.Next(0, 256));
        }
    }
}
