using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using client;
using MyClient;
using ServerCore;

namespace Client
{
    class Packet
    {
        public ushort size;
        public ushort packetId;
    }

    class PlayerInfo : Packet
    {
        public long playerId;
    }

    class PacketValue : Packet
    {
        public int hp;
        public int value;
    }

    public enum PacketID
    {
        Unknown = 0,
        PlayerInfo = 1,
        PlayerValue = 2,
    }

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
            ServerSession session = new ServerSession();
            connector.Connect(endPoint, () => { return session; });

            while (true) 
            {
                Console.WriteLine("���ڸ� �Է����ּ���.");
                string msg = Console.ReadLine();
                long input;
                try
                {
                    input = long.Parse(msg);
                }
                catch
                {
                    Console.WriteLine("�߸��� �Է��Դϴ�.");
                    continue;
                }
                PlayerInfo packet = new PlayerInfo() { size = 12, packetId = (ushort)PacketID.PlayerInfo, playerId = input };
                ArraySegment<byte> segment = SendBufferHelper.Open(4096);
                byte[] sizeBuffer = BitConverter.GetBytes(packet.size);
                byte[] packetIdBuffer = BitConverter.GetBytes(packet.packetId);
                byte[] inputBuffer = BitConverter.GetBytes(packet.playerId);

                ushort count = 0;
                bool success = true;
                success = BitConverter.TryWriteBytes(new Span<byte>(segment.Array, segment.Offset, segment.Count), packet.size);
                //Array.Copy(sizeBuffer, 0, segment.Array, segment.Offset + count, sizeBuffer.Length);
                count += sizeof(ushort);
                success = BitConverter.TryWriteBytes(new Span<byte>(segment.Array, segment.Offset + count, segment.Count - count), packet.packetId);
                //Array.Copy(packetIdBuffer, 0, segment.Array, segment.Offset + count, packetIdBuffer.Length);
                count += sizeof(ushort);
                success = BitConverter.TryWriteBytes(new Span<byte>(segment.Array, segment.Offset + count, segment.Count - count), packet.playerId);
                //Array.Copy(inputBuffer, 0, segment.Array, segment.Offset + count, inputBuffer.Length);
                count += sizeof(long);
                ArraySegment<byte> sendBuffer = SendBufferHelper.Close(count);

                if (success)
                    session.Send(sendBuffer);
            } // ���� ���α׷��� ������� �ʵ��� ����


        }
    }
}
