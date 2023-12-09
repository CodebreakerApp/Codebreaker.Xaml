# Codebreaker.XAML

This repository contains the code for all XAML based clients of [Codebreaker](https://github.com/CodebreakerApp).

## Builds

### XAML Clients

|Branch|WPF|MAUI-Android|WinUI|Platform UNO|Avalonia|
|:--:|:--:|:--:|:--:|:--:|:--:|
**main** | TBD | [![MAUI Android](https://github.com/CNILearn/codebreaker/actions/workflows/codebreaker-maui-android.yml/badge.svg)](https://github.com/CNILearn/codebreaker/actions/workflows/codebreaker-maui-android.yml) | [![WinUI](https://github.com/CNILearn/codebreaker/actions/workflows/codebreaker-winui.yml/badge.svg)](https://github.com/CNILearn/codebreaker/actions/workflows/codebreaker-winui.yml) | TBD | TBD |


### Libraries

|Branch|Shared|Client Services|MVVM|
|:--:|:--:|:--:|:--:|
**main**|[![Shared](https://github.com/CNinnovation/codebreaker/actions/workflows/codebreaker-lib-shared.yml/badge.svg)](https://github.com/CNinnovation/codebreaker/actions/workflows/codebreaker-lib-shared.yml)|[![Client Services](https://github.com/CNinnovation/codebreaker/actions/workflows/codebreaker-lib-services.yml/badge.svg)](https://github.com/CNinnovation/codebreaker/actions/workflows/codebreaker-lib-services.yml)|[![MVVM NuGet](https://github.com/CNinnovation/codebreaker/actions/workflows/codebreaker-lib-viewmodels.yml/badge.svg)](https://github.com/CNinnovation/codebreaker/actions/workflows/codebreaker-lib-viewmodels.yml)

## Guidelines

[Guidelines](guidelines.md)

## Codebreaker NuGet Packages

NuGet packages for Codebreaker can be found on the official [NuGet server](https://www.nuget.org/packages?q=Tags%3A%22Codebreaker%22).

## Codebreaker Client Apps

* Blazor app to play games and show game results using [MudBlazor](https://www.mudblazor.com/), [FastBlazor](https://github.com/microsoft/fast-blazor), and native, pure Blazor with only CSS
* WinUI app to play games calling the API, and show live services
* WPF app to play games calling the API
* .NET MAUI App to play games calling the API (Android, iOS, Windows)

* [Blazor Pure CSS](https://codebreaker-pure.azurewebsites.net/)
* [Blazor MudBlazor](https://codebreaker-mud.azurewebsites.net/)
* [Blazor Microsoft.Fast](https://codebreaker-fast.azurewebsites.net/)

## To be defined and developed

* Authentication with Microsoft, Facebook, Google accounts
* Platform Uno client
* Avalonia client
