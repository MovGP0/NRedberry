using System.Collections.Generic;
using NRedberry.Transformations.Options;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Options;

public sealed class TransformationBuilderTest
{
    private sealed class Opts
    {
        [Option(Name = "integer", Index = 0)]
        private int _integer;

        [Option(Name = "requiredInteger", Index = 1)]
        private int _requiredInteger;

        [Option(Name = "defaultString", Index = 2)]
        private string _defaultString = "default";

        [Option(Name = "string", Index = 3)]
        private string? _stringValue;

        public int Integer
        {
            get => _integer;
            set => _integer = value;
        }

        public int RequiredInteger
        {
            get => _requiredInteger;
            set => _requiredInteger = value;
        }

        public string DefaultString
        {
            get => _defaultString;
            set => _defaultString = value;
        }

        public string? StringValue
        {
            get => _stringValue;
            set => _stringValue = value;
        }

        public Opts()
        {
        }

        public Opts(int requiredInteger, string stringValue)
        {
            RequiredInteger = requiredInteger;
            StringValue = stringValue;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj is not Opts other)
            {
                return false;
            }

            return Integer == other.Integer
                && RequiredInteger == other.RequiredInteger
                && string.Equals(DefaultString, other.DefaultString, StringComparison.Ordinal)
                && string.Equals(StringValue, other.StringValue, StringComparison.Ordinal);
        }

        public override int GetHashCode()
        {
            HashCode hashCode = new();
            hashCode.Add(Integer);
            hashCode.Add(RequiredInteger);
            hashCode.Add(DefaultString, StringComparer.Ordinal);
            hashCode.Add(StringValue, StringComparer.Ordinal);
            return hashCode.ToHashCode();
        }
    }

    [Fact(Skip = "TransformationBuilder is not yet implemented.")]
    public void ShouldBuildOptionsFromMap()
    {
        _ = TransformationBuilder.BuildOptionsFromMap(
            typeof(Opts),
            new Dictionary<string, object?>
            {
                ["requiredInteger"] = 1,
                ["string"] = "string"
            });
    }

    [Fact(Skip = "TransformationBuilder is not yet implemented.")]
    public void ShouldCreateExpandAndEliminate()
    {
    }

    [Fact(Skip = "TransformationBuilder is not yet implemented.")]
    public void ShouldCreateExpand()
    {
    }

    [Fact(Skip = "TransformationBuilder is not yet implemented.")]
    public void ShouldCreateDifferentiate()
    {
    }

    [Fact(Skip = "TransformationBuilder is not yet implemented.")]
    public void ShouldCreateExpandTensors()
    {
    }

    [Fact(Skip = "TransformationBuilder is not yet implemented.")]
    public void ShouldCreateExpandWithDefaults()
    {
    }
}
