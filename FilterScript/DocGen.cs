using FilterLib;
using FilterLib.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Linq;

namespace FilterScript
{
    sealed class DocGen
    {
        private readonly StreamWriter sw;
        public DocGen(StreamWriter sw)
        {
            this.sw = sw;
        }

        public void Write()
        {
            sw.WriteLine("# FilterScript");
            sw.WriteLine();
            WriteFilters();
        }

        private void WriteFilters()
        {
            sw.WriteLine("## Filters");
            sw.WriteLine();
            SortedDictionary<string, List<Type>> filters = new();

            foreach (var fType in ReflectiveApi.GetFilterTypes())
            {
                string category = fType.Namespace[(fType.Namespace.LastIndexOf('.') + 1)..];
                if (!filters.ContainsKey(category))
                    filters.Add(category, new List<Type>());
                filters[category].Add(fType);
            }

            foreach (var entry in filters) WriteCategory(entry.Key, entry.Value);
        }

        private void WriteCategory(string category, List<Type> filters)
        {
            sw.WriteLine($"### {category}");
            sw.WriteLine();
            filters.ForEach(WriteFilter);
        }

        private void WriteFilter(Type fType)
        {
            sw.WriteLine($"**{fType.Name.Replace("Filter", "")}**");
            foreach (var prop in ReflectiveApi.GetFilterProperties(fType)) WriteProperty(prop);
            sw.WriteLine();
        }

        private void WriteProperty(PropertyInfo prop)
        {
            string min = GetMin(prop);
            string max = GetMax(prop);
            string range = "";
            if (min != "" || max != "")
            {
                if (min == "") min = "...";
                if (max == "") max = "...";
                range = $"[{min};{max}]";
            }
            sw.WriteLine($"- {prop.Name}: {prop.PropertyType.Name}{range}");
        }

        private static string GetMin(PropertyInfo prop)
        {
            var min = prop.GetCustomAttributes<FilterParamMinAttribute>().FirstOrDefault();
            if (min != null) return min.Value.ToString();
            var minf = prop.GetCustomAttributes<FilterParamMinFAttribute>().FirstOrDefault();
            if (minf != null) return minf.Value.ToString();
            return "";
        }

        private static string GetMax(PropertyInfo prop)
        {
            var max = prop.GetCustomAttributes<FilterParamMaxAttribute>().FirstOrDefault();
            if (max != null) return max.Value.ToString();
            var maxf = prop.GetCustomAttributes<FilterParamMaxFAttribute>().FirstOrDefault();
            if (maxf != null) return maxf.Value.ToString();
            return "";
        }
    }
}
