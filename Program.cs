using FileFormatWavefront;
using FileFormatWavefront.Model;
using SixLabors.ImageSharp.ColorSpaces;
using Index = FileFormatWavefront.Model.Index;

namespace tiny_renderer;
class Program
{
    static void Main(string[] args)
    {
        //Console.WriteLine("Hello, World!");
        //TestDrawLine();
        DrawModel();
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
                    DrawLine(image, x0, y0, x1, y1, color);
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
        //DrawLine(image, centerX, centerY, centerX + 100, centerY - 100, Color.Red);//中心右上角45度
        //DrawLine(image, centerX + 100, centerY, centerX + 100 + 100, centerY - 100, Color.Yellow);//中心右上角45度平行线
        //DrawLine(image, centerX, centerY, centerX + 100, centerY, Color.Green);//水平直线
        //DrawLine(image, centerX, centerY, centerX, centerY - 100, Color.Black);//垂直直线

        DrawLine(image, 450, 700, 480, 670, Color.White);//垂直直线
        //DrawLine(image,20,20,50,20,Color.Red);
        //DrawLine(image,20,20,50,50,Color.Red);
        string path = Environment.CurrentDirectory + "/framgent.png";
        image.SaveAsPng(path);
    }
    static void DrawLine(Image<Rgba32> image, int x0, int y0, int x1, int y1, Color color)
    {
        x0 = Math.Clamp(x0,0,799);
        x1 = Math.Clamp(x1, 0,799);
        y0 = Math.Clamp(y0, 0,799);
        y1 = Math.Clamp(y1, 0,799);
        int offsetX = Math.Abs(x1 - x0);
        int offsetY = Math.Abs(y1 - y0);
        bool steep = false;
        if (offsetX < offsetY)
        {
            steep = true;
            Swap(ref x0, ref y0);
            Swap(ref x1, ref y1);
        }
        if (x0 > x1)
        {
            Swap(ref x0, ref x1);
        }
        //if (y0 > y1)
        //{
        //    Swap(ref y0, ref y1);
        //}
        if (x0 == x1 && y1 ==y0)
        {
            //点
            image[x0 , y0 ] = color;
            return;
        }
        for (int deltaX = x0; deltaX <= x1; deltaX++)
        {
            float perX = (deltaX - x0) / (float)(x1 - x0);
            int deltaY = (int)Math.Ceiling(((y1 - y0) * perX) + y0);
            if (steep)
            {
                image[deltaY, deltaX] = color;
            }
            else
            {
                image[deltaX , deltaY ] = color;
            }

        }
    }
    static void Swap(ref int a, ref int b)
    {
        int tmp = a;
        a = b;
        b = tmp;
    }
}

