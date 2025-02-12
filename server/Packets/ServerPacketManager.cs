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
            _onRecv.Add((ushort)PacketID.C_PlayerName, MakePacket<C_PlayerName>);
            _handler.Add((ushort)PacketID.C_PlayerName, PacketHandler.C_PlayerNameHandler);
            _onRecv.Add((ushort)PacketID.C_PlayerChat, MakePacket<C_PlayerChat>);
            _handler.Add((ushort)PacketID.C_PlayerChat, PacketHandler.C_PlayerChatHandler);
            _onRecv.Add((ushort)PacketID.C_Disconnect, MakePacket<C_Disconnect>);
            _handler.Add((ushort)PacketID.C_Disconnect, PacketHandler.C_DisconnectHandler);

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
