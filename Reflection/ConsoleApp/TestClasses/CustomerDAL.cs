using Attributes.Infrastructure;
using ConsoleApp.TestInterfaces;

namespace ConsoleApp.TestClasses
{
    /// <summary>
    /// Represents a <see cref="CustomerDal"/> class.
    /// </summary>
    [Export(typeof(ICustomerDal))]
    public class CustomerDal : ICustomerDal
    {
    }
}
