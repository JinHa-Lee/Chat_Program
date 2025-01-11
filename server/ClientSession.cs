using Server;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace server
{
    abstract class Packet
    {
        public ushort size;
        public ushort packetId;

        public abstract ArraySegment<byte> Write();
        public abstract void Read(ArraySegment<byte> segment);
    }

    class PlayerInfo : Packet
    {
        // 패킷 정보 구조화

        public long playerId;
        public string playerName;

        public PlayerInfo()
        {
            this.packetId = (ushort)PacketID.PlayerInfo;
        }


        public override void Read(ArraySegment<byte> segment)
        {

            ReadOnlySpan<byte> s = new ReadOnlySpan<byte>(segment.Array, segment.Offset, segment.Count);

            // 패킷의 내용을 읽는 부분에서 size와 packetId는 굳이 추출하지 않는다
            ushort count = 0;
            count += sizeof(ushort); // size 크기
            count += sizeof(ushort); // packetId 크기

            this.playerId = BitConverter.ToInt64(s.Slice(count, s.Length - count));
            count += sizeof(long);

            // string
            ushort strlen = BitConverter.ToUInt16(s.Slice(count, s.Length - count));
            count += sizeof(ushort);
            this.playerName = Encoding.Unicode.GetString(s.Slice(count, strlen));

        }

        public override ArraySegment<byte> Write()
        {
            ArraySegment<byte> segment = SendBufferHelper.Open(4096);

            ushort count = 0;
            bool success = true;

            Span<byte> s = new Span<byte>(segment.Array, segment.Offset, segment.Count);

            count += sizeof(ushort);
            // Slice(시작(count), 길이(Length - count))
            success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), this.packetId);
            count += sizeof(ushort);
            success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), this.playerId);
            count += sizeof(long);

            // string 
            // 문자열의 길이를 먼저 전달 후 문자열 내용 복사
            ushort strlen = (ushort)Encoding.Unicode.GetBytes(this.playerName, 0, this.playerName.Length, segment.Array, segment.Offset + count + sizeof(ushort));
            success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), strlen);
            count += sizeof(ushort);
            count += strlen;

            success &= BitConverter.TryWriteBytes(s, count);
            if (success == false)
                return null;

            return SendBufferHelper.Close(count);
        }

    }

    class PacketValue : Packet
    {
        public int hp;
        public int value;

        public override void Read(ArraySegment<byte> segment)
        {
            throw new NotImplementedException();
        }

        public override ArraySegment<byte> Write()
        {
            throw new NotImplementedException();
        }
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
            Console.WriteLine($"PacketId : {packetId} \nPacketSize : {size}");

            switch((PacketID)packetId)
            {
                case PacketID.Unknown:
                    break;
                case PacketID.PlayerInfo:
                    PlayerInfo info = new PlayerInfo();
                    info.Read(buffer);                    
                    Console.WriteLine($"PlayerId : {info.playerId}\nPlayerName : {info.playerName}");
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
