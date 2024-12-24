using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MyClient
{
    public class ContinuousClient
    {
        TcpClient client = null;
        public void Run()
        {
            while (true)
            {
                Console.WriteLine("======= 클라이언트 콘솔창. ========");
                Console.WriteLine("1.서버연결");
                Console.WriteLine("2.Message 보내기");
                Console.WriteLine("===================================");

                string key = Console.ReadLine();
                int order = 0;

                // 입력받은 key의 값을 int형으로 변환
                // 변환을 성공하면 order를 이용하여 기능 작동
                // 실패하면 false를 반환하여 else 문 작동

                if (int.TryParse(key, out order))
                {
                    switch (order)
                    {
                        case 1:
                        {
                            if(client != null)
                                {
                                    Console.WriteLine("이미 서버와 연결되어있습니다.");
                                    Console.ReadKey();
                                }
                            else
                                {
                                    Connect();
                                }
                            break;
                        }
                        case 2:
                        {
                            if (client == null)
                            {
                                Console.WriteLine("서버와 연결해주시기 바랍니다.");
                                Console.ReadKey();
                            }
                            else
                            {
                                SendMessage();
                            }
                            break;
                        }
                    }
                
                }
                else
                {
                    Console.WriteLine("잘못 입력하셨습니다.");
                    Console.ReadKey();
                }
                Console.Clear();
            }
        }
           

        private void SendMessage()
        {
            Console.WriteLine("보낼 message를 입력해주세요.");
            string msg = Console.ReadLine();
            byte[] byteData = new byte[msg.Length];
            byteData = Encoding.Default.GetBytes(msg);


            client.GetStream().Write(byteData, 0, byteData.Length);
            Console.WriteLine("전송 성공");
            Console.ReadKey();
        }

        private void Connect()
        {
            client = new TcpClient();
            client.Connect("127.0.0.1", 9999);
            Console.WriteLine("서버연결 성공 이제 Message를 입력해주세요.");
            Console.ReadKey();
        }
    }
}


