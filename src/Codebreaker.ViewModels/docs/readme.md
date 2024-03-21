# CNinnovation.Codebreaker.ViewModels

This library contains view-model types for XAML-based applications (WinUI, WPF, .NET MAUI...) to create Codebreaker games.

It is part of the Codebreaker solution.

See https://github.com/codebreakerapp for more information on the complete solution.

## The ViewModels

### GamePageViewModel
The _GamePageViewModel_ is the view-model type for the game page with commands to start games, set moves.  
The _GamePageViewModel_ is the main view-model type to communicate with the application.

| Members     | Description        |
|------------|--------------------|
| ctor | Needs `IGamesClient` (communication with the games-service API), `IInfoBarService` |
| Game | The current game |
| IsLoading | Indicates processing activitiy, where showing a loading indicator is appropriate |
| SelectedFields | The fields used for selecting the colors for the next move |
| Username | The username of the player. This name is used when starting the game |
| StartGameCommand | Command to start a new game |
| MakeMoveCommand | Command to set a move |

## Model types

The following model types are used to contain information about the game.

### Game

| Members     | Description        |
|------------|--------------------|
| Id | The ID of the game |
| GameType | The type of the game |
| PlayerName | The name of the player |
| StartTime | The start time of the game |
| EndTime | The end time of the game |
| Duration | The duration of the game |
| NumberCodes | The number of codes in the game |
| MaxMoves | The maximum number of moves allowed |
| IsFinished | Indicates if the game is finished |
| IsVictory | Indicates if the game is a victory |
| FieldValues | The values of the fields |
| Moves | The moves made in the game |

### Move

| Members     | Description        |
|------------|--------------------|
| GuessPegs | The guess pegs from the user for this move |
| KeyPegs | The result from the analyer for this move based on the associated game that contains the move. (Null if the move was not analyzed yet.) |

### Field

| Members     | Description        |
|------------|--------------------|
| PossibleColors | The possible colors for the field |
| Color | The selected color for the field |


## Services
### IInfoBarService / InfoBarService
Service to show messages in the info bar.  
The UI is able to bind to the `Messages`-ObservableCollection for displaying the mssages.

```C#
// Register the service in the platform-specific project
services.AddScoped<IInfoBarService, InfoBarService>();
```

### INavigationService
Interface for the service to navigate between pages.  
The implementation for this interface needs to be made by the platform-specific project.

```C#
// Register the service in the platform-specific project
services.AddScoped<INavigationService, MyPlatformSpecificNavigationServiceImplementation>();
```