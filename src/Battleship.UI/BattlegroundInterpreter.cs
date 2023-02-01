using GameComponents.Playground;
using GameComponents.Playground.BuildingBlocks;
using System.Collections.Generic;
using System;
using System.Linq;
using UI.Drawing;

namespace UI
{
    internal class BattlegroundInterpreter
    {
        private readonly Dictionary<char, int> _columnMappings;
        private readonly IPrinter _printer;

        public BattlegroundInterpreter(Dictionary<char, int> columnMappings, IPrinter printer)
        {
            _columnMappings = columnMappings ?? throw new ArgumentNullException(nameof(columnMappings));
            _printer = printer ?? throw new ArgumentNullException(nameof(printer));
        }

        public HitMap Interpret(IBattlegroundScanning battleground)
        {
            if (battleground == null)
            {
                throw new ArgumentNullException(nameof(battleground));
            }

            var rowMarkApplier = new Dictionary<PositionStatus, Action<LineToDraw>>
            {
                { PositionStatus.Untouched, rowLine => rowLine.Attache(Mark.Space) },
                { PositionStatus.Empty, rowLine => rowLine.Attache(Mark.Miss) },
                { PositionStatus.OnFire, rowLine => rowLine.Attache(Mark.Fire) },
                { PositionStatus.Sank, rowLine => rowLine.Attache(Mark.Sink) },
            };

            var linesToDraw = new List<LineToDraw> { ColumnLine() };
            for (int rowIdx = battleground.FirstPosition.Row; rowIdx <= battleground.LastPosition.Row; rowIdx++)
            {
                var rowLine = new LineToDraw(RowLinePrefix(rowIdx, battleground.LastPosition.Row));
                for (int columnIdx = battleground.FirstPosition.Column; columnIdx <= battleground.LastPosition.Column; columnIdx++)
                {
                    var currentPosition = battleground.GetPositionByCoordinate(new Coordinate(rowIdx, columnIdx));
                    rowMarkApplier[currentPosition.Status](rowLine);
                }
                linesToDraw.Add(rowLine);
            }

            return new HitMap(_printer, linesToDraw);

            IEnumerable<Mark> RowLinePrefix(int currentRow, int lastRow) => new[]
            {
                Mark.StandardSymbol(currentRow.ToString()),
                Mark.Space,
                currentRow != lastRow ? Mark.Space : Mark.Empty
            };

            LineToDraw ColumnLine()
            {
                var columnLine = new LineToDraw(ColumnLinePrefix());
                foreach (var columnMapping in _columnMappings.Keys.Select(key => key.ToString().ToUpper()).GroupBy(column => column))
                {
                    columnLine.Attache(Mark.StandardSymbol(columnMapping.Key));
                }

                return columnLine;
            }

            IEnumerable<Mark> ColumnLinePrefix() => new[] { Mark.Space, Mark.Space, Mark.Space };
        }
    }
}
