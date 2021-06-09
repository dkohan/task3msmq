using System;
using System.IO;
using System.Messaging;

namespace ClientApp
{
    class Program
    {
        
        static void Main(string[] args)
        {
            FileSystemWatcher watcher = new FileSystemWatcher(@"C:\\FilesForMessage");

            watcher.EnableRaisingEvents = true;
            watcher.Created += Watcher_Created;

            Console.ReadKey();
        }

        private static void Watcher_Created(object sender, FileSystemEventArgs e)
        {
            MessageQueue msgQ = new MessageQueue(".\\Private$\\epamtestqueue");

            string value = $"Created: {e.FullPath}";
            Console.WriteLine(value);

            byte[] bytes = File.ReadAllBytes(e.FullPath);

            Message msg = new Message();
            msg.Label = e.Name;
            msg.Body = bytes;  
            
            msgQ.Send(msg);
        }
    }

    public class CustomMessage
    {
        public string Name { get; set; }
        public byte[] Content { get; set; }
    }
}
