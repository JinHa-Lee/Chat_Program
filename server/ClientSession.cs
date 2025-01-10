using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace server
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
    internal class ClientSession : PacketSession
    {
        public override void OnConnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnConnected : {endPoint}");

        }

        public override void OnDisconnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnDisconnected : {endPoint}");
        }

        public override void OnRecvPacket(ArraySegment<byte> buffer)
        {
            ushort count = 0;
            ushort size = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
            count += sizeof(ushort);
            ushort packetId = BitConverter.ToUInt16(buffer.Array, buffer.Offset + count);
            count += sizeof(ushort);

            switch((PacketID)packetId)
            {
                case PacketID.Unknown:
                    break;
                case PacketID.PlayerInfo:
                    long PlayerId = BitConverter.ToInt64(buffer.Array, buffer.Offset + count);
                    count += sizeof(long);
                    Console.WriteLine($"PacketId : {packetId} \nPacketSize : {size} \nPlayerId : {PlayerId}");
                    break;
                case PacketID.PlayerValue:
                    break;
            }

        }

        public override void OnSend(int numOfBytes)
        {
            Console.WriteLine($"Transferred bytes : {numOfBytes}");
        }
    }
}
