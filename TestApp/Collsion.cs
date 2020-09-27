using System;
using System.Collections.Generic;
using System.Windows.Shapes;
using System.Text;

namespace TestApp
{
    public static class Collsion
    {
        public enum Side
        {
            Top, Bottom, Left, Right, None
        }

        public static Side RectColidedSide(BetterShape r1, BetterShape r2)
        {
            if (r1.Left < r2.Right) return Side.Right;
            else if (r1.Right > r2.Left) return Side.Left;
            else if (r1.Top < r2.Bottom) return Side.Top;
            else if (r1.Bottom > r2.Top) return Side.Bottom;
            else return Side.None;
        }

        public static bool Any(BetterShape s1, BetterShape s2)
        {
            if (s1.shape is Rectangle && s2.shape is Rectangle)
                return RectRect(s1, s2);
            else if (s1.shape is Ellipse && s2.shape is Rectangle)
                return CircleRect(s1, s2);
            else if (s1.shape is Rectangle && s2.shape is Ellipse)
                return CircleRect(s2, s1);
            else if (s1.shape is Ellipse && s2.shape is Ellipse)
                return CircleCircle(s1, s2);
            else return false;
        }

        public static bool RectRect(BetterShape r1, BetterShape r2)
        {
            if (r1.Left < r2.Right && r1.Right > r2.Left &&
                r1.Top < r2.Bottom && r1.Bottom > r2.Top) return true;
            else return false;
        }

        public static bool CircleRect(BetterShape c1, BetterShape r2)
        {
            double circleX = c1.X;
            double circleY = c1.Y;

            if (c1.X < r2.Left) circleX = r2.Left;
            else if (c1.X > r2.Right) circleX = r2.Right;

            if (c1.Y < r2.Top) circleY = r2.Top;
            else if (c1.Y > r2.Bottom) circleY = r2.Bottom;

            double distX = c1.X - circleX;
            double distY = c1.Y - circleY;
            double distance = Math.Sqrt((distX * distX) + (distY * distY));

            if (distance <= c1.Height / 2)
                return true;
            else return false;
        }

        public static bool CircleCircle(BetterShape c1, BetterShape c2)
        {
            double x = c1.X - c2.X;
            double y = c1.Y - c2.Y;
            double distance = Math.Sqrt((x * x) + (y * y));
            if (distance < c1.Height / 2 + c2.Height / 2) return true;
            else return false;
        }
    }
}
