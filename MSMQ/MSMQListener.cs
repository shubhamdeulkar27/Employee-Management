using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Experimental.System.Messaging;

namespace MSMQ
{

    public delegate void MessageReceivedEventHandler(object sender, MessageEventArgs args);

    public class MSMQListener
    {

        SMTP smtpObject = new SMTP();

        private bool listen;
        MessageQueue messageQueue;

        public event MessageReceivedEventHandler MessageReceived;

        

        public MSMQListener(string queuePath)
        {
            messageQueue = new MessageQueue(queuePath);
        }

        public void Start()
        {
            listen = true;
            messageQueue.Formatter = new BinaryMessageFormatter();
            messageQueue.PeekCompleted += new PeekCompletedEventHandler(OnPeekCompleted);
            messageQueue.ReceiveCompleted += new ReceiveCompletedEventHandler(OnReceiveCompleted);
            StartListening();
        }

        public void Stop()
        {
            listen = false;
            messageQueue.PeekCompleted -= new PeekCompletedEventHandler(OnPeekCompleted);
            messageQueue.ReceiveCompleted -= new ReceiveCompletedEventHandler(OnReceiveCompleted);
        }

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
            catch (Exception e)
            {
                transaction.Abort();
            }
        }

        private void FireRecieveEvent(object body)
        {
            if (MessageReceived != null)
            {
                MessageReceived(this, new MessageEventArgs(body));
                string data = body.ToString();
                smtpObject.SendMail("Shubham","shubhamdeulkar@gmail.com",data);
            }
        }

        private void OnReceiveCompleted(object sender, ReceiveCompletedEventArgs args)
        {
            Message message = messageQueue.EndReceive(args.AsyncResult);

            StartListening();

            FireRecieveEvent(message.Body);
        }

    }

    public class MessageEventArgs : EventArgs
    {
        private object messageBody;

        private string name;
        private string mail;

        public object MessageBody
        {
            get { return messageBody; }
        }

        public string Email 
        {
            get { return name; }
        }

        public string UserName
        {
            get { return name; }
        }

        public MessageEventArgs(object body)
        {
            messageBody = body;
        }
    }

}
