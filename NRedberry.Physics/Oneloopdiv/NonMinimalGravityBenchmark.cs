namespace NRedberry.Physics.Oneloopdiv;

/// <summary>
/// Skeleton port of cc.redberry.physics.oneloopdiv.NonMinimalGravityBenchmark.
/// </summary>
public sealed class NonMinimalGravityBenchmark
{
    private NonMinimalGravityBenchmark()
    {
    }

    public static void Main(string[] args)
    {
        // Benchmarks.BurnJvm();
        Benchmarks.Timer timer = new();
        timer.Start();
        OneLoopUtils.SetUpRiemannSymmetries();
        // Benchmarks.TestNonMinimalGaugeGravity();
        Benchmarks.TestSpin3Ghosts();
        Console.WriteLine($"Non minimal gravity: {timer.ElapsedTimeInSeconds()} s.");
    }
}
