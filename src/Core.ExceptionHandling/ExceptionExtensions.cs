namespace CustomCode.Core.ExceptionHandling
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// Extension methods for the <see cref="Exception"/> tpye
    /// </summary>
    public static class ExceptionExtensions
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
        public static string GetFileName(this Exception exception)
        {
            if (exception == null)
            {
                return null;
            }

            var trace = new StackTrace(exception, true);
            var frames = trace.GetFrames();
            if (frames?.Length > 0)
            {
                var fileName = frames[0].GetFileName();
                if (!string.IsNullOrEmpty(fileName))
                {
                    return Path.GetFileName(fileName);
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the names of the sourcecode files where the specified <paramref name="exception"/>'s
        /// causing exceptions were raised if pdb's are deployed or null otherwise.
        /// if 
        /// </summary>
        /// <param name="exception"> The exception to act on. </param>
        /// <returns>
        /// The names of the sourcecode files where the specified <paramref name="exception"/>'s
        /// causing exceptions were raised if pdb's are deployed or null otherwise.
        /// </returns>
        public static IEnumerable<string> GetCausingFileNames(this Exception exception)
        {
            var result = new List<string>();
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
        public static bool TryGetCausingFileName(this Exception exception, out string causingFileName)
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

        /// <summary>
        /// Gets the line number inside of the sourcecode file where the specified <paramref name="exception"/> was raised.
        /// </summary>
        /// <param name="exception"> The exception to act on. </param>
        /// <returns> The line number inside of the sourcecode file where the specified <paramref name="exception"/> was raised. </returns>
        public static uint? GetLineNumber(this Exception exception)
        {
            if (exception == null)
            {
                return 0;
            }

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

        /// <summary>
        /// Get the <paramref name="exception"/>'s innermost exceptions (can be more than one in case of multiple asynchronous tasks).
        /// </summary>
        /// <param name="exception"> The exception to act on. </param>
        /// <returns>
        /// A collection that contains the innermost exceptions (more than one in case of multiple asynchronous tasks
        /// => see <see cref="AggregateException"/>) or is empty if no innermost exception exists.
        /// </returns>
        public static IEnumerable<Exception> GetCausingExceptions(this Exception exception)
        {
            if (exception?.InnerException == null)
            {
                return Enumerable.Empty<Exception>();
            }

            var result = new List<Exception>();
            GetCausingExceptionsRecursively(exception, ref result);
            return result;
        }

        /// <summary>
        /// Try to get the <paramref name="exception"/>'s innermost exception.
        /// </summary>
        /// <param name="exception"> The exception to act on. </param>
        /// <param name="causingException">
        /// The <paramref name="exception"/>'s innermost exception or null if no excpetion or more than one exception exist.
        /// </param>
        /// <returns>
        /// True if the innermost exception was found or false if no excpetion or more than one exception was found.
        /// </returns>
        public static bool TryGetCausingException(this Exception exception, out Exception causingException)
        {
            return TryGetCausingException(exception, out causingException, out _);
        }

        /// <summary>
        /// Try to get the <paramref name="exception"/>'s innermost exception.
        /// </summary>
        /// <param name="exception"> The exception to act on. </param>
        /// <param name="causingException">
        /// The <paramref name="exception"/>'s innermost exception or null if no excpetion or more than one exception exist.
        /// </param>
        /// <param name="causingExceptionCount"> The number of found innermost exceptions. </param>
        /// <returns>
        /// True if the innermost exception was found or false if no excpetion or more than one exception was found.
        /// </returns>
        public static bool TryGetCausingException(this Exception exception, out Exception causingException, out uint causingExceptionCount)
        {
            if (exception?.InnerException == null)
            {
                causingException = null;
                causingExceptionCount = 0;
                return false;
            }

            var result = GetCausingExceptions(exception);
            causingExceptionCount = (uint)result.Count();
            if (causingExceptionCount == 1)
            {
                causingException = result.First();
                return true;
            }

            causingException = null;
            return false;
        }

        /// <summary>
        /// Recursively search the given <paramref name="exception"/> for its innermost exceptions
        /// (more than one in case of multiple asynchronous tasks) and store their references in the
        /// <paramref name="result"/> collection.
        /// </summary>
        /// <param name="exception"> The exception whose innermost exceptions should be found. </param>
        /// <param name="result"> The found innermost exceptions. </param>
        private static void GetCausingExceptionsRecursively(Exception exception, ref List<Exception> result)
        {
            if (exception is AggregateException aggregateException)
            {
                foreach (var innerException in aggregateException.InnerExceptions)
                {
                    GetCausingExceptionsRecursively(innerException, ref result);
                }
                return;
            }

            if (exception.InnerException == null)
            {
                result.Add(exception);
                return;
            }

            GetCausingExceptionsRecursively(exception.InnerException, ref result);
        }

        #endregion
    }
}