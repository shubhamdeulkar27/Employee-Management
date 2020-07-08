using System;
using System.Collections.Generic;
using System.Text;

namespace MSMQ
{
    /// <summary>
    /// Class For Receiving Data From MSMQ.
    /// </summary>
    public class MessageListener
    {
        /// <summary>
        /// Main Method.
        /// </summary>
        public static void Main()
        {
            var listener = new MSMQListener(@".\Private$\messageq");
            listener.MessageReceived += new MessageReceivedEventHandler(listnerMessageReceived);
            listener.Start();
            Console.WriteLine("Read Message");
            Console.ReadLine();
            listener.Stop();
        }

        /// <summary>
        /// Event Handler For Data Received.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public static void listnerMessageReceived(object sender, MessageEventArgs args)
        {
            Console.WriteLine(args.MessageBody);
        }
    }
}
