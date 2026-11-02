namespace Ataxx.Model.Data;

public record Coordinates(int X, int Y)
{
    public string ToCoordString()
    {
        char col = (char)('A' + X);
        string row = (Y + 1).ToString();
        return $"{col}{row}";
    }
}