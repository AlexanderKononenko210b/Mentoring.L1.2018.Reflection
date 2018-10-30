using Attributes.Infrastructure;
using ConsoleApp.TestInterfaces;

namespace ConsoleApp.TestClasses
{
    /// <summary>
    /// Represents a model of <see cref="CustomerBll"/> class.
    /// </summary>
    [ImportConstructor]
    public class CustomerBllOther
    {
        /// <summary>
        /// Initialize a new <see cref="CustomerBll"/> instance.
        /// </summary>
        /// <param name="customerDal">The <see cref="ICustomerDal"/> instance.</param>
        public CustomerBllOther(ICustomerDal customerDal)
        {
            CustomerDal = customerDal;
        }

        /// <summary>
        /// Gets or sets a customer DAL.
        /// </summary>
        public ICustomerDal CustomerDal { get; set; }

        /// <summary>
        /// Gets or sets a Logger.
        /// </summary>
        public Logger LoggerInst { get; set; }
    }
}
