namespace NRedberry.Contexts.Defaults;

/// <summary>
/// The default Redberry context settings.
/// </summary>
/// <remarks>https://github.com/redberry-cas/core/blob/master/src/main/java/cc/redberry/core/context/defaults/DefaultContextSettings.java</remarks>
public static class DefaultContextSettings
{
    public static ContextSettings Create()
    {
        var defaultSettings = new ContextSettings();
        defaultSettings.AddMetricIndexType(IndexType.LatinLower);
        defaultSettings.AddMetricIndexType(IndexType.GreekLower);
        defaultSettings.AddMetricIndexType(IndexType.LatinUpper);
        defaultSettings.AddMetricIndexType(IndexType.GreekUpper);

        //Reading seed from environment variable if exists
        var seed = Environment.GetEnvironmentVariable("redberry.nmseed");
        if (seed != null)
        {
            defaultSettings.NameManagerSeed = int.TryParse(seed, out var seedValue) ? seedValue : 10;
        }

        return defaultSettings;
    }
}