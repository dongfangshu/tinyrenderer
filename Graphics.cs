using System;
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
        public static void DrawLine_Delta(Image<Rgba32> image, int x0, int y0, int x1, int y1, Color color)
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
        public static void DrawLine_GeneralEquation(Image<Rgba32> image, int x0, int y0, int x1, int y1, Color color)
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
        public static void DrawTriangle_LineSweeping(Image<Rgba32> image, Vector2 v1, Vector2 v2, Vector2 v3, Color color)
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
        /// 同侧法
        /// </summary>
        /// <param name="image"></param>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="v3"></param>
        public static void DrawTriangle_Zugammen(Image<Rgba32> image, Vector2 v1, Vector2 v2, Vector2 v3, Color color)
        {
            //box
            Box box = GetBox(new Vector2[3] { v1, v2, v3 });
            for (float x = box.minX; x < box.maxX; x++)
            {
                for (float y = box.minY; y < box.maxY; y++)
                {
                    Vector2 point = new Vector2(x, y);
                    if (InTriangle(point, v1, v2, v3))
                    {
                        image[(int)x, (int)y] = color;
                    }
                }
            }
        }
        /// <summary>
        /// 重心坐标法-数学计算uv版
        /// </summary>
        /// <param name="image"></param>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="v3"></param>
        public static void DrawTriangle_Barycentric(Image<Rgba32> image, Vector2 v1, Vector2 v2, Vector2 v3, Color color)
        {
            //box
            Box box = GetBox(new Vector2[3] { v1, v2, v3 });
            for (float x = box.minX; x < box.maxX; x++)
            {
                for (float y = box.minY; y < box.maxY; y++)
                {
                    Vector2 point = new Vector2(x, y);
                    if (Barycentric(point, v1, v2, v3))
                    {
                        image[(int)x, (int)y] = color;
                    }
                }
            }
        }
        /// <summary>
        /// 重心坐标法-线性叉乘求解版
        /// </summary>
        /// <param name="image"></param>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="v3"></param>
        public static void DrawTriangle_Barycentric_Cross(Image<Rgba32> image,Vector2 v1, Vector2 v2, Vector2 v3, Color color)
        {
            //box
            Box box = GetBox(new Vector2[3] { v1, v2, v3 });
            for (float x = box.minX; x < box.maxX; x++)
            {
                for (float y = box.minY; y < box.maxY; y++)
                {
                    Vector2 point = new Vector2(x, y);
                    Vector3 u = GetBarycentric(point, v1, v2, v3);
                    if (u.X<0||u.Y<0||u.Z<0)
                    {
                        continue;
                    }
                    image[(int)x, (int)y] = color;
                }
            }
        }
        public static Box GetBox(Vector2[] points)
        {
            int minX = int.MaxValue;
            int maxX = int.MinValue;
            int minY = int.MaxValue;
            int maxY = int.MinValue;
            for (int i = 0; i < points.Length; i++)
            {
                minX = Math.Min(minX, (int)points[i].X);
                maxX = Math.Max(maxX, (int)points[i].X);
                minY = Math.Min(minY, (int)points[i].Y);
                maxY = Math.Max(maxY, (int)points[i].Y);
            }
            return new Box() { minX = minX, maxX = maxX, minY = minY, maxY = maxY};
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
            bool azugament = PointInLineZugammen(a, point, b, c);
            if (!azugament)
            {
                return false;
            }
            bool bzugament = PointInLineZugammen(b, point, a, c);
            if (!bzugament)
            {
                return false;
            }
            bool czugament = PointInLineZugammen(c, point, a, b);
            if (!czugament)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 判断两点是否在同侧
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="lineStart"></param>
        /// <param name="lineEnd"></param>
        /// <returns></returns>
        public static bool PointInLineZugammen(Vector2 a, Vector2 b, Vector2 lineStart, Vector2 lineEnd)
        {
            Vector2 lineVector = lineEnd - lineStart;
            Vector2 av = a - lineStart;
            Vector2 bv = b - lineStart;
            float across = Cross(av, lineVector);
            float bcross = Cross(bv, lineVector);
            return across * bcross > 0;
        }
        public static float Cross(Vector2 v1, Vector2 v2)
        {
            return v1.X * v2.Y - v1.Y * v2.X;
        }
        /// <summary>
        /// 重心坐标法数学计算 判断点是否在三角形中
        /// </summary>
        /// <param name="point"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool Barycentric(Vector2 point, Vector2 a, Vector2 b, Vector2 c)
        {
            Vector2 pa = a - point;
            Vector2 ab = b - a;
            Vector2 ac = c - a;
            float v = (pa.X * ab.Y / ab.X - pa.Y) / (ac.Y - ac.X * ab.Y / ab.X);
            float u = -(pa.X + v * ac.X) / ab.X;
            return u >= 0 && v >= 0 && u + v <= 1;
        }
        /// <summary>
        /// 求p点相对重心坐标
        /// </summary>
        /// <param name="point"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static Vector3 GetBarycentric(Vector2 point, Vector2 a, Vector2 b, Vector2 c)
        {
            //ap=uab+vac
            //uap+vac+pa=0;
            //uab_x+vac_x+pa_x=0=>(u,v,1)[ab_x,ac_x,pa_x]=0
            //uab_y+vac_y+pa_y=0=>(u,v,1)[ab_y,ac_y,pa_y]=0
            //也就是说uv1垂直与[ab_x,ac_x,pa_x]和[ab_y,ac_y,pa_y]
            //求出(ku,kv,k)
            //所以u=u/k v=u/v
            //因为ap=uab+vac可以表示为p=(1-u-v)a+ub+vc
            //所以p点
            //ab
            //ac
            //pa
            Vector3 x = new Vector3(b.X-a.X,c.X-a.X,a.X-point.X);
            Vector3 y = new Vector3(b.Y-a.Y,c.Y-a.Y,a.Y-point.Y);
            Vector3 u = Vector3.Cross(y,x);//教程里原点是左下角，右手定则方向是向外。imageSharp原点在左上角，右手定则方向向里。这里应该反着乘
            //u=(u.x/u.z,u.y/u.z,1)
            //u=>u.x/u.z   v=>u.y/u.z
            if (Vector3.Abs(u).Z<=0)
            {
                return new Vector3(-1, 1, 1);//三点共线
            }
            return new Vector3(1f-(u.X+u.Y)/u.Z,u.X/u.Z,u.Y/u.Z);
        }
    }
}