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
        /// Gets the name of the method that has raised the specified <paramref name="exception"/>.
        /// </summary>
        /// <param name="exception"> The exception to act on. </param>
        /// <returns> The name of the method that has raised the specified <paramref name="exception"/>. </returns>
        public static string GetMethodName(this Exception exception)
        {
            if (exception == null)
            {
                return null;
            }

            var trace = new StackTrace(exception, true);
            var frames = trace.GetFrames();
            if (frames?.Length > 0)
            {
                var method = frames[0].GetMethod();
                return method.Name;
            }

            return null;
        }

        /// <summary>
        /// Gets the names of the methods that have raised the specified <paramref name="exception"/>'s causing exceptions.
        /// </summary>
        /// <param name="exception"> The exception to act on. </param>
        /// <returns>
        /// The names of the methods that have raised the specified <paramref name="exception"/>'s causing exceptions.
        /// </returns>
        public static IEnumerable<string> GetCausingMethodNames(this Exception exception)
        {
            var result = new List<string>();
            foreach (var causingException in exception.GetCausingExceptions())
            {
                result.Add(GetMethodName(causingException));
            }
            return result;
        }

        /// <summary>
        /// Try to get the name of the method that has raised the specified <paramref name="exception"/>'s
        /// causing exception if a single causing exception exists or null otherwise.
        /// </summary>
        /// <param name="exception"> The exception to act on. </param>
        /// <param name="causingMethodName">
        /// The name of the method that has raised the specified <paramref name="exception"/>'s
        /// causing exception if a single causing exception exists or null otherwise.
        /// </param>
        /// <returns>
        /// True if the specified <paramref name="exception"/>'s causing exception's method name
        /// was found, false otherwise.
        /// </returns>
        public static bool TryGetCausingMethodName(this Exception exception, out string causingMethodName)
        {
            var causingExceptions = GetCausingExceptions(exception);
            if (causingExceptions.Count() == 1)
            {
                causingMethodName = GetMethodName(causingExceptions.First());
                return true;
            }

            causingMethodName = null;
            return false;
        }

        #endregion
    }
}