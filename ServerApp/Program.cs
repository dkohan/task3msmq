using System;
using System.IO;
using System.Messaging;

namespace ServerApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Server started.");

            MessageQueue queue = new MessageQueue(".\\Private$\\epamtestqueue");
            queue.Formatter = new XmlMessageFormatter(new Type[] { typeof(byte[]) });

            queue.ReceiveCompleted += Queue_ReceiveCompleted; ;
            queue.BeginReceive();

            Console.ReadKey();

        }

        private static void Queue_ReceiveCompleted(object sender, ReceiveCompletedEventArgs e)
        {
            MessageQueue mq = (MessageQueue)sender;
            if (mq != null)
            {
                var message = e.Message;

                var fileName = message.Label;
                var content = (byte[])message.Body;

                using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(content, 0, content.Length);
                }

                Console.WriteLine("File:" + fileName + "is created!");
            }
        }
    }
}
