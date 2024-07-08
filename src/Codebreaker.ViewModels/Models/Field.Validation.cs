namespace Codebreaker.ViewModels.Models;

public partial class Field
{
    public bool IsSet =>
        PossibleColors is null || Color is not null &&
        PossibleShapes is null || Shape is not null;
}
