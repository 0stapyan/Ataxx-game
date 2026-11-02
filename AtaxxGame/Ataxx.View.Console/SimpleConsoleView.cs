namespace Ataxx.View.Console;

using System;
using System.Text;
using Ataxx.Model.Logic;
using Ataxx.Model.Data;
using Ataxx.Model;

public class SimpleConsoleView : IAtaxxView
{
    
    private Player? _playerX;
    public void ListenTo(IGameEvents gameEvents)
    {
        gameEvents.GameStarted += OnGameStarted;
        gameEvents.BoardUpdated += OnBoardUpdated;
        gameEvents.PlayerWon += OnPlayerWon;
        gameEvents.MatchDrawn += OnMatchDrawn;
        gameEvents.PlayerSwitched += OnPlayerSwitched;
        gameEvents.TurnSkipped += OnTurnSkipped;
        gameEvents.BoardRestored += OnBoardRestored;
    }
    
    public void Unlisten(IGameEvents gameEvents)
    {
        gameEvents.GameStarted -= OnGameStarted;
        gameEvents.BoardUpdated -= OnBoardUpdated;
        gameEvents.PlayerWon -= OnPlayerWon;
        gameEvents.MatchDrawn -= OnMatchDrawn;
        gameEvents.PlayerSwitched -= OnPlayerSwitched;
        gameEvents.TurnSkipped -= OnTurnSkipped;
        gameEvents.BoardRestored -= OnBoardRestored;
    }
    
    public void Redraw(Cell[,] field, Player? currentPlayer)
    {
        Console.Clear();
        Console.WriteLine("Ataxx -- Simple View");
        Console.WriteLine("-------------------");
        DrawBoard(field);
            
        if (currentPlayer != null)
        {
            OnPlayerSwitched(currentPlayer);
        }
    }
    
    private void OnGameStarted(Cell[,] field)
    {
        if (field[0, 0].MarkedByPlayer != null)
            _playerX = field[0, 0].MarkedByPlayer;
        
        Redraw(field, _playerX); 
        
        ShowMessage("Game started! Player X moves.");
    }
    
    public void ShowMessage(string message)
    {
        Console.WriteLine($"\n{message}");
    }

    private void OnBoardUpdated(Cell[,] field)
    {
        DrawBoard(field);
    }
    
    private void OnPlayerSwitched(Player player)
    {
        Console.WriteLine($"\nPlayer {player.Name}'s turn.");
    }
    
    private void OnTurnSkipped(Player skippedPlayer)
    {
        ShowMessage($"Player {skippedPlayer.Name} has no moves! Turn skipped.");
    }
    
    private void OnPlayerWon(Player player)
    {
        Console.WriteLine($"\n--- GAME OVER ---");
        Console.WriteLine($"Player {player.Name} won!");
    }
    
    private void OnMatchDrawn()
    {
        Console.WriteLine($"\n--- GAME OVER ---");
        Console.WriteLine($"It's a draw!");
    }

    private void DrawBoard(Cell[,] field)
    {
        int size = field.GetLength(0); 
            
        var sb = new StringBuilder();
        
        sb.Append("  ");
        for (int i = 0; i < size; i++)
        {
            sb.Append($" {(char)('A' + i)}");
        }
        sb.AppendLine();
        
        for (int y = 0; y < size; y++)
        {
            sb.Append($"{y + 1} ");
            for (int x = 0; x < size; x++)
            {
                sb.Append(" " + GetCellChar(field[x, y]));
            }
            sb.AppendLine();
        }
            
        Console.SetCursorPosition(0, 3);
        Console.Write(sb.ToString());
    }

    private char GetCellChar(Cell cell)
    {
        if (cell.IsWall) return '#';
        if (cell.MarkedByPlayer == null) return '.';
        
        return cell.MarkedByPlayer.Name[0]; 
    }
    
    private void OnBoardRestored(Cell[,] field, Player? player)
    {
        Redraw(field, player);
    }
}