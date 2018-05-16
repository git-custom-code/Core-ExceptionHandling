namespace CustomCode.Core.ExceptionHandling
{
    using System;

    /// <summary>
    /// Interface for exceptions that aggregate multiple other exceptions (usually from multiple asynchronous tasks).
    /// </summary>
    /// <remarks>
    /// Note that c# does not support multiple inheritance and does not allow using catch with interfaces.
    /// But starting from C# 6.0 you can use exception filtering to achive the same result.
    /// 
    /// try
    /// {
    ///     ...
    /// }
    /// catch(Exception e) when (e is IAggregateException ae)
    /// {
    ///     ...
    /// }
    /// </remarks>
    public interface IAggregateException : ILocalizableException
    {
        /// <summary>
        /// Gets the exceptions that have caused this exception to be thrown.
        /// </summary>
        ReadonlyCollection<Exception> InnerExceptions { get; }
    }
}