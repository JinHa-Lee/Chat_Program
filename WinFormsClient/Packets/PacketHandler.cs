using ServerCore;
using client;
using WinFormsClient;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

class PacketHandler
{
    // 패킷을 받게되면 실행할 기능관리
    public static void S_BroadcastEnterRoomHandler(PacketSession session, IPacket packet, Form1 form)
    {
        S_BroadcastEnterRoom p = packet as S_BroadcastEnterRoom;
        ServerSession _session = session as ServerSession;

        form.DisplayText($"{p.playerName}님이 입장하셨습니다.");
        form.Add_User(p.playerName);
    }

    public static void S_PlayerListHandler(PacketSession session, IPacket packet, Form1 form)
    {
        S_PlayerList ps = packet as S_PlayerList;
        ServerSession _session = session as ServerSession;
        foreach (S_PlayerList.Player p in ps.players)
        {
            if (p.isSelf == false)
                form.read_UserList(p.playerName);
        }

    }

    public static void S_BroadcastChatHandler(PacketSession session, IPacket packet, Form1 form)
    {
        S_BroadcastChat p = packet as S_BroadcastChat;
        ServerSession _session = session as ServerSession;


        form.DisplayText($"{p.playerName} : {p.contents}");
    }

    public static void S_BroadcastDisconnectHandler(PacketSession session, IPacket packet, Form1 form)
    {
        S_BroadcastDisconnect p = packet as S_BroadcastDisconnect;
        ServerSession _session = session as ServerSession;

        Console.WriteLine($"{p.playerId} disconnected");
    }


}
