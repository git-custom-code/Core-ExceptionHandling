namespace CustomCode.Core.ExceptionHandling
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    /// <summary>
    /// Extension methods for the <see cref="Exception"/> tpye
    /// </summary>
    public static partial class ExceptionExtensions
    {
        #region Logic

        /// <summary>
        /// Gets the line number inside of the sourcecode file where the specified <paramref name="exception"/> was raised.
        /// </summary>
        /// <param name="exception"> The exception to act on. </param>
        /// <returns> The line number inside of the sourcecode file where the specified <paramref name="exception"/> was raised. </returns>
        public static uint? GetLineNumber(this Exception exception)
        {
            var trace = new StackTrace(exception, true);
            var frames = trace.GetFrames();
            if (frames?.Length > 0)
            {
                var lineNumber = (uint)frames[0].GetFileLineNumber();
                return lineNumber;
            }

            return null;
        }

        /// <summary>
        /// Gets the line numbers inside of the sourcecode files where the specified <paramref name="exception"/>'s
        /// causing exceptions were raised if pdb's are deployed or null otherwise.
        /// if 
        /// </summary>
        /// <param name="exception"> The exception to act on. </param>
        /// <returns>
        /// The line numbers inside of the sourcecode files where the specified <paramref name="exception"/>'s
        /// causing exceptions were raised if pdb's are deployed or null otherwise.
        /// </returns>
        public static IEnumerable<uint?> GetCausingLineNumbers(this Exception exception)
        {
            var result = new List<uint?>();
            foreach (var causingException in exception.GetCausingExceptions())
            {
                result.Add(GetLineNumber(causingException));
            }
            return result;
        }

        /// <summary>
        /// Try to get the line number inside of the sourcecode file where the specified
        /// <paramref name="exception"/>'s causing exception was raised if pdb's are deployed
        /// and a single causing exception exists or null otherwise.
        /// </summary>
        /// <param name="exception"> The exception to act on. </param>
        /// <param name="causingLineNumber">
        /// The line number inside of the sourcecode file where the specified <paramref name="exception"/>'s
        /// causing exception was raised or null.
        /// </param>
        /// <returns>
        /// True if the specified <paramref name="exception"/>'s causing exception's sourcecode file
        /// was found, false otherwise.
        /// </returns>
        public static bool TryGetCausingLineNumber(this Exception exception, out uint? causingLineNumber)
        {
            var causingExceptions = GetCausingExceptions(exception);
            if (causingExceptions.Count() == 1)
            {
                causingLineNumber = GetLineNumber(causingExceptions.First());
                return true;
            }

            causingLineNumber = null;
            return false;
        }

        #endregion
    }
}