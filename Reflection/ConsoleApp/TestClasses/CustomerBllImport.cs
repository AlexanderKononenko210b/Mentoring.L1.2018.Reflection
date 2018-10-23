using Attributes.Infrastructure;
using ConsoleApp.TestInterfaces;

namespace ConsoleApp.TestClasses
{
    /// <summary>
    /// Represents a model <see cref="CustomerBllImport"/> class.
    /// </summary>
    public class CustomerBllImport
    {
        /// <summary>
        /// Gets or sets the customer DAL.
        /// </summary>
        [Import]
        public ICustomerDal CustomerDal { get; set; }

        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        [Import]
        public Logger LoggerInst { get; set; }
    }
}
