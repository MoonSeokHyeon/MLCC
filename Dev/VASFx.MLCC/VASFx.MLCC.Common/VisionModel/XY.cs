using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VASFx.MLCC.Common.VisionModel
{
    public class XY
    {
        public double X { get; set; }

        public double Y { get; set; }

        public XY()
        {
            this.X = 0.0;
            this.Y = 0.0;
        }

        public XY(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        public XY(XY other)
        {
            this.X = other.X;
            this.Y = other.Y;
        }

        public static XY operator +(XY e1, XY e2)
        {
            if (e1 == null)
                e1 = new XY();
            if (e2 == null)
                e2 = new XY();
            return new XY() { X = e1.X + e2.X, Y = e1.Y + e2.Y };
        }

        public static XYT operator +(XY e1, XYT e2)
        {
            if (e1 == null)
                e1 = new XY();
            if (e2 == null)
                e2 = new XYT();
            return new XYT()
            {
                X = e1.X + e2.X,
                Y = e1.Y + e2.Y,
                T = e2.T
            };
        }

        public static XY operator -(XY e1, XY e2)
        {
            if (e1 == null)
                e1 = new XY();
            if (e2 == null)
                e2 = new XY();
            return new XY() { X = e1.X - e2.X, Y = e1.Y - e2.Y };
        }

        public static XYT operator -(XY e1, XYT e2)
        {
            if (e1 == null)
                e1 = new XY();
            if (e2 == null)
                e2 = new XYT();
            return new XYT()
            {
                X = e1.X - e2.X,
                Y = e1.Y - e2.Y,
                T = e2.T
            };
        }

        public static XY operator *(XY e1, XY e2)
        {
            if (e1 == null)
                e1 = new XY();
            if (e2 == null)
                e2 = new XY();
            return new XY() { X = e1.X * e2.X, Y = e1.Y * e2.Y };
        }

        public static XYT operator *(XY e1, XYT e2)
        {
            if (e1 == null)
                e1 = new XY();
            if (e2 == null)
                e2 = new XYT();
            return new XYT()
            {
                X = e1.X * e2.X,
                Y = e1.Y * e2.Y,
                T = e2.T
            };
        }

        public static XY operator /(XY e1, XY e2)
        {
            if (e1 == null)
                e1 = new XY();
            if (e2 == null)
                e2 = new XY();
            return new XY() { X = e1.X / e2.X, Y = e1.Y / e2.Y };
        }

        public static XYT operator /(XY e1, XYT e2)
        {
            if (e1 == null)
                e1 = new XY();
            if (e2 == null)
                e2 = new XYT();
            return new XYT()
            {
                X = e1.X / e2.X,
                Y = e1.Y / e2.Y,
                T = e2.T
            };
        }

        public static XY operator *(XY e1, double mul)
        {
            if (e1 == null)
                e1 = new XY();
            return new XY() { X = e1.X * mul, Y = e1.Y * mul };
        }

        public static XY operator /(XY e1, double div)
        {
            if (e1 == null)
                e1 = new XY();
            return new XY() { X = e1.X / div, Y = e1.Y / div };
        }

        public static bool Equal(XY e1, XY e2)
        {
            if (e1 == null)
                e1 = new XY();
            if (e2 == null)
                e2 = new XY();
            return e1.X == e2.X && e1.Y == e2.Y;
        }

        public static double GetDistance(XY e1, XY e2)
        {
            if (e1 == null)
                e1 = new XY();
            if (e2 == null)
                e2 = new XY();
            return Math.Sqrt((e2.X - e1.X) * (e2.X - e1.X) + (e2.Y - e1.Y) * (e2.Y - e1.Y));
        }

        public static double GetLength(XY e1)
        {
            if (e1 == null)
                e1 = new XY();
            return XY.GetDistance(e1, new XY());
        }

        public static XY Normalize(XY e)
        {
            return e * (1.0 / XY.GetLength(e));
        }

        public static XY RotationDegree(XY e, double angle_degree)
        {
            double angle_rad = angle_degree * Math.PI / 180.0;
            return XY.Rotation(e, angle_rad);
        }

        public static XY Rotation(XY e, double angle_rad)
        {
            return new XY(Math.Cos(angle_rad) * e.X - Math.Sin(angle_rad) * e.Y, Math.Sin(angle_rad) * e.X + Math.Cos(angle_rad) * e.Y);
        }

        public override string ToString()
        {
            return string.Format("{0} :\n X: {1}\n Y: {2}\n", (object)typeof(XY).ToString(), (object)this.X, (object)this.Y);
        }
    }
}
