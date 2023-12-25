using lab1;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;

public class Program
{
    private static void AddCylinder(string[] commandParts, Container container, int lineCounter)
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
    private static void AddParallelepiped(string[] commandParts, Container container, int lineCounter)
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
    private static void AddSphere(string[] commandParts, Container container, int lineCounter)
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
    
    private static void AddFigure(string[] commandParts, int lineCounter, Container container)
    {
        int MinCommandLen = 2;
        int SphereCommandLen = 5;
        int ParallelepipedCommandLen = 7;
        int CylinderCommandLen = 7;

        if (commandParts.Length > MinCommandLen)
        {
            string type = commandParts[1];

            if (type == "sphere" && commandParts.Length == SphereCommandLen)
            {
                AddSphere(commandParts, container, lineCounter);
            }
            else if (type == "parallelepiped" && commandParts.Length == ParallelepipedCommandLen)
            {
                AddParallelepiped(commandParts, container, lineCounter);
            }
            else if (type == "cylinder" && commandParts.Length == CylinderCommandLen)
            {
                AddCylinder(commandParts, container, lineCounter);
            }
            else
            {
                Console.WriteLine($"Ошибка: Неизвестный тип фигуры в строке {lineCounter}.");
            }
        }
        else
        {
            Console.WriteLine($"Ошибка: Недостаточно параметров для команды 'add' в строке {lineCounter}.");
        }
    }

    private static void RemFigure(string[] commandParts, int lineCounter, Container container, string condition)
    {
        int CommandRemoveByConditionLen = 4;
        int CommandRemoveByTypeLen = 2;

        if (commandParts.Length == CommandRemoveByConditionLen)
        {
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
        else if (commandParts.Length == CommandRemoveByTypeLen)
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
        }
    }

    private static void OpenFile()
    {

        using (StreamReader inputFile = new StreamReader("data.txt"))
        {
            if (inputFile == null)
            {
                Console.Error.WriteLine("Не удалось открыть файл 'data.txt'");
                return;
            }
            else
            {
                ReadFile(inputFile);
            } 
        }
    }

    private static void ReadFile(StreamReader inputFile)
    {
        Container container = new Container();
        int lineCounter = 0;
        string line;

        
        while ((line = inputFile.ReadLine()) != null)
        {
            try
            {
                lineCounter++;

                string[] commandParts = line.Split(' ');
                string command = commandParts[0];
                

                if (command == "add")
                {
                    AddFigure(commandParts, lineCounter, container);
                }
                else if (command == "rem")
                {
                    string condition = line.Substring(command.Length + 1);
                    RemFigure(commandParts, lineCounter, container, condition);
                }
                else if (command == "print")
                {
                    container.PrintFigures();
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine($"Ошибка: Неизвестная команда \"{command}\" в строке {lineCounter}.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при обработке строки {lineCounter}, сообщение: {ex.Message}");
            }
        }
        Console.ReadKey();  //test
    }

    public static void Main()
    {
            OpenFile();
    }
}
