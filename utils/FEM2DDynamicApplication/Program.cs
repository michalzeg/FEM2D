using System;
using static FEM2DDynamicApplication.Calculations;

namespace FEM2DDynamicApplication
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Initialize();

            DynamicLoadInCentre();
            DynamicLoadInCentre();
            DynamicLoadInCentre();
            DynamicLoadInCentre();
            DynamicLoadInCentre();

            Console.ReadKey();
        }
    }
}