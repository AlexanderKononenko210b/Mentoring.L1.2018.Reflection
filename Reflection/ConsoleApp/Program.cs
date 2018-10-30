using System;
using System.Reflection;
using ConsoleApp.TestClasses;
using ConsoleApp.TestInterfaces;
using DI.Resolver;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = new Container();

            container.AddType(typeof(CustomerBll));
            container.AddType(typeof(Logger));
            container.AddType(typeof(ICustomerDal), typeof(CustomerDal));
            container.AddType(typeof(CustomerBllImport));

            //container.AddAssembly(Assembly.GetExecutingAssembly());//for test load types from assembly (you should uncomment this line and comment line 15-18).

            try
            {
                var customerBll = (CustomerBll)container.Get(typeof(CustomerBll));

                var customerBllImport = (CustomerBllImport)container.Get(typeof(CustomerBllImport));

                var customerBllOther = container.Get<CustomerBll>();

                var customerBllImportOther = container.Get<CustomerBllImport>();

                var interfaceActivator = container.Get<ICustomerDal>();

                var interfaceActivatorOther = container.Get(typeof(ICustomerDal));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
