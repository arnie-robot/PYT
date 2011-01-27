using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PYT
{
    class EquationParameters
    {
        // parameters to be used in equations
        protected double a;
        protected double b;
        protected double d;
        protected double f;

        // input values
        protected double start;
        protected double end;

        /**
         * Constructs the parameters, computing the values for the internal numbers
         * 
         * @param float start       the start point
         * @param float end         the end point
         * 
         * @return EquationParameters
         */
        public EquationParameters(double start, double end)
        {
            this.start = start;
            this.end = end;
            this.a = 0.5 * (end - start);
            this.b = (15.0 / 16.0) * (end - start);
            this.d = -(5.0 / 8.0) * (end - start);
            this.f = (3.0 / 16.0) * (end - start);
        }

        /**
         * Compute the equation's value at the given point
         * 
         * @param double value      the point to compute at
         * 
         * @return double
         */
        public double Compute(double value)
        {
            return (this.a + (this.b*value) + (this.d*value*value*value) + (this.f*value*value*value*value*value) + this.start);
        }
    }
}
