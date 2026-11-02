using Ataxx.Model.Logic;
using Ataxx.View.Console;
using Ataxx.Input.Console;
using Ataxx.Model.Data;

namespace Ataxx.App.Commands;
public class StartCommand : ICommand
{
    private readonly GameContext _ctx;

    public StartCommand(GameContext ctx) => _ctx = ctx;

    public void Execute()
    {
        _ctx.IsBotActive = false;
        _ctx.Input.ParseAndExecute("start", _ctx.Game);
    }
}