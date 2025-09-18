using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Grocery.Core.Interfaces.Services;

namespace Grocery.App.ViewModels;

public partial class LoginViewModel : BaseViewModel
{
    private readonly IAuthService _authService;
    private readonly GlobalViewModel _global;

    [ObservableProperty] private string email = "user3@mail.com";

    [ObservableProperty] private string loginMessage;

    [ObservableProperty] private string password = "user3";

    public LoginViewModel(IAuthService authService, GlobalViewModel global)
    {
        _authService = authService;
        _global = global;
    }

    [RelayCommand]
    private void Login()
    {
        var authenticatedClient = _authService.Login(Email, Password);

        if (authenticatedClient != null)
        {
            LoginMessage = $"Welkom {authenticatedClient.Name}!";
            _global.Client = authenticatedClient;

            // Navigatie via MainPage naar AppShell
            Application.Current.MainPage = new AppShell();
        }
        else
        {
            LoginMessage = "Ongeldige inloggegevens.";
        }
    }
}