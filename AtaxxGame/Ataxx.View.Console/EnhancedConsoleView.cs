namespace Ataxx.View.Console;

using System;
using System.Text;
using Ataxx.Model.Logic;
using Ataxx.Model.Data;
using Ataxx.Model;

public class EnhancedConsoleView : IAtaxxView
{
    private Player? _playerX;
    private Player? _playerO;

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
        Console.WriteLine("╔═══════════════════════════════╗");
        Console.WriteLine("║   A T A X X (Enhanced View) ║");
        Console.WriteLine("╚═══════════════════════════════╝");
        
        DrawBoard(field);
            
        if (currentPlayer != null)
        {
            ShowMessage($"Player {currentPlayer.Name}'s turn.");
        }
    }
    
    private void OnGameStarted(Cell[,] field)
    {
        if (field[0, 0].MarkedByPlayer != null)
            _playerX = field[0, 0].MarkedByPlayer;
        
        int lastColumn = field.GetLength(0) - 1;
        if (field[lastColumn, 0].MarkedByPlayer != null)
            _playerO = field[lastColumn, 0].MarkedByPlayer;
        
        Redraw(field, _playerX); 
        ShowMessage("Game started! Player X moves.");
    }
    
    private void OnTurnSkipped(Player skippedPlayer)
    {
        ShowMessage($"Player {skippedPlayer.Name} has no moves! Turn skipped.");
    }
    
    public void ShowMessage(string message)
    {
        int messageLine = 18; 
        
        Console.SetCursorPosition(0, messageLine);
        Console.Write(new string(' ', Console.WindowWidth)); 
        Console.SetCursorPosition(0, messageLine);
        Console.Write(message);
    }

    private void OnBoardUpdated(Cell[,] field)
    {
        DrawBoard(field);
    }
    
    private void OnPlayerSwitched(Player player)
    {
        ShowMessage($"Player {player.Name}'s turn.");
    }
    
    private void OnPlayerWon(Player player)
    {
        ShowMessage($"--- GAME OVER --- Player {player.Name} won!");
    }
    
    private void OnMatchDrawn()
    {
        ShowMessage("--- GAME OVER --- It's a draw!");
    }

    private void DrawBoard(Cell[,] field)
    {
        int size = field.GetLength(0);
        var sb = new StringBuilder();
        
        DrawColumnHeaders(sb, size);
        
        DrawTopBorder(sb, size);
        
        for (int y = 0; y < size; y++)
        {
            DrawRow(sb, field, y, size);
            
            bool isLastRow = (y == size - 1);
            if (!isLastRow)
            {
                DrawMiddleBorder(sb, size);
            }
        }
        
        DrawBottomBorder(sb, size);
        
        Console.SetCursorPosition(0, 3);
        Console.Write(sb.ToString());
        
        DrawColors(field);
    }
    
    private void DrawColumnHeaders(StringBuilder sb, int size)
    {
        sb.Append("    ");
        
        for (int i = 0; i < size; i++)
        {
            char columnLetter = (char)('A' + i);
            sb.Append($" {columnLetter}  ");
        }
        
        sb.AppendLine();
    }
    
    private void DrawTopBorder(StringBuilder sb, int size)
    {
        sb.Append("  ╔");
        
        for (int i = 0; i < size; i++)
        {
            sb.Append("═══");
            
            bool isLastColumn = (i == size - 1);
            if (isLastColumn)
            {
                sb.Append("╗");
            }
            else
            {
                sb.Append("╦");
            }
        }
        
        sb.AppendLine();
    }
    
    private void DrawRow(StringBuilder sb, Cell[,] field, int y, int size)
    {
        int rowNumber = y + 1;
        sb.Append($"{rowNumber.ToString().PadLeft(2)}║");
        
        for (int x = 0; x < size; x++)
        {
            char cellSymbol = GetCellChar(field[x, y]);
            sb.Append($" {cellSymbol} ║");
        }
        
        sb.AppendLine();
    }
    
    private void DrawMiddleBorder(StringBuilder sb, int size)
    {
        sb.Append("  ╟");
        
        for (int i = 0; i < size; i++)
        {
            sb.Append("───");
            
            bool isLastColumn = (i == size - 1);
            if (isLastColumn)
            {
                sb.Append("╢");
            }
            else
            {
                sb.Append("╫");
            }
        }
        
        sb.AppendLine();
    }
    
    private void DrawBottomBorder(StringBuilder sb, int size)
    {
        sb.Append("  ╚");
        
        for (int i = 0; i < size; i++)
        {
            sb.Append("═══");
            
            bool isLastColumn = (i == size - 1);
            if (isLastColumn)
            {
                sb.Append("╝");
            }
            else
            {
                sb.Append("╩");
            }
        }
        
        sb.AppendLine();
    }
    
    private void DrawColors(Cell[,] field)
    {
        int size = field.GetLength(0);
        Console.ResetColor();
            
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                var cell = field[x, y];
                
                int screenX = 4 + (x * 4);
                int screenY = 5 + (y * 2);
                    
                Console.SetCursorPosition(screenX, screenY);
                
                if (cell.MarkedByPlayer == _playerX)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                }
                else if (cell.MarkedByPlayer == _playerO)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }
                else if (cell.IsWall)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
                    
                Console.Write(GetCellChar(cell));
                Console.ResetColor();
            }
        }
    }

    private char GetCellChar(Cell cell)
    {
        if (cell.IsWall)
        {
            return '#';
        }
        
        if (cell.MarkedByPlayer == null)
        {
            return '.';
        }
        
        return cell.MarkedByPlayer.Name[0];
    }
    
    private void OnBoardRestored(Cell[,] field, Player? player)
    {
        Redraw(field, player);
    }
}