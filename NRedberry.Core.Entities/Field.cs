namespace NRedberry;

public interface Field<T>
{
    T Zero { get; }
    T One { get; }
    TC GetRuntimeClass<TC>() where TC : FieldElement<T>;
}