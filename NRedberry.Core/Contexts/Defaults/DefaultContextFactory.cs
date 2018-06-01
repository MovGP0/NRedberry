using NRedberry.Core.Indices;
using NRedberry.Core.Tensors;

namespace NRedberry.Core.Contexts.Defaults
{
    public sealed class DefaultContextFactory : IContextFactory
    {
        public static DefaultContextFactory Instance = new DefaultContextFactory();

        private DefaultContextFactory()
        {
        }

        public Context CreateContext()
        {
            //Creating context defaults
            return new Context(DefaultContextSettings.create());
        }
    }

    public static class DefaultContextSettings
    {
        public static ContextSettings create()
        {
            ContextSettings defaultSettings = new ContextSettings(OutputFormat.Redberry, "d");
            defaultSettings.setMetricName("g");

            defaultSettings.addMetricIndexType(IndexType.LatinLower);
            defaultSettings.addMetricIndexType(IndexType.GreekLower);
            defaultSettings.addMetricIndexType(IndexType.LatinUpper);
            defaultSettings.addMetricIndexType(IndexType.GreekUpper);

            //Reading seed from property if exists
            //if (System.getProperty("redberry.nmseed") != null)
            //    defaultSettings.setNameManagerSeed(Long.parseLong(System.getProperty("redberry.nmseed"), 10));

            return defaultSettings;
        }
    }

    /// <summary>
    /// A factory interface for <see cref="Context"/> creation.
    /// </summary>
    public interface IContextFactory
    {
        /// <summary>
        /// Creates a context object.
        /// </summary>
        /// <returns>context</returns>
        Context CreateContext();
    }
}
