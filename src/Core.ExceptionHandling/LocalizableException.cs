namespace CustomCode.Core.ExceptionHandling
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// Base type for all exceptions that need localization (i.e. for displaying exception messages directly inside of the ui).
    /// This type will provide easy access to the exception's causing context and context
    /// as well as the innermost exception that started the exception chain.
    /// </summary>
    public abstract class LocalizableException : Exception, ILocalizableException
    {
        #region Dependencies

        /// <summary>
        /// Creates a new instance of the <see cref="LocalizableException"/> type.
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
        protected LocalizableException(string developerMessage, string userMessageResourceKey = DefaultResxKey)
            : base(developerMessage ?? $"Localizable exception with ResourceKey: {userMessageResourceKey}")
        {
            if (string.IsNullOrWhiteSpace(userMessageResourceKey))
            {
                userMessageResourceKey = DefaultResxKey;
            }

            _causingContexts = new Lazy<ReadonlyCollection<ExceptionContext>>(() =>
                new ReadonlyCollection<ExceptionContext>(this.GetCausingContexts()));
            _causingExceptions = new Lazy<ReadonlyCollection<Exception>>(() =>
                new ReadonlyCollection<Exception>(this.GetCausingExceptions()));
            _context = new Lazy<ExceptionContext>(() => this.GetContext());
            ResourceKey = userMessageResourceKey;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="LocalizableException"/> type.
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
        protected LocalizableException(Exception innerException, string developerMessage, string userMessageResourceKey = DefaultResxKey)
            : base(developerMessage ?? $"Localizable exception with ResourceKey: {userMessageResourceKey}", innerException)
        {
            if (string.IsNullOrWhiteSpace(userMessageResourceKey))
            {
                userMessageResourceKey = DefaultResxKey;
            }

            _causingContexts = new Lazy<ReadonlyCollection<ExceptionContext>>(() =>
                new ReadonlyCollection<ExceptionContext>(this.GetCausingContexts()));
            _causingExceptions = new Lazy<ReadonlyCollection<Exception>>(() =>
                new ReadonlyCollection<Exception>(this.GetCausingExceptions()));
            _context = new Lazy<ExceptionContext>(() => this.GetContext());
            ResourceKey = userMessageResourceKey;
        }

        #endregion

        #region Data

        #region CausingContexts

        /// <summary>
        /// Gets the exception's causing exceptions' contexts (see <see cref="ExceptionContext"/> for more details).
        /// </summary>
        public ReadonlyCollection<ExceptionContext> CausingContexts
        {
            get { return _causingContexts.Value; }
        }

        /// <summary> The exception's causing exceptions' contexts (see <see cref="ExceptionContext"/> for more details). </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Lazy<ReadonlyCollection<ExceptionContext>> _causingContexts;

        #endregion

        #region CausingExceptions

        /// <summary>
        /// Gets the exception's causing exceptions' contexts (see <see cref="ExceptionContext"/> for more details).
        /// </summary>
        public ReadonlyCollection<Exception> CausingExceptions
        {
            get { return _causingExceptions.Value; }
        }

        /// <summary> The exception's causing exceptions' contexts (see <see cref="ExceptionContext"/> for more details). </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Lazy<ReadonlyCollection<Exception>> _causingExceptions;

        #endregion

        #region Context

        /// <summary>
        /// Gets the exception's context (see <see cref="ExceptionContext"/> for more details).
        /// </summary>
        public ExceptionContext Context
        {
            get { return _context.Value; }
        }

        /// <summary> The exception's context (see <see cref="ExceptionContext"/> for more details). </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Lazy<ExceptionContext> _context;

        #endregion

        /// <summary>
        /// The .resx file key that represents a general default exception message.
        /// </summary>
        private const string DefaultResxKey = "ErrorDefaultMessage";

        /// <summary>
        /// Gets a .resx key that can be used to localize the exception's message.
        /// </summary>
        public string ResourceKey { get; }

        #endregion

        #region Logic

        /// <summary>
        /// Convert exception data to an object array that can be used via <see cref="string.Format(string, object[])" /> for
        /// localization purposes.
        /// </summary>
        /// <returns> The exception's format items for localization or null. </returns>
        public abstract object[] GetFormatItems();

        /// <summary>
        /// Converts this instance to a human readable string representation.
        /// </summary>
        /// <returns> A human readable string representation of this instance. </returns>
        public override string ToString()
        {
            var rn = Environment.NewLine;
            return $"Type: {GetType().Name}{rn}Message: \"{Message}\"{rn}StackTrace:{rn}{StackTrace}";
        }

        #endregion
    }
}