using Ataxx.Model.Logic;
using Ataxx.View.Console;
using Ataxx.Input.Console;
using Ataxx.Model;

namespace Ataxx.App.Commands;

public class GameContext
{
    public IGameController Game { get; }
    public IGameEvents GameEvents => (IGameEvents)Game;
    public IAtaxxView CurrentView { get; set; }
    public IAtaxxView SimpleView { get; }
    public IAtaxxView EnhancedView { get; }
    public AtaxxInputHandler Input { get; }
    public HintService Hints { get; }
    public RandomBot Bot { get; }
    public bool IsBotActive { get; set; }
    public DateTime LastBotMoveTime { get; set; }

    public GameContext(
        IGameController game,
        IAtaxxView currentView,
        IAtaxxView simpleView,
        IAtaxxView enhancedView,
        AtaxxInputHandler input,
        HintService hints,
        RandomBot bot
    )
    {
        Game = game;
        CurrentView = currentView;
        SimpleView = simpleView;
        EnhancedView = enhancedView;
        Input = input;
        Hints = hints;
        Bot = bot;
        LastBotMoveTime = DateTime.MinValue;
    }
}