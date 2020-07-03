using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLayer
{
    /// <summary>
    /// Class For Handelling Custom Exception.
    /// </summary>
    class CustomExceptions : Exception 
    {
        /// <summary>
        /// Enum For Custom Exceptions.
        /// </summary>
        public enum ExceptionType 
        {
            NULL_FIELD_EXCEPTION,
            INVALID_FIELD_EXCEPTION
        }

        /// <summary>
        /// Enum Reference For Exception Type.
        /// </summary>
        ExceptionType type;

        /// <summary>
        /// Constructor For Setting Exception Type.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="message"></param>
        public CustomExceptions(CustomExceptions.ExceptionType type, string message):base(message)
        {
            this.type = type;
        }

    }
}
