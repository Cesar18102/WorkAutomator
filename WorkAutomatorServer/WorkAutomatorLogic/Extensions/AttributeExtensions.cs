using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Collections;

namespace WorkAutomatorLogic.Extensions
{
    public static class AttributeExtensions
    {
        public static List<PropertyInfo> FindPropertyPath<TAttribute>(Type type) where TAttribute : Attribute
        {
            List<PropertyInfo> propertiesPath = new List<PropertyInfo>();

            if (type.IsValueType || type.Equals(typeof(string)))
                return propertiesPath;

            PropertyInfo[] properties = type.GetProperties();

            PropertyInfo found = properties.FirstOrDefault(
                prop => prop.GetCustomAttribute<TAttribute>() != null
            );

            if (found != null)
            {
                propertiesPath.Add(found);
                return propertiesPath;
            }

            foreach (PropertyInfo property in properties)
            {
                List<PropertyInfo> path = null;

                if (property.PropertyType.IsGenericType && typeof(IEnumerable<>).IsAssignableFrom(property.PropertyType.GetGenericTypeDefinition()))
                    path = FindPropertyPath<TAttribute>(property.PropertyType.GetGenericArguments()[0]);
                else if(property.PropertyType.IsArray)
                    path = FindPropertyPath<TAttribute>(property.PropertyType.GetElementType());
                else
                    path = FindPropertyPath<TAttribute>(property.PropertyType);

                if (path.Count == 0)
                    continue;

                propertiesPath.Add(property);
                propertiesPath.AddRange(path);

                break;
            }

            return propertiesPath;
        }

        public static object[] GetMarkedValue<TAttribute>(this object source) where TAttribute : Attribute
        {
            List<PropertyInfo> path = FindPropertyPath<TAttribute>(source.GetType());
            return source.GetValueByPath(path);
        }

        public static object[] GetMarkedValueFromArgumentList<TAttribute>(this object[] arguments, ParameterInfo[] parameters) where TAttribute : Attribute
        {
            ParameterInfo parameter = parameters.FirstOrDefault(
                param => param.GetCustomAttribute<TAttribute>() != null
            );

            if (parameter != null)
                return new object[] { arguments[parameter.Position] };

            foreach (ParameterInfo param in parameters)
            {
                object argument = arguments[param.Position];

                List<PropertyInfo> path = FindPropertyPath<TAttribute>(argument.GetType());

                if (path.Count == 0)
                    continue;

                return argument.GetValueByPath(path);
            }

            return null;
        }

        private static object[] GetValueByPath(this object source, List<PropertyInfo> path)
        {
            if (source == null || path.Count == 0)
                return new object[] { source };

            PropertyInfo property = path.First();

            if (property.PropertyType.IsGenericType && property.GetValue(source) is IEnumerable<object> collection)
                return collection.SelectMany(value => GetValueByPath(value, path.Skip(1).ToList())).ToArray();
            else if (property.PropertyType.IsArray && property.GetValue(source) is object[] array)
                return array.SelectMany(value => GetValueByPath(value, path.Skip(1).ToList())).ToArray();
            else
                return GetValueByPath(path.First().GetValue(source), path.Skip(1).ToList());
        }
    }
}
