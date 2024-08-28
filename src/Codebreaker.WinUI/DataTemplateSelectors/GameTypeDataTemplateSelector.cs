using Codebreaker.GameAPIs.Client.Models;

namespace CodeBreaker.WinUI.DataTemplateSelectors;

public class GameTypeDataTemplateSelector : DataTemplateSelector
{
    public GameType GameType { get; set; }

    public DataTemplate? GameType6x4Template { get; set; }

    public DataTemplate? GameType6x4MiniTemplate { get; set; }

    public DataTemplate? GameType8x5Template { get; set; }

    public DataTemplate? GameType5x5x4Template { get; set; }

    protected override DataTemplate SelectTemplateCore(object item) =>
        GameType switch
        {
            GameType.Game6x4 => GameType6x4Template ?? throw new InvalidOperationException($"{nameof(GameType6x4Template)} is not set and therefore null"),
            GameType.Game6x4Mini => GameType6x4MiniTemplate ?? throw new InvalidOperationException($"{nameof(GameType6x4MiniTemplate)} is not set and therefore null"),
            GameType.Game8x5 => GameType8x5Template ?? throw new InvalidOperationException($"{nameof(GameType8x5Template)} is not set and therefore null"),
            GameType.Game5x5x4 => GameType5x5x4Template ?? throw new InvalidOperationException($"{nameof(GameType5x5x4Template)} is not set and therefore null"),
            _ => throw new InvalidOperationException("Invalid game type")
        };
}