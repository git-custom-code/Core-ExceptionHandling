namespace CustomCode.Core.ExceptionHandling
{
    using System;

    /// <summary>
    /// Interface for exceptions that support localization.
    /// </summary>
    /// <remarks>
    /// Note that c# does not support multiple inheritance and does not allow using catch with interfaces.
    /// But starting from C# 6.0 you can use exception filtering to achive the same result.
    /// 
    /// try
    /// {
    ///     ...
    /// }
    /// catch(Exception e) when (e is ILocalizableException le)
    /// {
    ///     ...
    /// }
    /// </remarks>
    public interface ILocalizableException
    {
        /// <summary>
        /// Gets the exception's causing exceptions' contexts (see <see cref="ExceptionContext"/> for more details).
        /// </summary>
        ReadonlyCollection<ExceptionContext> CausingContexts { get; }

        /// <summary>
        /// Gets the exception's causing exceptions' contexts (see <see cref="ExceptionContext"/> for more details).
        /// </summary>
        ReadonlyCollection<Exception> CausingExceptions { get; }

        /// <summary>
        /// Gets the exception's context (see <see cref="ExceptionContext"/> for more details).
        /// </summary>
        ExceptionContext Context { get; }

        /// <summary>
        /// Gets a .resx key that can be used to localize the exception's message.
        /// </summary>
        string ResourceKey { get; }

        /// <summary>
        /// Convert exception data to an object array that can be used via <see cref="string.Format(string, object[])" /> for
        /// localization purposes.
        /// </summary>
        /// <returns> The exception's format items for localization or null. </returns>
        object[]? GetFormatItems();
    }
}