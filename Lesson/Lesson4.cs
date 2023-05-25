using System;
using System.Numerics;

namespace tiny_renderer
{
    public class Lesson4:ILesson
    {
        public Lesson4()
        {
        }

        public void Start()
        {
            //透视投影
            Matrix4x4 matrix4X4 = Matrix4x4.CreateScale(Vector3.One);
            //matrix4X4.M11 = 1;
            //matrix4X4.M14 = 1;
            //matrix4X4.M44 = 1;
            matrix4X4.M41 = 1;
            //matrix4X4=Matrix4x4.Transpose(matrix4X4);
            Image<Rgba32> image = new Image<Rgba32>(800,800,Color.Black);
            for (int x = 200; x < 300; x++)
            {
                int y = x;
                image[x, y] = Color.White;
                image[x, 800 - y-1] = Color.White;
                Vector2 v = new Vector2(x, y);
                Vector2 tv = Vector2.Transform(v, matrix4X4);
                image[(int)tv.X, (int)tv.Y] = Color.Red;
                v = new Vector2(x, 800 - y - 1);
                tv = Vector2.Transform(v, matrix4X4);
                image[(int)tv.X, (int)tv.Y] = Color.Green;
            }
            image.Save("Matrix.png");
            Console.WriteLine("Matrix");
            /* 1 0 0 1
             * 0 1 0 0
             * 0 0 1 0
             * 0 0 0 1
             * 
             * */
        }
    }
}

