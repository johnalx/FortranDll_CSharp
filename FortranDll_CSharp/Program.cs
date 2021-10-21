using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Fortran_Dll_CS
{
    public class Curve
    {
        public Curve(int size)
        {
            this.size = size;
            this.x = new double[size];
            this.y = new double[size];
        }
        public Curve(int size, double[] x, double[] y)
        {
            this.size = size;
            this.x = x;
            this.y = y;
        }
        readonly int size;
        readonly double[] x;
        readonly double[] y;

        public int Size => size;
        public double[] X => x;
        public double[] Y => y;

        #region Formatting
        public override string ToString() => ToString("g");
        public string ToString(string formatting) => ToString(formatting, null);
        public string ToString(string format, IFormatProvider provider)
        {
            return $@"Curve:
X: {string.Join(",", x.Take(size).Select((v) => v.ToString(format, provider)))}
Y: {string.Join(",", y.Take(size).Select((v) => v.ToString(format, provider)))}";
        }
        #endregion
    }

    static class Program
    {
        #region Fortran
        [DllImport("FortranLib.dll", EntryPoint = "adder")]
        static extern void adder(int a, int b, [Out] out int x, [Out] out int y);

        [DllImport("FortranLib.dll", EntryPoint = "fill_curve")]
        static extern void fill_curve(
            int n, 
            double[] x, 
            double[] y);

        #endregion
        static void Main(string[] args)
        {
            int a = 4;
            int b = 3;
            int x = 0;
            int y = 0;

            Console.WriteLine($"a={a}, b={b}, x={x}, y={y}");
            // a = 4, b = 3, x = 0, y = 0

            adder(a, b, out x, out y);

            Console.WriteLine($"a={a}, b={b}, x={x}, y={y}");
            // a = 4, b = 3, x = 17, y = 7

            int n = 10;
            double[] xc = new double[n];
            double[] yc = new double[n];
            fill_curve(n, xc, yc);

            var curve = new Curve(n, xc, yc);

            Console.WriteLine(curve.ToString("G2"));
        }
    }
}
