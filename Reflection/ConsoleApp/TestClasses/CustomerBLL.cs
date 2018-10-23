using Attributes.Infrastructure;
using ConsoleApp.TestInterfaces;

namespace ConsoleApp.TestClasses
{
    /// <summary>
    /// Represents a model of <see cref="CustomerBll"/> class.
    /// </summary>
    [ImportConstructor]
    public class CustomerBll
    {
        /// <summary>
        /// Initialize a new <see cref="CustomerBll"/> instance.
        /// </summary>
        /// <param name="customerDal">The <see cref="ICustomerDal"/> instance.</param>
        /// <param name="logger">The <see cref="Logger"/> instance.</param>
        public CustomerBll(ICustomerDal customerDal, Logger logger)
        {
            CustomerDal = customerDal;
            LoggerInst = logger;
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
