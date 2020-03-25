using System.Collections.Generic;
using System.Drawing;

namespace FilterScript.Model
{
    sealed class Batch
    {
        private readonly List<ITask> tasks;
        public InputTask InputTask { get; private set; }

        public Batch()
        {
            tasks = new List<ITask>();
            InputTask = new InputTask();
            tasks.Add(InputTask);
        }

        public void AddTask(ITask task)
        {
            tasks.Add(task);
        }

        public Bitmap Execute()
        {
            return tasks[tasks.Count - 1].Execute();
        }

    }
}
