namespace NRedberry.Core.Numbers;

public interface IField<T>
{
    T GetZero();
    T GetOne();
    TC GetRuntimeClass<TC>() where TC : IFieldElement<T>;
}