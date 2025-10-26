using System.Reflection;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Gb;

internal static class PolynomialReflectionHelpers
{
    private const BindingFlags Flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

    public static GenPolynomialRing<C>? GetPolynomialRing<C>(GenPolynomial<C>? polynomial) where C : RingElem<C>
    {
        if (polynomial == null)
        {
            return null;
        }

        Type type = polynomial.GetType();

        PropertyInfo? prop = type.GetProperty("Ring", Flags);
        if (prop is not null && typeof(GenPolynomialRing<C>).IsAssignableFrom(prop.PropertyType))
        {
            return (GenPolynomialRing<C>?)prop.GetValue(polynomial);
        }

        FieldInfo? field = type.GetField("ring", Flags);
        if (field is not null && typeof(GenPolynomialRing<C>).IsAssignableFrom(field.FieldType))
        {
            return (GenPolynomialRing<C>?)field.GetValue(polynomial);
        }

        try
        {
            ElemFactory<GenPolynomial<C>> factory = polynomial.Factory();
            if (factory is GenPolynomialRing<C> ring)
            {
                return ring;
            }
        }
        catch (NotImplementedException)
        {
        }

        return null;
    }

    public static bool IsZero<C>(GenPolynomial<C> polynomial) where C : RingElem<C>
        => InvokeBool(polynomial, "IsZERO", "isZERO");

    public static bool IsConstant<C>(GenPolynomial<C> polynomial) where C : RingElem<C>
        => InvokeBool(polynomial, "IsConstant", "isConstant");

    public static bool IsOne<C>(GenPolynomial<C> polynomial) where C : RingElem<C>
        => InvokeBool(polynomial, "IsONE", "isONE");

    public static int Length<C>(GenPolynomial<C> polynomial) where C : RingElem<C>
        => InvokeInt(polynomial, "Length", "length");

    public static GenPolynomial<C> Monic<C>(GenPolynomial<C> polynomial) where C : RingElem<C>
        => (GenPolynomial<C>)InvokeRequired(polynomial, "Monic", "monic")!;

    public static object? LeadingExpVector<C>(GenPolynomial<C> polynomial) where C : RingElem<C>
        => Invoke(polynomial, "LeadingExpVector", "leadingExpVector");

    public static object? LeadingMonomial<C>(GenPolynomial<C> polynomial) where C : RingElem<C>
        => Invoke(polynomial, "LeadingMonomial", "leadingMonomial");

    public static int[]? DependencyOnVariables(object expVector)
        => Invoke(expVector, "DependencyOnVariables", "dependencyOnVariables") as int[];

    public static dynamic? GetCoFactor<C>(GenPolynomial<C> polynomial) where C : RingElem<C>
    {
        GenPolynomialRing<C>? ring = GetPolynomialRing(polynomial);
        if (ring is not null)
        {
            return ring.CoFac;
        }

        object? result = GetMember(polynomial, "CoFac", "coFac");
        return result ?? GetMember(polynomial, "Factory", "factory");
    }

    public static dynamic? GetCoFactorFromRing<C>(GenPolynomialRing<C> ring) where C : RingElem<C>
        => ring.CoFac;

    public static int Signum(object obj)
        => InvokeInt(obj, "Signum", "signum");

    public static bool ExpVectorMultipleOf(object left, object right)
        => InvokeBool(left, ["MultipleOf", "multipleOf"], right);

    public static object? ExpVectorSubtract(object left, object right)
        => Invoke(left, ["Subtract", "subtract"], right);

    public static C DivideCoefficient<C>(C numerator, C denominator) where C : RingElem<C>
        => (C)InvokeRequired(numerator, ["Divide", "divide"], denominator)!;

    public static GenPolynomial<C> SumMonomial<C>(GenPolynomial<C> polynomial, C coefficient, object expVector) where C : RingElem<C>
        => (GenPolynomial<C>)InvokeRequired(polynomial, ["Sum", "sum"], coefficient, expVector)!;

    public static GenPolynomial<C> SubtractMonomial<C>(GenPolynomial<C> polynomial, C coefficient, object expVector) where C : RingElem<C>
        => (GenPolynomial<C>)InvokeRequired(polynomial, ["Subtract", "subtract"], coefficient, expVector)!;

