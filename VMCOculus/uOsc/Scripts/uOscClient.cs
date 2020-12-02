using System;
using System.Collections.Concurrent;
using System.IO;
using UnityEngine;

namespace uOSC
{

    public class uOscClient : IDisposable
    {
        private const int BufferSize = 8192;
        private const int MaxQueueSize = 100;
        string address = "127.0.0.1";
        int port = 39540;

        public uOscClient()
        {
            this.OnEnable();
        }

#if NETFX_CORE
    Udp udp_ = new Uwp.Udp();
    Thread thread_ = new Uwp.Thread();
#else
        Udp udp_ = new DotNet.Udp();
        Thread thread_ = new DotNet.Thread();
#endif
        ConcurrentQueue<object> messages_ = new ConcurrentQueue<object>();
        private bool disposedValue;

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

        void Enqueue(object data)
        {
            messages_.Enqueue(data);
            while (messages_.Count > MaxQueueSize) {
                messages_.TryDequeue(out _);
            }
        }


        public void Enqueue(string address, params object[] values)
        {
            this.Enqueue(new Message()
            {
                address = address,
                values = values
            });
        }

        public void Enqueue(Message message)
        {
            this.Enqueue((object)message);
        }

        public void Enqueue(Bundle bundle)
        {
            this.Enqueue((object)bundle);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue) {
                if (disposing) {
                    // TODO: マネージド状態を破棄します (マネージド オブジェクト)
                    this.OnDisable();
                }

                // TODO: アンマネージド リソース (アンマネージド オブジェクト) を解放し、ファイナライザーをオーバーライドします
                // TODO: 大きなフィールドを null に設定します
                disposedValue = true;
            }
        }

        // // TODO: 'Dispose(bool disposing)' にアンマネージド リソースを解放するコードが含まれる場合にのみ、ファイナライザーをオーバーライドします
        // ~uOscClient()
        // {
        //     // このコードを変更しないでください。クリーンアップ コードを 'Dispose(bool disposing)' メソッドに記述します
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // このコードを変更しないでください。クリーンアップ コードを 'Dispose(bool disposing)' メソッドに記述します
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}