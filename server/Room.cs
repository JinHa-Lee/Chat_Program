using ServerCore;
using System.Data;

namespace server
{
    internal class Room : IJobQueue
    {
        // Queue에서 lock처리를 하기때문에 lock 제거
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

            S_BroadcastDisconnect p = new S_BroadcastDisconnect();
            p.playerId = session.sessionId;
            p.playerName = session.playerName;
            Broadcast(p.Write());

            session.Disconnect();
        }
        public void Broadcast(ArraySegment<byte> segment)
        {
            _pendingList.Add(segment);
        }
        public void Chat(ClientSession session, C_PlayerChat packet)
        {
            // 클라이언트에서 받은 채팅을 DB에 insert
            string sql = $"INSERT INTO log_chat(playerId, playerName, chat) VALUES('{session.sessionId}','{session.playerName}','{packet.contents}')";
            Console.WriteLine(sql);
            Database db = new Database();
            db.Open();
            db.CUDQuery(sql);
            db.Close();
            // 클라이언트에서 받은 채팅을 룸에있는 다른 세션들에게 전송
            S_BroadcastChat p = new S_BroadcastChat();
            p.playerId = session.sessionId;
            p.playerName = session.playerName;
            p.contents = packet.contents;
            Broadcast(p.Write());

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

            // 새로운 플레이어에게 기존 플레이어 정보 전송
            session.playerName = packet.playerName;
            S_PlayerList players = new S_PlayerList();
            foreach (ClientSession s in _sessions)
            {
                players.players.Add(new S_PlayerList.Player()
                {
                    isSelf = (s == session),
                    playerName = s.playerName,
                });
            }
            session.Send(players.Write());
            // 새로운 플레이어의 접속을 기존 플레이어에게 전송
            S_BroadcastEnterRoom pBroadcast = new S_BroadcastEnterRoom();
            pBroadcast.playerId = session.sessionId;
            pBroadcast.playerName = packet.playerName;
            Broadcast(pBroadcast.Write());
        }
    }
}
