using System;
using UdpServer.uOsc.Scripts;

namespace uOSC
{

    public class uOscServer : IDisposable
    {
        int port = 39539;

#if NETFX_CORE
    Udp udp_ = new Uwp.Udp();
    Thread thread_ = new Uwp.Thread();
#else
        Udp udp_ = new DotNet.Udp();
        Thread thread_ = new DotNet.Thread();
#endif
        Parser parser_ = new Parser();
        private bool disposedValue;

        public event ReciveMessageHandler RecivedEvent;

        public uOscServer()
        {
            udp_.StartServer(port);
            thread_.Start(UpdateMessage);
        }

        void UpdateMessage()
        {
            while (udp_.messageCount > 0) {
                var buf = udp_.Receive();
                int pos = 0;
                parser_.Parse(buf, ref pos, buf.Length);
            }

            while (parser_.messageCount > 0) {
                var message = parser_.Dequeue();
                this.RecivedEvent?.Invoke(this, new ReciveMessageArgs(message));
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue) {
                if (disposing) {
                    // TODO: マネージド状態を破棄します (マネージド オブジェクト)
                    thread_.Stop();
                    udp_.Stop();
                }

                // TODO: アンマネージド リソース (アンマネージド オブジェクト) を解放し、ファイナライザーをオーバーライドします
                // TODO: 大きなフィールドを null に設定します
                disposedValue = true;
            }
        }

        // // TODO: 'Dispose(bool disposing)' にアンマネージド リソースを解放するコードが含まれる場合にのみ、ファイナライザーをオーバーライドします
        // ~uOscServer()
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