# Copilot Instructions for Codebreaker.Xaml

## Repository Guidelines Alignment

This document extends the existing [guidelines.md](../guidelines.md) with .NET 9 specific guidance:

- **Modern Technologies**: .NET 9 represents the latest modern technology stack
- **Productivity**: Leverage .NET 9 features for shorter, more efficient code
- **Released Versions**: Use .NET 9 as it's a released, stable version
- **Built-in Features**: Prefer .NET 9 built-in features over external libraries

## Target Framework

**Use .NET 9 as the primary target framework** instead of .NET 8 for all new projects and when updating existing projects.

> This follows the repository's guideline to use modern, released versions of .NET. .NET 9 provides significant performance improvements, new language features, and enhanced cross-platform capabilities compared to .NET 8.

### Framework Guidelines

- **Primary Target**: Use `net9.0` for most projects
- **Multi-targeting**: When supporting multiple frameworks, include `net9.0` as the primary target
- **Platform-specific**: Use `net9.0-windows` for Windows-specific projects (WinUI, WPF)
- **Mobile/Cross-platform**: Use appropriate TFMs like `net9.0-android`, `net9.0-ios` for mobile projects
- **Library Projects**: Use multi-targeting for STS and LTS, thus `net9.0` and `net8.0` for maximum compatibility

### Project Types and Framework Usage

#### Desktop Applications
- **WinUI projects**: Use `net9.0-windows10.0.19041.0` (or later Windows version)
- **WPF projects**: Use `net9.0-windows`
- **Avalonia projects**: Use `net9.0` for cross-platform, `net9.0-windows` for Windows-specific features

#### Cross-Platform Applications
- **MAUI projects**: Use `net9.0` as base with platform-specific TFMs
- **Uno Platform projects**: Follow Uno Platform .NET 9 compatibility guidelines
- **Avalonia cross-platform**: Use `net9.0`

#### Libraries and Shared Code
- **Shared libraries**: Use `net9.0` and `net8.0` for maximum compatibility
- **Multi-target libraries**: Include `net9.0` as primary target, consider `net8.0` for backward compatibility if needed
- **ViewModels**: Target `net9.0` and `net8.0` 

## Language and Code Style

- **C# Language Version**: Use the latest C# version compatible with .NET 9
- **Lang Version**: Set `<LangVersion>latest</LangVersion>` in project files to use latest C# features
- **Nullable Reference Types**: Enable with `<Nullable>enable</Nullable>`
- **Implicit Usings**: Enable with `<ImplicitUsings>enable</ImplicitUsings>`

## Modern .NET 9 Features

When writing code, prefer modern .NET 9 and C# features:

### Recommended Patterns
- Use **primary constructors** for classes where appropriate
- Use **collection expressions** for initializing collections
- Use **file-scoped namespaces** for cleaner code organization
- Prefer **record types** over classes for data containers
- Use **minimal APIs** for web services instead of controllers
- Leverage **source generators** for performance-critical scenarios

### Dependency Injection and Configuration
- Use the modern **Host.CreateApplicationBuilder()** pattern
- Prefer **IServiceCollection** extensions for service registration
- Use **configuration binding** with strongly-typed options
- Leverage **keyed services** in .NET 9 for multiple implementations

## Package References

When adding or updating NuGet packages:

- **Microsoft packages**: Use .NET 9 compatible versions (9.x.x)
- **Third-party packages**: Ensure compatibility with .NET 9
- **Community Toolkit**: Use latest versions that support .NET 9
- **UI Frameworks**: Use .NET 9 compatible versions of WinUI, Avalonia, etc.

## Platform-Specific Considerations

### WinUI Projects
- Target Windows 10/11 APIs appropriately
- Use Windows App SDK versions compatible with .NET 9
- Leverage WinUI 3 features optimized for .NET 9

### MAUI Projects
- Use .NET MAUI versions that support .NET 9
- Target latest platform versions where possible
- Utilize MAUI-specific .NET 9 optimizations

### Avalonia Projects
- Use Avalonia versions compatible with .NET 9
- Leverage cross-platform .NET 9 features
- Consider Avalonia's .NET 9 performance improvements

### Uno Platform Projects
- Follow Uno Platform's .NET 9 migration guidelines
- Use compatible Uno.Sdk versions
- Update global.json files to reference .NET 9 compatible tooling

## Codebreaker-Specific Guidance

### Configuration and Dependency Injection
```csharp
// Use .NET 9 Host.CreateApplicationBuilder() pattern
var builder = Host.CreateApplicationBuilder();

// Modern service registration
builder.Services.AddScoped<GamePageViewModel>();
builder.Services.AddHttpClient<IGamesClient, GamesClient>();
```

### Cross-Platform UI Development
- **WinUI**: Leverage .NET 9 performance improvements for Windows applications
- **Avalonia**: Use .NET 9 for enhanced cross-platform compatibility
- **MAUI**: Target .NET 9 for latest mobile development features

### Game Logic and ViewModels
- Use **record types** for game state and model objects
- Leverage **collection expressions** for game collections
- Use **primary constructors** in ViewModels where appropriate

## Migration Guidance

When updating existing projects from .NET 8 to .NET 9:

1. **Update TargetFramework**: Change `net8.0` to `net9.0`
2. **Update Package References**: Bump Microsoft.* packages to 9.x versions
3. **Review Breaking Changes**: Check for any .NET 9 breaking changes
4. **Test Thoroughly**: Ensure all functionality works with .NET 9
5. **Update Global.json**: Ensure SDK references support .NET 9

## Performance and Modern Practices

- Utilize .NET 9 performance improvements
- Use **span and memory APIs** for performance-critical code
- Leverage **async/await patterns** with .NET 9 enhancements
- Consider **AOT compilation** where supported
- Use **generic math** and other .NET 9 features for better performance

## Testing

- Use latest versions of testing frameworks compatible with .NET 9
- Ensure test projects target `net9.0`
- Leverage .NET 9 testing improvements and new APIs

## Build and Deployment

- Update CI/CD pipelines to use .NET 9 SDK
- Ensure deployment targets support .NET 9 runtime
- Use .NET 9 optimizations in release builds
- Consider using .NET 9 specific features for better performance

## Documentation

When documenting code or creating examples:
- Reference .NET 9 APIs and patterns
- Include .NET 9 specific features in code samples
- Update any framework version references in documentation
- Highlight .NET 9 benefits and new capabilities