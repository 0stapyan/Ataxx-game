namespace Ataxx.Input.Console;

using System;
using System.Linq;
using Ataxx.Model;
using Ataxx.Model.Logic;
using Ataxx.Model.Data;

public class AtaxxInputHandler
    {

        public void ParseAndExecute(string input, IGameController game)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return;
            }

            var parts = input.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string command = parts[0].ToLower();

            switch (command)
            {
                case "start":
                    game.StartGame();
                    break;

                case "move":
                    if (game.IsEnded)
                        throw new Exception("Game is over. Start a new match with 'start' or 'start_pve'.");
    
                    HandleMove(parts, game);
                    break;

                
                case "switch_view":
                    break;
                
                case "start_pve":
                    break;
                
                case "hint":
                    break;
                
                case "undo":
                    break;

                default:
                    throw new Exception("Unknown command");
            }
        }
        
        private void HandleMove(string[] parts, IGameController game)
        {
            if (game == null || game.CurrentPlayer == null || game.IsEnded)
            {
                throw new Exception("Game is not running. Type 'start' to begin.");
            }
            
            if (parts.Length != 3)
            {
                throw new Exception("Invalid move format. Expected: move [from] [to] (e.g., move A1 B2)");
            }

            if (!CoordinateParser.TryParse(parts[1], out int fromX, out int fromY))
            {
                throw new Exception("Invalid 'from' coordinate");
            }
            
            if (!CoordinateParser.TryParse(parts[2], out int toX, out int toY))
            {
                throw new Exception("Invalid 'to' coordinate");
            }
            
            var fromCoord = new Coordinates(fromX, fromY);
            var toCoord = new Coordinates(toX, toY);
            var move = new Move(fromCoord, toCoord);
            
            game.MakeMove(move);
        }
    }
    