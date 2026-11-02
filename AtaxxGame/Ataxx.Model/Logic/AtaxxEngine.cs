namespace Ataxx.Model.Logic;

using System;
using System.Collections.Generic;
using System.Linq;
using Ataxx.Model.Data;

public class AtaxxEngine
{
    protected readonly Player PlayerX;

    protected readonly Player PlayerO;

    protected AtaxxBoard Board;
    
    private Stack<GameSnapshot> _history = new Stack<GameSnapshot>();
    
    public int HistoryCount => _history.Count;

    public Player CurrentPlayer { get; private set; }

    public Player? Winner { get; private set; }

    public bool IsEnded { get; private set; }
    private List<Move> _validMovesList = new List<Move>();
    
    private readonly BoardFactory _boardFactory;

    public AtaxxEngine(Player playerX, Player playerO)
    {
        if (playerX == null || playerO == null)
        {
            throw new Exception("Players can not be null.");
        }

        PlayerX = playerX;
        PlayerO = playerO;
        
        _boardFactory = new BoardFactory();
    }

    public Cell[,] GetFieldCopy()
    {
        if (Board == null)
        {
            throw new Exception("Game has not started yet.");
        }

        return Board.GetFieldCopy();
    }

    public virtual void StartGame(int size = 7)
    {
        _history.Clear();
        Board = _boardFactory.CreateRandomLayoutBoard();
        int actualSize = Board.Size;

        Board.GetCell(0, 0).SetPlayer(PlayerX);
        Board.GetCell(size - 1, size - 1).SetPlayer(PlayerX);

        Board.GetCell(0, size - 1).SetPlayer(PlayerO);
        Board.GetCell(size - 1, 0).SetPlayer(PlayerO);

        CurrentPlayer = PlayerX;
        Winner = null;
        IsEnded = false;
        
        _validMovesList = CalculateValidMoves(CurrentPlayer);
    }

    public virtual void MakeMove(Move move)
    {
        if (IsEnded)
        {
            throw new Exception("Game has already ended");
        }
        if (CurrentPlayer == null)
        {
            throw new Exception("Game has not started.");
        }
        
        if (!_validMovesList.Contains(move))
        {
            throw new Exception("Invalid move");
        }
        
        SaveSnapshot();
        PlaceSymbol(move);
        ConvertNeighbours(move.To, CurrentPlayer);
        SwitchPlayer();
    }

    protected virtual void PlaceSymbol(Move move)
    {
        var fromCell = Board.GetCell(move.From.X, move.From.Y);
        var toCell = Board.GetCell(move.To.X, move.To.Y);

        if (move.IsJump)
        {
            fromCell.SetPlayer(null);
        }

        toCell.SetPlayer(CurrentPlayer);
    }

    protected virtual void ConvertNeighbours(Coordinates to, Player player)
    {
        Player opponent = (player == PlayerX) ? PlayerO : PlayerX;
        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0) continue;

                int checkX = to.X + dx;
                int checkY = to.Y + dy;

                if (Board.IsOnBoard(checkX, checkY))
                {
                    Cell neighbourCell = Board.GetCell(checkX, checkY);
                    if (neighbourCell.MarkedByPlayer == opponent)
                    {
                        neighbourCell.SetPlayer(player);
                    }
                }
            }
        }
    }

    protected virtual void SwitchPlayer()
    {
        CurrentPlayer = (CurrentPlayer == PlayerX) ? PlayerO : PlayerX;
        
        CheckGameState();
    }
    
    private void SaveSnapshot()
    {
        if (Board == null || CurrentPlayer == null) return; 
        var snapshot = new GameSnapshot(Board.GetFieldCopy(), CurrentPlayer, _validMovesList, IsEnded, Winner);
        _history.Push(snapshot);
    }
    
    public virtual void UndoMove()
    {
        if (!_history.Any())
        {
            throw new InvalidOperationException("Cannot undo. No moves in history.");
        }

        var lastState = _history.Pop();
        
        Board = new AtaxxBoard(lastState.BoardState);
        CurrentPlayer = lastState.CurrentPlayer;
        _validMovesList = lastState.ValidMoves;
        IsEnded = lastState.IsEnded;
        Winner = lastState.Winner;
    }

    protected virtual void CheckGameState()
    {
        if (CurrentPlayer == null) return;
        
        _validMovesList = CalculateValidMoves(CurrentPlayer);

        if (!_validMovesList.Any())
        {
            CurrentPlayer = (CurrentPlayer == PlayerX) ? PlayerO : PlayerX;
            _validMovesList = CalculateValidMoves(CurrentPlayer);
            
            if (!_validMovesList.Any())
            {
                EndGame();
            }
        }
    }

    protected virtual void EndGame()
    {
        IsEnded = true;
        (int xCount, int oCount) = CountPieces();
        if (xCount > oCount) Winner = PlayerX;
        else if (oCount > xCount) Winner = PlayerO;
        else Winner = null;
    }
    
    public List<Move> GetAllValidMoves(Player player)
    {
        if (player == CurrentPlayer)
        {
            return new List<Move>(_validMovesList);
        }
        return CalculateValidMoves(player);
    }
    
    private List<Move> CalculateValidMoves(Player player)
    {
        var moves = new List<Move>();
        if (Board == null) return moves;

        for (int y = 0; y < Board.Size; y++)
        {
            for (int x = 0; x < Board.Size; x++)
            {
                if (Board.GetCell(x, y).MarkedByPlayer != player)
                {
                    continue;
                }
                    
                var fromCoord = new Coordinates(x, y);
                
                for (int dx = -2; dx <= 2; dx++)
                {
                    for (int dy = -2; dy <= 2; dy++)
                    {
                        if (dx == 0 && dy == 0) continue;

                        var toCoord = new Coordinates(x + dx, y + dy);
                        
                        if (IsValidMoveTarget(toCoord))
                        {
                            moves.Add(new Move(fromCoord, toCoord));
                        }
                    }
                }
            }
        }
        return moves;
    }
    
    private bool IsValidMoveTarget(Coordinates toCoord)
    {
        if (!Board.IsOnBoard(toCoord.X, toCoord.Y))
        {
            return false;
        }
        return Board.GetCell(toCoord.X, toCoord.Y).IsEmpty;
    }

    private (int countX, int countO) CountPieces()
    {
        int xCount = 0;
        int oCount = 0;
        for (int y = 0; y < Board.Size; y++)
        {
            for (int x = 0; x < Board.Size; x++)
            {
                var player = Board.GetCell(x, y).MarkedByPlayer;
                if (player == PlayerX) xCount++;
                else if (player == PlayerO) oCount++;
            }
        }
        return (xCount, oCount);
    }
}