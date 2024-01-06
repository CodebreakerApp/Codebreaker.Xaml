using Codebreaker.ViewModels.Contracts.Services;

namespace Codebreaker.WPF.Contracts.Services.Navigation;

internal interface IWPFNavigationService : INavigationService
{
    Frame Frame { get; set; }
}
