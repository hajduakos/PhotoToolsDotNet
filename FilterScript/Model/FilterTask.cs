using FilterLib.Filters;
using System.Drawing;

namespace FilterScript.Model
{
    sealed class FilterTask : ITask
    {
        public ITask Parent { get; private set; }
        public IFilter Filter { get; private set; }
        private Bitmap result;
        public FilterTask(IFilter filter, ITask parent)
        {
            this.Filter = filter;
            this.Parent = parent;
            result = null;
        }
        public Bitmap Execute()
        {
            if (result == null) result = Filter.Apply(Parent.Execute());
            return result;
        }
    }
}
