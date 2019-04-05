namespace CustomCode.Core.ExceptionHandling
{
    using System;

    /// <summary>
    /// Use this type (or derived types) whenever a technical exception has occured.
    /// </summary>
    /// <remarks>
    /// Note that all technical exceptions should be displayed to the user using a generalized
    /// "Unexpected Exception" message (hiding the real cause of the problem from the application users).
    /// </remarks>
    public class TechnicalException : LocalizableException, ITechnicalException
    {
        #region Dependencies

        /// <summary>
        /// Creates a new instance of the <see cref="TechnicalException"/> type.
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
        public TechnicalException(
            string developerMessage = "An unexpected technical exception has occured.",
            string userMessageResourceKey = DefaultResxKey)
            : base(developerMessage, string.IsNullOrWhiteSpace(userMessageResourceKey) ? DefaultResxKey : userMessageResourceKey)
        { }

        /// <summary>
        /// Creates a new instance of the <see cref="TechnicalException"/> type.
        /// </summary>
        /// <param name="innerException"> The exception that has caused this exception to be thrown. </param>
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
        public TechnicalException(
            Exception? innerException,
            string developerMessage = "An unexpected technical exception has occured.",
            string userMessageResourceKey = DefaultResxKey)
            : base(innerException, developerMessage, string.IsNullOrWhiteSpace(userMessageResourceKey) ? DefaultResxKey : userMessageResourceKey)
        { }

        #endregion

        #region Data

        /// <summary>
        /// The .resx file key that represents a general default message for technical exceptions.
        /// </summary>
        private const string DefaultResxKey = "ErrorTechnicalDefaultMessage";

        #endregion

        #region Logic

        /// <summary>
        /// Convert exception data to an object array that can be used via <see cref="string.Format(string, object[])"/> for
        /// localization purposes.
        /// </summary>
        /// <returns> The exception's format items for localization or null. </returns>
        public override object[]? GetFormatItems()
        {
            return null;
        }

        #endregion
    }
}