namespace Shouldly;

public static class TypeExtensions
{
    extension(Type type)
    {
        public void ShouldHaveMethod(string name, Type returnType, params Type[] parameterTypes)
        {
            var method = type.GetMethod(name, parameterTypes);

            method.ShouldNotBeNull();
            method.ReturnType.ShouldBe(returnType);

            var interfaceParameterTypes = method.GetParameters().Select(parameter => parameter.ParameterType).ToArray();
            interfaceParameterTypes.ShouldBe(parameterTypes);
        }
    }
}
