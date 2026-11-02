namespace Ataxx.View.Console;

using Ataxx.Model.Logic;
using Ataxx.Model.Data;
using Ataxx.Model;

public interface IAtaxxView
{ 
    void ListenTo(IGameEvents gameEvents);
    
    void Unlisten(IGameEvents gameEvents);
    
    void Redraw(Cell[,] field, Player? currentPlayer);
    
    void ShowMessage(string message);
}