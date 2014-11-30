using System;
using System.Runtime.Serialization;

namespace SharpThings {
    /// <summary>
    /// The exception that is thrown when an assertion fails.
    /// </summary>
    [Serializable]
    public sealed class AssertionFailedException : Exception {
        /// <summary>
        /// Initializes a new instance of the <see cref="AssertionFailedException"/> class.
        /// </summary>
        public AssertionFailedException () {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssertionFailedException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public AssertionFailedException (string message)
            : base(message) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssertionFailedException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public AssertionFailedException (string message, Exception innerException)
            : base(message, innerException) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssertionFailedException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="info"/> parameter is null. </exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"/> is zero (0). </exception>
        protected AssertionFailedException (SerializationInfo info, StreamingContext context)
            : base(info, context) {
        }
    }
}
