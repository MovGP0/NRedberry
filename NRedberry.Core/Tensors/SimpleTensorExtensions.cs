using System;
using NRedberry.Contexts;
using NRedberry.Core.Indices;

namespace NRedberry.Core.Tensors;

public static class SimpleTensorExtensions
{
    /**
     * Returns {@code true} if specified tensor is a Kronecker tensor
     *
     * @param t tensor
     * @return {@code true} if specified tensor is a Kronecker tensor
     */
    public static bool IsKronecker(this Context context, SimpleTensor t) {
        return context.nameManager.IsKroneckerOrMetric(t.Name)
               && !IndicesUtils.HaveEqualStates(t.Indices[0], t.Indices[1]);
    }

    /**
     * Returns {@code true} if specified tensor is a metric tensor
     *
     * @param t tensor
     * @return {@code true} if specified tensor is a metric tensor
     */
    public static bool IsMetric(this Context context, SimpleTensor t) {
        return context.nameManager.IsKroneckerOrMetric(t.Name)
               && IndicesUtils.HaveEqualStates(t.Indices[0], t.Indices[1]);
    }

    /**
     * Returns {@code true} if specified tensor is a metric or a Kronecker tensor
     *
     * @param t tensor
     * @return {@code true} if specified tensor is a metric or a Kronecker tensor
     */
    public static bool IsKroneckerOrMetric(this Context context, SimpleTensor t) {
        return context.nameManager.IsKroneckerOrMetric(t.Name);
    }

    /**
     * Returns Kronecker tensor with specified upper and lower indices.
     *
     * @param index1 first index
     * @param index2 second index
     * @return Kronecker tensor with specified upper and lower indices
     * @throws IllegalArgumentException if indices have same states
     * @throws IllegalArgumentException if indices have different types
     */
    public static SimpleTensor CreateKronecker(this Context context, int index1, int index2) {
        byte type;
        if ((type = IndicesUtils.GetType_(index1)) != IndicesUtils.GetType_(index2) || IndicesUtils.GetRawStateInt((int)index1) == IndicesUtils.GetRawStateInt((int)index2))
            throw new ArgumentException("This is not kronecker indices!");

        if (!IsMetric(type) && IndicesUtils.GetState(index2))
        {
            (index1, index2) = (index2, index1);
        }

        SimpleIndices indices = IndicesFactory.CreateSimple(null, (int)index1, (int)index2);
        var nd = context.nameManager.mapNameDescriptor(context.nameManager.getKroneckerName(), new StructureOfIndices(indices));
        var name = nd.Id;
        return Tensor.SimpleTensor(name, indices);
    }

    private static readonly LongBackedBitArray metricTypes = new(128);

    public static bool IsMetric(byte type)
    {
        return metricTypes[type];
    }

    /**
     * Returns metric tensor with specified indices.
     *
     * @param index1 first index
     * @param index2 second index
     * @return metric tensor with specified indices
     * @throws IllegalArgumentException if indices have different states
     * @throws IllegalArgumentException if indices have different types
     * @throws IllegalArgumentException if indices have non metric types
     */
    public static SimpleTensor CreateMetric(this Context context, int index1, int index2) {
        byte type;
        if ((type = IndicesUtils.GetType_(index1)) != IndicesUtils.GetType_(index2)
            || !IndicesUtils.HaveEqualStates(index1, index2)
            || !context.metricTypes.Get(type))
            throw new ArgumentException("Not metric indices.");
        var indices = IndicesFactory.CreateSimple(null, (int)index1, (int)index2);
        var nd = context.nameManager.mapNameDescriptor(context.nameManager.GetMetricName(), new StructureOfIndices(indices));
        var name = nd.Id;
        return Tensor.SimpleTensor(name, indices);
    }

    /**
     * Returns metric tensor if specified indices have same states and
     * Kronecker tensor if specified indices have different states.
     *
     * @param index1 first index
     * @param index2 second index
     * @return metric tensor if specified indices have same states and
     *         Kronecker tensor if specified indices have different states
     * @throws IllegalArgumentException if indices have different types
     * @throws IllegalArgumentException if indices have same states and non metric types
     */
    public static SimpleTensor CreateMetricOrKronecker(this Context context, int index1, int index2) {
        if (IndicesUtils.GetRawStateInt(index1) == IndicesUtils.GetRawStateInt(index2))
            return createMetric(context, index1, index2);
        return createKronecker(context, index1, index2);
    }

    public static SimpleTensor createMetric(this Context context, int index1, int index2)
    {
        throw new NotImplementedException();
    }

    public static SimpleTensor createKronecker(this Context context, int index1, int index2)
    {
        throw new NotImplementedException();
    }
}