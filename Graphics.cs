﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace tiny_renderer
{
    public static class Graphics
    {
        /// <summary>
        /// 变化增量式
        /// </summary>
        /// <param name="image"></param>
        /// <param name="x0"></param>
        /// <param name="y0"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="color"></param>
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
        /// <summary>
        /// 直线一般方程式 ax+by+c=0
        /// </summary>
        /// <param name="image"></param>
        /// <param name="x0"></param>
        /// <param name="y0"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="color"></param>
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
        /// <summary>
        /// 扫线法
        /// </summary>
        /// <param name="image"></param>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="v3"></param>
        public static void DrawTriangle(Image<Rgba32> image, Vector2 v1, Vector2 v2, Vector2 v3,Color color)
        {
            //  这里到原点在左上角，和教程不一样。y越大越往下。所以这里是上半部分，教程是下半部分
            if (v1.Y > v2.Y)
            {
                MathHelper.Swap(ref v1, ref v2);
            }
            if (v1.Y > v3.Y)
            {
                MathHelper.Swap(ref v1, ref v3);
            }
            if (v2.Y > v3.Y)
            {
                MathHelper.Swap(ref v2, ref v3);
            }
            //从上到下 v1 v2 v3

            //上半部分
            float totalHeight = v3.Y - v1.Y;
            float top_half_segmentHeight = v2.Y - v1.Y;
            for (float y = v1.Y; y < v2.Y; y++)
            {
                float a = (y - v1.Y) / totalHeight;
                float b = (y - v1.Y) / top_half_segmentHeight;
                Vector2 A = v1 + (v3 - v1) * a;
                Vector2 B = v1 + (v2 - v1) * b;
                int left = (int)Math.Ceiling(A.X);
                int right = (int)Math.Ceiling(B.X);
                if (left > right)
                {
                    MathHelper.Swap(ref left, ref right);
                }
                for (int x = left; x < right; x++)
                {
                    image[x, (int)y] = color;
                }
            }
            //下半部分
            float upper_half_segmentHeight = v3.Y - v2.Y;
            for (float y = v2.Y; y < v3.Y; y++)
            {
                float a = (y - v1.Y) / totalHeight;
                float b = (y - v2.Y) / upper_half_segmentHeight;
                Vector2 A = v1 + (v3 - v1) * a;
                Vector2 B = v2 + (v3 - v2) * b;
                int left = (int)Math.Ceiling(A.X);
                int right = (int)Math.Ceiling(B.X);
                if (left > right)
                {
                    MathHelper.Swap(ref left, ref right);
                }
                for (int x = left; x < right; x++)
                {
                    image[x, (int)y] = color;
                }
            }
        }
        /// <summary>
        /// 叉乘法
        /// </summary>
        /// <param name="image"></param>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="v3"></param>
        public static void DrawTriangle2(Image<Rgba32> image, Vector2 v1, Vector2 v2, Vector2 v3,Color color)
        {
            //box
            Box box = GetBox(new Vector2[3] {v1,v2,v3 });
            for (float x = box.minX; x < box.maxX ; x++)
            {
                for (float y = box.minY; y < box.maxY; y++)
                {
                    Vector2 point = new Vector2(x,y);
                    if (InTriangle(point,v1,v2,v3))
                    {
                        image[(int)x, (int)y] = color;
                    }
                }
            }
        }
        public static Box GetBox(Vector2[] points)
        {
            float minX = 0;
            float maxX = 0;
            float minY = 0;
            float maxY = 0;
            for (int i = 0; i < points.Length; i++)
            {
                minX = Math.Min(minX, points[i].X);
                maxX = Math.Max(maxX, points[i].X);
                minY = Math.Min(minY, points[i].Y);
                maxY = Math.Max(maxY, points[i].Y);
            }
            return new Box() { minX = minX,maxX=maxX,minY=minY,maxY=maxY};
        }
        /// <summary>
        /// 判断点是否在三角形中
        /// </summary>
        /// <param name="point"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool InTriangle(Vector2 point, Vector2 a, Vector2 b, Vector2 c)
        {
            bool azugament = PointInLineZugammen(a,point,b,c);
            bool bzugament = PointInLineZugammen(b,point,a,c);
            bool czugament = PointInLineZugammen(c,point,a,b);
            return azugament && bzugament && czugament;
        }
        /// <summary>
        /// 判断两点是否在同侧
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="lineStart"></param>
        /// <param name="lineEnd"></param>
        /// <returns></returns>
        public static bool PointInLineZugammen(Vector2 a,Vector2 b,Vector2 lineStart,Vector2 lineEnd)
        {
            Vector2 lineVector = lineEnd - lineStart;
            Vector2 av = a - lineStart;
            Vector2 bv = b - lineStart;
            float across = Cross(av,lineVector);
            float bcross = Cross(bv,lineVector);
            return (across < 0 && bcross < 0) || (across > 0 && bcross > 0);
        }
        public static float Cross(Vector2 v1, Vector2 v2)
        {
            return v1.X * v2.Y - v1.Y * v2.X;
        }
    }
}