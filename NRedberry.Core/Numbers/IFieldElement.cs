namespace NRedberry.Core.Numbers;

public interface IFieldElement<T>
{
    T Add(T a);
    T Subtract(T a);
    T Negate();
    T Multiply(int n);
    T Multiply(T a);
    T Divide(T a);
    T Reciprocal();
    IField<T> GetField();
}