using System.Collections.Generic;
using System.Drawing;
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

        public Bitmap Execute(Bitmap input)
        {
            InputTask.Input = input;
            Bitmap result = tasks[tasks.Count - 1].Execute();
            foreach (ITask t in tasks.Take(tasks.Count - 1)) t.Clear();
            return result;
        }

    }
}
