using System;
using System.Collections.Generic;

namespace UI.Drawing
{
    internal class LineToDraw
    {
        private readonly List<Mark> _line;

        public LineToDraw(IEnumerable<Mark> linePrefix)
        {
            _line = new List<Mark>(linePrefix ?? throw new ArgumentNullException(nameof(linePrefix)));
        }

        public IEnumerable<Mark> Line => _line;

        public void Attache(Mark markToAttache)
        {
            _line.Add(markToAttache ?? throw new ArgumentNullException(nameof(markToAttache)));
            _line.Add(Mark.Space);
        }
    }
}