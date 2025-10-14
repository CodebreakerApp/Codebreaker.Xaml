# Codebreaker.WinForms

A Windows Forms implementation of the Codebreaker game, providing a classic Windows desktop experience.

## Overview

This project is a Windows Forms client application that allows users to play the Codebreaker game. It integrates with the existing CNinnovation.Codebreaker.ViewModels library for MVVM logic and communicates with the Codebreaker API service.

## Features

- **Classic Windows UI**: Traditional Windows Forms interface with familiar controls
- **Full Gameplay**: Complete implementation of the Codebreaker game mechanics
- **MVVM Integration**: Uses the CNinnovation.Codebreaker.ViewModels library for consistent game logic
- **Real-time Updates**: UI updates in response to game state changes
- **Visual Feedback**: Color-coded pegs and move history display
- **Info Bar**: Displays messages and notifications during gameplay

## Requirements

- .NET 9.0 or later
- Windows operating system
- Visual Studio 2022 (or later) for development

## Getting Started

### Building the Project

1. Open the solution file: `src/CodeBreaker.WinForms.sln`
2. Restore NuGet packages
3. Build the solution (Ctrl+Shift+B)

### Running the Application

1. Press F5 in Visual Studio to run with debugging, or Ctrl+F5 to run without debugging
2. Alternatively, run the built executable from `bin/Debug/net9.0-windows/` or `bin/Release/net9.0-windows/`

### Playing the Game

1. **Start**: Enter your username and click "Start Game"
2. **Select Colors**: Choose colors for each peg position using the dropdown menus
3. **Make Move**: Click "Set Move" to submit your guess
4. **Review Feedback**: Check the key pegs to see how many colors are correct and in the right position
5. **Win/Lose**: Continue until you guess correctly or run out of moves

## Project Structure

```
Codebreaker.WinForms/
├── Extensions/
│   └── ConfigurationExtensions.cs    # Configuration helper methods
├── Helpers/
│   └── ColorHelper.cs                # Color conversion utilities
├── Services/
│   └── WinFormsDialogService.cs      # Dialog service implementation
├── MainForm.cs                       # Main game form
├── Program.cs                        # Application entry point
├── GlobalUsings.cs                   # Global using directives
├── appsettings.json                  # Application configuration
└── Codebreaker.WinForms.csproj       # Project file
```

## Configuration

The application uses `appsettings.json` for configuration. Key settings include:

- **ApiBase**: The base URL for the Codebreaker API service

```json
{
  "ApiBase": "https://gameapis.kindbeach-def2191a.westeurope.azurecontainerapps.io"
}
```

You can override settings for different environments using:
- `appsettings.Development.json` - Development environment
- `appsettings.Production.json` - Production environment

## Architecture

### Dependency Injection

The application uses Microsoft.Extensions.Hosting for dependency injection:

```csharp
builder.Services.AddSingleton<IInfoBarService, InfoBarService>();
builder.Services.AddSingleton<IDialogService, WinFormsDialogService>();
builder.Services.AddScoped<GamePageViewModel>();
builder.Services.AddHttpClient<IGamesClient, GamesClient>();
```

### MVVM Pattern

The UI binds to the `GamePageViewModel` from the CNinnovation.Codebreaker.ViewModels library:
- **GamePageViewModel**: Manages game state and provides commands
- **IInfoBarService**: Displays messages to the user
- **IDialogService**: Shows dialog boxes
- **IGamesClient**: Communicates with the game API

## Key Components

### MainForm

The main form contains:
- **Start Game Panel**: Username input and start button
- **Peg Selection Panel**: Dropdown menus for color selection
- **Moves List**: History of all moves with visual feedback
- **Status Bar**: Current game status and progress indicator
- **Info Bar**: Messages and notifications

### Custom Drawing

The application uses custom drawing for:
- **Color Pegs**: Rendered as colored circles in combo boxes
- **Move History**: Each move displays guess pegs and key peg feedback

## Dependencies

- **CNInnovation.Codebreaker.ViewModels**: MVVM view models and game logic
- **Microsoft.Extensions.Hosting**: Dependency injection and configuration
- **Microsoft.Extensions.Http**: HTTP client factory

## Related Projects

- **Codebreaker.WPF**: WPF implementation
- **Codebreaker.WinUI**: WinUI implementation
- **Codebreaker.MAUI**: Cross-platform MAUI implementation

## Contributing

Follow the repository guidelines for contributions. See [guidelines.md](../../guidelines.md) for more information.

## License

MIT License - See the LICENSE file in the repository root.
