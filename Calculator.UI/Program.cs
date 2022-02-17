using System;
using System.IO;
using Calculator.Application;
using Microsoft.Extensions.DependencyInjection;

namespace Calculator.UI
{
    /// <summary>
    /// Console UI of Calculator, for expressions input,  manual or from file.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main method, with mode choosing for expressions input.
        /// </summary>
        public static void Main()
        {
            char mode = ChooseCalcMode();
            string expression;
            double result = 0;
            EnumErrors errors;
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddScoped<ICalculate, Calculate>();
            var provider = serviceCollection.BuildServiceProvider();
            var service = provider.GetRequiredService<ICalculate>();

            if (mode == 'm')
            {
                expression = EnterExpression();
                errors = service.CalculateManualExpression(expression, ref result);
                if (errors == EnumErrors.Success)
                {
                    Console.WriteLine(expression + "=" + result);
                }
                else
                {
                    Console.WriteLine(errors);
                }
            }
            else
            {
                service.CalculateFileExpressions(EnterPathToFile());
            }
        }

        private static string EnterExpression()
        {
            Console.Write("Enter the expression without brackets: ");
            return Console.ReadLine();
        }

        private static string EnterPathToFile()
        {
            string path;

            do
            {
                Console.Write("Enter Path to File: ");
                path = Console.ReadLine();
                Console.WriteLine(File.Exists(path) ? " " : "File doesn't exist!");
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
