namespace Ataxx.Model.Data;

public class Cell
{
    public Player? MarkedByPlayer { get; private set; }
    
    public bool IsWall { get; internal set; }
    
    public Cell(bool isWall = false)
    {
        IsWall = isWall;
        MarkedByPlayer = null;
    }
    
    public bool IsEmpty => MarkedByPlayer == null && !IsWall;
    
    public bool IsOccupied => MarkedByPlayer != null;
    
    internal void SetPlayer(Player? player)
    {
        if (IsWall)
        {
            throw new Exception("Cannot place a piece on a wall.");
        }

        MarkedByPlayer = player;
    }
    
    internal void SetWall()
    {
        if (IsOccupied)
        {
            throw new Exception("Cannot set wall on an occupied cell.");
        }
        IsWall = true;
    }
    
    internal Cell(Cell source)
    {
        MarkedByPlayer = source.MarkedByPlayer;
        IsWall = source.IsWall;
    }
}