using ServerCore;
using client;
using System;
using System.Collections.Generic;



class PacketHandler
{
    // 패킷을 받게되면 실행할 기능관리

    public static void S_BroadcastChatHandler(PacketSession session, IPacket packet)
    {
        S_BroadcastChat p = packet as S_BroadcastChat;
        ServerSession _session = session as ServerSession;

        Console.WriteLine($"PlayerId : {p.playerId} \nPlayerName : {p.playerName} \nContents : {p.contents}");
    }



}

