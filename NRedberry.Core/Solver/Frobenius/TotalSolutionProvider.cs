using NRedberry.Concurrent;

namespace NRedberry.Solver.Frobenius;

/*
 * Original: ./core/src/main/java/cc/redberry/core/solver/frobenius/TotalSolutionProvider.java
 */

internal sealed class TotalSolutionProvider : IOutputPort<int[]>
{
    public TotalSolutionProvider(SolutionProvider[] providers)
    {
        ArgumentNullException.ThrowIfNull(providers);

        _providers = providers;
    }

    public int[] Take()
    {
        if (!_inited)
        {
            foreach (SolutionProvider provider in _providers)
            {
                provider.Tick();
            }

            _inited = true;
        }

        int i = _providers.Length - 1;
        int[] solution = _providers[i].Take();
        if (solution is not null)
        {
            return solution;
        }

        while (true)
        {
            bool r;
            while ((r = !_providers[i--].Tick()) && i >= 0)
            {
            }

            if (i == -1 && r)
            {
                return null!;
            }

            i += 2;
            for (; i < _providers.Length; ++i)
            {
                if (!_providers[i].Tick())
                {
                    i--;
                    goto Outer;
                }
            }

            System.Diagnostics.Debug.Assert(i == _providers.Length);
            i--;
            solution = _providers[i].Take();
            if (solution is not null)
            {
                return solution;
            }

            Outer:
            ;
        }
    }

    private readonly SolutionProvider[] _providers;
    private bool _inited;
}
