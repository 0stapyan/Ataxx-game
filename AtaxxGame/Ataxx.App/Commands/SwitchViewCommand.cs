using Ataxx.Model.Logic;
using Ataxx.View.Console;
using Ataxx.Input.Console;
using Ataxx.Model.Data;

namespace Ataxx.App.Commands;
public class SwitchViewCommand : ICommand
{
    private readonly GameContext _ctx;

    public SwitchViewCommand(GameContext ctx) => _ctx = ctx;

    public void Execute()
    {
        _ctx.CurrentView.Unlisten(_ctx.GameEvents);
        _ctx.CurrentView = _ctx.CurrentView is SimpleConsoleView ? _ctx.EnhancedView : _ctx.SimpleView;
        _ctx.CurrentView.ListenTo(_ctx.GameEvents);
        var engine = (AtaxxEngineWithEvents)_ctx.Game;
        _ctx.CurrentView.Redraw(engine.GetFieldCopy(), engine.CurrentPlayer);
        _ctx.CurrentView.ShowMessage("View switched.");
    }
}