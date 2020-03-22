namespace FilterLib.Reporting
{
    public interface IReporter
    {
        void Start();
        void Done();
        void Report(int value, int min = 0, int max = 100);
    }
}
