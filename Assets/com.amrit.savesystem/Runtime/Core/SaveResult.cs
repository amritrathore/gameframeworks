using System;

namespace Amrit.SaveSystem
{
    public sealed class SaveResult
    {
        public bool Success { get; }

        public string Message { get; }

        public Exception Exception { get; }

        public SaveResult(
            bool success,
            string message = "",
            Exception exception = null)
        {
            Success = success;
            Message = message;
            Exception = exception;
        }

        public static SaveResult Ok(string message = "")
        {
            return new SaveResult(true, message);
        }

        public static SaveResult Fail(
            string message,
            Exception exception = null)
        {
            return new SaveResult(
                false,
                message,
                exception);
        }
    }
}