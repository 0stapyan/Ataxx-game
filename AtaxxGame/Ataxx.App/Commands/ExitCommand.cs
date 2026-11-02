using Ataxx.Model.Logic;
using Ataxx.View.Console;
using Ataxx.Input.Console;
using Ataxx.Model.Data;

namespace Ataxx.App.Commands;
public class ExitCommand : ICommand
{
    private readonly GameContext _ctx;
    public ExitCommand(GameContext ctx) => _ctx = ctx;
    public void Execute() => Environment.Exit(0);
}