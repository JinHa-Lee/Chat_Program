using ServerCore;
using server;
using System;
using System.Collections.Generic;



class PacketHandler
{
    // 패킷을 받게되면 실행할 기능관리
    public static void C_PlayerNameHandler(PacketSession session, IPacket packet)
    {
        ClientSession clientSession = session as ClientSession;
        C_PlayerName p = packet as C_PlayerName;

        if (clientSession.Room == null)
            return;

        Console.WriteLine($"{p.playerName}");

        // null 크래시 방지
        Room room = clientSession.Room;
        room.Push(
            () => { room.SetName(clientSession, p); }
            );

    }

    public static void C_PlayerChatHandler(PacketSession session, IPacket packet)
    {
        ClientSession clientSession = session as ClientSession;
        C_PlayerChat p = packet as C_PlayerChat;

        if (clientSession.Room == null)
            return;

        Console.WriteLine($"{clientSession.playerName} : {p.contents}");

        // null 크래시 방지
        Room room = clientSession.Room;
        room.Push(
            () => { room.Chat(clientSession, p); }
            );
        
    }

    public static void C_DisconnectHandler(PacketSession session, IPacket packet)
    {
        ClientSession clientSession = session as ClientSession;
        C_Disconnect p = packet as C_Disconnect;

        if (clientSession.Room == null)
            return;

        // null 크래시 방지
        Room room = clientSession.Room;
        room.Push(
            () => { room.Leave(clientSession); }
            );

    }

}

