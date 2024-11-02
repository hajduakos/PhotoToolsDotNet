using FilterLib;
using FilterLib.Filters;

namespace FilterScript.Model
{
    sealed class FilterTask : ITask
    {
        public ITask Parent { get; private set; }
        public IFilter Filter { get; private set; }

        private Image result;
        public FilterTask(IFilter filter, ITask parent)
        {
            this.Filter = filter;
            this.Parent = parent;
            result = null;
        }
        public Image Execute()
        {
            result ??= Filter.Apply(Parent.Execute());
            return result;
        }

        public void Clear()
        {
            result = null;
        }
    }
}
