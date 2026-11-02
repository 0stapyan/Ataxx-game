using Ataxx.Model.Logic;
using Ataxx.View.Console;
using Ataxx.Input.Console;
using Ataxx.Model.Data;

namespace Ataxx.App.Commands;
public class HintCommand : ICommand
{
    private readonly GameContext _ctx;
    public HintCommand(GameContext ctx) => _ctx = ctx;
    public void Execute() => _ctx.CurrentView.ShowMessage(_ctx.Hints.GetHint(_ctx.Game));
}