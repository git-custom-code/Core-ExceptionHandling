namespace CustomCode.Core.ExceptionHandling
{
    using System;

    /// <summary>
    /// Extension methods for the <see cref="Exception"/> tpye
    /// </summary>
    public static class ExceptionExtensions
    {
        public static Exception GetCausingException(this Exception exception)
        {
            var causingException = exception;
            while (causingException.InnerException != null)
            {
                causingException = causingException.InnerException;
            }

            return causingException;
        }
    }
}