namespace Ataxx.Model;

using System;
using Ataxx.Model.Data;
    
public interface IGameEvents
{
    event Action<Cell[,]> GameStarted;
    event Action<Cell[,]> BoardUpdated;
    event Action<Cell[,], Player?> BoardRestored;
    event Action<Player> PlayerSwitched;
    event Action<Player> PlayerWon;
    event Action MatchDrawn;
    event Action<Player> TurnSkipped;
}