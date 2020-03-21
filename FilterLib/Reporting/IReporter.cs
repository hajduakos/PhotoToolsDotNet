namespace FilterLib.Reporting
{
    public interface IReporter
    {
        void Report(int value, int min = 0, int max = 100);
    }
}
