using System;

namespace Core.SaveSystem
{
    public class SaveException : Exception
    {
        public SaveException()
        {
        }

        public SaveException(string message)
            : base(message)
        {
        }

        public SaveException(
            string message,
            Exception innerException)
            : base(message, innerException)
        {
        }
    }
}