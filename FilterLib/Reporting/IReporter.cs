namespace FilterLib.Reporting;

/// <summary>
/// Interface for reporting the progress of a filter operation.
/// </summary>
public interface IReporter
{
    /// <summary>
    /// Report that the operation has started.
    /// </summary>
    void Start();

    /// <summary>
    /// Report that the operation has finished.
    /// </summary>
    void Done();

    /// <summary>
    /// Report the current progress of the operation.
    /// </summary>
    /// <param name="value">Current progress value between <paramref name="min"/> and <paramref name="max"/></param>
    /// <param name="min">Minimum value corresponding to no progress</param>
    /// <param name="max">Maximum value corresponding to full progress</param>
    void Report(int value, int min = 0, int max = 100);
}
