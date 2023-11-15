using CommunityToolkit.Mvvm.ComponentModel;

namespace CoffeeSpace.Client._ViewModels;

public sealed partial class OrderPaymentViewModel : ObservableObject, IQueryAttributable
{
    [ObservableProperty]
    private string _paymentApprovalLink;

   
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        PaymentApprovalLink = (query["Payment Approval Link"] as string)!;
    }
}