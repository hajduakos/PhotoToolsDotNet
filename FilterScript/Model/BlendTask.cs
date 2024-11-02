using FilterLib.Blending;
using FilterLib;

namespace FilterScript.Model
{
    sealed class BlendTask : ITask
    {
        public ITask BottomParent { get; private set; }
        public ITask TopParent { get; private set; }
        public IBlend Blend { get; private set; }

        private Image result;

        public BlendTask(IBlend blend, ITask bottomParent, ITask topParent)
        {
            this.Blend = blend;
            this.BottomParent = bottomParent;
            this.TopParent = topParent;
            result = null;
        }
        public Image Execute()
        {
            result ??= Blend.Apply(BottomParent.Execute(), TopParent.Execute());
            return result;
        }

        public void Clear()
        {
            result = null;
        }
    }
}
