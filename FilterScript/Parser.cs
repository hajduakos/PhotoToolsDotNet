using FilterLib;
using FilterLib.Blending;
using FilterLib.Filters;
using FilterScript.Model;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace FilterScript
{
    static class Parser
    {
        private const string VARPREFIX = "#";
        private const string INPUTTASK = VARPREFIX + "0";
        private const string ASSIGNOP = ":";
        public static Script Parse(string[] lines)
        {
            Dictionary<string, ITask> vars = new();
            List<ITask> tasks = new();
            Script batch = new();
            vars.Add(INPUTTASK, batch.InputTask);
            tasks.Add(batch.InputTask);
            IFilter currentFilter = null;
            IBlend currentBlend = null;

            for (int lineNo = 1; lineNo <= lines.Length; ++lineNo)
            {
                // Replace multiple whitespaces with one and trim leading/trailing whitespace
                string line = Regex.Replace(lines[lineNo - 1], @"\s+", " ").Trim();
                if (line == "") continue;

                if (line.StartsWith("- ")) // Property setter, expected form: - NAME: VALUE
                {
                    line = line[2..];
                    if (currentFilter == null && currentBlend == null)
                        throw new ParseException(lineNo, "Trying to set property without filter or blend.");
                    int colon = line.IndexOf(':');
                    if (colon <= 0)
                        throw new ParseException(lineNo, "Expected ':' in property setter.");
                    string propName = line[..colon];
                    string propValue = line[(colon + 1)..].Trim();

                    if (currentFilter != null) // Current task is either filter
                        ReflectiveApi.SetFilterPropertyByName(currentFilter, propName, propValue);
                    if (currentBlend != null) // Or blend
                    {
                        if (propName != "Opacity") // Currently only opacity is supported for blends
                            throw new ParseException(lineNo, $"Unsupported property '{propName}' for blend.");
                        currentBlend.Opacity = int.Parse(propValue);
                    }
                }
                else // Task: filter or blend, expected form: [VAR:] NAME [VAR] [VAR]
                {
                    currentFilter = null;
                    currentBlend = null;

                    // Check if there is an assignment, if yes take LHS as variable name, otherwise generate
                    int assign = line.IndexOf(ASSIGNOP);
                    string newVarName;
                    if (assign == 0)
                        throw new ParseException(lineNo, "Empty left hand side for assignment.");
                    if (assign < 0)
                        newVarName = "_" + vars.Count; // Auto generated ID
                    else
                    {
                        newVarName = line[..assign]; // ID by user
                        if (!newVarName.StartsWith(VARPREFIX))
                            throw new ParseException(lineNo, $"Variable name must start with '{VARPREFIX}'.");
                    }
                    // Check variable name
                    if (vars.ContainsKey(newVarName))
                        throw new ParseException(lineNo, $"Redefinition of variable '{newVarName}'.");
                    if (newVarName == INPUTTASK)
                        throw new ParseException(lineNo, $"Redefinition of reserved input variable '{newVarName}'.");

                    // Take the rest of the line and split into name and arguments
                    line = line[(assign + ASSIGNOP.Length)..].Trim();
                    string[] tokens = line.Split(' ');
                    string name = tokens[0];
                    tokens = tokens.Skip(1).ToArray();

                    if (ReflectiveApi.CheckFilterExists(name)) // Try filter
                    {
                        currentFilter = ReflectiveApi.ConstructFilterByName(name);
                        if (tokens.Length > 1)
                            throw new ParseException(lineNo, "Too many arguments for filter.");

                        ITask parentTask = tasks[^1]; // By default we take the previous task
                        if (tokens.Length == 1) // But if there is an argument, we take that
                        {
                            if (!vars.ContainsKey(tokens[0]))
                                throw new ParseException(lineNo, $"Undeclared variable '{tokens[0]}'.");
                            parentTask = vars[tokens[0]];
                        }
                        FilterTask ft = new(currentFilter, parentTask);
                        batch.AddTask(ft);
                        vars.Add(newVarName, ft);
                        tasks.Add(ft);
                    }
                    else if (ReflectiveApi.CheckBlendExists(name)) // Try blend
                    {
                        currentBlend = ReflectiveApi.ConstructBlendByName(name);

                        if (tokens.Length == 1 || tokens.Length > 2)
                            throw new ParseException(lineNo, "Either 0 or 2 arguments have to be given to blend.");

                        // By default we take the previous two tasks (or the previous one twice if there is only one)
                        ITask parentTask1 = tasks[System.Math.Max(tasks.Count - 2, 0)];
                        ITask parentTask2 = tasks[^1];
                        if (tokens.Length == 2) // But if there are arguments, we take those
                        {
                            if (!vars.ContainsKey(tokens[0]))
                                throw new ParseException(lineNo, $"Undeclared variable '{tokens[0]}'.");
                            if (!vars.ContainsKey(tokens[1]))
                                throw new ParseException(lineNo, $"Undeclared variable '{tokens[1]}'.");
                            parentTask1 = vars[tokens[0]];
                            parentTask2 = vars[tokens[1]];
                        }

                        BlendTask bt = new(currentBlend, parentTask1, parentTask2);
                        batch.AddTask(bt);
                        vars.Add(newVarName, bt);
                        tasks.Add(bt);
                    }
                    else
                        throw new ParseException(lineNo, $"No filter or blend found with name '{name}'.");
                }
            }

            return batch;
        }
    }
}