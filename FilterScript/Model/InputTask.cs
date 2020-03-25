using System.Drawing;

namespace FilterScript.Model
{
    sealed class InputTask : ITask
    {
        public Bitmap Input { get; set; }

        public InputTask(Bitmap input = null) => this.Input = input;

        public Bitmap Execute() => Input;
    }
}
