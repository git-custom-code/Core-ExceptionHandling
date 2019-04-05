namespace CustomCode.Core.ExceptionHandling
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Extension methods for the <see cref="Exception"/> tpye
    /// </summary>
    public static partial class ExceptionExtensions
    {
        #region Logic

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
        public static bool TryGetCausingException(this Exception exception, out Exception? causingException)
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
        public static bool TryGetCausingException(this Exception exception, out Exception? causingException, out uint causingExceptionCount)
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

            if (exception is IAggregateException iAggregateException)
            {
                foreach (var innerException in iAggregateException.InnerExceptions)
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