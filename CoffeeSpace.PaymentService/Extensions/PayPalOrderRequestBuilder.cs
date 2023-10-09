using CoffeeSpace.Domain.Ordering.Orders;
using CoffeeSpace.PaymentService.Helpers;
using PayPalCheckoutSdk.Orders;

namespace CoffeeSpace.PaymentService.Extensions;


public class PayPalOrderRequestBuilder
{
    private readonly OrderRequest _orderRequest;

    public PayPalOrderRequestBuilder()
    {
        _orderRequest = new OrderRequest();
    }
    
    public PayPalOrderRequestBuilder WithDefaultContext()
    {
        _orderRequest.CheckoutPaymentIntent = PaymentConstants.CheckoutPaymentIntent;
        _orderRequest.ApplicationContext = new ApplicationContext
        {
            BrandName = PaymentConstants.BrandName,
            LandingPage = PaymentConstants.LandingPage,
            CancelUrl = PaymentConstants.CancelUrl,
            ReturnUrl = PaymentConstants.ReturnUrl,
            UserAction = PaymentConstants.UserAction,
            ShippingPreference = PaymentConstants.ShippingPreference
        };
        return this;
    }

    public PayPalOrderRequestBuilder WithPurchaseUnits(ICollection<OrderItem> orderItems)
    {
        string orderItemsSum = orderItems.Sum(orderItem => orderItem.Total).ToString("F");
        var purchaseUnitRequest = new PurchaseUnitRequest
        {
            ReferenceId = PaymentConstants.ReferenceId,
            Description = PaymentConstants.Description,
            CustomId = PaymentConstants.CustomId,
            SoftDescriptor = PaymentConstants.SoftDescriptor,
            AmountWithBreakdown = new AmountWithBreakdown
            {
                CurrencyCode = "USD",
                Value = orderItemsSum,
                AmountBreakdown = new AmountBreakdown
                {
                    ItemTotal = new Money
                    {
                        CurrencyCode = "USD",
                        Value = orderItemsSum
                    }
                }
            },
            Items = orderItems.Select(orderItem => new Item
            {
                Name = orderItem.Title,
                Description = orderItem.Description.Length > 100 ? orderItem.Description[..100] : orderItem.Description,
                Quantity = orderItem.Quantity.ToString(),
                UnitAmount = new Money
                {
                    CurrencyCode = "USD",
                    Value = orderItem.UnitPrice.ToString("F")
                },
                Category = "PHYSICAL_GOODS",
            }).ToList()
        };

        _orderRequest.PurchaseUnits = new List<PurchaseUnitRequest> { purchaseUnitRequest };
        return this;
    }

    public PayPalOrderRequestBuilder WithAddress(Address address)
    {
        if (_orderRequest.PurchaseUnits is not {Count: > 0})
        {
            return this;
        }

        var shippingDetail = new ShippingDetail
        {
            Name = new Name { FullName = "Coffee Space User" },
            AddressPortable = new AddressPortable
            {
                AddressLine1 = address.Street,
                AddressLine2 = address.Street,
                AdminArea1 = address.Country,
                AdminArea2 = address.City,
                CountryCode = "US",
                PostalCode = "11111"
            }
        };

        _orderRequest.PurchaseUnits[0].ShippingDetail = shippingDetail;
        return this;
    }

    public OrderRequest Build()
    {
        return _orderRequest;
    }
}