﻿using CommunityToolkit.Mvvm.ComponentModel;
using CodeBreaker.Services;
using CommunityToolkit.Mvvm.Input;
using CodeBreaker.ViewModels.Services;
using Microsoft.Extensions.Logging;
using CodeBreaker.Services.Authentication;
using CodeBreaker.Services.Authentication.Definitions;

namespace CodeBreaker.ViewModels;

public partial class AuthPageViewModel : ObservableObject
{
    private readonly IAuthService _authService;

    private readonly IDialogService _dialogService;

    private readonly ILogger _logger;

    private readonly IViewModelNavigationService _navigationService;

    private static readonly IAuthDefinition _authDefinition = new ApiServiceAuthDefinition();

    public AuthPageViewModel(IAuthService authService, IDialogService dialogService, ILogger<AuthPageViewModel> logger, IViewModelNavigationService navigationService)
    {
        _authService = authService;
        _dialogService = dialogService;
        _logger = logger;
        _navigationService = navigationService;
    }

    [RelayCommand(AllowConcurrentExecutions = false, FlowExceptionsToTaskScheduler = true)]
    private async Task LoginAsync(CancellationToken cancellation = default)
    {
        try
        {
            await _authService.AquireTokenAsync(_authDefinition, cancellation);
        }
        catch (Exception)
        {
            await _dialogService.ShowMessageAsync("Could not authenticate");
            return;
        }

        _navigationService.NavigateToViewModel(typeof(GamePageViewModel), clearNavigation: true);
    }

    [RelayCommand]
    private void ContinueAsGuest() =>
        _navigationService.NavigateToViewModel(typeof(GamePageViewModel), clearNavigation: true);
}
