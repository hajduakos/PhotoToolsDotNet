using FilterLib.Filters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace FilterLib
{
    public static class ReflectiveApi
    {
        public static Type GetFilterTypeByName(string name)
        {
            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
                if (type.GetCustomAttributes(typeof(FilterAttribute), false).Length > 0 &&
                    (type.Name == name || type.Name == name + "Filter"))
                    return type;

            throw new ArgumentException("Filter '" + name + "' not found.");
        }

        public static IFilter ConstructFilterByName(string name)
        {
            Type type = GetFilterTypeByName(name);

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
    
        public static PropertyInfo GetFilterPropertyByName(Type type, string name)
        {
            foreach (PropertyInfo pi in type.GetProperties())
                if (pi.GetCustomAttributes(typeof(FilterParamAttribute), false).Length > 0 && pi.Name == name)
                    return pi;

            throw new ArgumentException("Property '" + name + "' not found for '" + type.Name + "'.");
        }

        public static object ParseParamValue(Type type, string value)
        {
            NumberFormatInfo nfi = CultureInfo.InvariantCulture.NumberFormat;
            if (type == typeof(Int32)) return Convert.ToInt32(value);
            if (type == typeof(Single)) return Convert.ToSingle(value, nfi);
            if (type == typeof(Boolean)) return Convert.ToBoolean(value);
            throw new ArgumentException("Parsing type '" + type.Name + "' is not yet supported.");
        }

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

        public static void SetFilterPropertyByName(IFilter filter, string name, string value)
        {
            PropertyInfo pi = GetFilterPropertyByName(filter.GetType(), name);
            object valueObj = ParseParamValue(pi.PropertyType, value);
            RangeCheck(pi, valueObj);
            pi.SetValue(filter, valueObj);
        }
    }
}
