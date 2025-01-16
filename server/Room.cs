using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server
{
    internal class Room
    {
        List<ClientSession> _sessions = new List<ClientSession>();
        object _lock = new object();

        public void Enter(ClientSession session)
        {
            lock (_lock)
            {
                _sessions.Add(session);
                session.Room = this;
            }
        }

        public void Leave(ClientSession session)
        {
            lock (_lock)
            {
                _sessions.Remove(session);
            }
        }

        public void Broadcast(ClientSession session,C_PlayerChat packet)
        {
            // 클라이언트에서 받은 채팅을 룸에있는 다른 세션들에게 전송

            S_BroadcastChat p = new S_BroadcastChat();
            p.playerId = packet.playerId;
            p.playerName = packet.playerName;
            p.contents = packet.contents;
            ArraySegment<byte> segment = p.Write();
            lock (_lock)
            {
                foreach (ClientSession s in _sessions)
                    s.Send(segment);
            }
        }
    }
}
