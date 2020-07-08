using System;
using System.Collections.Generic;
using System.Text;

namespace MSMQ
{
    public class MessageListener
    {
        public static void Main()
        {
            var listener = new MSMQListener(@".\Private$\messageq");
            listener.MessageReceived += new MessageReceivedEventHandler(listnerMessageReceived);
            listener.Start();
            Console.WriteLine("Read Message");
            Console.ReadLine();
            listener.Stop();
        }

        public static void listnerMessageReceived(object sender, MessageEventArgs args)
        {
            Console.WriteLine(args.MessageBody);
        }
    }
}
