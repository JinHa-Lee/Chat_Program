using System.Net;
using client;
using server;

namespace Client
{
    class chat_Client
    {

        static void Main_test(string[] args)
        {
            //DNS (Domain Name System) ���
            string host = Dns.GetHostName();   // ���� ���� ȣ��Ʈ ����
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777); //endPoint - ip, port �ּ�


            Connector connector = new Connector();
            connector.Connect(endPoint, () => { return SessionManager.Instance.Generate(); });
            Console.WriteLine("PlayerName�� �Է����ּ���.");
            string input = Console.ReadLine();
            SessionManager.Instance.SetPlayerName(input);

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
