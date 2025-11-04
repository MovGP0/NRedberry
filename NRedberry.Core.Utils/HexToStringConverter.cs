namespace NRedberry.Core.Utils;

public sealed class HexToStringConverter : IToStringConverter<int>
{
    public string ToString(int t)
    {
        return Convert.ToString(t, 16);
    }
}
