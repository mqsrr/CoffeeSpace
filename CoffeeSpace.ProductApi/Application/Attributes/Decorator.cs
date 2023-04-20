namespace CoffeeSpace.ProductApi.Application.Attributes;

[AttributeUsage(validOn: AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
internal sealed class Decorator : Attribute
{
}