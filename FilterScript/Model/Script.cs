using System.Collections.Generic;
using FilterLib;
using System.Linq;

namespace FilterScript.Model
{
    sealed class Script
    {
        private readonly List<ITask> tasks;

        internal InputTask InputTask { get; }

        public Script()
        {
            tasks = new List<ITask>();
            InputTask = new InputTask();
            tasks.Add(InputTask);
        }

        public void AddTask(ITask task) => tasks.Add(task);

        public Image Execute(Image input)
        {
            InputTask.Input = input;
            Image result = tasks[^1].Execute();
            foreach (ITask t in tasks.Take(tasks.Count - 1)) t.Clear();
            return result;
        }

    }
}
