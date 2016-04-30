using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace oprCourseSoloviev
{
    public class ParamBoundaries
    {
        public ParamBoundaries(double x1Left, double x1Right, double x2Left, double x2Right, double step)
        {
            this.X1Left = x1Left;
            this.X1Right = x1Right;
            this.X2Left = x2Left;
            this.X2Right = x2Right;
            this.Step = step;
        }

        public double X1Left { get; set; }
        public double X1Right { get; set; }
        public double X2Left { get; set; }
        public double X2Right { get; set; }
        public double Step { get; set; }
    }
}
