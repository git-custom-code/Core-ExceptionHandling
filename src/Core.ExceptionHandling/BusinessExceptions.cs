namespace CustomCode.Core.ExceptionHandling
{
    using System;

    /// <summary>
    /// Use this type (or derived types) whenever a business exception has occured.
    /// </summary>
    /// <remarks>
    /// Note that all business exceptions should contain a user friendly message text that can and
    /// should be displayed to the application users.
    /// </remarks>
    public class BusinessException : LocalizableException, IBusinessException
    {
        #region Dependencies

        /// <summary>
        /// Creates a new instance of the <see cref="BusinessException"/> type.
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
        public BusinessException(
            string userMessageResourceKey,
            string developerMessage = "An unexpected business exception has occured.")
            : base(developerMessage, string.IsNullOrEmpty(userMessageResourceKey) ? DefaultResxKey : userMessageResourceKey)
        { }

        /// <summary>
        /// Creates a new instance of the <see cref="TechnicalException"/> type.
        /// </summary>
        /// <param name="userMessageResourceKey">
        /// A .resx key that can be used to localize the exception's message.
        /// Note that this message should be displayed to the application users and therefore can or should be localized
        /// if the application requires it. This message should not contain any informations that is irrelevant or
        /// confusing for the application users.
        /// </param>
        /// <param name="innerException"> The exception that has caused this exception to be thrown. </param>
        /// <param name="developerMessage">
        /// The exception's message.
        /// Note that this message is not localized and should not be displayed to the application users.
        /// This message is meant to contain developer relevant informations and can be used for logging purposes.
        /// </param>
        public BusinessException(
            string userMessageResourceKey,
            Exception innerException,
            string developerMessage = "An unexpected business exception has occured.")
            : base(innerException, developerMessage, string.IsNullOrEmpty(userMessageResourceKey) ? DefaultResxKey : userMessageResourceKey)
        { }

        #endregion

        #region Data

        /// <summary>
        /// The .resx file key that represents a general default message for business exceptions.
        /// </summary>
        private const string DefaultResxKey = "ErrorBusinessDefaultMessage";

        #endregion

        #region Logic

        /// <summary>
        /// Convert exception data to an object array that can be used via <see cref="string.Format(string, object[])"/> for
        /// localization purposes.
        /// </summary>
        /// <returns> The exception's format items for localization or null. </returns>
        public override object[] GetFormatItems()
        {
            return null;
        }

        #endregion
    }
}