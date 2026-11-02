using Ataxx.Model.Logic;
using Ataxx.View.Console;
using Ataxx.Input.Console;
using Ataxx.Model.Data;

namespace Ataxx.App.Commands;
public class UnknownCommand : ICommand
{
    private readonly GameContext _ctx;
    private readonly string _input;
    public UnknownCommand(GameContext ctx, string input) => (_ctx, _input) = (ctx, input);
    public void Execute() => _ctx.CurrentView.ShowMessage($"Unknown command: {_input}");
}