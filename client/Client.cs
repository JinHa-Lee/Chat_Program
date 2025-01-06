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
            //DNS (Domain Name System) ���
            string host = Dns.GetHostName();   // ���� ���� ȣ��Ʈ ����
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777); //endPoint - ip, port �ּ�

            // ���� ����
            Socket socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                // ���� ���� �õ�
                socket.Connect(endPoint);

                // ���� ���� �� ����
                Console.WriteLine($"Connected Server, {socket.RemoteEndPoint.ToString()}");

                // �������� �۽�
                byte[] sendBuffer = Encoding.UTF8.GetBytes("Hello Server!");
                int sendBytes = socket.Send(sendBuffer);

                // ���� ���� ����
                byte[] recvBuffer = new byte[1024];
                int recvByte = socket.Receive(recvBuffer);
                string recvData = Encoding.UTF8.GetString(recvBuffer, 0, recvByte);
                Console.WriteLine($"���� ���� �޽���: {recvData}");

                // Ŭ���̾�Ʈ ���� ����
                socket.Shutdown(SocketShutdown.Both); // ���� ���� ����
                socket.Close(); // ���� ����



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }
    }
}
