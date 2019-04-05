namespace CustomCode.Core.ExceptionHandling
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    /// <summary>
    /// A readonly collection whose main purpose is to override the <see cref="ToString"/>
    /// method to provide an improved debug experience for developers.
    /// </summary>
    /// <typeparam name="T"> The type of the collection's data elements. </typeparam>
    public sealed class ReadonlyCollection<T> : IEnumerable<T>
    {
        #region Dependencies

        /// <summary>
        /// Creates a new instance of the <see cref="ReadonlyCollection{T}"/> type.
        /// </summary>
        /// <param name="data"> The collection's readonly data. </param>
        public ReadonlyCollection(IEnumerable<T>? data)
        {
            Data = data ?? Enumerable.Empty<T>();
        }

        #endregion

        #region Data

        /// <summary>
        /// Gets the collection's readonly data.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        private IEnumerable<T> Data { get; }

        #endregion

        #region Logic

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns> An <see cref="IEnumerator{T}"/> object that can be used to iterate through the collection. </returns>
        public IEnumerator<T> GetEnumerator()
        {
            foreach (var item in Data)
            {
                yield return item;
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns> An <see cref="IEnumerator"/> object that can be used to iterate through the collection. </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Converts this instance to a human readable string representation.
        /// </summary>
        /// <returns> A human readable string representation of this instance. </returns>
        public override string ToString()
        {
            if (Data?.Count() == 1)
            {
                return Data.First()?.ToString() ?? "null";
            }

            return $"Count = {Data?.Count()}";
        }

        #endregion
    }
}