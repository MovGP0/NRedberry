using NRedberry.IndexMapping;
using Xunit.Sdk;

namespace NRedberry.Core.Tests.Indexmapping;

/// <summary>
/// Skeleton port of cc.redberry.core.indexmapping.IndexMappingTestUtils.
/// </summary>
public static class IndexMappingTestUtils
{
    private static readonly IComparer<Mapping> s_comparator = Comparer<Mapping>.Default;

    public static Mapping Parse(string value)
    {
        throw SkipException.ForSkip("Pending port from Java.");
    }

    public static bool Compare(IList<Mapping> first, IList<Mapping> second)
    {
        throw SkipException.ForSkip("Pending port from Java.");
    }

    public static IComparer<Mapping> GetComparator()
    {
        throw SkipException.ForSkip("Pending port from Java.");
    }

    private static int ParseIndex(string value)
    {
        throw SkipException.ForSkip("Pending port from Java.");
    }
}
