namespace WorkyOne.Infrastructure.Exceptions.Utilities.ColorUtility
{
    public class WrongColorFormatException : Exception
    {
        public WrongColorFormatException()
            : base("Color must be in HEX format (#RRGGBB)") { }
    }
}
