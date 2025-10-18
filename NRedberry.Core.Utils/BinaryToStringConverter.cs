namespace NRedberry.Core.Utils;

public sealed class BinaryToStringConverter : IToStringConverter<int>
{
    public string ToString(int t)
    {
        return Convert.ToString(t, 2);
    }
}