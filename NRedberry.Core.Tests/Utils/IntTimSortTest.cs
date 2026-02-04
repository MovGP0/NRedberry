using NRedberry.Core.Utils;
using Xunit;

namespace NRedberry.Core.Tests.Utils;

public sealed class IntTimSortTest
{
    private const int K = 200;

    [Fact]
    public void ShouldSortWithCoSortArray()
    {
        const int pivot = K / 6;
        Random random = new();
        for (int n = 0; n < 1000; ++n)
        {
            int[] main = new int[K];
            int[] perm = new int[K];
            int counter = 0;
            for (int i = 0; i < K; ++i)
            {
                int value = random.Next(K / 4);
                main[i] = value;
                perm[i] = value;
                if (value == pivot)
                {
                    perm[i] = counter++;
                }
            }

            IntTimSort.Sort(main, perm);
            counter = 0;
            for (int i = 0; i < K; ++i)
            {
                if (i < K - 1)
                {
                    Assert.True(main[i] <= main[i + 1]);
                }

                if (main[i] != pivot)
                {
                    Assert.Equal(main[i], perm[i]);
                }
                else
                {
                    Assert.Equal(counter++, perm[i]);
                }
            }
        }
    }
}
