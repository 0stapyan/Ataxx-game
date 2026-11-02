namespace Ataxx.Model.Logic;

using System.Linq;
using System.Text;
using System.Collections.Generic;
using Ataxx.Model.Data;

public class HintService
{
    public string GetHint(IGameController game)
    {
        if (game.CurrentPlayer == null || game.IsEnded)
        {
            return "Game is not running. No hints available.";
        }
        
        var moves = game.GetAllValidMoves(game.CurrentPlayer);

        if (!moves.Any())
        {
            return "You have no possible moves. Turn will be skipped.";
        }
        
        var movesByFrom = moves.GroupBy(m => m.From);
            
        var sb = new StringBuilder("Possible moves: ");
            
        foreach (var group in movesByFrom)
        {
            sb.Append($"From {group.Key.ToCoordString()} to ["); 
            sb.Append(string.Join(", ", group.Select(m => m.To.ToCoordString())));
            sb.Append("] | ");
        }
        
        return sb.ToString().TrimEnd(' ', '|');
    }
}