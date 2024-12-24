using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using MyClient;


class chat_client
{
    static void Main(string[] args)
    {
        ContinuousClient client = new ContinuousClient();
        client.Run();
    }
}