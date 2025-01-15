using ServerCore;
using server;
using System;
using System.Collections.Generic;



class PacketHandler
{
    // 패킷을 받게되면 실행할 기능관리

    public static void C_PlayerInfoHandler(PacketSession session, IPacket packet)
    {
        C_PlayerInfo p = packet as C_PlayerInfo;

        Console.WriteLine($"PlayerId : {p.playerId}\nPlayerName : {p.playerName}");

        foreach (C_PlayerInfo.SkillInfo skill in p.skillInfos)
            Console.WriteLine($"Skill({skill.id})({skill.level})({skill.duration})");
    }

    public static void C_PlayerChatHandler(PacketSession session, IPacket packet)
    {
        C_PlayerInfo p = packet as C_PlayerInfo;

        Console.WriteLine($"PlayerId : {p.playerId}\nPlayerName : {p.playerName}");

        foreach (C_PlayerInfo.SkillInfo skill in p.skillInfos)
            Console.WriteLine($"Skill({skill.id})({skill.level})({skill.duration})");
    }

}

