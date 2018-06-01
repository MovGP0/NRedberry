namespace NRedberry.Core.Contexts
{
    public sealed class ParseManager
    {
        private Parser parser;

        public ParseManager(Parser parser)
        {
            this.parser = parser;
        }
    }
}