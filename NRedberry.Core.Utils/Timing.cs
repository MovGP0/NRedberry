namespace NRedberry.Core.Utils;

/// <summary>
/// Port of cc.redberry.core.utils.Timing.
/// </summary>
public sealed class Timing
{
    /// <summary>
    /// Defines a job whose execution time is measured.
    /// </summary>
    /// <typeparam name="T">Result type.</typeparam>
    public interface ITimingJob<T>
    {
        /// <summary>
        /// Executes the job.
        /// </summary>
        /// <returns>The job result.</returns>
        T DoJob();
    }

    private Timing()
    {
        throw new NotImplementedException();
    }

    private static object[] MeasureInternal<T>(ITimingJob<T> job, bool printMessage, long divisor, string suffix)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Measures execution time in milliseconds.
    /// </summary>
    /// <typeparam name="T">Result type.</typeparam>
    /// <param name="job">Job to execute.</param>
    /// <param name="printMessage">Whether to print duration.</param>
    /// <returns>Elapsed time and result.</returns>
    public static object[] Measure<T>(ITimingJob<T> job, bool printMessage)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Measures execution time in microseconds.
    /// </summary>
    /// <typeparam name="T">Result type.</typeparam>
    /// <param name="job">Job to execute.</param>
    /// <param name="printMessage">Whether to print duration.</param>
    /// <returns>Elapsed time and result.</returns>
    public static object[] MeasureMicro<T>(ITimingJob<T> job, bool printMessage)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Measures execution time in nanoseconds.
    /// </summary>
    /// <typeparam name="T">Result type.</typeparam>
    /// <param name="job">Job to execute.</param>
    /// <param name="printMessage">Whether to print duration.</param>
    /// <returns>Elapsed time and result.</returns>
    public static object[] MeasureNano<T>(ITimingJob<T> job, bool printMessage)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Measures execution time in milliseconds and prints the duration.
    /// </summary>
    /// <typeparam name="T">Result type.</typeparam>
    /// <param name="job">Job to execute.</param>
    /// <returns>Elapsed time and result.</returns>
    public static object[] Measure<T>(ITimingJob<T> job)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Measures execution time in microseconds and prints the duration.
    /// </summary>
    /// <typeparam name="T">Result type.</typeparam>
    /// <param name="job">Job to execute.</param>
    /// <returns>Elapsed time and result.</returns>
    public static object[] MeasureMicro<T>(ITimingJob<T> job)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Measures execution time in nanoseconds and prints the duration.
    /// </summary>
    /// <typeparam name="T">Result type.</typeparam>
    /// <param name="job">Job to execute.</param>
    /// <returns>Elapsed time and result.</returns>
    public static object[] MeasureNano<T>(ITimingJob<T> job)
    {
        throw new NotImplementedException();
    }
}
