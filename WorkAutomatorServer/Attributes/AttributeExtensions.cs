using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace Attributes
{
    public static class AttributeExtensions
    {
        public static List<(TAttribute, List<PropertyInfo>)> FindPropertyPaths<TAttribute>(Type type) where TAttribute : Attribute
        {
            List<(TAttribute, List<PropertyInfo>)> propertiesPath = new List<(TAttribute, List<PropertyInfo>)>();

            if (type.IsValueType || type.Equals(typeof(string)))
                return propertiesPath;

            PropertyInfo[] properties = type.GetProperties();

            IEnumerable<PropertyInfo> markedProperties = properties.Where(
                prop => prop.GetCustomAttribute<TAttribute>() != null
            );
            PropertyInfo[] notMarkedProperties = properties.Except(markedProperties).ToArray();

            propertiesPath.AddRange(
                markedProperties.Select(
                    prop => (
                        prop.GetCustomAttribute<TAttribute>(), 
                        new List<PropertyInfo>() { prop }
                    )
                )
            );

            foreach (PropertyInfo property in notMarkedProperties)
            {
                List<(TAttribute, List<PropertyInfo>)> subPath = null;

                if (property.PropertyType.IsGenericType && (typeof(IEnumerable<>).IsAssignableFrom(property.PropertyType.GetGenericTypeDefinition()) || typeof(ICollection<>).IsAssignableFrom(property.PropertyType.GetGenericTypeDefinition())))
                    subPath = FindPropertyPaths<TAttribute>(property.PropertyType.GetGenericArguments()[0]);
                else if(property.PropertyType.IsArray)
                    subPath = FindPropertyPaths<TAttribute>(property.PropertyType.GetElementType());
                else
                    subPath = FindPropertyPaths<TAttribute>(property.PropertyType);

                propertiesPath.AddRange(
                    subPath.Select(
                        p => (p.Item1, p.Item2.Prepend(property).ToList())
                    )
                );
            }

            return propertiesPath;
        }

        public static Dictionary<(Guid, TAttribute), object[]> GetMarkedMapFromArgumentList<TAttribute>(this object[] arguments, ParameterInfo[] parameters) 
            where TAttribute : Attribute
        {
            Dictionary<(Guid, TAttribute), object[]> result = new Dictionary<(Guid, TAttribute), object[]>();

            ParameterInfo parameter = parameters.FirstOrDefault(
                param => param.GetCustomAttribute<TAttribute>() != null
            );

            if (parameter != null)
            {
                result.Add(
                    (Guid.NewGuid(), parameter.GetCustomAttribute<TAttribute>()),
                    new object[] { arguments[parameter.Position] }
                );

                return result;
            }

            foreach (ParameterInfo param in parameters)
            {
                object argument = arguments[param.Position];

                List<(TAttribute, List<PropertyInfo>)> paths = FindPropertyPaths<TAttribute>(argument.GetType());

                if (paths.Count == 0)
                    continue;

                foreach((TAttribute, List<PropertyInfo>) path in paths)
                    result.Add((Guid.NewGuid(), path.Item1), argument.GetValuesByPath(path.Item2));
            }

            return result;
        }

        public static object[] GetMarkedValueFromArgumentList<TAttribute>(this object[] arguments, ParameterInfo[] parameters) where TAttribute : Attribute
        {
            return arguments.GetMarkedMapFromArgumentList<TAttribute>(parameters)?.SelectMany(kvp => kvp.Value).ToArray();
        }

        public static object[] GetValuesByPath(this object source, List<PropertyInfo> path)
        {
            if (source == null || path.Count == 0)
                return new object[] { source };

            PropertyInfo property = path.First();

            if (property.PropertyType.IsGenericType && property.GetValue(source) is IEnumerable<object> collection)
                return collection.SelectMany(value => GetValuesByPath(value, path.Skip(1).ToList())).ToArray();
            else if (property.PropertyType.IsArray && property.GetValue(source) is object[] array)
                return array.SelectMany(value => GetValuesByPath(value, path.Skip(1).ToList())).ToArray();
            else
                return GetValuesByPath(path.First().GetValue(source), path.Skip(1).ToList());
        }
    }
}
