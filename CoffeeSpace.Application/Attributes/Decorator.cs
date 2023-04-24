namespace CoffeeSpace.Application.Attributes;

[AttributeUsage(validOn: AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class Decorator : Attribute
{
}