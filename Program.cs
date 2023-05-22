using FileFormatWavefront;
using FileFormatWavefront.Model;
using SixLabors.ImageSharp.ColorSpaces;
using System.ComponentModel;
using System.Numerics;
using Index = FileFormatWavefront.Model.Index;

namespace tiny_renderer;
class Program
{
    static void Main(string[] args)
    {
        //Console.WriteLine("Hello, World!");
        //TestDrawLine();
        //DrawModel();
        //DrawCircle();
        //Test1();
        Test2();
    }
    static void DrawCircle()
    {
        //(x-a)^2+(y-b)^2=z^2

        //(y-b)^2 = z^2-(x-a)^2

        //(y-b) = 
        int width = 800;
        int height = 800;
        int a = width/2;
        int b = height/2;
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
                    int x0 = (int)Math.Ceiling((v0.x * width / 2)+width/2);
                    int y0 = (int)Math.Ceiling((v0.y * height / 2)+height/2);
                    int x1 = (int)Math.Ceiling((v1.x * width / 2)+width / 2);
                    int y1 = (int)Math.Ceiling(((v1.y * height / 2))+height / 2);
                    Graphics.DrawLine(image, x0, y0, x1, y1, color);
                }

            }
        }
        string path = Environment.CurrentDirectory + "/framgent.png";
        image.SaveAsPng(path);
    }

    private static Color RandomColor()
    {
        return Color.White;
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
    static void Test1()
    {
        int width = 800;
        int height = 800;
        Image<Rgba32> image = new Image<Rgba32>(width, height, Color.Black);
        Vector2[] v1 = new Vector2[3]{new Vector2(10, 70), new Vector2(50, 160), new Vector2(70, 80) };
        Vector2[] v2 = new Vector2[3] { new Vector2(180, 50), new Vector2(150, 1), new Vector2(70, 180) };
        Vector2[] v3 = new Vector2[3] { new Vector2(180, 150), new Vector2(120, 160), new Vector2(130, 180) };
        DrawTriangle(image,v1[0], v1[1], v1[2]);
        DrawTriangle(image,v2[0], v2[1], v2[2]);
        DrawTriangle(image,v3[0], v3[1], v3[2]);
        string path = Environment.CurrentDirectory + "/framgent1.png";
        image.SaveAsPng(path);
    }
    static void Test2()
    {
        int width = 800;
        int height = 800;
        Image<Rgba32> image = new Image<Rgba32>(width, height, Color.Black);
        Vector2[] v1 = new Vector2[3] { new Vector2(10, 70), new Vector2(50, 160), new Vector2(70, 80) };
        Vector2[] v2 = new Vector2[3] { new Vector2(180, 50), new Vector2(150, 1), new Vector2(70, 180) };
        Vector2[] v3 = new Vector2[3] { new Vector2(180, 150), new Vector2(120, 160), new Vector2(130, 180) };
        DrawTriangle2(image, v1[0], v1[1], v1[2]);
        DrawTriangle2(image, v2[0], v2[1], v2[2]);
        DrawTriangle2(image, v3[0], v3[1], v3[2]);
        string path = Environment.CurrentDirectory + "/framgent2.png";
        image.SaveAsPng(path);
    }
    static void DrawTriangle(Image<Rgba32> image,Vector2 v1, Vector2 v2, Vector2 v3)
    {
        Graphics.DrawLine2(image,(int)v1.X, (int)v1.Y, (int)v2.X, (int)v2.Y,Color.Red);
        Graphics.DrawLine2(image,(int)v1.X, (int)v1.Y, (int)v3.X, (int)v3.Y,Color.Green);
        Graphics.DrawLine2(image,(int)v3.X, (int)v3.Y, (int)v2.X, (int)v2.Y,Color.Blue);
    }
    static void DrawTriangle2(Image<Rgba32> image, Vector2 v1, Vector2 v2, Vector2 v3)
    {
        //  这里到原点在左上角，和教程不一样。y越大越往下。所以这里是上半部分，教程是下半部分
        if (v1.Y>v2.Y)
        {
            Swap(ref v1,ref v2);
        }
        if (v1.Y>v3.Y)
        {
            Swap(ref v1,ref v3);
        }
        if (v2.Y>v3.Y)
        {
            Swap(ref v2,ref v3);
        }
        //从上到下 v3 v2 v1
        float totalHeight =v3.Y - v1.Y;
        float segmentHeight = v2.Y - v1.Y;
        for (float y = v1.Y; y < v2.Y; y++)
        {
            float a = (y - v1.Y) / totalHeight;
            float b = (y - v1.Y) / segmentHeight;
            Vector2 A = v1 + (v3 - v1) * a;
            Vector2 B = v1 + (v2 - v1) * b;
            int left =(int)Math.Ceiling(A.X);
            int right =(int)Math.Ceiling(B.X);
            
            image[left, (int)y] = Color.Red;
            image[right, (int)y] = Color.White;
        }
    }
    static void Swap(ref Vector2 v1, ref Vector2 v2)
    {
        Vector2 tmp = v1;
        v1 = v2;
        v2 = tmp;
    }
}

