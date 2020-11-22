using System;
using uOSC;

namespace UdpServer
{
    class Program : IDisposable
    {
        static uOscServer server;
        private bool disposedValue;

        static void Main(string[] args)
        {
            server = new uOscServer();
            server.RecivedEvent += Server_RecivedEvent;

            while (true) ;
        }

        private static void Server_RecivedEvent(object sender, uOsc.Scripts.ReciveMessageArgs e)
        {
            foreach (var item in e.Message.values) {
                Console.WriteLine($"{item}");
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue) {
                if (disposing) {
                    // TODO: マネージド状態を破棄します (マネージド オブジェクト)
                    server.RecivedEvent -= Server_RecivedEvent;
                    server.Dispose();
                }

                // TODO: アンマネージド リソース (アンマネージド オブジェクト) を解放し、ファイナライザーをオーバーライドします
                // TODO: 大きなフィールドを null に設定します
                disposedValue = true;
            }
        }

        // // TODO: 'Dispose(bool disposing)' にアンマネージド リソースを解放するコードが含まれる場合にのみ、ファイナライザーをオーバーライドします
        // ~Program()
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
