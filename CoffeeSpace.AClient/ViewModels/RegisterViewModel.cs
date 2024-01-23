using System.Threading;
using System.Threading.Tasks;
using CoffeeSpace.AClient.Contracts.Requests.Auth;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CoffeeSpace.AClient.ViewModels;

public sealed partial class RegisterViewModel : ViewModelBase
{

    [ObservableProperty] private RegisterRequest _registerRequest = null!;

    public RegisterViewModel()
    {

    }

    [RelayCommand]
    private Task RegisterAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}