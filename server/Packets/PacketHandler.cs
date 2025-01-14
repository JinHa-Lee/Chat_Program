using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server
{
    internal class PacketHandler
    {
        // 패킷을 받게되면 실행할 기능관리

        public static void PlayerInfoHandler(PacketSession session, IPacket packet)
        {
            PlayerInfo p = packet as PlayerInfo;

            Console.WriteLine($"PlayerId : {p.playerId}\nPlayerName : {p.playerName}");

            foreach (PlayerInfo.SkillInfo skill in p.skillInfos)
                Console.WriteLine($"Skill({skill.id})({skill.level})({skill.duration})");
        }

    }
}
