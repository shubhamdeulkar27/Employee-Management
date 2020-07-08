using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Experimental.System.Messaging;

namespace EmployeeManagement
{
    /// <summary>
    /// Sender Class For Sending Message into MSMQ.
    /// </summary>
    public class Sender
    {
        /// <summary>
        /// Send Method For Sending Data into MSMQ.
        /// </summary>
        /// <param name="input"></param>
        public void Send(string input)
        {
            MessageQueue messageQ;

            //If Queue Does not Exists it will create a Queue in MSMQ.
            if (MessageQueue.Exists(@".\Private$\messageq"))
            {
                messageQ = new MessageQueue(@".\Private$\messageq");
            }
            else
            {
                messageQ = MessageQueue.Create(@".\Private$\messageq");
            }

            //Creating Message and Sending it to MSMQ.
            Message message = new Message();
            message.Formatter = new BinaryMessageFormatter();
            message.Body = input;
            message.Label = "Registration";
            message.Priority = MessagePriority.Normal;
            messageQ.Send(message);
        }
    }
}
