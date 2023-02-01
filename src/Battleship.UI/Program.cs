using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using GameComponents;
using GameComponents.Playground;
using GameComponents.Playground.BuildingBlocks;
using UI.Drawing;

namespace UI
{
    public class Program
    {
        private const int Rows = 10;
        private const int Columns = 10;
        private static bool Exit;
        private static bool Completed;
        private static readonly int[] ShipSizes = { 5, 4, 4 };
        private static readonly Dictionary<char, int> ColumnMappings = new()
        {
            { 'A', 1 }, { 'a', 1 },
            { 'B', 2 }, { 'b', 2 },
            { 'C', 3 }, { 'c', 3 },
            { 'D', 4 }, { 'd', 4 },
            { 'E', 5 }, { 'e', 5 },
            { 'F', 6 }, { 'f', 6 },
            { 'G', 7 }, { 'g', 7 },
            { 'H', 8 }, { 'h', 8 },
            { 'I', 9 }, { 'i', 9},
            { 'J', 10 }, { 'j', 10 }
        };

        public static void Main()
        {
            (bool, string) previousInput = new(false, string.Empty);
            var battlegroundInterpreter = new BattlegroundInterpreter(ColumnMappings, new ConsoleAdapter());
            var game = Game.Start(ShipSizes, BattlegroundCreator.Create(Rows, Columns), GameCompleted);
            var battleground = game.Battleground;
            while (!Exit)
            {
                LegendDisplay();
                battlegroundInterpreter.Interpret(battleground).Drawing();
                if (Completed)
                {
                    Exit = true;
                    ExitHandling();
                    continue;
                }
                var (formatInvalid, inputValue) = previousInput;
                if (formatInvalid)
                {
                    InvalidInputDisplay(inputValue);
                }

                Console.Write("Select position(columnNamerowNumber e.g. 'A1' or 'a1') and press 'Enter': ");
                var inputCoordinates = Console.ReadLine();
                if (Regex.IsMatch(inputCoordinates ?? string.Empty, @"^([a-jA-J])+([1-9]|10)$"))
                {
                    previousInput = new(false, inputCoordinates);
                    battleground = game.MakeAttack(MapTo(previousInput.Item2));
                }
                else
                {
                    previousInput = new(true, inputCoordinates);
                }
            }
        }

        private static void GameCompleted() => Completed = true;

        private static Coordinate MapTo(string inputFormat) =>
            new(int.Parse(inputFormat[1..]), ColumnMappings[inputFormat[0]]);

        private static void LegendDisplay()
        {
            Console.Clear();
            Console.WriteLine("Marks explanation:");
            Console.ForegroundColor = Mark.Space.Color;
            Console.WriteLine($"{Mark.Space} - position untouched");
            Console.ForegroundColor = Mark.Miss.Color;
            Console.WriteLine($"{Mark.Miss} - missed");
            Console.ForegroundColor = Mark.Fire.Color;
            Console.WriteLine($"{Mark.Fire} - ship hit");
            Console.ForegroundColor = Mark.Sink.Color;
            Console.WriteLine($"{Mark.Sink} - ship sank");
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine();
        }

        private static void ExitHandling()
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("All enemy battleships were destroyed");
            Console.ResetColor();
            Console.WriteLine("To exit press any key");
            Console.ReadKey();
        }

        private static void InvalidInputDisplay(string inputValue)
        {
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"Previous provided position: {inputValue} was invalid");
            Console.ResetColor();
        }
    }
}