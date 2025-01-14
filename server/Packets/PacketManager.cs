using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server
{
    internal class PacketManager
    {
        // 패킷 관리 매니저

        #region Singleton
        // 인스턴스를 재생성 할 일이 없다고 판단
        // 싱글톤 패턴으로 생성
        static PacketManager _instance;
        public static PacketManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new PacketManager();
                return _instance;
            }
        }
        #endregion

        
        PacketManager()
        {
            // PacketManager 생성시 등록

            Register();
        }

        // 기능 등록을위한 딕셔너리
        Dictionary<ushort, Action<PacketSession, ArraySegment<byte>>> _onRecv = new Dictionary<ushort, Action<PacketSession, ArraySegment<byte>>>();
        Dictionary<ushort, Action<PacketSession, IPacket>> _handler = new Dictionary<ushort, Action<PacketSession, IPacket>>();

        public void Register()
        {
            // 생성된 딕셔너리에 기능 등록
            _onRecv.Add((ushort)PacketID.PlayerInfo, MakePacket<PlayerInfo>);
            _handler.Add((ushort)PacketID.PlayerInfo, PacketHandler.PlayerInfoHandler);
        }

        public void OnRecvPacket(PacketSession session,ArraySegment<byte> buffer)
        {
            ushort count = 0;
            ushort size = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
            count += sizeof(ushort);
            ushort packetId = BitConverter.ToUInt16(buffer.Array, buffer.Offset + count);
            count += sizeof(ushort);
            Console.WriteLine($"PacketId : {packetId} \nPacketSize : {size}");

            // _onRecv 딕셔너리 내부에 등록된 기능 실행
            Action<PacketSession, ArraySegment<byte>> action = null;
            if (_onRecv.TryGetValue(packetId, out action))
                action.Invoke(session, buffer);

        }

        void MakePacket<T>(PacketSession session, ArraySegment<byte> buffer) where T : IPacket, new()
        {
            T pkt = new T();
            pkt.Read(buffer);

            // _handler 딕셔너리 내부에 등록된 기능 실행
            Action<PacketSession, IPacket> action = null;
            if (_handler.TryGetValue(pkt.PacketId, out action))
                action.Invoke(session, pkt);
        }
    }
}
