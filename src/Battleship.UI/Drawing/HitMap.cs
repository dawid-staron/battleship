using System;
using System.Collections.Generic;

namespace UI.Drawing
{
    internal class HitMap
    {
        private readonly IEnumerable<LineToDraw> _lines;
        private readonly IPrinter _printer;

        public HitMap(IPrinter printer, IEnumerable<LineToDraw> lines)
        {
            _printer = printer ?? throw new ArgumentNullException(nameof(printer));
            _lines = lines ?? throw new ArgumentNullException(nameof(lines));
        }

        public void Drawing()
        {
            foreach (var toDraw in _lines)
            {
                foreach (var markToPrint in toDraw.Line)
                {
                    _printer.Print(markToPrint);
                }
                _printer.ShiftToNewLine();
            }
        }
    }
}