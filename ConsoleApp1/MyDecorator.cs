using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class MyDecorator:MethodDecoratorInterfaces.MethodDecoratorAttribute
    {
        string Mn;
        public override void Init(object instance, MethodBase method, object[] args)
        {
            Mn = method.Name;
            base.Init(instance, method, args);
        }

        public override void OnEntry()
        {
            Console.WriteLine("Entering" + Mn);
            base.OnEntry();
        }

        public override void OnExit()
        {
            base.OnExit();
            Console.WriteLine("Exiting " + Mn);
        }
    }
}
