using System;
using System.Linq;
using Ataxx.Model.Logic;
using Ataxx.Model.Data;
using Ataxx.View.Console;
using Ataxx.Input.Console;
using Ataxx.App.Commands;

var playerX = new Player("X");
var playerO = new Player("O");

var game = new AtaxxEngineWithEvents(playerX, playerO);
var inputHandler = new AtaxxInputHandler();

IAtaxxView simpleView = new SimpleConsoleView();
IAtaxxView enhancedView = new EnhancedConsoleView();

IAtaxxView currentView = simpleView;
currentView.ListenTo(game);

game.PlayerWon += winner => currentView.ShowMessage($" GAME OVER — Player {winner.Name} wins!");
game.MatchDrawn += () => currentView.ShowMessage(" GAME OVER — It's a draw!");

var bot = new RandomBot(); 
var hintService = new HintService();
bool isBotActive = false;
DateTime lastBotMoveTimestamp = DateTime.MinValue;

var context = new GameContext(game, currentView, simpleView, enhancedView, inputHandler, hintService, bot);
var factory = new CommandFactory(context);


currentView.ShowMessage("Welcome to Ataxx!");
currentView.ShowMessage("Type 'start' (for PvP) or 'start_pve' (for Player vs Bot).");
currentView.ShowMessage("Type 'move A1 B2' to make a move.");
currentView.ShowMessage("Type 'switch_view' to change UI.");
currentView.ShowMessage("Type 'hint' to see possible moves.");
currentView.ShowMessage("Type 'undo' to revert the last move (PvE only).");
currentView.ShowMessage("Type 'exit' to quit.");


while (true)
{
    Console.Write("\n> ");
    var input = Console.ReadLine()?.Trim().ToLower();
    if (string.IsNullOrWhiteSpace(input)) continue;

    var command = factory.Create(input);
    command.Execute();
    
    if (context.IsBotActive && game.CurrentPlayer == playerO && !game.IsEnded)
    {
        currentView.ShowMessage("Bot is thinking...");
        Thread.Sleep(1000);
        var move = bot.GetNextMove(game, playerO);
        if (move != null)
        {
            game.MakeMove(move);
            context.LastBotMoveTime = DateTime.Now;
        }
    }
}