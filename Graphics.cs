using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tiny_renderer
{
    public static class Graphics
    {
        public static void DrawLine2(Image<Rgba32> image, int x0, int y0, int x1, int y1, Color color)
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
                    MathHelper.Swap(ref y0, ref y1);
                }
                for (int y = y0; y < y1; y++)
                {
                    int x = -(b * y + c) / a;
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
            else
            {
                if (x0 > x1)
                {
                    MathHelper.Swap(ref x0, ref x1);
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
        public static void DrawLine(Image<Rgba32> image, int x0, int y0, int x1, int y1, Color color)
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
                if (y1 < y0)
                {
                    MathHelper.Swap(ref y1, ref y0);
                    MathHelper.Swap(ref x0, ref x1);
                }
                for (int deltaY = y0; deltaY < y1; deltaY++)
                {
                    float perY = (deltaY - y0) / (float)(y1 - y0);
                    int deltaX = (int)Math.Ceiling(((x1 - x0) * perY) + x0);
                    deltaX = Math.Clamp(deltaX, 0, 799);
                    image[deltaX, deltaY] = color;
                }
            }
            else
            {
                if (x0 > x1)
                {
                    MathHelper.Swap(ref x0, ref x1);
                    MathHelper.Swap(ref y0, ref y1);
                }
                for (int deltaX = x0; deltaX < x1; deltaX++)
                {
                    float perX = (deltaX - x0) / (float)(x1 - x0);
                    int deltaY = (int)Math.Ceiling(((y1 - y0) * perX) + y0);
                    deltaY = Math.Clamp(deltaY, 0, 799);
                    image[deltaX, deltaY] = color;
                }
            }
        }
    }
}
