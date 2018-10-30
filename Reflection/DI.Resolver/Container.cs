using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Attributes.Infrastructure;
using DI.Resolver.Exceptions;
using DI.Resolver.Resources;

namespace DI.Resolver
{
    /// <summary>
    /// Represents a model of the <see cref="Container"/> class.
    /// </summary>
    public sealed class Container
    {
        private  readonly Dictionary<Type, Type> _resolverData = new Dictionary<Type, Type>();

        /// <summary>
        /// Add type in container.
        /// </summary>
        /// <param name="type">The type.</param>
        public void AddType(Type type)
        {
            if (type.IsClass)
            {
                if (!_resolverData.ContainsKey(type))
                {
                    _resolverData.Add(type, type);
                }
            }
        }

        /// <summary>
        /// Add type with specify key in container.
        /// </summary>
        /// <param name="typeKey">The key type.</param>
        /// <param name="typeValue">The value type.</param>
        public void AddType(Type typeKey, Type typeValue)
        {
            if (typeValue.IsClass)
            {
                if (!_resolverData.ContainsKey(typeKey))
                {
                    _resolverData.Add(typeKey, typeValue);
                }
            }
        }

        /// <summary>
        /// Add all classes marked as [ImportConstructor], [Import] и [Export] using assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        public void AddAssembly(Assembly assembly)
        {
            var typesForResolve = assembly.GetExportedTypes()
                .Where(type => IsImportConstructorAttribute(type) ||
                               IsExportAttribute(type) ||
                               IsIncludeImportAttribute(type));

            foreach (var type in typesForResolve)
            {
                if (IsExportAttribute(type))
                {
                    var attribute = (ExportAttribute) type.GetCustomAttribute(typeof(ExportAttribute));

                    if (attribute != null && attribute.Type != null)
                    {
                        AddType(attribute.Type, type);
                        continue;
                    }
                }

                AddType(type);
            }
        }

        /// <summary>
        /// Get object using specify type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The <see cref="object"/> instance.</returns>
        public object Get(Type type)
        {
            var isImportConstructor = IsImportConstructorAttribute(type);
            var isImport = IsIncludeImportAttribute(type);

            if (isImportConstructor && isImport)
            {
                throw new ArgumentOutOfRangeException(Messages.MarkAttributeIssue);
            }

            if (isImportConstructor)
            {
                return ImportConstructorAttributeActivator(type);
            }

            if (isImport)
            {
                return ImportAttributeActivator(type);
            }

            if (_resolverData.ContainsKey(type))
            {
                return Activator.CreateInstance(_resolverData[type]);
            }

            return null;
        }

        /// <summary>
        /// Get instance.
        /// </summary>
        /// <typeparam name="T">The type of instance.</typeparam>
        /// <returns>The instance type T.</returns>
        public T Get<T>()
        {
            var requared = typeof(T);
            var isImportConstructor = IsImportConstructorAttribute(requared);
            var isImport = IsIncludeImportAttribute(requared);

            if (isImportConstructor && isImport)
            {
                throw new ArgumentOutOfRangeException(Messages.MarkAttributeIssue);
            }

            if (isImportConstructor)
            {
                return (T) ImportConstructorAttributeActivator(requared);
            }

            if (isImport)
            {
                return (T) ImportAttributeActivator(requared);
            }

            if (_resolverData.ContainsKey(requared))
            {
                return (T) Activator.CreateInstance(_resolverData[requared]);
            }

            return default(T);
        }

        /// <summary>
        /// Type checking for an Import Constructor attribute.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>True if class has ImportConstructor attibute otherwase false.</returns>
        private bool IsImportConstructorAttribute(Type type) => type.GetCustomAttribute(typeof(ImportConstructorAttribute)) != null;

        /// <summary>
        /// Type checking for include an Import attribute.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>True if class has properties with Import attribute otherwase false.</returns>
        private bool IsIncludeImportAttribute(Type type) => type.GetProperties()
            .Any(property => property.GetCustomAttribute(typeof(ImportAttribute)) != null);

        /// <summary>
        /// Type checking for an Import attribute.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>True if class has properties with Import attribute otherwase false.</returns>
        private bool IsImportAttribute(PropertyInfo type) => type.GetCustomAttribute(typeof(ImportAttribute)) != null;

        /// <summary>
        /// Type checking for an Export attribute.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>True if class has Export attribute otherwase false.</returns>
        private bool IsExportAttribute(Type type) => type.GetCustomAttribute(typeof(ExportAttribute)) != null;

        /// <summary>
        /// Get instance of class which has Import Constructor attribute.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The <see cref="object"/></returns>
        private object ImportConstructorAttributeActivator(Type type)
        {
            var constructors = type.GetConstructors()
                .Where(constructor => constructor.IsPublic &&
                                      !constructor.IsStatic).ToArray();

            if (constructors.Length > 0)
            {
                var parameteres = constructors[0].GetParameters();
                var listParametersInstances = new List<object>();

                foreach (var parameter in parameteres)
                {
                    var parameterType = parameter.ParameterType;

                    if (_resolverData.ContainsKey(parameterType))
                    {
                        listParametersInstances.Add(CreateParameterInstance(parameterType));
                    }
                    else
                    {
                        throw new UnresolvedDependenciesException(Messages.UnresolvedDependency);
                    }
                }

                return constructors[0].Invoke(listParametersInstances.ToArray());
            }

            return null;
        }

        /// <summary>
        /// Get instance of class which has Import attribute on public properties.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The <see cref="object"/></returns>
        private object ImportAttributeActivator(Type type)
        {
            if (_resolverData.ContainsKey(type))
            {
                var properties = type.GetProperties().Where(property => IsImportAttribute(property)).ToArray();

                if (properties.Length > 0)
                {
                    var outputInstance = Activator.CreateInstance(_resolverData[type]);

                    foreach (var propertyInfo in properties)
                    {
                        var propertyType = propertyInfo.PropertyType;

                        if (_resolverData.ContainsKey(propertyType))
                        {
                            propertyInfo.SetValue(outputInstance, Activator.CreateInstance(_resolverData[propertyType]));
                        }
                        else
                        {
                            throw new UnresolvedDependenciesException(Messages.UnresolvedDependency);
                        }
                    }

                    return outputInstance;
                }
            }

            return null;
        }

        /// <summary>
        /// Create parameter using parameter info.
        /// </summary>
        /// <param name="parameterType">The parameter type.</param>
        /// <returns>The <see cref="object"/></returns>
        private object CreateParameterInstance(Type parameterType)
        {
            if (parameterType.IsClass)
            {
                if (IsExportAttribute(parameterType))
                {
                    return Activator.CreateInstance(_resolverData[parameterType]);
                }
            }

            if (parameterType.IsInterface || parameterType.IsAbstract)
            {
                if (IsExportAttribute(_resolverData[parameterType]))
                {
                    return Activator.CreateInstance(_resolverData[parameterType]);
                }
            }

            return null;
        }
    }
}
