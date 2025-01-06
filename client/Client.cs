using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using MyClient;

namespace Client
{
    class chat_Client
    {
        static void Main(string[] args)
        {
            //DNS (Domain Name System) 사용
            string host = Dns.GetHostName();   // 현재 로컬 호스트 저장
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777); //endPoint - ip, port 주소

            // 소켓 설정
            Socket socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                // 서버 연결 시도
                socket.Connect(endPoint);

                // 연결 성공 후 진행
                Console.WriteLine($"Connected Server, {socket.RemoteEndPoint.ToString()}");

                // 서버에게 송신
                byte[] sendBuffer = Encoding.UTF8.GetBytes("Hello Server!");
                int sendBytes = socket.Send(sendBuffer);

                // 서버 정보 수신
                byte[] recvBuffer = new byte[1024];
                int recvByte = socket.Receive(recvBuffer);
                string recvData = Encoding.UTF8.GetString(recvBuffer, 0, recvByte);
                Console.WriteLine($"서버 수신 메시지: {recvData}");

                // 클라이언트 연결 해제
                socket.Shutdown(SocketShutdown.Both); // 연결 해제 예고
                socket.Close(); // 연결 해제



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }
    }
}
