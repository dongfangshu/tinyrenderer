using FileFormatWavefront;
using FileFormatWavefront.Model;
using SixLabors.ImageSharp.ColorSpaces;
using System.ComponentModel;
using System.Numerics;
using tiny_renderer.Lesson;
using Index = FileFormatWavefront.Model.Index;

namespace tiny_renderer;
class Program
{
    static void Main(string[] args)
    {
        //Console.WriteLine("Hello, World!");
        //Lesson1 lesson1 = new Lesson1();
        //lesson1.Start();

        //Lesson2 lesson2 = new Lesson2();
        //lesson2.Start();

        Lesson3 lesson3 = new Lesson3();
        lesson3.Start();
        //Test1();
        //Test2();
        //Test3();
        //TestMath();
    }
    static void TestMath()
    {
        Vector2 v1 = new Vector2(0,0);
        Vector2 v2 = new Vector2(3,0);
        Vector2 v3 = new Vector2(0,3);
        Vector2 p = new Vector2(1,1.5f);//在
        Vector2 p2 = new Vector2(3,1.5f);//不在
        Graphics.InTriangle(p, v1, v2, v3);
        Graphics.InTriangle(p2, v1, v2, v3);

        //float a= Graphics.Cross(new Vector2(3,1.5f),new Vector2(1,3));
        //float b= Graphics.Cross(new Vector2(3,1.5f),new Vector2(3,1));
    }
}

