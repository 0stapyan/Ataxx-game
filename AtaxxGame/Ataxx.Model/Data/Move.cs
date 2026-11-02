namespace Ataxx.Model.Data;

public record Move(Coordinates From, Coordinates To)
{
    public bool IsJump
    {
        get
        {
            int dx = Math.Abs(From.X - To.X);
            int dy = Math.Abs(From.Y - To.Y);
            
            return Math.Max(dx, dy) == 2;
        }
    }
    public bool IsClone => !IsJump;
}