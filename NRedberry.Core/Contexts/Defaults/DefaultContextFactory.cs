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
}
