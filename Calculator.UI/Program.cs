using System;
using System.IO;

namespace Calculator.UI
{
    public class Program
    {
        public static void Main()
        {
            char mode = ChooseCalcMode();
            string expression;
            string path;

            if (mode == 'm')
            {
                expression = EnterExpression();
            }
            else
            {
                path = EnterPathToFile();
            }
        }

        private static string EnterExpression()
        {
            string expression;
            Console.Write("Enter the expression without brackets: ");
            expression = Console.ReadLine();

            return expression;
        }

        private static string EnterPathToFile()
        {
            string path;

            do
            {
                Console.Write("Enter Path to File: ");
                path = Console.ReadLine();
                Console.WriteLine(File.Exists(path) ? "File exists" : "File doesn't exist!");
            }
            while (!File.Exists(path));

            return path;
        }

        private static char ChooseCalcMode()
        {
            char mode;

            do
            {
                Console.WriteLine("For manual input press: 'm'\nFor reading and writing from file press: 'f'");
                mode = Console.ReadLine()[0];
            }
            while (mode != 'f' && mode != 'm');

            return mode;
        }
    }
}
