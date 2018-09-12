using System;
using static FEM2DDynamicTestApplication.Calculations;

namespace FEM2DDynamicTestApplication
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