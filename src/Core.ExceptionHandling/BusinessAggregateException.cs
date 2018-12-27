namespace CustomCode.Core.ExceptionHandling
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Use this type (or derived types) whenever a business aggregate exception has occured.
    /// </summary>
    /// <remarks>
    /// Note that all business aggregate exceptions should contain a user friendly message text that can and
    /// should be displayed to the application users.
    /// </remarks>
    public class BusinessAggregateException : BusinessException, IAggregateException
    {
        #region Dependencies

        /// <summary>
        /// Creates a new instance of the <see cref="BusinessAggregateException"/> type.
        /// </summary>
        /// <param name="userMessageResourceKey">
        /// A .resx key that can be used to localize the exception's message.
        /// Note that this message should be displayed to the application users and therefore can or should be localized
        /// if the application requires it. This message should not contain any informations that is irrelevant or
        /// confusing for the application users.
        /// </param>
        /// <param name="developerMessage">
        /// The exception's message.
        /// Note that this message is not localized and should not be displayed to the application users.
        /// This message is meant to contain developer relevant informations and can be used for logging purposes.
        /// </param>
        public BusinessAggregateException(
            string userMessageResourceKey,
            string developerMessage = "An unexpected business aggregate exception has occured.")
            : base(string.IsNullOrEmpty(userMessageResourceKey) ? DefaultResxKey : userMessageResourceKey, developerMessage)
        {
            InnerExceptions = new ReadonlyCollection<Exception>(null);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="TechnicalAggregateException"/> type.
        /// </summary>
        /// <param name="userMessageResourceKey">
        /// A .resx key that can be used to localize the exception's message.
        /// Note that this message should be displayed to the application users and therefore can or should be localized
        /// if the application requires it. This message should not contain any informations that is irrelevant or
        /// confusing for the application users.
        /// </param>
        /// <param name="innerExceptions"> The exceptions that have caused this exception to be thrown. </param>
        /// <param name="developerMessage">
        /// The exception's message.
        /// Note that this message is not localized and should not be displayed to the application users.
        /// This message is meant to contain developer relevant informations and can be used for logging purposes.
        /// </param>
        public BusinessAggregateException(
            string userMessageResourceKey,
            IEnumerable<Exception> innerExceptions,
            string developerMessage = "An unexpected business aggregate exception has occured.")
            : base(
                  string.IsNullOrEmpty(userMessageResourceKey) ? DefaultResxKey : userMessageResourceKey,
                  (innerExceptions?.Count() > 0) ? innerExceptions.First() : null,
                  developerMessage)
        {
            InnerExceptions = new ReadonlyCollection<Exception>(innerExceptions);
        }

        #endregion

        #region Data

        /// <summary>
        /// The .resx file key that represents a general default message for business aggregate exceptions.
        /// </summary>
        private const string DefaultResxKey = "ErrorBusinessAggregateDefaultMessage";

        /// <summary>
        /// Gets the exceptions that have caused this exception to be thrown.
        /// </summary>
        public ReadonlyCollection<Exception> InnerExceptions { get; }

        #endregion
    }
}