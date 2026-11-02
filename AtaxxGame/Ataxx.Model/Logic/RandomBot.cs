namespace Ataxx.Model.Logic;

using System;
using System.Linq;
using Ataxx.Model.Data;

public class RandomBot
{
    private readonly Random _random;

    public RandomBot()
    {
        _random = new Random();
    }
    
    public Move? GetNextMove(IGameController game, Player player)
    {
        var validMoves = game.GetAllValidMoves(player);
        
        if (!validMoves.Any())
        {
            return null;
        }
        
        int index = _random.Next(validMoves.Count);
        return validMoves[index];
    }
}