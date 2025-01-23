using ServerCore;
using client;
using System;
using System.Collections.Generic;



class PacketHandler
{
    // 패킷을 받게되면 실행할 기능관리
    public static void S_BroadcastEnterRoomHandler(PacketSession session, IPacket packet)
    {
        S_BroadcastChat p = packet as S_BroadcastChat;
        ServerSession _session = session as ServerSession;

        Console.WriteLine($"{p.playerName} : {p.contents}");
    }

    public static void S_BroadcastChatHandler(PacketSession session, IPacket packet)
    {
        S_BroadcastChat p = packet as S_BroadcastChat;
        ServerSession _session = session as ServerSession;

        Console.WriteLine($"{p.playerName} : {p.contents}");
    }

    public static void S_PlayerListHandler(PacketSession session, IPacket packet)
    {
        S_BroadcastChat p = packet as S_BroadcastChat;
        ServerSession _session = session as ServerSession;

        Console.WriteLine($"{p.playerName} : {p.contents}");
    }

    public static void S_BroadcastDisconnectHandler(PacketSession session, IPacket packet)
    {
        S_BroadcastDisconnect p = packet as S_BroadcastDisconnect;
        ServerSession _session = session as ServerSession;

        Console.WriteLine($"{p.playerId} disconnected");
    }

}

