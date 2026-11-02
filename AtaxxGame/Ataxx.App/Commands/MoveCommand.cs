using Ataxx.Model.Logic;
using Ataxx.View.Console;
using Ataxx.Input.Console;
using Ataxx.Model.Data;

namespace Ataxx.App.Commands;
public class MoveCommand : ICommand
{
    private readonly GameContext _ctx;
    private readonly string _input;

    public MoveCommand(GameContext ctx, string input)
    {
        _ctx = ctx;
        _input = input;
    }

    public void Execute()
    {
        if (_ctx.Game.IsEnded)
        {
            _ctx.CurrentView.ShowMessage("Game is over. Start a new match.");
            return;
        }

        _ctx.Input.ParseAndExecute(_input, _ctx.Game);
    }
}