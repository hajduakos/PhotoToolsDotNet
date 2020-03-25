using FilterLib.Blending;
using FilterLib.Filters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Linq;

namespace FilterLib
{
    /// <summary>
    /// A collection of functions related to querying, constructing and manipulating
    /// filters and blends via reflection.
    /// </summary>
    public static class ReflectiveApi
    {
        /// <summary>
        /// List all filter types.
        /// </summary>
        /// <returns>Filter types</returns>
        public static IEnumerable<Type> GetFilterTypes() =>
            Assembly.GetExecutingAssembly().GetTypes().Where(t => t.GetCustomAttributes(typeof(FilterAttribute), false).Length > 0);

        /// <summary>
        /// Get a filter by its name. Returns null if not found.
        /// </summary>
        /// <param name="name">Name</param>
        /// <returns>Filter type</returns>
        private static Type GetFilterTypeByName(string name)
        {
            foreach (Type type in GetFilterTypes())
                if (type.Name == name || type.Name == name + "Filter")
                    return type;

            return null;
        }

        public static bool CheckFilterExists(string name) => GetFilterTypeByName(name) != null;

        /// <summary>
        /// Construct a filter by its name. Throws an exception if not found
        /// or no parameterless constructor is available.
        /// </summary>
        /// <param name="name">Filter name</param>
        /// <returns>Filter instance</returns>
        public static IFilter ConstructFilterByName(string name)
        {
            Type type = GetFilterTypeByName(name);
            if (type == null)
                throw new ArgumentException("Filter '" + name + "' not found.");

            foreach (var constr in type.GetConstructors())
            {
                List<object> parameters = new List<object>();
                foreach (var param in constr.GetParameters())
                    if (param.HasDefaultValue)
                        parameters.Add(param.DefaultValue);

                if (parameters.Count == constr.GetParameters().Length)
                    return (IFilter)constr.Invoke(parameters.ToArray());
            }

            throw new ArgumentException("No parameterless constructor found for '" + name + "'.");
        }

        /// <summary>
        /// Get all filter properties for a filter.
        /// </summary>
        /// <param name="type">Filter type</param>
        /// <returns>List of properties</returns>
        public static IEnumerable<PropertyInfo> GetFilterProperties(Type type) =>
            type.GetProperties().Where(p => p.GetCustomAttributes(typeof(FilterParamAttribute), false).Length > 0);
    
        /// <summary>
        /// Get a filter property by name. Throws an exception if not found.
        /// </summary>
        /// <param name="type">Filter type</param>
        /// <param name="name">Property name</param>
        /// <returns>Property info</returns>
        private static PropertyInfo GetFilterPropertyByName(Type type, string name)
        {
            foreach (PropertyInfo pi in GetFilterProperties(type))
                if (pi.Name == name)
                    return pi;

            throw new ArgumentException("Property '" + name + "' not found for '" + type.Name + "'.");
        }

        /// <summary>
        /// Parse a string value to a given type. Throws exception if cannot be parsed.
        /// </summary>
        /// <param name="type">Type to be parsed</param>
        /// <param name="value">Value as string</param>
        /// <returns>Value as type</returns>
        private static object ParseStringToType(Type type, string value)
        {
            NumberFormatInfo nfi = CultureInfo.InvariantCulture.NumberFormat;
            if (type == typeof(Int32)) return Convert.ToInt32(value);
            if (type == typeof(Single)) return Convert.ToSingle(value, nfi);
            if (type == typeof(Boolean)) return Convert.ToBoolean(value);
            throw new ArgumentException("Parsing type '" + type.Name + "' is not yet supported.");
        }

        /// <summary>
        /// Range check based on min/max attributes. Throws exception if out of range.
        /// </summary>
        /// <param name="pi">Filter property to be checked</param>
        /// <param name="value">Value</param>
        private static void RangeCheck(PropertyInfo pi, object value)
        {
            foreach (var attr in pi.GetCustomAttributes<FilterParamMinAttribute>())
                if (Convert.ToInt32(value) < attr.Value)
                    throw new ArgumentOutOfRangeException("Parameter value " + value + " is less than minimum (" + attr.Value + ")");
            foreach (var attr in pi.GetCustomAttributes<FilterParamMaxAttribute>())
                if (Convert.ToInt32(value) > attr.Value)
                    throw new ArgumentOutOfRangeException("Parameter value " + value + " is greater than maximum (" + attr.Value + ")");
            foreach (var attr in pi.GetCustomAttributes<FilterParamMinFAttribute>())
                if (Convert.ToDouble(value) < attr.Value)
                    throw new ArgumentOutOfRangeException("Parameter value " + value + " is less than minimum (" + attr.Value + ")");
            foreach (var attr in pi.GetCustomAttributes<FilterParamMaxFAttribute>())
                if (Convert.ToDouble(value) > attr.Value)
                    throw new ArgumentOutOfRangeException("Parameter value " + value + " is greater than maximum (" + attr.Value + ")");
        }

        /// <summary>
        /// Set a filter property by name with a value by string. Throws exception if
        /// property not found or if value cannot be parsed.
        /// </summary>
        /// <param name="filter">Filter instance</param>
        /// <param name="name">Property name</param>
        /// <param name="value">Value to be set</param>
        public static void SetFilterPropertyByName(IFilter filter, string name, string value)
        {
            PropertyInfo pi = GetFilterPropertyByName(filter.GetType(), name);
            object valueObj = ParseStringToType(pi.PropertyType, value);
            RangeCheck(pi, valueObj);
            pi.SetValue(filter, valueObj);
        }

        /// <summary>
        /// Get all blend types.
        /// </summary>
        /// <returns>List of blends</returns>
        public static IEnumerable<Type> GetBlendTypes() =>
            Assembly.GetExecutingAssembly().GetTypes().Where(t => t.GetCustomAttributes(typeof(BlendAttribute), false).Length > 0);

        /// <summary>
        /// Get a blend by name. Returns null if not found.
        /// </summary>
        /// <param name="name">Blend name</param>
        /// <returns>Blend type</returns>
        private static Type GetBlendTypeByName(string name)
        {
            foreach (Type type in GetBlendTypes())
                if (type.Name == name || type.Name == name + "Blend")
                    return type;

            return null;
        }

        public static bool CheckBlendExists(string name) => GetBlendTypeByName(name) != null;

        /// <summary>
        /// Construct a blend by its name. Throws an exception if not found
        /// or no parameterless constructor is available.
        /// </summary>
        /// <param name="name">Blend name</param>
        /// <returns>Blend instance</returns>
        public static IBlend ConstructBlendByName(string name)
        {
            Type type = GetBlendTypeByName(name);
            if (type == null)
                throw new ArgumentException("Blend '" + name + "' not found.");

            foreach (var constr in type.GetConstructors())
            {
                List<object> parameters = new List<object>();
                foreach (var param in constr.GetParameters())
                    if (param.HasDefaultValue)
                        parameters.Add(param.DefaultValue);

                if (parameters.Count == constr.GetParameters().Length)
                    return (IBlend)constr.Invoke(parameters.ToArray());
            }

            throw new ArgumentException("No parameterless constructor found for '" + name + "'.");
        }
    }
}
