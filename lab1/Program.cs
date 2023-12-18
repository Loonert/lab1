using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;

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

class Node
{
    public Figure Figure { get; set; }
    public Node Prev { get; set; }
    public Node Next { get; set; }

    public Node(Figure figure)
    {
        Figure = figure;
        Prev = null;
        Next = null;
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

class Container
{
    private Node head;
    private Node tail;

    public Container()
    {
        head = null;
        tail = null;
    }

    public void AddFigure(Figure figure)
    {
        Node newNode = new Node(figure);
        if (head == null)
        {
            head = newNode;
            tail = newNode;
        }
        else
        {
            tail.Next = newNode;
            newNode.Prev = tail;
            tail = newNode;
        }
        figure.Print("Added");
    }

    public void RemoveFiguresByCondition(string condition, int lineNumber)
    {
        Node current = head;
        while (current != null)
        {
            bool shouldRemove = CheckCondition(current.Figure, condition);
            if (shouldRemove)
            {
                current.Figure.Print($"Deleted because {condition} в строке {lineNumber}");
                Node nodeToRemove = current;
                current = current.Next;
                RemoveNode(nodeToRemove);
            }
            else
            {
                current = current.Next;
            }
        }
    }

    private bool CheckCondition(Figure figure, string condition)
    {
        string[] conditionParts = condition.Split(' ');
        string propertyName = conditionParts[0];
        string comparisonOperator = conditionParts[1];

        var property = figure.GetType().GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

        if (property == null)
        {
            return false;
        }

        object propertyValue = property.GetValue(figure);

        if (propertyName.ToLower() == "owner")
        {
            string ownerValue = conditionParts[2];
            switch (comparisonOperator)
            {
                case "==":
                    return Convert.ToString(propertyValue) == ownerValue;
                case "!=":
                    return Convert.ToString(propertyValue) != ownerValue;
                default:
                    return false;
            }
        }

        if (double.TryParse(conditionParts[2].Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out double numericValue))
        {
            switch (comparisonOperator)
            {
                case "==":
                    return Convert.ToDouble(propertyValue) == numericValue;
                case "!=":
                    return Convert.ToDouble(propertyValue) != numericValue;
                case ">":
                    return Convert.ToDouble(propertyValue) > numericValue;
                case "<":
                    return Convert.ToDouble(propertyValue) < numericValue;
                default:
                    return false;
            }
        }

        return false;
    }


    public void RemoveFiguresByType(string figureType, int lineNumber)
    {
        Node current = head;
        while (current != null)
        {
            if (current.Figure.GetType().Name.ToLower() == figureType.ToLower())
            {
                current.Figure.Print($"Deleted because type is {figureType}");
                Node nodeToRemove = current;
                current = current.Next;
                RemoveNode(nodeToRemove);
            }
            else
            {
                current = current.Next;
            }
        }
    }

    public void PrintFigures()
    {
        Node current = head;
        while (current != null)
        {
            current.Figure.Print();
            current = current.Next;
        }
    }

    private void RemoveNode(Node nodeToRemove)
    {
        if (nodeToRemove.Prev != null)
        {
            nodeToRemove.Prev.Next = nodeToRemove.Next;
        }
        else
        {
            head = nodeToRemove.Next;
        }

        if (nodeToRemove.Next != null)
        {
            nodeToRemove.Next.Prev = nodeToRemove.Prev;
        }
        else
        {
            tail = nodeToRemove.Prev;
        }

        nodeToRemove.Figure = null;
        nodeToRemove = null;
    }
}

class Program
{

    static void Main()
    {
        Container container = new Container();

        using (StreamReader inputFile = new StreamReader("data.txt"))
        {
            if (inputFile == null)
            {
                Console.Error.WriteLine("Не удалось открыть файл 'data.txt'");
                return;
            }

            int lineCounter = 0;
            string line;

            while ((line = inputFile.ReadLine()) != null)
            {
                lineCounter++;

                string[] commandParts = line.Split(' ');
                string command = commandParts[0];

                try
                {
                    if (command == "add")
                    {
                        if (commandParts.Length < 2)
                        {
                            Console.WriteLine($"Ошибка: Недостаточно параметров для команды 'add' в строке {lineCounter}.");
                            continue;
                        }

                        string type = commandParts[1];

                        if (type == "sphere" && commandParts.Length == 5)
                        {
                            if (double.TryParse(commandParts[2].Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out double radius) &&
                                double.TryParse(commandParts[3].Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out double density))
                            {
                                string owner = commandParts[4];
                                Sphere sphere = new Sphere(radius, density, owner);
                                container.AddFigure(sphere);
                            }
                            else
                            {
                                Console.WriteLine($"Ошибка: Неверные числовые параметры для команды 'add sphere' в строке {lineCounter}.");
                            }
                        }
                        else if (type == "parallelepiped" && commandParts.Length == 7)
                        {
                            if (double.TryParse(commandParts[2].Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out double a) &&
                                double.TryParse(commandParts[3].Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out double b) &&
                                double.TryParse(commandParts[4].Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out double c) &&
                                double.TryParse(commandParts[5].Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out double density))
                            {
                                string owner = commandParts[6];
                                Parallelepiped parallelepiped = new Parallelepiped(a, b, c, density, owner);
                                container.AddFigure(parallelepiped);
                            }
                            else
                            {
                                Console.WriteLine($"Ошибка: Неверные числовые параметры для команды 'add parallelepiped' в строке {lineCounter}.");
                            }
                        }
                        else if (type == "cylinder" && commandParts.Length == 9)
                        {
                            if (double.TryParse(commandParts[2].Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out double x) &&
                                double.TryParse(commandParts[3].Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out double y) &&
                                double.TryParse(commandParts[4].Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out double z) &&
                                double.TryParse(commandParts[5].Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out double radius) &&
                                double.TryParse(commandParts[6].Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out double height) &&
                                double.TryParse(commandParts[7].Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out double density))
                            {
                                string owner = commandParts[8];
                                Cylinder cylinder = new Cylinder(x, y, z, radius, height, density, owner);
                                container.AddFigure(cylinder);
                            }
                            else
                            {
                                Console.WriteLine($"Ошибка: Неверные числовые параметры для команды 'add cylinder' в строке {lineCounter}.");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Ошибка: Неизвестный тип фигуры или недостаточно параметров для команды 'add' в строке {lineCounter}.");
                        }
                    }
                    else if (command == "rem")
                    {
                        if (commandParts.Length == 4)
                        {
                            string condition = line.Substring(command.Length + 1);

                            string[] conditionParts = condition.Split(' ');
                            string propertyName = conditionParts[0];
                            string op = conditionParts[1];
                            string value = conditionParts[2];

                            if ((propertyName == "owner" || propertyName == "density" || propertyName == "radius" || propertyName == "a" || propertyName == "b" || propertyName == "c" || propertyName == "x" || propertyName == "y" || propertyName == "z" || propertyName == "height")
                            & (op == "==" || op == "!=" || op == ">" || op == "<"))
                            {
                                container.RemoveFiguresByCondition(condition, lineCounter);
                            }
                            else
                            {
                                Console.WriteLine($"Ошибка: Неверное свойство или оператор стравнения для условия удаления в строке {lineCounter}.");
                            }
                        }
                        else if (commandParts.Length == 2)
                        {
                            string figureType = commandParts[1];

                            if (figureType.ToLower() == "cylinder" || figureType.ToLower() == "sphere" || figureType.ToLower() == "parallelepiped")
                            {
                                container.RemoveFiguresByType(figureType, lineCounter);
                            }
                            else
                            {
                                Console.WriteLine($"Ошибка: Неверный тип фигуры для условия удаления в строке {lineCounter}.");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Ошибка: Неверное количество параметров для команды 'rem' в строке {lineCounter}.");
                            continue;
                        }
                    }
                    else if (command == "print")
                    {
                        container.PrintFigures();
                        Console.WriteLine();
                    }
                    else
                    {
                        Console.WriteLine($"Ошибка: Неизвестная команда \"{command}\" в строке {lineCounter}.");
                        continue;
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine($"Ошибка: Неверный числовой параметр в строке {lineCounter}.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при обработке строки {lineCounter}: {ex.Message}");
                }
            }

        }
        Console.ReadKey();  //test
    }
}
