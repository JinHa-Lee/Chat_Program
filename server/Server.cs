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

        static void chat_action(Socket clientSocket)
        {
            try
            {
                // 클라이언트 정보 수신
                byte[] recvBuffer = new byte[1024];
                int recvByte = clientSocket.Receive(recvBuffer);
                string recvData = Encoding.UTF8.GetString(recvBuffer, 0, recvByte);
                Console.WriteLine($"클라이언트 수신 메시지: {recvData}");

                // 클라이언트에게 송신
                byte[] sendBuffer = Encoding.UTF8.GetBytes("Complete connect Server.");
                clientSocket.Send(sendBuffer);

                // 클라이언트 연결 해제
                clientSocket.Shutdown(SocketShutdown.Both); // 연결 해제 예고
                clientSocket.Close(); // 연결 해제
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }


        static void Main(string[] args)
        {
            //DNS (Domain Name System) 사용
            string host = Dns.GetHostName();   // 현재 로컬 호스트 저장
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777); //endPoint - ip, port 주소

            _listener.Init(endPoint, chat_action);
            Console.WriteLine("Listening...");

            while (true) { } // 메인 프로그램이 종료되지 않도록 유지

        }
    }
}
