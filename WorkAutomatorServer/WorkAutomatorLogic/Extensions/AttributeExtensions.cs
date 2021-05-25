using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace WorkAutomatorLogic.Extensions
{
    public static class AttributeExtensions
    {
        public static List<PropertyInfo> FindPropertyPath<TAttribute>(this object source) where TAttribute : Attribute
        {
            List<PropertyInfo> propertiesPath = new List<PropertyInfo>();

            if (source == null)
                return propertiesPath;

            Type type = source.GetType();

            if (type.IsValueType || type.Equals(typeof(string)))
                return propertiesPath;

            PropertyInfo[] properties = source.GetType().GetProperties();

            PropertyInfo found = properties.FirstOrDefault(
                prop => prop.GetCustomAttribute<TAttribute>() != null
            );

            if (found != null)
            {
                propertiesPath.Add(found);
                return propertiesPath;
            }

            foreach(PropertyInfo property in properties)
            {
                List<PropertyInfo> path = property.GetValue(source).FindPropertyPath<TAttribute>();

                if (path.Count == 0)
                    continue;

                propertiesPath.Add(property);
                propertiesPath.AddRange(path);

                break;
            }

            return propertiesPath;
        }

        public static object GetMarkedValue<TAttribute>(this object source) where TAttribute : Attribute
        {
            List<PropertyInfo> path = source.FindPropertyPath<TAttribute>();
            return source.GetValueByPath(path);
        }

        public static object GetMarkedValueFromArgumentList<TAttribute>(this object[] arguments, ParameterInfo[] parameters) where TAttribute : Attribute
        {
            ParameterInfo parameter = parameters.FirstOrDefault(
                param => param.GetCustomAttribute<TAttribute>() != null
            );

            if (parameter != null)
                return arguments[parameter.Position];

            foreach (ParameterInfo param in parameters)
            {
                object argument = arguments[param.Position];

                List<PropertyInfo> path = argument.FindPropertyPath<TAttribute>();

                if (path.Count == 0)
                    continue;

                return argument.GetValueByPath(path);
            }

            return null;
        }

        private static object GetValueByPath(this object source, List<PropertyInfo> path)
        {
            if (path.Count == 0)
                return source;

            return GetValueByPath(path.First().GetValue(source), path.Skip(1).ToList());
        }
    }
}
