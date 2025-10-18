namespace NRedberry.Core.Numbers.Parser;

/*
 * Original: ./core/src/main/java/cc/redberry/core/number/parser/TokenParser.java
 */

public interface INumberTokenParser<T>
{
    T Parse(string expression, NumberParser<T> parser);
}
