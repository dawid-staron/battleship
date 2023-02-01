using System;
using GameComponents;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using TestingUtilities;
using UI.Drawing;
using Xunit;

namespace UI.Tests
{
    public class DrawingTests
    {
        private readonly PainterSpike _sut = new();

        [Fact]
        public void CurrentBattlegroundState_AfterInterpretation_IsReflectInDrawingProcess()
        {
            var expectedSymbolsUsedOnHitMap = new StringBuilder()
                .AppendLine("   A B C D E F G H I J")
                .AppendLine("1  $ *                ")
                .AppendLine("2    * x              ")
                .AppendLine("3    * x              ")
                .AppendLine("4  $ *                ")
                .AppendLine("5    *                ")
                .AppendLine("6                     ")
                .AppendLine("7                     ")
                .AppendLine("8                     ")
                .AppendLine("9                     ")
                .AppendLine("10                    ")
                .ToString();
            var expectedColorsUsedOnHitMap = new StringBuilder()
                .AppendLine($"{new string('G', 22)}")
                .AppendLine("GGGRGGGGGGGGGGGGGGGGGG")
                .AppendLine("GGGGGGGBGGGGGGGGGGGGGG")
                .AppendLine("GGGGGGGBGGGGGGGGGGGGGG")
                .AppendLine("GGGRGGGGGGGGGGGGGGGGGG")
                .AppendLine($"{new string('G', 22)}")
                .AppendLine($"{new string('G', 22)}")
                .AppendLine($"{new string('G', 22)}")
                .AppendLine($"{new string('G', 22)}")
                .AppendLine($"{new string('G', 22)}")
                .AppendLine($"{new string('G', 22)}")
                .ToString();
            var missAttackAttempts = Factories.CreatePositions(
                Factories.CreatePosition(1, 2),
                Factories.CreatePosition(2, 2),
                Factories.CreatePosition(3, 2),
                Factories.CreatePosition(4, 2),
                Factories.CreatePosition(5, 2));
            var firstShipLocation = Factories.CreatePositions(
                Factories.CreatePosition(1, 1),
                Factories.CreatePosition(2, 1),
                Factories.CreatePosition(3, 1),
                Factories.CreatePosition(4, 1));
            var secondShipLocation = Factories.CreatePositions(
                Factories.CreatePosition(2, 3),
                Factories.CreatePosition(3, 3));
            var game = Game.Start(
                new[] { firstShipLocation.Count, secondShipLocation.Count },
                new BattlegroundWithSemiManualShipLocationStub(
                    Factories.FormBattlegroundPositions(10, 10),
                    firstShipLocation,
                    secondShipLocation),
                battleCompleted: () => { });
            // Destroy secondShip
            game.MakeAttack(secondShipLocation.First().Coordinate);
            game.MakeAttack(secondShipLocation.Last().Coordinate);
            // Put on fire first and last positions of firstShip
            game.MakeAttack(firstShipLocation.First().Coordinate);
            game.MakeAttack(firstShipLocation.Last().Coordinate);
            // Make some misses
            missAttackAttempts.ForEach(position => game.MakeAttack(position.Coordinate));

            new BattlegroundInterpreter(
                new Dictionary<char, int>
                {
                    { 'A', 1 }, { 'B', 2 }, { 'C', 3 }, { 'D', 4 }, { 'E', 5 },
                    { 'F', 6 }, { 'G', 7 }, { 'H', 8 }, { 'I', 9 }, { 'J', 10 }
                },
                _sut)
                .Interpret(game.Battleground)
                .Drawing();

            _sut.DrawingSymbolsUsedResult.Should().Be(expectedSymbolsUsedOnHitMap);
            _sut.DrawingColorsUsedResult.Should().Be(expectedColorsUsedOnHitMap);
        }

        private class PainterSpike : IPrinter
        {
            private readonly StringBuilder _symbolsBuilder = new();
            private readonly StringBuilder _colorsBuilder = new();

            public string DrawingSymbolsUsedResult => _symbolsBuilder.ToString();

            public string DrawingColorsUsedResult => _colorsBuilder.ToString();

            public void Print(Mark markToPrint)
            {
                _symbolsBuilder.Append(markToPrint.Symbol);
                _colorsBuilder.Append(ColorShortName());

                string ColorShortName() =>
                    markToPrint.Color switch
                    {
                        ConsoleColor.Red => "R",
                        ConsoleColor.DarkBlue => "B",
                        _ => "G"
                    };
            }

            public void ShiftToNewLine()
            {
                _symbolsBuilder.Remove(_symbolsBuilder.Length - 1, 1);
                _symbolsBuilder.AppendLine();
                _colorsBuilder.Remove(_colorsBuilder.Length - 1, 1);
                _colorsBuilder.AppendLine();
            }
        }
    }
}