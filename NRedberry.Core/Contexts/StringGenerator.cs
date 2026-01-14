namespace NRedberry.Contexts;

internal sealed class StringGenerator
{
    private long count;

    public string NextString() => $"{NameManager.DEFAULT_VAR_SYMBOL_PREFIX}{count++}";
}
