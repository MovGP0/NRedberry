///<summary>
/// AST nodes types.
///</summary>
public enum TokenType
{
    /// <summary>
    /// Enum values are equal to string representations of corresponding classes:
    /// (TensorType.Power <-> Power and so on)
    /// </summary>
    SimpleTensor,
    TensorField,
    Product,
    Sum,
    Expression,
    Power,
    Number,
    ScalarFunction,
    Dummy
}