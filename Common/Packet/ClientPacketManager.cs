using ServerCore;
using System;
using System.Collections.Generic;

public class PacketManager
{
    #region Singleton
    static PacketManager _instance = new PacketManager();
    public static PacketManager Instance { get { return _instance; } }
    #endregion

    PacketManager()
    {
        Register();
    }

    Dictionary<ushort, Action<PacketSession, ArraySegment<byte>>> _onRecv = new Dictionary<ushort, Action<PacketSession, ArraySegment<byte>>>();
    Dictionary<ushort, Action<PacketSession, IPacket>> _handler = new Dictionary<ushort, Action<PacketSession, IPacket>>();


    public void Register()
    {
            _onRecv.Add((ushort)PacketID.S_BroadcastEnterRoom, MakePacket<S_BroadcastEnterRoom>);
            _handler.Add((ushort)PacketID.S_BroadcastEnterRoom, PacketHandler.S_BroadcastEnterRoomHandler);
            _onRecv.Add((ushort)PacketID.S_PlayerList, MakePacket<S_PlayerList>);
            _handler.Add((ushort)PacketID.S_PlayerList, PacketHandler.S_PlayerListHandler);
            _onRecv.Add((ushort)PacketID.S_BroadcastChat, MakePacket<S_BroadcastChat>);
            _handler.Add((ushort)PacketID.S_BroadcastChat, PacketHandler.S_BroadcastChatHandler);
            _onRecv.Add((ushort)PacketID.S_BroadcastDisconnect, MakePacket<S_BroadcastDisconnect>);
            _handler.Add((ushort)PacketID.S_BroadcastDisconnect, PacketHandler.S_BroadcastDisconnectHandler);

    }

    public void OnRecvPacket(PacketSession session,ArraySegment<byte> buffer)
    {
        ushort count = 0;
        ushort size = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
        count += sizeof(ushort);
        ushort packetId = BitConverter.ToUInt16(buffer.Array, buffer.Offset + count);
        count += sizeof(ushort);

        Action<PacketSession, ArraySegment<byte>> action = null;
        if (_onRecv.TryGetValue(packetId, out action))
            action.Invoke(session, buffer);
    }

    void MakePacket<T>(PacketSession session, ArraySegment<byte>buffer) where T : IPacket, new()
    {
        T pkt = new T();
        pkt.Read(buffer);

        Action<PacketSession, IPacket> action = null;
        if (_handler.TryGetValue(pkt.PacketId, out action))
            action.Invoke(session, pkt);
    }
}
