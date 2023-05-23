using System;
using System.ComponentModel;

namespace tiny_renderer.Lesson
{
    public class Lesson3:ILesson
    {
        public Lesson3()
        {
        }

        public void Start()
        {
            int width = 800;
            int height = 800;
            Image<Rgba32> image = new Image<Rgba32>(width, height, Color.Black);
            // scene "2d mesh"
            Graphics.DrawLine_GeneralEquation(image, 20, 34, 744, 400,Color.Red);
            Graphics.DrawLine_GeneralEquation(image, 120, 434, 444, 400, Color.Green);
            Graphics.DrawLine_GeneralEquation(image, 330, 463, 594, 200, Color.Blue);
            // screen line
            Graphics.DrawLine_GeneralEquation(image, 10, 10, 790, 10, Color.White);
            string path = Environment.CurrentDirectory + "/Lesson3.png";
            image.SaveAsPng(path);
        }
    }
}

