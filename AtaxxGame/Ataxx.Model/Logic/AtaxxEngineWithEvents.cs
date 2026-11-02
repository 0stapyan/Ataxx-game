namespace Ataxx.Model.Logic;

using System;
using System.Collections.Generic;
using Ataxx.Model.Data;

public class AtaxxEngineWithEvents : AtaxxEngine, IGameController, IGameEvents
{
    public event Action<Cell[,]> GameStarted;
    
    public event Action<Cell[,]> BoardUpdated;
    
    public event Action<Cell[,], Player?>? BoardRestored;
    
    public event Action<Player> PlayerSwitched;
    
    public event Action<Player> PlayerWon;
    
    public event Action MatchDrawn;
    public event Action<Player> TurnSkipped;

    public AtaxxEngineWithEvents(Player playerX, Player playerO) : base(playerX, playerO)
    {
    }

    public override void StartGame(int size = 7)
    {
        base.StartGame(size);
        GameStarted?.Invoke(GetFieldCopy());
    }
    
    public override void MakeMove(Move move)
    {
        base.MakeMove(move);
    }

    protected override void PlaceSymbol(Move move)
    {
        base.PlaceSymbol(move);
        BoardUpdated?.Invoke(GetFieldCopy());
    }

    protected override void ConvertNeighbours(Coordinates to, Player player)
    {
        base.ConvertNeighbours(to, player);
        BoardUpdated?.Invoke(GetFieldCopy());
    }

    protected override void SwitchPlayer()
    {
        base.SwitchPlayer();
        PlayerSwitched?.Invoke(CurrentPlayer);
    }
    
    public override void UndoMove()
    {
        base.UndoMove();
        BoardRestored?.Invoke(GetFieldCopy(), CurrentPlayer);
    }
    
    protected override void CheckGameState()
    {
        if (CurrentPlayer == null) return;

        var oldPlayer = CurrentPlayer;
        base.CheckGameState(); 
        var newPlayer = CurrentPlayer;

        if (oldPlayer == newPlayer && !IsEnded)
        {
            var skippedPlayer = (oldPlayer == PlayerX) ? PlayerO : PlayerX;
            TurnSkipped?.Invoke(skippedPlayer);
        }
    }

    protected override void EndGame()
    {
        base.EndGame();
        
        if (Winner == null)
        {
            MatchDrawn?.Invoke();
        }
        else
        {
            PlayerWon?.Invoke(Winner);
        }
    }
}