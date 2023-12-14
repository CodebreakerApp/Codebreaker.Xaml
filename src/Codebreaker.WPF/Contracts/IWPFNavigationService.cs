﻿using Codebreaker.ViewModels.Contracts.Services;

namespace Codebreaker.WPF.Contracts;

internal interface IWPFNavigationService : INavigationService
{
    Frame Frame { get; set; }
}
