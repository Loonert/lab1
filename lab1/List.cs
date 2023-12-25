using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace lab1
{
    class List
    {
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
}
