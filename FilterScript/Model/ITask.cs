using FilterLib;

namespace FilterScript.Model
{
    interface ITask
    {
        public Image Execute();

        public void Clear();
    }
}
