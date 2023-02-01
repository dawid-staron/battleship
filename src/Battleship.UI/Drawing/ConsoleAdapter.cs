using System;

namespace UI.Drawing
{
    internal class ConsoleAdapter : IPrinter
    {
        public void Print(Mark markToPrint)
        {
            Console.ForegroundColor = markToPrint.Color;
            Console.Write(markToPrint.Symbol);
            Console.ResetColor();
        }

        public void ShiftToNewLine()
        {
            Console.WriteLine();
        }
    }
}