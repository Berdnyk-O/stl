using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStl
{
    internal class Program
    {
        static Point[] vertex = new Point[6];
        static Point[] normals = new Point[8];
        static double distance;
        static void Main()
        {
            double side = 0;
            bool success = false;
            while (!success)
            {
                Console.Write("Задайте довжину сторони правильного октаедра: ");
                success = double.TryParse(Console.ReadLine(), out side);
            }
            distance = side*Math.Sqrt(2) / 2;
            SearchVertex();
            SearchNormals();
            WriteFile();
            Console.WriteLine("success");
        }
        static void WriteFile()
        {
            StreamWriter streamWriter = new StreamWriter("Mystlfile.stl", false);
            string text = $"solid octahedron\nfacet normal {normals[0].X} {normals[0].Y} {normals[0].Z}\nouter loop\nvertex {vertex[0].X} {vertex[0].Y} {vertex[0].Z}\n" +
                $"vertex {vertex[5].X} {vertex[5].Y} {vertex[5].Z}\nvertex {vertex[1].X} {vertex[1].Y} {vertex[1].Z}\nendloop\nendfacet\n" +
                $"facet normal {normals[1].X} {normals[1].Y} {normals[1].Z}\nouter loop\nvertex {vertex[5].X} {vertex[5].Y} {vertex[5].Z}\n" +
                $"vertex {vertex[3].X} {vertex[3].Y} {vertex[3].Z}\nvertex {vertex[1].X} {vertex[1].Y} {vertex[1].Z}\nendloop\nendfacet\n" +
                $"facet normal {normals[2].X} {normals[2].Y} {normals[2].Z}\nouter loop\nvertex {vertex[1].X} {vertex[1].Y} {vertex[1].Z}\n" +
                $"vertex {vertex[3].X} {vertex[3].Y} {vertex[3].Z}\nvertex {vertex[2].X} {vertex[2].Y} {vertex[2].Z}\nendloop\nendfacet\n" +
                $"facet normal {normals[3].X} {normals[3].Y} {normals[3].Z}\nouter loop\nvertex {vertex[0].X} {vertex[0].Y} {vertex[0].Z}\n" +
                $"vertex {vertex[1].X} {vertex[1].Y} {vertex[1].Z}\nvertex {vertex[2].X} {vertex[2].Y} {vertex[2].Z}\nendloop\nendfacet\n" +
                $"facet normal {normals[4].X} {normals[4].Y} {normals[4].Z}\nouter loop\nvertex {vertex[0].X} {vertex[0].Y} {vertex[0].Z}\n" +
                $"vertex {vertex[2].X} {vertex[2].Y} {vertex[2].Z}\nvertex {vertex[4].X} {vertex[4].Y} {vertex[4].Z}\nendloop\nendfacet\n" +
                $"facet normal {normals[5].X} {normals[5].Y} {normals[5].Z}\nouter loop\nvertex {vertex[0].X} {vertex[0].Y} {vertex[0].Z}\n" +
                $"vertex {vertex[4].X} {vertex[4].Y} {vertex[4].Z}\nvertex {vertex[5].X} {vertex[5].Y} {vertex[5].Z}\nendloop\nendfacet\n" +
                $"facet normal {normals[6].X} {normals[6].Y} {normals[6].Z}\nouter loop\nvertex {vertex[5].X} {vertex[5].Y} {vertex[5].Z}\n" +
                $"vertex {vertex[4].X} {vertex[4].Y} {vertex[4].Z}\nvertex {vertex[3].X} {vertex[3].Y} {vertex[3].Z}\nendloop\nendfacet\n" +
                $"facet normal {normals[7].X} {normals[7].Y} {normals[7].Z}\nouter loop\nvertex {vertex[3].X} {vertex[3].Y} {vertex[3].Z}\n" +
                $"vertex {vertex[4].X} {vertex[4].Y} {vertex[4].Z}\nvertex {vertex[2].X} {vertex[2].Y} {vertex[2].Z}\nendloop\nendfacet\nendsolid octahedron";
            streamWriter.Write(text);
            streamWriter.Close();
        }
        static void SearchVertex()
        {
            vertex[0] = new Point(distance, 0, 0); //права 
            vertex[1] = new Point(0, distance, 0); //верхня
            vertex[2] = new Point(0, 0, distance); //передня
            vertex[3] = new Point(-distance, 0, 0); //ліва
            vertex[4] = new Point(0, -distance, 0); //нижня
            vertex[5] = new Point(0, 0, -distance); //задня
        }
        static void SearchNormals()
        {
            Point a = new Point(vertex[1].X - vertex[0].X, vertex[1].Y - vertex[0].Y, vertex[1].Z - vertex[0].Z);
            Point b = new Point(vertex[5].X - vertex[0].X, vertex[5].Y - vertex[0].Y, vertex[5].Z - vertex[0].Z);
            Normal(a, b, 0);

            a = Vector(1, 3);
            b = Vector(5, 3); //верхня, ліва, задня сторона (v3,v1,v5)
            Normal(a, b, 1);

            a = Vector(2, 3);
            b = Vector(1, 3); //верхня, ліва, передня сторона (v3,v1,v2)
            Normal(a, b, 2);

            a = Vector(0, 2);
            b = Vector(1, 2); //верхня, права, передня сторона (v0,v1,v2)
            Normal(a, b, 3);

            a = Vector(0, 2);
            b = Vector(4, 2); //нижня, права, передня сторона (v0,v2,v4)
            Normal(a, b, 4);

            a = Vector(5, 0);
            b = Vector(4, 0); //нижня, права задня сторона (v0,v4,v5)
            Normal(a, b, 5);

            a = Vector(4, 3);
            b = Vector(5, 3);  //нижня, ліва, задня сторона (v3,v4,v5)
            Normal(a, b, 6);

            a = Vector(2, 3);
            b = Vector(4, 3); //нижня, ліва, передня сторона (v3,v4,v2)
            Normal(a, b, 7);
        }
        static Point Vector(int i, int j)
        {
            return new Point(vertex[i].X - vertex[j].X, vertex[i].Y - vertex[j].Y, vertex[i].Z - vertex[j].Z);
        }
        static void Normal(Point a, Point b, int i)
        {
            normals[i] = new Point();
            normals[i].X = (a.Z * b.Y) - (a.Y * b.Z);
            normals[i].Y = (a.X * b.Z) - (a.Z * b.X);
            normals[i].Z = (a.Y * b.X) - (a.X * b.Y);
        }
        
    }
}
