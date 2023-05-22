using System;
using FileFormatWavefront;
using FileFormatWavefront.Model;

namespace tiny_renderer
{
    public class Lesson1: ILesson
    {
        public Lesson1()
        {
        }
        public void Start()
        {
            //TestDrawLine();
            //DrawCircle();
            DrawModel();
        }
        static void TestDrawLine()
        {
            int width = 800;
            int height = 800;
            Image<Rgba32> image = new Image<Rgba32>(width, height, Color.Black);
            int centerX = width / 2;
            int centerY = height / 2;
            //左上角是原点
            Graphics.DrawLine2(image, centerX, centerY, centerX + 100, centerY - 100, Color.Red);//中心右上角45度
            Graphics.DrawLine2(image, centerX + 100, centerY, centerX + 100 + 100, centerY - 100, Color.Yellow);//中心右上角45度平行线
                                                                                                                //DrawLine(image, centerX, centerY, centerX + 100, centerY, Color.Green);//水平直线
                                                                                                                //DrawLine(image, centerX, centerY, centerX, centerY - 100, Color.Black);//垂直直线

            //DrawLine(image, 450, 700, 480, 670, Color.White);//垂直直线
            //DrawLine(image,20,20,50,20,Color.Red);
            //DrawLine(image,20,20,50,50,Color.Red);
            string path = Environment.CurrentDirectory + "/framgent.png";
            image.SaveAsPng(path);
        }
        static void DrawCircle()
        {
            //(x-a)^2+(y-b)^2=z^2

            //(y-b)^2 = z^2-(x-a)^2

            //(y-b) = 
            int width = 800;
            int height = 800;
            int a = width / 2;
            int b = height / 2;
            int r = 400;
            Image<Rgba32> image = new Image<Rgba32>(width, height, Color.Black);
            //DrawLine(image, a - r, b, a, b, Color.White);
            //DrawLine(image, a - 283, b - 283, a, b, Color.White);
            //DrawLine(image, a, b - r, a, b, Color.White);
            //DrawLine(image, a + 283, b - 283, a, b, Color.White);
            //DrawLine(image, a + r, b, a, b, Color.White);
            //DrawLine(image, a + 283, b + 283, a, b, Color.White);
            //DrawLine(image, a, b + r, a, b, Color.White);
            //DrawLine(image, a - 283, b + 283, a, b, Color.White);
            for (int i = 0; i <= r; i += 10)
            {
                int x = i;
                int dy = (int)Math.Ceiling(Math.Sqrt(Math.Pow(r, 2) - Math.Pow(x, 2)));
                Graphics.DrawLine(image, a + x, b + dy, a, b, Color.White);
                Graphics.DrawLine(image, a + x, b - dy, a, b, Color.White);
                Graphics.DrawLine(image, a - x, b + dy, a, b, Color.White);
                Graphics.DrawLine(image, a - x, b - dy, a, b, Color.White);
            }
            string path = Environment.CurrentDirectory + "/framgent.png";
            image.SaveAsPng(path);
        }
        static void DrawModel()
        {
            string modelPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "obj", "african_head.obj");
            var result = FileFormatObj.Load(modelPath, false);
            /*
             * The Model property is of type Scene and contains the following members:

    Vertices all vertex data.
    Uvs all material coordinate data.
    Normals all normal data.
    Materials all material data.
    Groups all objects (groups of faces).
    UngroupedFaces all faces not grouped into objects.
             * */
            int width = 800;
            int height = 800;
            Image<Rgba32> image = new Image<Rgba32>(width, height, Color.Black);
            foreach (Group group in result.Model.Groups)
            {
                Color color = RandomColor();
                foreach (Face face in group.Faces)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        int v0_index = face.Indices[i].vertex;
                        int v1_index = face.Indices[(i + 1) % 3].vertex;
                        Vertex v0 = result.Model.Vertices[v0_index];
                        Vertex v1 = result.Model.Vertices[v1_index];
                        int x0 = (int)Math.Ceiling((v0.x * width / 2) + width / 2);
                        int y0 = (int)Math.Ceiling((v0.y * height / 2) + height / 2);
                        int x1 = (int)Math.Ceiling((v1.x * width / 2) + width / 2);
                        int y1 = (int)Math.Ceiling(((v1.y * height / 2)) + height / 2);
                        Graphics.DrawLine(image, x0, y0, x1, y1, color);
                    }

                }
            }
            string path = Environment.CurrentDirectory + "/Lesson1.png";
            image.SaveAsPng(path);
        }

        private static Color RandomColor()
        {
            return Color.White;
        }
    }
}

