using NRedberry.Indices;
using NRedberry.Numbers;
using NRedberry.Tensors;
using NRedberry.Tensors.Functions;

namespace NRedberry.Parsers;

/*
 * Original: ./core/src/main/java/cc/redberry/core/parser/ParseUtils.java
 */

public static class ParseUtils
{
    public static bool CheckBracketsConsistence(string expression)
    {
        ArgumentNullException.ThrowIfNull(expression);

        var levels = new int[3];
        foreach (char c in expression)
        {
            if (levels[0] < 0 || levels[1] < 0 || levels[2] < 0)
            {
                return false;
            }

            switch (c)
            {
                case '(':
                {
                    ++levels[0];
                    break;
                }

                case ')':
                {
                    --levels[0];
                    break;
                }

                case '[':
                {
                    ++levels[1];
                    break;
                }

                case ']':
                {
                    --levels[1];
                    break;
                }

                case '{':
                {
                    ++levels[2];
                    break;
                }

                case '}':
                {
                    --levels[2];
                    break;
                }
            }
        }

        return levels[0] == 0 && levels[1] == 0 && levels[2] == 0;
    }

    public static ParseToken TensorToAst(Tensor tensor)
    {
        if (tensor is TensorField field)
        {
            var content = new ParseToken[field.Size];
            int i = 0;
            foreach (Tensor t in field)
            {
                content[i++] = TensorToAst(t);
            }

            return new ParseTokenTensorField(field.SimpleIndices, field.GetStringName(), content, field.GetArgIndices());
        }

        if (tensor is SimpleTensor simpleTensor)
        {
            return new ParseTokenSimpleTensor(simpleTensor.SimpleIndices, simpleTensor.GetStringName());
        }

        if (tensor is Complex complex)
        {
            return new ParseTokenNumber(complex);
        }

        if (tensor is Expression)
        {
            return new ParseTokenExpression(false, TensorToAst(tensor[0]), TensorToAst(tensor[1]));
        }

        var nodes = new ParseToken[tensor.Size];
        int index = 0;
        foreach (Tensor t in tensor)
        {
            nodes[index++] = TensorToAst(t);
        }

        if (tensor is ScalarFunction)
        {
            return new ParseTokenScalarFunction(tensor.GetType().Name, nodes);
        }

        return new ParseToken(Enum.Parse<TokenType>(tensor.GetType().Name, false), nodes);
    }

    public static ISet<int> GetAllIndices(ParseToken node)
    {
        ArgumentNullException.ThrowIfNull(node);

        HashSet<int> set = [];
        GetAllIndicesCore(node, set);
        return set;
    }

    public static ISet<int> GetAllIndicesT(ParseToken node)
    {
        ArgumentNullException.ThrowIfNull(node);

        HashSet<int> set = [];
        GetAllIndicesCore(node, set);
        return set;
    }

    private static void GetAllIndicesCore(ParseToken node, ISet<int> set)
    {
        if (node is ParseTokenSimpleTensor)
        {
            Indices.Indices indices = node.GetIndices();
            for (int i = indices.Size() - 1; i >= 0; --i)
            {
                set.Add(IndicesUtils.GetNameWithType(indices[i]));
            }
        }
        else
        {
            foreach (ParseToken pn in node.Content)
            {
                if (pn is not ParseTokenScalarFunction)
                {
                    GetAllIndicesCore(pn, set);
                }
            }
        }
    }
}
