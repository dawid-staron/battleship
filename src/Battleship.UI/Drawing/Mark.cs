using System;

namespace UI.Drawing
{
    public class Mark
    {
        private Mark(string symbol, ConsoleColor color)
        {
            Symbol = symbol;
            Color = color;
        }

        public string Symbol { get; }

        public ConsoleColor Color { get; }

        public static Mark StandardSymbol(string symbol) => new(symbol, ConsoleColor.Gray);

        public static Mark Sink => new("x", ConsoleColor.DarkBlue);

        public static Mark Fire => new("$", ConsoleColor.Red);

        public static Mark Miss => new("*", ConsoleColor.Gray);

        public static Mark Space => new(" ", ConsoleColor.Gray);

        public static Mark Empty => new(string.Empty, ConsoleColor.Gray);

        public override string ToString() => Symbol;
    }
}