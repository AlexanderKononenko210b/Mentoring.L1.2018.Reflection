using System;

namespace Attributes.Infrastructure
{
    /// <summary>
    /// Represents a model <see cref="ExportAttribute"/> class.
    /// </summary>
    public class ExportAttribute : Attribute
    {
        /// <summary>
        /// Initialize a new instance <see cref="ExportAttribute"/> class.
        /// </summary>
        public ExportAttribute()
        { }

        /// <summary>
        /// Initialize a new instance <see cref="ExportAttribute"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        public ExportAttribute(Type type)
        {
            Type = type;
        }

        /// <summary>
        /// Gets or sets the type of the 
        /// </summary>
        public Type Type { get; set; }
    }
}
