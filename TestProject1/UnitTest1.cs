using NUnit.Framework;
using System.IO;
using System;
using NUnit.Framework.Legacy;

namespace TestNamespace.Tests
{
    [TestFixture]
    public class ProgramTests
    {
        private const string TestDataFile = "data.txt";

        [Test]
        public void Main_AddSphere_PrintsAddedSphere()
        {
            using (StreamWriter writer = new StreamWriter(TestDataFile))
            {
                writer.WriteLine("add sphere 1.0 2.0 John");
                writer.WriteLine("print");
            }

            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
                using (StreamReader sr = new StreamReader(TestDataFile))
                {
                    Console.SetIn(sr);
                    Program.Main();
                }

                StringAssert.Contains("Added Sphere", sw.ToString());
            }
        }

        [Test]
        public void Main_RemoveByCondition_PrintsDeletedByCondition()
        {
            using (StreamWriter writer = new StreamWriter(TestDataFile))
            {
                writer.WriteLine("add sphere 1.0 2.0 John");
                writer.WriteLine("rem density == 2.0");
                writer.WriteLine("print");
            }

            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
                using (StreamReader sr = new StreamReader(TestDataFile))
                {
                    Console.SetIn(sr);
                    Program.Main();
                }

                StringAssert.Contains("Deleted because density == 2.0 в строке 2", sw.ToString());
            }
        }

        [Test]
        public void Main_RemoveByType_PrintsDeletedByType()
        {
            using (StreamWriter writer = new StreamWriter(TestDataFile))
            {
                writer.WriteLine("add sphere 1.0 2.0 John");
                writer.WriteLine("rem sphere");
                writer.WriteLine("print");
            }

            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
                using (StreamReader sr = new StreamReader(TestDataFile))
                {
                    Console.SetIn(sr);
                    Program.Main();
                }

                StringAssert.Contains("Deleted because type is sphere", sw.ToString());
            }
        }

        [Test]
        public void Main_InvalidCommand_PrintsErrorMessage()
        {
            using (StreamWriter writer = new StreamWriter(TestDataFile))
            {
                writer.WriteLine("invalid command");
            }

            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
                using (StreamReader sr = new StreamReader(TestDataFile))
                {
                    Console.SetIn(sr);
                    Program.Main();
                }

                StringAssert.Contains("Ошибка: Неизвестная команда", sw.ToString());
            }
        }
    }
}
