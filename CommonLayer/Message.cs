using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLayer
{
    /// <summary>
    /// Class For Response Message.
    /// </summary>
    public class Message
    {
        //Properties For Response.
        public string Status { get; set; }
        public string ResponseMessage { get; set; }
        public string Data { get; set; }
    }
}
