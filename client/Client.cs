using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using client;
using MyClient;
using server;
using ServerCore;

namespace Client
{
    class chat_Client
    {

        static void cliect_chat_action(Socket serverSocket)
        {
            try
            {

                // ���� ���� �� ����
                Console.WriteLine($"Connected Server, {serverSocket.RemoteEndPoint.ToString()}");

                // �������� �۽�
                byte[] sendBuffer = Encoding.UTF8.GetBytes("Hello Server!");
                int sendBytes = serverSocket.Send(sendBuffer);

                // ���� ���� ����
                byte[] recvBuffer = new byte[1024];
                int recvByte = serverSocket.Receive(recvBuffer);
                string recvData = Encoding.UTF8.GetString(recvBuffer, 0, recvByte);
                Console.WriteLine($"���� ���� �޽���: {recvData}");

                // Ŭ���̾�Ʈ ���� ����
                serverSocket.Shutdown(SocketShutdown.Both); // ���� ���� ����
                serverSocket.Close(); // ���� ����

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }


        static void Main(string[] args)
        {
            //DNS (Domain Name System) ���
            string host = Dns.GetHostName();   // ���� ���� ȣ��Ʈ ����
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777); //endPoint - ip, port �ּ�


            Connector connector = new Connector();
            connector.Connect(endPoint, () => { return SessionManager.Instance.Generate(); });

            while (true) 
            {
                //Console.WriteLine("contents�� �Է����ּ���.");
                //string msg = Console.ReadLine();

                try
                {
                    SessionManager.Instance.SendForEach();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }

                Thread.Sleep(500);
            } // ���� ���α׷��� ������� �ʵ��� ����


        }
    }
}
