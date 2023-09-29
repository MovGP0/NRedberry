namespace NRedberry;

public interface FieldElement<T>
{
    T Add(T a);
    T Subtract(T a);
    T Negate();
    T Multiply(int n);
    T Multiply(T a);
    T Divide(T a);
    T Reciprocal();
    Field<T> GetField();
}