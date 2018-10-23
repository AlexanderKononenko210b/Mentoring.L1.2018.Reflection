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

            #region Test object CreateInstance(Type type) 

            var customerBll = (CustomerBll)container.CreateInstance(typeof(CustomerBll));

            CustomerBllValidator(customerBll);

            var customerBllImport = (CustomerBllImport)container.CreateInstance(typeof(CustomerBllImport));

            CustomerBllImportValidator(customerBllImport);

            #endregion

            #region Test CreateInstance<T>()

            var customerBllOther = container.CreateInstance<CustomerBll>();

            CustomerBllValidator(customerBllOther);

            var customerBllImportOther = container.CreateInstance<CustomerBllImport>();

            CustomerBllImportValidator(customerBllImportOther);

            #endregion
        }

        #region Helpers

        /// <summary>
        /// Validate CustomerBll instance.
        /// </summary>
        /// <param name="customerBll">The <see cref="CustomerBll"/> instance.</param>
        private static void CustomerBllValidator(CustomerBll customerBll)
        {
            if (customerBll != null &&
                customerBll.CustomerDal != null &&
                customerBll.LoggerInst != null)
            {
                Console.WriteLine($"Instance type: {typeof(CustomerBll)} is created succesfully");
            }
            else
            {
                Console.WriteLine($"Instance type: {typeof(CustomerBll)} is not created correctly");
            }
        }

        /// <summary>
        /// Validate CustomerBllImport instance.
        /// </summary>
        /// <param name="customerBllimport">The <see cref="CustomerBllImport"/> instance.</param>
        private static void CustomerBllImportValidator(CustomerBllImport customerBllimport)
        {
            if (customerBllimport != null &&
                customerBllimport.CustomerDal != null &&
                customerBllimport.LoggerInst != null)
            {
                Console.WriteLine($"Instance type: {typeof(CustomerBll)} is created succesfully");
            }
            else
            {
                Console.WriteLine($"Instance type: {typeof(CustomerBll)} is not created correctly");
            }
        }

        #endregion
    }
}
