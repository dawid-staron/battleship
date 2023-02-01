namespace UI.Drawing
{
    internal interface IPrinter
    {
        void Print(Mark markToPrint);

        void ShiftToNewLine();
    }
}