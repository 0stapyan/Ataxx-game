namespace Ataxx.Model.Data;

using System.Collections.Generic;

internal class GameSnapshot
{
    public Cell[,] BoardState { get; }
    public Player CurrentPlayer { get; }
    public List<Move> ValidMoves { get; }
    public bool IsEnded { get; }
    public Player? Winner { get; }

    public GameSnapshot(Cell[,] board, Player player, List<Move> moves, bool isEnded, Player? winner)
    {
        BoardState = DeepCopyBoard(board); 
            
        CurrentPlayer = player;
        ValidMoves = new List<Move>(moves);
        IsEnded = isEnded;
        Winner = winner;
    }
    
    private Cell[,] DeepCopyBoard(Cell[,] original)
    {
        int size = original.GetLength(0);
        var newBoard = new Cell[size, size];
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                newBoard[x, y] = new Cell(original[x, y]); 
            }
        }
        return newBoard;
    }
}