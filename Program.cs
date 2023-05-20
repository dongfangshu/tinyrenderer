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
        //DrawModel();
        DrawCircle();
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
        //DrawLine2(image, a-r,b, a, b, Color.White);
        //DrawLine2(image, a-283,b-283, a, b, Color.White);
        //DrawLine2(image, a,b-r, a, b, Color.White);
        //DrawLine2(image, a+283,b-283, a, b, Color.White);
        //DrawLine2(image, a+r,b, a, b, Color.White);
        //DrawLine2(image, a+283,b+283, a, b, Color.White);
        //DrawLine2(image, a,b+r, a, b, Color.White);
        //DrawLine2(image, a-283,b+283, a, b, Color.White);
        for (int i = 0; i < r; i += 10)
        {
            int x = i;
            int dy = (int)Math.Ceiling(Math.Cbrt(Math.Pow(r, 2) - Math.Pow(x, 2)));
            DrawLine2(image, a + x, b + dy, a, b, Color.White);
            DrawLine2(image, a + x, b - dy, a, b, Color.White);
            DrawLine2(image, a - x, b + dy, a, b, Color.White);
            DrawLine2(image, a - x, b - dy, a, b, Color.White);
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
                    DrawLine2(image, x0, y0, x1, y1, color);
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
        DrawLine2(image, centerX, centerY, centerX + 100, centerY - 100, Color.Red);//中心右上角45度
        DrawLine2(image, centerX + 100, centerY, centerX + 100 + 100, centerY - 100, Color.Yellow);//中心右上角45度平行线
        //DrawLine(image, centerX, centerY, centerX + 100, centerY, Color.Green);//水平直线
        //DrawLine(image, centerX, centerY, centerX, centerY - 100, Color.Black);//垂直直线

        //DrawLine(image, 450, 700, 480, 670, Color.White);//垂直直线
        //DrawLine(image,20,20,50,20,Color.Red);
        //DrawLine(image,20,20,50,50,Color.Red);
        string path = Environment.CurrentDirectory + "/framgent.png";
        image.SaveAsPng(path);
    }
    static void DrawLine2(Image<Rgba32> image, int x0, int y0, int x1, int y1, Color color)
    {
        int offsetX = Math.Abs(x1 - x0);
        int offsetY = Math.Abs(y1 - y0);
        bool steep = false;
        if (offsetX < offsetY)
        {
            steep = true;
        }

        int a = y1 - y0;
        int b = x0 - x1;
        int c = x1 * y0 - x0 * y1;
        if (steep)
        {
            if (y0 > y1)
            {
                Swap(ref y0, ref y1);
            }
            for (int y = y0; y < y1; y++)
            {
                int x = -(b * y + c) / a;
                if (x > 799||x<0)
                {
                    Console.WriteLine("x:"+x);
                    x = Math.Clamp(x,0,799);
                }
                if (y > 799 || y < 0)
                {
                    Console.WriteLine("y:"+y);
                    y = Math.Clamp(y, 0, 799);
                }
                image[x, y] = color;
            }
        }
        else
        {
            if (x0 > x1)
            {
                Swap(ref x0, ref x1);
            }
            for (int x = x0; x < x1; x++)
            {
                int y = -(a * x + c) / b;
                if (x > 799 || x < 0)
                {
                    Console.WriteLine("x:" + x);
                    x = Math.Clamp(x, 0, 799);
                }
                if (y > 799 || y < 0)
                {
                    Console.WriteLine("y:" + y);
                    y = Math.Clamp(y, 0, 799);
                }
                image[x, y] = color;
            }
        }

    }
    static void DrawLine(Image<Rgba32> image, int x0, int y0, int x1, int y1, Color color)
    {
        if (x0 == x1 && y1 == y0)
        {
            //点
            x0 = Math.Clamp(x0, 0, 799);
            y0 = Math.Clamp(y0, 0, 799);
            image[x0, y0] = color;
            return;
        }
        //x0 = Math.Clamp(x0,0,799);
        //y0 = Math.Clamp(y0, 0,799);
        //x1 = Math.Clamp(x1, 0,799);
        //y1 = Math.Clamp(y1,0,799);
        int offsetX = Math.Abs(x1 - x0);
        int offsetY = Math.Abs(y1 - y0);
        bool steep = false;
        if (offsetX < offsetY)
        {
            steep = true;
        }
        if (steep)
        {
            if (y1<y0)
            {
                Swap(ref y1,ref y0);
            }
            for (int deltaY = y0; deltaY < y1; deltaY++)
            {
                float perY = (deltaY - y0) / (float)(y1 - y0);
                int deltaX = (int)Math.Ceiling(((x1-x0) * perY) + x0);
                deltaX = Math.Clamp(deltaX, 0, 799);
                image[deltaX, deltaY] = color;
            }
        }
        else
        {
            if (x0 > x1)
            {
                Swap(ref x0, ref x1);
            }
            for (int deltaX = x0; deltaX < x1; deltaX++)
            {
                float perX = (deltaX - x0) / (float)(x1 - x0);
                int deltaY = (int)Math.Ceiling(((y1-y0) * perX) + y0);
                deltaY = Math.Clamp(deltaY, 0, 799);
                image[deltaX, deltaY] = color;
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

