using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MethodDecoratorInterfaces;
namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Internal(6));
        }

        [MyDecorator]
        private static int Internal(int i) {
            return i + 5;
        }
    }
}
