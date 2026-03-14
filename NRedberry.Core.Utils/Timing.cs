using System.Diagnostics;

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
    }

    private static object[] MeasureInternal<T>(ITimingJob<T> job, bool printMessage, long divisor, string suffix)
    {
        ArgumentNullException.ThrowIfNull(job);

        long start = Stopwatch.GetTimestamp();
        T result = job.DoJob();
        long elapsed = ToScaledUnits(Stopwatch.GetTimestamp() - start, divisor);
        if (printMessage)
        {
            Console.WriteLine($"Timing: {elapsed}{suffix}");
        }

        return [elapsed, result!];
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
        return MeasureInternal(job, printMessage, 1_000_000L, "ms");
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
        return MeasureInternal(job, printMessage, 1_000L, "µs");
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
        return MeasureInternal(job, printMessage, 1L, "ns");
    }

    /// <summary>
    /// Measures execution time in milliseconds and prints the duration.
    /// </summary>
    /// <typeparam name="T">Result type.</typeparam>
    /// <param name="job">Job to execute.</param>
    /// <returns>Elapsed time and result.</returns>
    public static object[] Measure<T>(ITimingJob<T> job)
    {
        return Measure(job, true);
    }

    /// <summary>
    /// Measures execution time in microseconds and prints the duration.
    /// </summary>
    /// <typeparam name="T">Result type.</typeparam>
    /// <param name="job">Job to execute.</param>
    /// <returns>Elapsed time and result.</returns>
    public static object[] MeasureMicro<T>(ITimingJob<T> job)
    {
        return MeasureMicro(job, true);
    }

    /// <summary>
    /// Measures execution time in nanoseconds and prints the duration.
    /// </summary>
    /// <typeparam name="T">Result type.</typeparam>
    /// <param name="job">Job to execute.</param>
    /// <returns>Elapsed time and result.</returns>
    public static object[] MeasureNano<T>(ITimingJob<T> job)
    {
        return MeasureNano(job, true);
    }

    private static long ToScaledUnits(long elapsedTicks, long divisor)
    {
        long nanoseconds = elapsedTicks * 1_000_000_000L / Stopwatch.Frequency;
        return divisor == 1L ? nanoseconds : nanoseconds / divisor;
    }
}
