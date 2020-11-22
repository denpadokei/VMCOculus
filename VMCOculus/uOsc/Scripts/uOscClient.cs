using System.Collections.Concurrent;
using System.IO;
using UnityEngine;

namespace uOSC
{

    public class uOscClient : MonoBehaviour
    {
        private const int BufferSize = 8192;
        private const int MaxQueueSize = 100;

        [SerializeField]
        string address = "192.168.3.3";

        [SerializeField]
        int port = 39540;

#if NETFX_CORE
    Udp udp_ = new Uwp.Udp();
    Thread thread_ = new Uwp.Thread();
#else
        Udp udp_ = new DotNet.Udp();
        Thread thread_ = new DotNet.Thread();
#endif
        ConcurrentQueue<object> messages_ = new ConcurrentQueue<object>();
        void OnEnable()
        {
            udp_.StartClient(address, port);
            thread_.Start(UpdateSend);
        }

        void OnDisable()
        {
            thread_.Stop();
            udp_.Stop();
        }

        void UpdateSend()
        {
            while (messages_.TryDequeue(out var message)) {
                using (var stream = new MemoryStream(BufferSize)) {
                    if (message is Message) {
                        ((Message)message).Write(stream);
                    }
                    else if (message is Bundle) {
                        ((Bundle)message).Write(stream);
                    }
                    else {
                        return;
                    }
                    udp_.Send(Util.GetBuffer(stream), (int)stream.Position);
                }
            }
        }

        void Add(object data)
        {
            messages_.Enqueue(data);
            while (messages_.Count > MaxQueueSize) {
                messages_.TryDequeue(out _);
            }
        }


        public void Send(string address, params object[] values)
        {
            Send(new Message()
            {
                address = address,
                values = values
            });
        }

        public void Send(Message message)
        {
            Add(message);
        }

        public void Send(Bundle bundle)
        {
            Add(bundle);
        }
    }
}