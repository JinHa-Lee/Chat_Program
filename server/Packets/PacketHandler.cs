using ServerCore;
using server;
using System;
using System.Collections.Generic;



class PacketHandler
{
    // 패킷을 받게되면 실행할 기능관리

    public static void C_PlayerChatHandler(PacketSession session, IPacket packet)
    {
        ClientSession clientSession = session as ClientSession;
        C_PlayerChat p = packet as C_PlayerChat;

        if (clientSession.Room == null)
            return;

        // null 크래시 방지
        Room room = clientSession.Room;
        room.Push(
            () => { room.Broadcast(clientSession, p); }
            );
        
    }

}

