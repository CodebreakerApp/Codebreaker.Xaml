namespace Codebreaker.ViewModels.Models;

public partial class Game(
    Guid id,
    GameType gameType,
    string playerName,
    DateTime startTime,
    int numberCodes,
    int maxMoves,
    IDictionary<string, string[]> fieldValues) : ObservableObject
{
    /// <summary>
    /// Gets the unique identifier of the game.
    /// </summary>
    public Guid Id { get; private set; } = id;

    /// <summary>
    /// Gets the type of the game. <see cref="GameType"/>
    /// </summary>
    public GameType GameType { get; private set; } = gameType;

    /// <summary>
    /// Gets the name of the player.
    /// </summary>
    public string PlayerName { get; private set; } = playerName;

    /// <summary>
    /// Gets information if the player was authenticated while playing the game.
    /// </summary>
    public bool PlayerIsAuthenticated { get; set; } = false;

    /// <summary>
    /// Gets the start time of the game
    /// </summary>
    public DateTime StartTime { get; private set; } = startTime;

    /// <summary>
    /// Gets the end time of the game or null if it did not end yet. This value is set from a game guess anylzer after the game was ended.
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsFinished))]
    private DateTime? _endTime;

    /// <summary>
    /// Gets the duration of the game or null if it did not end yet
    /// </summary>
    [ObservableProperty]
    private TimeSpan? _duration;

    /// <summary>
    /// Gets the number of codes the player needs to fill.
    /// </summary>  
    public int NumberCodes { get; private set; } = numberCodes;

    /// <summary>
    /// Gets the maximum number of moves the game ends when its not solved.
    /// </summary>
    public int MaxMoves { get; private set; } = maxMoves;

    /// <summary>
    /// Gets a boolean value indicating if the game is finished.
    /// </summary>
    public bool IsFinished => EndTime is not null;

    /// <summary>
    /// Did the player win the game?
    /// </summary>  
    [ObservableProperty]
    private bool _isVictory = false;

    /// <summary>
    /// A list of possible field values the user has to chose from
    /// </summary>
    public IDictionary<string, string[]> FieldValues { get; init; } = fieldValues;

    /// <summary>
    /// A list of moves the player made
    /// </summary>
    public ICollection<Move> Moves { get; } = new ObservableCollection<Move>();

    public override string ToString() => $"{GameId}:{GameType} - {StartTime}";
}