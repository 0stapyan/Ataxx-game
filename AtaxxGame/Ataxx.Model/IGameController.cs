namespace Ataxx.Model;

using Ataxx.Model.Data;
    
public interface IGameController
{
    void StartGame(int size = 7);
    void MakeMove(Move move);
    
    void UndoMove(); 
    int HistoryCount { get; }
    
    Player? CurrentPlayer { get; }
    bool IsEnded { get; }
    List<Move> GetAllValidMoves(Player player);
}