namespace Clickstreamer.Extensions
{
    using System;

    public static class EnumerationExtensions
    {
        public static string GetName(this Enum value)
        {
            return Enum.GetName(value.GetType(), value);
        }
    }
}