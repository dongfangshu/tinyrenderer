using System;
namespace tiny_renderer
{
    public struct Box
    {
        public int minX;
        public int minY;
        public int maxX;
        public int maxY;
        public int Width
        {
            get
            {
                return maxX - minX;
            }
        }
        public int Height
        {
            get
            {
                return maxY - minY;
            }
        }
    }
}

