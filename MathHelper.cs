using System;
using System.Numerics;

namespace tiny_renderer
{
    public static class MathHelper
    {
        public static void Swap(ref int a, ref int b)
        {
            int tmp = a;
            a = b;
            b = tmp;
        }
        public static void Swap(ref Vector2 v1, ref Vector2 v2)
        {
            Vector2 tmp = v1;
            v1 = v2;
            v2 = tmp;
        }
    }
}

