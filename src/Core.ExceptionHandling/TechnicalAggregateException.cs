namespace CustomCode.Core.ExceptionHandling
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Use this type (or derived types) whenever a technical aggregate exception has occured.
    /// </summary>
    /// <remarks>
    /// Note that all technical aggregate exceptions should be displayed to the user using a generalized
    /// "Unexpected Exception" message (hiding the real cause of the problem from the application users).
    /// </remarks>
    public class TechnicalAggregateException : TechnicalException, IAggregateException
    {
        #region Dependencies

        /// <summary>
        /// Creates a new instance of the <see cref="TechnicalAggregateException"/> type.
        /// </summary>
        /// <param name="developerMessage">
        /// The exception's message.
        /// Note that this message is not localized and should not be displayed to the application users.
        /// This message is meant to contain developer relevant informations and can be used for logging purposes.
        /// </param>
        /// <param name="userMessageResourceKey">
        /// A .resx key that can be used to localize the exception's message.
        /// Note that this message should be displayed to the application users and therefore can or should be localized
        /// if the application requires it. This message should not contain any informations that is irrelevant or
        /// confusing for the application users.
        /// </param>
        public TechnicalAggregateException(
            string developerMessage = "An unexpected technical aggregate exception has occured.",
            string userMessageResourceKey = DefaultResxKey)
            : base(developerMessage, string.IsNullOrEmpty(userMessageResourceKey) ? DefaultResxKey : userMessageResourceKey)
        {
            InnerExceptions = new ReadonlyCollection<Exception>(null);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="TechnicalAggregateException"/> type.
        /// </summary>
        /// <param name="innerExceptions"> The exceptions that have caused this exception to be thrown. </param>
        /// <param name="developerMessage">
        /// The exception's message.
        /// Note that this message is not localized and should not be displayed to the application users.
        /// This message is meant to contain developer relevant informations and can be used for logging purposes.
        /// </param>
        /// <param name="userMessageResourceKey">
        /// A .resx key that can be used to localize the exception's message.
        /// Note that this message should be displayed to the application users and therefore can or should be localized
        /// if the application requires it. This message should not contain any informations that is irrelevant or
        /// confusing for the application users.
        /// </param>
        public TechnicalAggregateException(
            IEnumerable<Exception> innerExceptions,
            string developerMessage = "An unexpected technical aggregate exception has occured.",
            string userMessageResourceKey = DefaultResxKey)
            : base(
                  (innerExceptions?.Count() > 0) ? innerExceptions.First() : null,
                  developerMessage,
                  string.IsNullOrEmpty(userMessageResourceKey) ? DefaultResxKey : userMessageResourceKey)
        {
            InnerExceptions = new ReadonlyCollection<Exception>(innerExceptions);
        }

        #endregion

        #region Data

        /// <summary>
        /// The .resx file key that represents a general default message for technical aggregate exceptions.
        /// </summary>
        private const string DefaultResxKey = "ErrorTechnicalAggregateDefaultMessage";

        /// <summary>
        /// Gets the exceptions that have caused this exception to be thrown.
        /// </summary>
        public ReadonlyCollection<Exception> InnerExceptions { get; }

        #endregion
    }
}