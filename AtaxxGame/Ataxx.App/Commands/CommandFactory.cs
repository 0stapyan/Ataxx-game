namespace Ataxx.App.Commands;

public class CommandFactory
{
    private readonly GameContext _context;

    public CommandFactory(GameContext context)
    {
        _context = context;
    }

    public ICommand Create(string input)
    {
        if (input == "start") return new StartCommand(_context);
        if (input == "start_pve") return new StartPvECommand(_context);
        if (input.StartsWith("move")) return new MoveCommand(_context, input);
        if (input == "undo") return new UndoCommand(_context);
        if (input == "hint") return new HintCommand(_context);
        if (input == "switch_view") return new SwitchViewCommand(_context);
        if (input == "exit") return new ExitCommand(_context);

        return new UnknownCommand(_context, input);
    }
}