using System;
using System.Collections.Generic;
using System.Text;
using uOSC;

namespace UdpServer.uOsc.Scripts
{
    public class ReciveMessageArgs : EventArgs
    {
        public Message Message { get; }

        public ReciveMessageArgs(Message message)
        {
            this.Message = message;
        }
    }

    public delegate void ReciveMessageHandler(object sender, ReciveMessageArgs e);
}
