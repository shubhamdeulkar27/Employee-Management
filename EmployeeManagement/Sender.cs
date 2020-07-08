using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Experimental.System.Messaging;

namespace EmployeeManagement
{
    public class Sender
    {
        public void Send(string input)
        {
            MessageQueue messageQ;

            if (MessageQueue.Exists(@".\Private$\messageq"))
            {
                messageQ = new MessageQueue(@".\Private$\messageq");
            }
            else
            {
                messageQ = MessageQueue.Create(@".\Private$\messageq");
            }

            Message message = new Message();
            message.Formatter = new BinaryMessageFormatter();
            message.Body = input;
            message.Label = "Registration";
            message.Priority = MessagePriority.Normal;
            messageQ.Send(message);
        }
    }
}
