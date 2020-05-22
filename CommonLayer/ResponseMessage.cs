using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLayer
{
    /// <summary>
    /// Class For Response Message.
    /// </summary>
    public class ResponseMessage<T>
    {
        //Properties For Response.
        public string Status { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}
