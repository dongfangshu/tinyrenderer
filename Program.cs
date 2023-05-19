using SixLabors.ImageSharp.ColorSpaces;

namespace tiny_renderer;
class Program
{
    static void Main(string[] args)
    {
        //Console.WriteLine("Hello, World!");
        int width = 800;
        int height = 800;
        Image<Rgba32> image = new Image<Rgba32>(width, height);
        int centerX = width / 2;
        int centerY = height / 2;
        //左上角是原点
        DrawLine(image, centerX, centerY, centerX+100, centerY-100, Color.Red);//中心右上角45度
        DrawLine(image, centerX+100, centerY, centerX+100+100, centerY-100, Color.Yellow);//中心右上角45度平行线
        DrawLine(image, centerX, centerY, centerX+100, centerY, Color.Green);//水平直线
        DrawLine(image, centerX, centerY, centerX, centerY-100, Color.Black);//垂直直线
        //DrawLine(image,20,20,50,20,Color.Red);
        //DrawLine(image,20,20,50,50,Color.Red);
        string path = Environment.CurrentDirectory + "/framgent.png";
        image.SaveAsPng(path);
    }
    static void DrawLine(Image<Rgba32> image,int x0,int y0, int x1,int y1,Color color)
    {
        int offsetX = Math.Abs(x1-x0);
        int offsetY = Math.Abs(y1-y0);
        bool steep = false;
        if (offsetX<offsetY)
        {
            steep = true;
            Swap(ref x0,ref y0);
            Swap(ref x1,ref y1);
        }
        if (x0>x1)
        {
            Swap(ref x0,ref x1);
        }
        if (y0>y1)
        {
            Swap(ref y0,ref y1);
        }
        for (int deltaX = x0; deltaX <= x1; deltaX++)
        {
            float perX = (deltaX - x0) / (float)(x1 - x0);
            int deltaY = (int)Math.Ceiling((y1 - y0) * perX) +y0;
            if (steep)
            {
                image[deltaY, deltaX] = color;
            }
            else
            {
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

