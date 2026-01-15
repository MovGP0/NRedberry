using System.Globalization;
using System.Text;
using ContextCC = NRedberry.Contexts.CC;
using NRedberry.Contexts;
using NRedberry.Indices;
using NRedberry.Tensors;

namespace NRedberry.Parsers;

/*
 * Original: ./core/src/main/java/cc/redberry/core/parser/ParseTokenTensorField.java
 */

public class ParseTokenTensorField : ParseTokenSimpleTensor
{
    public SimpleIndices[] ArgumentsIndices { get; }

    public ParseTokenTensorField(
        SimpleIndices indices,
        string name,
        ParseToken[] content,
        SimpleIndices[] argumentsIndices)
        : base(indices, name, TokenType.TensorField, content)
    {
        ArgumentNullException.ThrowIfNull(argumentsIndices);

        ArgumentsIndices = argumentsIndices;
    }

    public override NameAndStructureOfIndices GetIndicesTypeStructureAndName()
    {
        var typeStructures = new StructureOfIndices[1 + ArgumentsIndices.Length];
        typeStructures[0] = StructureOfIndices.Create(Indices);
        for (int i = 0; i < ArgumentsIndices.Length; ++i)
        {
            if (ArgumentsIndices[i] is null)
            {
                ArgumentsIndices[i] = IndicesFactory.CreateSimple(null, Content[i].GetIndices().GetFree());
            }

            typeStructures[i + 1] = StructureOfIndices.Create(ArgumentsIndices[i]);
        }

        return new NameAndStructureOfIndices(Name, typeStructures);
    }

    public override string ToString()
    {
        StringBuilder sb = new();
        sb.Append(base.ToString()).Append('[');
        foreach (ParseToken node in Content)
        {
            sb.Append(node).Append(", ");
        }

        sb.Remove(sb.Length - 2, 2).Append(']');
        return sb.ToString();
    }

    public override Tensor ToTensor()
    {
        Tensor[] arguments = ContentToTensors();
        for (int i = 0; i < arguments.Length; ++i)
        {
            if (ArgumentsIndices[i] is null)
            {
                ArgumentsIndices[i] = IndicesFactory.CreateSimple(null, arguments[i].Indices.GetFree());
            }
        }

        int tildeIndex = Name.IndexOf('~', StringComparison.Ordinal);
        if (tildeIndex >= 0)
        {
            string ordersDescriptor = Name[(tildeIndex + 1)..].Replace(" ", string.Empty);
            string fieldName = Name[..tildeIndex];
            if (ordersDescriptor.Length == 0)
            {
                throw new ParserException($"Error in derivative orders in \"{Name}\"");
            }

            if (ordersDescriptor[0] == '(')
            {
                if (ordersDescriptor[^1] != ')')
                {
                    throw new ParserException($"Unbalanced brackets in derivative orders in \"{Name}\"");
                }

                ordersDescriptor = ordersDescriptor[1..^1];
            }

            string[] ordersStr = ordersDescriptor.Split(',', StringSplitOptions.None);
            if (ordersStr.Length != arguments.Length)
            {
                throw new ParserException($"Number of arguments does not match number of derivative orders in \"{Name}\"");
            }

            var orders = new int[ordersStr.Length];
            for (int i = orders.Length - 1; i >= 0; --i)
            {
                if (!int.TryParse(ordersStr[i], NumberStyles.Integer, CultureInfo.InvariantCulture, out orders[i]))
                {
                    throw new ParserException($"Illegal order of derivative: \"{ordersStr[i]}\" in \"{Name}\"");
                }
            }

            return FieldDerivative(fieldName, Indices, ArgumentsIndices, arguments, orders);
        }

        return TensorField.Create(Name, Indices, ArgumentsIndices, arguments);
    }

    private static TensorField FieldDerivative(
        string name,
        SimpleIndices indices,
        SimpleIndices[] argIndices,
        Tensor[] arguments,
        int[] orders)
    {
        if (argIndices.Length != arguments.Length)
        {
            throw new ArgumentException("Argument indices array and arguments array have different length.");
        }

        if (arguments.Length == 0)
        {
            throw new ArgumentException("No arguments in field.");
        }

        for (int i = 0; i < argIndices.Length; ++i)
        {
            if (!arguments[i].Indices.GetFree().EqualsRegardlessOrder(argIndices[i]))
            {
                throw new ArgumentException("Arguments indices are inconsistent with arguments.");
            }
        }

        try
        {
            var structures = new StructureOfIndices[argIndices.Length + 1];
            StructureOfIndices structureOfIndices = indices.StructureOfIndices;
            for (int i = argIndices.Length - 1; i >= 0; --i)
            {
                structures[i + 1] = argIndices[i].StructureOfIndices;
                for (int j = orders[i]; j > 0; --j)
                {
                    structureOfIndices = structureOfIndices.Subtract(structures[i + 1]);
                }
            }

            structures[0] = structureOfIndices;

            var fieldDescriptor = (NameDescriptorForTensorField)ContextCC.NameManager.MapNameDescriptor(name, structures);
            NameDescriptor derivativeDescriptor = fieldDescriptor.GetDerivative(orders);
            SimpleIndices tensorIndices = IndicesFactory.CreateSimple(derivativeDescriptor.GetSymmetries(), indices);
            return new TensorField(derivativeDescriptor.Id, tensorIndices, arguments, argIndices);
        }
        catch (Exception ex)
        {
            throw new ArgumentException("Inconsistent derivative orders/indices.", ex);
        }
    }
}
