namespace NRedberry.Core.Tensors
{
    public sealed class ProductFactory : ITensorFactory
    {
        public static ProductFactory Factory = new ProductFactory();

        public Tensor Create(params Tensor[] tensors)
        {
            throw new System.NotImplementedException();
        }
    }
}