using NRedberry.Contexts;

namespace NRedberry.Core.Contexts;

/*
 * Original: ./core/src/main/java/cc/redberry/core/context/ToString.java
 */

public interface IOutputFormattable
{
    string ToString(OutputFormat outputFormat);

    new string ToString();
}
