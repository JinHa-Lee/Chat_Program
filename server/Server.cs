using server;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    class chat_Server
    {
        static Listener _listener = new Listener();
        public static Room Room = new Room();

        static void Main(string[] args)
        {
            //DNS (Domain Name System) 사용
            string host = Dns.GetHostName();   // 현재 로컬 호스트 저장
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777); //endPoint - ip, port 주소

            _listener.Init(endPoint, () => { return SessionManager.Instance.Generate(); });
            Console.WriteLine("Listening...");

            while (true) { } // 메인 프로그램이 종료되지 않도록 유지

        }
    }
}
