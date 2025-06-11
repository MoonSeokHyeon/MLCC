using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VASFx.MLCC.Common.VisionModel
{
    public class XYT
    {
        public double X { get; set; }

        public double Y { get; set; }

        public double T { get; set; }

        public XYT()
        {
            this.X = 0.0;
            this.Y = 0.0;
            this.T = 0.0;
        }

        public XYT(XYT xyt)
        {
            this.X = xyt.X;
            this.Y = xyt.Y;
            this.T = xyt.T;
        }

        public XYT(double x, double y, double t)
        {
            this.X = x;
            this.Y = y;
            this.T = t;
        }

        public XYT(double x, double y)
        {
            this.X = x;
            this.Y = y;
            this.T = 0.0;
        }

        public XYT(XY xy)
        {
            this.X = xy.X;
            this.Y = xy.Y;
            this.T = 0.0;
        }

        public static XYT operator +(XYT e1, XYT e2)
        {
            if (e1 == null)
                e1 = new XYT();
            if (e2 == null)
                e2 = new XYT();
            return new XYT()
            {
                X = e1.X + e2.X,
                Y = e1.Y + e2.Y,
                T = e1.T + e2.T
            };
        }

        public static XYT operator -(XYT e1, XYT e2)
        {
            if (e1 == null)
                e1 = new XYT();
            if (e2 == null)
                e2 = new XYT();
            return new XYT()
            {
                X = e1.X - e2.X,
                Y = e1.Y - e2.Y,
                T = e1.T - e2.T
            };
        }

        public static XYT operator *(XYT e1, XYT e2)
        {
            if (e1 == null)
                e1 = new XYT();
            if (e2 == null)
                e2 = new XYT();
            return new XYT()
            {
                X = e1.X * e2.X,
                Y = e1.Y * e2.Y,
                T = e1.T * e2.T
            };
        }

        public static XYT operator /(XYT e1, XYT e2)
        {
            if (e1 == null)
                e1 = new XYT();
            if (e2 == null)
                e2 = new XYT();
            return new XYT()
            {
                X = e1.X / e2.X,
                Y = e1.Y / e2.Y,
                T = e1.T / e2.T
            };
        }

        public static XYT operator +(XYT e1, XY e2)
        {
            if (e1 == null)
                e1 = new XYT();
            if (e2 == null)
                e2 = new XY();
            return new XYT()
            {
                X = e1.X + e2.X,
                Y = e1.Y + e2.Y,
                T = e1.T
            };
        }

        public static XYT operator -(XYT e1, XY e2)
        {
            if (e1 == null)
                e1 = new XYT();
            if (e2 == null)
                e2 = new XY();
            return new XYT()
            {
                X = e1.X - e2.X,
                Y = e1.Y - e2.Y,
                T = e1.T
            };
        }

        public static XYT operator *(XYT e1, XY e2)
        {
            if (e1 == null)
                e1 = new XYT();
            if (e2 == null)
                e2 = new XY();
            return new XYT()
            {
                X = e1.X * e2.X,
                Y = e1.Y * e2.Y,
                T = e1.T
            };
        }

        public static XYT operator /(XYT e1, XY e2)
        {
            if (e1 == null)
                e1 = new XYT();
            if (e2 == null)
                e2 = new XY();
            return new XYT()
            {
                X = e1.X / e2.X,
                Y = e1.Y / e2.Y,
                T = e1.T
            };
        }

        public static XYT operator *(XYT e1, double mul)
        {
            if (e1 == null)
                e1 = new XYT();
            return new XYT()
            {
                X = e1.X * mul,
                Y = e1.Y * mul,
                T = e1.T * mul
            };
        }

        public static XYT operator /(XYT e1, double div)
        {
            if (e1 == null)
                e1 = new XYT();
            return new XYT()
            {
                X = e1.X / div,
                Y = e1.Y / div,
                T = e1.T / div
            };
        }

        public static bool Equal(XYT e1, XYT e2)
        {
            if (e1 == null)
                e1 = new XYT();
            if (e2 == null)
                e2 = new XYT();
            return e1.X == e2.X && e1.Y == e2.Y && e1.T == e2.T;
        }

        public override string ToString()
        {
            return string.Format("{0} :\n X: {1}\n Y: {2}\n T: {3}\n", (object)typeof(XYT).ToString(), (object)this.X, (object)this.Y, (object)this.T);
        }

        public XY ToXY()
        {
            return new XY(this.X, this.Y);
        }
    }
}
