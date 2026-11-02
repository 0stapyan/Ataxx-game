using Ataxx.Model.Logic;
using Ataxx.View.Console;
using Ataxx.Input.Console;
using Ataxx.Model.Data;

namespace Ataxx.App.Commands;
public class UndoCommand : ICommand
{
    private readonly GameContext _ctx;

    public UndoCommand(GameContext ctx) => _ctx = ctx;

    public void Execute()
    {
        if (!_ctx.IsBotActive)
        {
            _ctx.CurrentView.ShowMessage("Undo only available in PvE.");
            return;
        }

        if ((DateTime.Now - _ctx.LastBotMoveTime).TotalSeconds > 3)
        {
            _ctx.CurrentView.ShowMessage("Undo expired.");
            return;
        }

        if (_ctx.Game.HistoryCount > 0) _ctx.Game.UndoMove();
        if (_ctx.Game.HistoryCount > 0) _ctx.Game.UndoMove();

        _ctx.CurrentView.ShowMessage("Undo complete.");
    }
}