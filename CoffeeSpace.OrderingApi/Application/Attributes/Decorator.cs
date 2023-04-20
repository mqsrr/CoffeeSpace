namespace CoffeeSpace.OrderingApi.Application.Attributes;

[AttributeUsage(validOn: AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
internal sealed class Decorator : Attribute
{
}