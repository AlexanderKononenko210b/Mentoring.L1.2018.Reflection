using System;
using System.Runtime.Serialization;

namespace DI.Resolver.Exceptions
{
    /// <summary>
    /// Exception when conteiner can not resolve dependency.
    /// </summary>
    [Serializable]
    public class UnresolvedDependenciesException : Exception
    {
        public UnresolvedDependenciesException()
        {
        }

        public UnresolvedDependenciesException(string message) : base(message)
        {
        }

        public UnresolvedDependenciesException(string message, Exception inner) : base(message, inner)
        {
        }

        protected UnresolvedDependenciesException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