    public static GenPolynomial<C> SubtractPolynomial<C>(GenPolynomial<C> polynomial, GenPolynomial<C> other) where C : RingElem<C>
        => (GenPolynomial<C>)InvokeRequired(polynomial, ["Subtract", "subtract"], other)!;

    public static GenPolynomial<C> MultiplyMonomial<C>(GenPolynomial<C> polynomial, C coefficient, object expVector) where C : RingElem<C>
        => (GenPolynomial<C>)InvokeRequired(polynomial, ["Multiply", "multiply"], coefficient, expVector)!;

    public static GenPolynomial<C> GetZeroPolynomial<C>(GenPolynomial<C> polynomial) where C : RingElem<C>
    {
        GenPolynomialRing<C>? ring = GetPolynomialRing(polynomial);
        if (ring == null)
        {
            throw new InvalidOperationException("Polynomial ring not available.");
        }
        return GenPolynomialRing<C>.Zero;
    }

    public static bool CoefficientFieldIsField<C>(GenPolynomial<C> polynomial) where C : RingElem<C>
    {
        dynamic? coFac = GetCoFactor(polynomial);
        if (coFac == null)
        {
            return false;
        }
        try
        {
            return InvokeBool(coFac, new[] { "IsField", "isField" });
        }
        catch (InvalidOperationException)
        {
            return false;
        }
    }

    public static object? GetMapEntryKey(object entry)
    {
        object? key = GetMember(entry, "Key", "key");
        if (key is not null)
        {
            return key;
        }
        return Invoke(entry, "GetKey", "getKey");
    }

    public static object? GetMapEntryValue(object entry)
    {
        object? value = GetMember(entry, "Value", "value");
        if (value is not null)
        {
            return value;
        }
        return Invoke(entry, "GetValue", "getValue");
    }

    public static dynamic? GetMember(object target, params string[] memberNames)
    {
        foreach (string name in memberNames)
        {
            PropertyInfo? prop = target.GetType().GetProperty(name, Flags);
            if (prop is not null)
            {
                return prop.GetValue(target);
            }

            FieldInfo? field = target.GetType().GetField(name, Flags);
            if (field is not null)
            {
                return field.GetValue(target);
            }
        }

        return null;
    }

    public static bool InvokeBool(object target, string[] methodNames, params object?[] args)
    {
        object? result = Invoke(target, methodNames, args);
        return result is true;
    }

    public static bool InvokeBool(object target, params string[] methodNames)
        => InvokeBool(target, methodNames, []);

    public static int InvokeInt(object target, string[] methodNames, params object?[] args)
    {
        object? result = Invoke(target, methodNames, args);
        return result is int value ? value : 0;
    }

    public static int InvokeInt(object target, params string[] methodNames)
        => InvokeInt(target, methodNames, []);

    public static object? Invoke(object target, string[] methodNames, params object?[] args)
    {
        foreach (string name in methodNames)
        {
            MethodInfo? method = FindMethod(target.GetType(), name, args.Length);
            if (method is not null)
            {
                return method.Invoke(target, args);
            }
        }
        return null;
    }

    public static object? Invoke(object target, params string[] methodNames)
        => Invoke(target, methodNames, []);

    public static object InvokeRequired(object target, string[] methodNames, params object?[] args)
    {
        object? result = Invoke(target, methodNames, args);
        if (result == null)
        {
            throw new InvalidOperationException($"Unable to invoke method(s): {string.Join(", ", methodNames)} on {target.GetType().FullName}.");
        }
        return result;
    }

    public static object InvokeRequired(object target, params string[] methodNames)
        => InvokeRequired(target, methodNames, []);

    private static MethodInfo? FindMethod(Type type, string name, int parameterCount)
    {
        foreach (MethodInfo method in type.GetMethods(Flags))
        {
            if (string.Equals(method.Name, name, StringComparison.OrdinalIgnoreCase) &&
                method.GetParameters().Length == parameterCount)
            {
                return method;
            }
        }
        return null;
    }
}
