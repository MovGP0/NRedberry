namespace Shouldly;

public static class BoolExtensions
{
    extension(bool? value)
    {
        public void ShouldBeFalse()
        {
            value.ShouldNotBeNull().ShouldBeFalse();
        }

        public void ShouldBeTrue()
        {
            value.ShouldNotBeNull().ShouldBeTrue();
        }
    }
}
