using System.Net;
using client;
using server;

namespace Client
{
    class chat_Client
    {

        static void Main_test(string[] args)
        {
            //DNS (Domain Name System) 사용
            string host = Dns.GetHostName();   // 현재 로컬 호스트 저장
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777); //endPoint - ip, port 주소


            Connector connector = new Connector();
            connector.Connect(endPoint, () => { return SessionManager.Instance.Generate(); });
            Console.WriteLine("PlayerName를 입력해주세요.");
            string input = Console.ReadLine();
            SessionManager.Instance.SetPlayerName(input);

            while (true) 
            {
                //Console.WriteLine("contents를 입력해주세요.");
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
            } // 메인 프로그램이 종료되지 않도록 유지


        }
    }
}
