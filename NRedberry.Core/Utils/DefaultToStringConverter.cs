namespace NRedberry.Core.Utils
{
    public sealed class DefaultToStringConverter : IToStringConverter<int>
    {
        public string ToString(int t)
        {
            return t.ToString();
        }
    }
}