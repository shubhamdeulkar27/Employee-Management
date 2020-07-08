using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Experimental.System.Messaging;

namespace MSMQ
{
    /// <summary>
    /// Delegate For Event Handler.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    public delegate void MessageReceivedEventHandler(object sender, MessageEventArgs args);

    /// <summary>
    /// MSMQListener Helper Class.
    /// </summary>
    public class MSMQListener
    {
        //SMTP class object For Calling Send Email Function.
        SMTP smtpObject = new SMTP();

        //Variable.
        private bool listen;
        
        //MessageQueue.
        MessageQueue messageQueue;
        
        //Event.
        public event MessageReceivedEventHandler MessageReceived;

        /// <summary>
        /// Constructor For Setting Queue.
        /// </summary>
        /// <param name="queuePath"></param>
        public MSMQListener(string queuePath)
        {
            messageQueue = new MessageQueue(queuePath);
        }

        /// <summary>
        /// Function For Start Receiving Data From Queue.
        /// </summary>
        public void Start()
        {
            listen = true;
            messageQueue.Formatter = new BinaryMessageFormatter();
            messageQueue.PeekCompleted += new PeekCompletedEventHandler(OnPeekCompleted);
            messageQueue.ReceiveCompleted += new ReceiveCompletedEventHandler(OnReceiveCompleted);
            StartListening();
        }

        /// <summary>
        /// Function For Stop Receving Data From MSMQ.
        /// </summary>
        public void Stop()
        {
            listen = false;
            messageQueue.PeekCompleted -= new PeekCompletedEventHandler(OnPeekCompleted);
            messageQueue.ReceiveCompleted -= new ReceiveCompletedEventHandler(OnReceiveCompleted);
        }

        /// <summary>
        /// Function For Receving Data From MSMQ.
        /// </summary>
        private void StartListening()
        {
            if (!listen)
            {
                return;
            }

            if (messageQueue.Transactional)
            {
                messageQueue.BeginPeek();
            }
            else
            {
                messageQueue.BeginReceive();
            }
        }

        /// <summary>
        /// Event Handler For Peek Completed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnPeekCompleted(object sender, PeekCompletedEventArgs args)
        {
            messageQueue.EndPeek(args.AsyncResult);
            MessageQueueTransaction transaction = new MessageQueueTransaction();
            Message message = null;

            try
            {
                transaction.Begin();
                message = messageQueue.Receive(transaction);
                transaction.Commit();

                StartListening();
                FireRecieveEvent(message.Body);
            }
            catch 
            {
                transaction.Abort();
            }
        }

        /// <summary>
        /// Function For Terminate Receive Event and Sending Email.
        /// </summary>
        /// <param name="body"></param>
        private void FireRecieveEvent(object body)
        {
            if (MessageReceived != null)
            {
                MessageReceived(this, new MessageEventArgs(body));
                string data = body.ToString();
                smtpObject.SendMail("Shubham","shubhamdeulkar27@gmail.com",data);
            }
        }

        /// <summary>
        /// Event Handler for Receive Completion.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnReceiveCompleted(object sender, ReceiveCompletedEventArgs args)
        {
            Message message = messageQueue.EndReceive(args.AsyncResult);

            StartListening();

            FireRecieveEvent(message.Body);
        }

    }

    /// <summary>
    /// Class For Event Arguments.
    /// </summary>
    public class MessageEventArgs : EventArgs
    {
        private object messageBody;

        public object MessageBody
        {
            get { return messageBody; }
        }

        public MessageEventArgs(object body)
        {
            messageBody = body;
        }
    }

}
