using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VASFx.MLCC.Common.VisionModel;

namespace VASFx.MLCC.Common.Utils
{
    public static class GMathUtil
    {
        public static XYT ApplyTargetOffset(XYT point, XYT offset)
        {
            XYT xyt = new XYT();
            double t = point.T;
            xyt.X = point.X + Math.Cos(t) * offset.X - Math.Sin(t) * offset.Y;
            xyt.Y = point.Y + Math.Sin(t) * offset.X + Math.Cos(t) * offset.Y;
            xyt.T = point.T + offset.T * (Math.PI / 180.0);

            return xyt;
        }

        public static XYT ApplyTargetOffset(XY point1, XY point2, object offset)
        {
            double num = NormalizeTheta(GetTheta(point1.X, point1.Y, point2.X, point2.Y));
            double x1 = 0.0;
            double y1 = 0.0;
            double x2 = 0.0;
            double y2 = 0.0;

            if (offset.GetType() == typeof(XY))
            {
                XY xy2 = offset as XY;
                XY xy3 = (point1 + point2) / 0.5;
                return ApplyTargetOffset(new XYT(xy3.X, xy3.Y, 0.0), new XYT(xy2));
            }

            if (offset.GetType() == typeof(XYT))
            {
                XYT _offset1 = offset as XYT;
                XY xy2 = (point1 + point2) * 0.5;
                return ApplyTargetOffset(new XYT(xy2.X, xy2.Y, num), _offset1);
            }

            XYT xyt = new XYT()
            {
                X = (x1 + x2) / 2.0,
                Y = (y1 + y2) / 2.0,
                T = GetTheta(x1, y1, x2, y2)
            };
            xyt.T = NormalizeTheta(xyt.T);

            return xyt;
        }

        public static double GetTheta(double x1, double y1, double x2, double y2)
        {
            double num1 = x2 - x1;
            double num2 = y2 - y1;

            return num1 == 0.0 ? Math.PI / 2.0 : Math.Atan(num2 / num1);
        }

        public static double GetThetaColumn(double x1, double x2, double y1, double y2)
        {
            double num1 = x2 - x1;
            double num2 = y2 - y1;

            return num2 != 0.0 ? NormalizeTheta(Math.Atan(num2 / num1)) : 0.0;
        }

        public static void GetThetaColumn(double x1, double x2, double y1, double y2, ref double dt)
        {
            double num1 = x2 - x1;
            double num2 = y2 - y1;
            if (num2 != 0.0)
            {
                dt = Math.Atan(num2 / num1);
                dt = NormalizeTheta(dt);
            }
            else
                dt = 0.0;
        }

        public static double GetTheta(XY point1, XY point2)
        {
            var dx = point2.X - point1.X;
            var dy = point2.Y - point1.Y;

            return dy == 0.0 ? Math.PI / 2.0 : Math.Atan(dy / dx);
        }

        public static double GetTheta(XYT point1, XYT point2)
        {
            var dx = point2.X - point1.X;
            var dy = point2.Y - point1.Y;

            return dx == 0.0 ? Math.PI / 2.0 : Math.Atan(dy / dx);
        }

        public static double NormalizeTheta(double radian)
        {
            var theta = radian + Math.PI;
            while (theta > Math.PI / 4.0)
                theta -= Math.PI / 2.0;

            return theta;
        }

        public static XY GetPointMovedTheta(XY point, double degree)
        {
            XY moved = new XY();

            var theta = degree * (Math.PI / 180);

            moved.X = Math.Cos(theta) * point.X - Math.Sin(theta) * point.Y;
            moved.Y = Math.Sin(theta) * point.X + Math.Cos(theta) * point.Y;

            return moved;
        }

        public static double GetCrossDistance(XY point, XY startPoint, XY endPoint)
        {
            return Math.Abs(CrossProduct(point, startPoint, endPoint));
        }

        public static double GetDotDistance(XY point, XY startPoint, XY endPoint)
        {
            return Math.Abs(DotProduct(point, startPoint, endPoint));
        }

        public static double CrossProduct(XY point, XY startPoint, XY endPoint)
        {
            double length = GetLength(startPoint, endPoint);
            XY xy1 = new XY()
            {
                X = point.X - startPoint.X,
                Y = point.Y - startPoint.Y
            };
            XY xy2 = new XY()
            {
                X = (endPoint.X - startPoint.X) / length,
                Y = (endPoint.Y - startPoint.Y) / length
            };
            return xy2.X * xy1.Y - xy2.Y * xy1.X;
        }

        public static XY GetRotationPoint(XY point)
        {
            XY xy = new XY();

            xy.X = Math.Round(point.X * Math.Cos(Math.PI / 2) - point.Y * Math.Sin(Math.PI / 2));
            xy.Y = Math.Round(point.X * Math.Sin(Math.PI / 2) + point.Y * Math.Cos(Math.PI / 2));

            return xy;
        }

        public static XY GetCrossPoint(XY point, XY startPoint, XY endPoint)
        {
            XY xy = new XY();
            if (endPoint.X == startPoint.X)
            {
                if (endPoint.Y == startPoint.Y)
                {
                    xy.X = startPoint.X;
                    xy.Y = startPoint.Y;
                }
                xy.X = startPoint.X - point.X;
                xy.Y = point.Y;
            }
            else if (endPoint.Y == startPoint.Y)
            {
                xy.X = point.X;
                xy.Y = startPoint.Y - point.Y;
            }
            else
            {
                double num1 = (endPoint.Y - startPoint.Y) / (endPoint.X - startPoint.X);
                double num2 = -1.0 / num1;
                double num3 = startPoint.Y - num1 * startPoint.X;
                double num4 = point.Y - num2 * point.X;
                xy.X = (num4 - num3) / (num1 - num2);
                xy.Y = num2 * xy.X + num4;
            }

            return xy;
        }

        public static double DotProduct(XY point, XY startPoint, XY endPoint)
        {
            double length = GetLength(startPoint, endPoint);
            XY xy1 = new XY()
            {
                X = point.X - startPoint.X,
                Y = point.Y - startPoint.Y
            };
            XY xy2 = new XY()
            {
                X = (endPoint.X - startPoint.X) / length,
                Y = (endPoint.Y - startPoint.Y) / length
            };

            return xy2.X * xy1.X + xy2.Y * xy1.Y;
        }

        public static double GetLength(XY startPoint, XY endPoint)
        {
            return Math.Sqrt((endPoint.X - startPoint.X) * (endPoint.X - startPoint.X) + (endPoint.Y - startPoint.Y) * (endPoint.Y - startPoint.Y));
        }

        public static double GetLength(XYT startPoint, XYT endPoint)
        {
            return Math.Sqrt((endPoint.X - startPoint.X) * (endPoint.X - startPoint.X) + (endPoint.Y - startPoint.Y) * (endPoint.Y - startPoint.Y));
        }

        public static double RadianToDegree(double radian)
        {
            return radian / (180.0 / Math.PI);
        }

        public static double DegreeToRadian(double degree)
        {
            return degree * (Math.PI / 180.0);
        }
    }
}
