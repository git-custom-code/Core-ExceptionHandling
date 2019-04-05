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
        /// Gets the context of the specified <paramref name="exception"/>
        /// (see <see cref="ExceptionContext"/> for more details).
        /// </summary>
        /// <param name="exception"> The exception to act on. </param>
        /// <returns>
        /// The specified <paramref name="exception"/>'s context.
        /// </returns>
        public static ExceptionContext GetContext(this Exception exception)
        {
            var methodName = GetMethodName(exception);
            var typeName = GetTypeName(exception);
            var fileName = GetFileName(exception);
            var lineNumber = GetLineNumber(exception);
            return new ExceptionContext(methodName ?? "UnknownMethod", typeName ?? "UnkownType", fileName, lineNumber);
        }

        /// <summary>
        /// Gets the contexts of the specified <paramref name="exception"/>'s causing exceptions
        /// (see <see cref="ExceptionContext"/> for more details).
        /// </summary>
        /// <param name="exception"> The exception to act on. </param>
        /// <returns>
        /// The contexts of the specified <paramref name="exception"/>'s causing exceptions.
        /// </returns>
        public static IEnumerable<ExceptionContext> GetCausingContexts(this Exception exception)
        {
            var result = new List<ExceptionContext>();
            foreach (var causingException in exception.GetCausingExceptions())
            {
                result.Add(GetContext(causingException));
            }
            return result;
        }

        /// <summary>
        /// Try to get the context of the specified <paramref name="exception"/>'s single causing exception
        /// (see <see cref="ExceptionContext"/> for more details) or null if no such exception exists.
        /// </summary>
        /// <param name="exception"> The exception to act on. </param>
        /// <param name="causingContext">
        /// The context of the specified <paramref name="exception"/>'s single causing exception or null.
        /// </param>
        /// <returns>
        /// True if the specified <paramref name="exception"/>'s causing exception's context
        /// was found, false otherwise.
        /// </returns>
        public static bool TryGetCausingContext(this Exception exception, out ExceptionContext? causingContext)
        {
            var causingExceptions = GetCausingExceptions(exception);
            if (causingExceptions.Count() == 1)
            {
                causingContext = GetContext(causingExceptions.First());
                return true;
            }

            causingContext = null;
            return false;
        }

        #endregion
    }
}