namespace NRedberry.Core.Tensors;

public static class TensorArrayUtils
{
    public static Tensor[] AddAll(Tensor[] array1, params Tensor[] array2)
    {
        Tensor[] r = new Tensor[array1.Length + array2.Length];
        Array.Copy(array1, 0, r, 0, array1.Length);
        Array.Copy(array2, 0, r, array1.Length, array2.Length);
        return r;
    }

    public static Tensor[] Remove(Tensor[] array, int i)
    {
        Tensor[] r = new Tensor[array.Length - 1];
        Array.Copy(array, 0, r, 0, i);
        if (i < array.Length - 1)
            Array.Copy(array, i + 1, r, i, array.Length - i - 1);
        return r;
    }
}
