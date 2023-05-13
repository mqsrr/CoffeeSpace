namespace CoffeeSpace.Application.Attributes;

[AttributeUsage(validOn: AttributeTargets.Class, Inherited = false)]
public sealed class Decorator : Attribute
{
}