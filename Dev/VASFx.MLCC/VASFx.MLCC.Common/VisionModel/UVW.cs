using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VASFx.MLCC.Common.VisionModel
{
    public class UVW
    {
        public double U { get; set; }

        public double V { get; set; }

        public double W { get; set; }

        public UVW()
        {
            this.U = 0.0;
            this.V = 0.0;
            this.W = 0.0;
        }

        public UVW(UVW uvw)
        {
            this.U = uvw.U;
            this.V = uvw.V;
            this.W = uvw.W;
        }

        public UVW(double u, double v, double w)
        {
            this.U = u;
            this.V = v;
            this.W = w;
        }

        public static UVW operator +(UVW e1, UVW e2)
        {
            if (e1 == null)
                e1 = new UVW();
            if (e2 == null)
                e2 = new UVW();
            return new UVW()
            {
                U = e1.U + e2.U,
                V = e1.V + e2.V,
                W = e1.W + e2.W
            };
        }

        public static UVW operator -(UVW e1, UVW e2)
        {
            if (e1 == null)
                e1 = new UVW();
            if (e2 == null)
                e2 = new UVW();
            return new UVW()
            {
                U = e1.U - e2.U,
                V = e1.V - e2.V,
                W = e1.W - e2.W
            };
        }

        public static UVW operator *(UVW e1, UVW e2)
        {
            if (e1 == null)
                e1 = new UVW();
            if (e2 == null)
                e2 = new UVW();
            return new UVW()
            {
                U = e1.U * e2.U,
                V = e1.V * e2.V,
                W = e1.W * e2.W
            };
        }

        public static UVW operator /(UVW e1, UVW e2)
        {
            if (e1 == null)
                e1 = new UVW();
            if (e2 == null)
                e2 = new UVW();
            return new UVW()
            {
                U = e1.U / e2.U,
                V = e1.V / e2.V,
                W = e1.W / e2.W
            };
        }

        public static UVW operator *(UVW e1, double mul)
        {
            if (e1 == null)
                e1 = new UVW();
            return new UVW()
            {
                U = e1.U * mul,
                V = e1.V * mul,
                W = e1.W * mul
            };
        }

        public static UVW operator /(UVW e1, double div)
        {
            if (e1 == null)
                e1 = new UVW();
            return new UVW()
            {
                U = e1.U / div,
                V = e1.V / div,
                W = e1.W / div
            };
        }

        public static bool Equal(UVW e1, UVW e2)
        {
            if (e1 == null)
                e1 = new UVW();
            if (e2 == null)
                e2 = new UVW();
            return e1.U == e2.U && e1.V == e2.V && e1.W == e2.W;
        }

        public override string ToString()
        {
            return string.Format("{0} :\n U: {1}\n V: {2}\n W: {3}\n", (object)typeof(UVW).ToString(), (object)this.U, (object)this.V, (object)this.W);
        }
    }
}
