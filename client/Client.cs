using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using client;
using MyClient;

namespace Client
{
    class chat_Client
    {

        static void cliect_chat_action(Socket serverSocket)
        {
            try
            {

                // 연결 성공 후 진행
                Console.WriteLine($"Connected Server, {serverSocket.RemoteEndPoint.ToString()}");

                // 서버에게 송신
                byte[] sendBuffer = Encoding.UTF8.GetBytes("Hello Server!");
                int sendBytes = serverSocket.Send(sendBuffer);

                // 서버 정보 수신
                byte[] recvBuffer = new byte[1024];
                int recvByte = serverSocket.Receive(recvBuffer);
                string recvData = Encoding.UTF8.GetString(recvBuffer, 0, recvByte);
                Console.WriteLine($"서버 수신 메시지: {recvData}");

                // 클라이언트 연결 해제
                serverSocket.Shutdown(SocketShutdown.Both); // 연결 해제 예고
                serverSocket.Close(); // 연결 해제

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


            Connector connector = new Connector();
            ServerSession session = new ServerSession();
            connector.Connect(endPoint, () => { return session; });

            while (true) 
            {
                Console.WriteLine("보낼 message를 입력해주세요.");
                string msg = Console.ReadLine();
                byte[] sendBuffer = Encoding.UTF8.GetBytes(msg);
                session.Send(sendBuffer);
            } // 메인 프로그램이 종료되지 않도록 유지


        }
    }
}
