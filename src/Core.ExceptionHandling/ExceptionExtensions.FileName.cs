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
        /// Gets the name of the sourcecode file where the specified <paramref name="exception"/> was raised
        /// if pdb's are deployed or null otherwise.
        /// </summary>
        /// <param name="exception"> The exception to act on. </param>
        /// <returns>
        /// The name of the sourcecode file where the specified <paramref name="exception"/> was raised
        /// (if pdb's are deployed) or null otherwise.
        /// </returns>
        public static string? GetFileName(this Exception exception)
        {
            var trace = new StackTrace(exception, true);
            var frames = trace.GetFrames();
            if (frames?.Length > 0)
            {
                var fileName = frames[0].GetFileName();
                if (!string.IsNullOrEmpty(fileName))
                {
                    return fileName;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the names of the sourcecode files where the specified <paramref name="exception"/>'s
        /// causing exceptions were raised if pdb's are deployed or null otherwise.
        /// </summary>
        /// <param name="exception"> The exception to act on. </param>
        /// <returns>
        /// The names of the sourcecode files where the specified <paramref name="exception"/>'s
        /// causing exceptions were raised if pdb's are deployed or null otherwise.
        /// </returns>
        public static IEnumerable<string?> GetCausingFileNames(this Exception exception)
        {
            var result = new List<string?>();
            foreach (var causingException in exception.GetCausingExceptions())
            {
                result.Add(GetFileName(causingException));
            }
            return result;
        }

        /// <summary>
        /// Try to get the name of the sourcecode file where the specified <paramref name="exception"/>'s
        /// causing exception was raised if pdb's are deployed and a single causing exception exists
        /// or null otherwise.
        /// </summary>
        /// <param name="exception"> The exception to act on. </param>
        /// <param name="causingFileName">
        /// The name of the sourcecode file where the specified <paramref name="exception"/>'s
        /// causing exception was raised or null.
        /// </param>
        /// <returns>
        /// True if the specified <paramref name="exception"/>'s causing exception's sourcecode file
        /// was found, false otherwise.
        /// </returns>
        public static bool TryGetCausingFileName(this Exception exception, out string? causingFileName)
        {
            var causingExceptions = GetCausingExceptions(exception);
            if (causingExceptions.Count() == 1)
            {
                causingFileName = GetFileName(causingExceptions.First());
                return true;
            }

            causingFileName = null;
            return false;
        }

        #endregion
    }
}