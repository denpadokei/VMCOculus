using System;
using System.Diagnostics;
using uOSC;

namespace UdpServer
{
    class Program
    {
        static uOscServer server;
        static void Main(string[] args)
        {
            server = new uOscServer();
            server.RecivedEvent += Server_RecivedEvent;
            
            while (!string.IsNullOrEmpty(Console.ReadLine())) ;

            server.RecivedEvent -= Server_RecivedEvent;
            server.Dispose();
        }

        private static void Server_RecivedEvent(object sender, uOsc.Scripts.ReciveMessageArgs e)
        {
            foreach (var item in e.Message.values) {
                Console.WriteLine($"{item}");
            }
        }
    }
}
