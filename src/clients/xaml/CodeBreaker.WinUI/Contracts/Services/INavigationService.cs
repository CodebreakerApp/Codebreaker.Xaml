﻿using CodeBreaker.Services;

namespace CodeBreaker.WinUI.Contracts.Services;

public interface INavigationService : INavigationServiceCore
{
    event NavigatedEventHandler Navigated;

    bool CanGoBack { get; }

    Frame Frame { get; set; }
}