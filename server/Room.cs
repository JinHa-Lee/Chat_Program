using Microsoft.VisualBasic;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server
{
    internal class Room : IJobQueue
    {
        // room에서 lock처리를 하기때문에 lock 제거
        List<ClientSession> _sessions = new List<ClientSession>();
        JobQueue _jobQueue = new JobQueue();
        List<ArraySegment<byte>> _pendingList = new List<ArraySegment<byte>>();

        public void Enter(ClientSession session)
        {
            _sessions.Add(session);
            session.Room = this;

        }

        public void Leave(ClientSession session)
        {
            _sessions.Remove(session);
            session.Disconnect();
        }
        public void Broadcast(ArraySegment<byte> segment)
        {
            _pendingList.Add(segment);
        }
        public void Broadcast(ClientSession session, C_PlayerChat packet)
        {
            // 클라이언트에서 받은 채팅을 룸에있는 다른 세션들에게 전송

            S_BroadcastChat p = new S_BroadcastChat();
            p.playerId = session.sessionId;
            p.playerName = session.playerName;
            p.contents = packet.contents;
            ArraySegment<byte> segment = p.Write();

            // 패킷을 매번보내지 않고 모아서 보낸다
            _pendingList.Add(segment);

        }

        public void Push(Action job)
        {
            _jobQueue.Push(job);
        }

        public void Flush()
        {
            foreach (ClientSession session in _sessions)
                session.Send(_pendingList);

            _pendingList.Clear();
        }

        public void SetName(ClientSession session, C_PlayerName packet)
        {
            session.playerName = packet.playerName;
            S_PlayerList players = new S_PlayerList();
            foreach (ClientSession s in _sessions)
            {
                players.players.Add(new S_PlayerList.Player()
                {
                    playerName = s.playerName,
                });
            }
            session.Send(players.Write());
        }
    }
}
