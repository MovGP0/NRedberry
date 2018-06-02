namespace NRedberry.Core.Combinatorics
{
    public interface IOutputPortUnsafe<out T>
    {
        T Take();
    }
}