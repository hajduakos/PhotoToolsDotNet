using FilterLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FilterScript
{
    sealed class DocGen
    {
        private StreamWriter sw;
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
            SortedDictionary<string, List<Type>> filters = new SortedDictionary<string, List<Type>>();

            foreach (var fType in ReflectiveApi.GetFilterTypes())
            {
                string category = fType.Namespace.Substring(fType.Namespace.LastIndexOf('.') + 1);
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
            sw.WriteLine($"*{fType.Name.Replace("Filter", "")}*");
            foreach (var prop in ReflectiveApi.GetFilterProperties(fType)) WriteProperty(prop);
            sw.WriteLine();
        }

        private void WriteProperty(System.Reflection.PropertyInfo prop)
        {
            sw.WriteLine($"- {prop.Name}: {prop.PropertyType.Name}");
        }
    }
}
