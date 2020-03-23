using FilterLib.Filters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace FilterLib.Scripting
{
    /// <summary>
    /// Helper class for parsing filter scripts.
    /// </summary>
    public static class Parser
    {
        /// <summary>
        /// Parse a script into a list of filters. Throws exceptions if any
        /// error is encountered during parsing.
        /// </summary>
        /// <param name="script">Script to be parsed</param>
        /// <returns>List of parsed filters</returns>
        public static List<IFilter> Parse(string[] script)
        {
            List<IFilter> filters = new List<IFilter>();
            IFilter currentFilter = null;

            // Loop through each line
            for (int i = 0; i < script.Length; ++i)
            {
                string line = script[i].Trim();
                int lineNo = i + 1;

                if (line.StartsWith("- ")) // Parameter
                {
                    if (currentFilter == null) throw new SyntaxException(lineNo, "Expected filter but got parameter.");
                    // Remove '- ' from beginning
                    line = line.Substring(2);
                    // Split to 'Name: value'
                    int colon = line.IndexOf(':');
                    if (colon == -1) throw new SyntaxException(lineNo, "Expected format 'Name: value'.");
                    string name = line.Substring(0, colon).Trim();
                    string value = line.Substring(colon + 1).Trim();
                    // Try to get parameter by name
                    PropertyInfo pi = GetFilterParamByName(lineNo, currentFilter.GetType(), name);
                    // Try to parse
                    object valueObj = ParseParamValue(lineNo, pi, value);
                    // Check range
                    RangeCheck(lineNo, pi, valueObj);
                    // Set value
                    pi.SetValue(currentFilter, valueObj);
                }
                else // Filter
                {
                    currentFilter = GetFilterByName(lineNo, line);
                    filters.Add(currentFilter);
                }
            }

            return filters;
        }

        /// <summary>
        /// Get a filter by name. Throws exception if not found.
        /// </summary>
        /// <param name="line">Line number (for error reporting)</param>
        /// <param name="name">Name of the filter</param>
        /// <returns>Filter instance</returns>
        private static IFilter GetFilterByName(int line, string name)
        {
            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (type.GetCustomAttributes(typeof(FilterAttribute), false).Length > 0 && type.Name == name + "Filter")
                {
                    foreach (var ci in type.GetConstructors())
                    {
                        List<object> parameters = new List<object>();
                        foreach (var p in ci.GetParameters())
                            if (p.HasDefaultValue) parameters.Add(p.DefaultValue);
                        if (parameters.Count == ci.GetParameters().Length)
                            return (IFilter)ci.Invoke(parameters.ToArray());
                    }

                    throw new FilterNotFoundException(line, "No parameterless constructor found for '" + name + "'.");
                }
            }
            throw new FilterNotFoundException(line, "Filter '" + name + "' not found.");
        }

        /// <summary>
        /// Get a filter parameter by name. Throws exception if not found.
        /// </summary>
        /// <param name="line">Line number (for error reporting)</param>
        /// <param name="type">Type of the filter</param>
        /// <param name="name">Name of the parameter</param>
        /// <returns>PropertyInfo corresponding to the parameter</returns>
        private static PropertyInfo GetFilterParamByName(int line, Type type, string name)
        {
            foreach(PropertyInfo pi in type.GetProperties())
                if (pi.GetCustomAttributes(typeof(FilterParamAttribute), false).Length > 0 && pi.Name == name) return pi;
            throw new ParamNotFoundException(line, "Parameter '" + name + "' not found.");
        }

        /// <summary>
        /// Parse the value corresponding to a given type.
        /// </summary>
        /// <param name="line">Line number (for error reporting)</param>
        /// <param name="pi">Property info</param>
        /// <param name="value">Value as string</param>
        /// <returns>Value parsed to appropriate type</returns>
        private static object ParseParamValue(int line, PropertyInfo pi, string value)
        {
            NumberFormatInfo nfi = CultureInfo.InvariantCulture.NumberFormat;
            try
            {
                if (pi.PropertyType == typeof(Int32)) return Convert.ToInt32(value);
                if (pi.PropertyType == typeof(Single)) return Convert.ToSingle(value, nfi);
                if (pi.PropertyType == typeof(Boolean)) return Convert.ToBoolean(value);
            } catch(Exception e)
            {
                throw new ParseException(line, e);
            }
            throw new SyntaxException(line, "Parsing type '" + pi.PropertyType + "' is not yet supported.");
        }

        private static void RangeCheck(int line, PropertyInfo pi, object value)
        {
            foreach (var attr in pi.GetCustomAttributes<FilterParamMinAttribute>())
                if (Convert.ToInt32(value) < attr.Value)
                    throw new ParamRangeException(line, "Parameter value " + value + " is less than minimum (" + attr.Value + ")");
            foreach (var attr in pi.GetCustomAttributes<FilterParamMaxAttribute>())
                if (Convert.ToInt32(value) > attr.Value)
                    throw new ParamRangeException(line, "Parameter value " + value + " is greater than maximum (" + attr.Value + ")");
            foreach (var attr in pi.GetCustomAttributes<FilterParamMinFAttribute>())
                if (Convert.ToDouble(value) < attr.Value)
                    throw new ParamRangeException(line, "Parameter value " + value + " is less than minimum (" + attr.Value + ")");
            foreach (var attr in pi.GetCustomAttributes<FilterParamMaxFAttribute>())
                if (Convert.ToDouble(value) > attr.Value)
                    throw new ParamRangeException(line, "Parameter value " + value + " is greater than maximum (" + attr.Value + ")");
        }
    }
}
