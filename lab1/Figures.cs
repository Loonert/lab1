using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab1
{
    class Figure
    {
        public string Owner { get; set; }
        public double Density { get; set; }

        public Figure(string owner, double density)
        {
            Owner = owner;
            Density = density;
        }

        public virtual void Print(string operation = "")
        {

        }
    }

    class Sphere : Figure
    {
        public double Radius { get; set; }

        public Sphere(double radius, double density, string owner)
            : base(owner, density)
        {
            Radius = radius;
        }

        public override void Print(string operation)
        {
            Console.WriteLine($"{operation} Sphere: Radius={Radius}, Density={Density}, Owner={Owner}");
        }
    }

    class Parallelepiped : Figure
    {
        public double A { get; set; }
        public double B { get; set; }
        public double C { get; set; }

        public Parallelepiped(double a, double b, double c, double density, string owner)
            : base(owner, density)
        {
            A = a;
            B = b;
            C = c;
        }

        public override void Print(string operation)
        {
            Console.WriteLine($"{operation} Parallelepiped: a={A}, b={B}, c={C}, Density={Density}, Owner={Owner}");
        }
    }

    class Cylinder : Figure
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public double Radius { get; set; }
        public double Height { get; set; }

        public Cylinder(double x, double y, double z, double radius, double height, double density, string owner)
            : base(owner, density)
        {
            X = x;
            Y = y;
            Z = z;
            Radius = radius;
            Height = height;
        }

        public override void Print(string operation)
        {
            Console.WriteLine($"{operation} Cylinder: (x={X}, y={Y}, z={Z}), Radius={Radius}, Height={Height}, Density={Density}, Owner={Owner}");
        }
    }
}
