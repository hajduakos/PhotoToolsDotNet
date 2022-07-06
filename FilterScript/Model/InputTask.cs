using FilterLib;

namespace FilterScript.Model
{
    sealed class InputTask : ITask
    {
        public Image Input { get; set; }

        public InputTask(Image input = null) => this.Input = input;

        public Image Execute() => Input;

        public void Clear() { }
    }
}
