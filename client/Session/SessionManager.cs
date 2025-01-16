using client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server
{
    internal class SessionManager
    {
        static SessionManager _sessionManager = new SessionManager();
        public static SessionManager Instance { get { return _sessionManager; } } 

        List<ServerSession> _sessions = new List<ServerSession>();
        object _lock = new object();

        public ServerSession Generate()
        {
            // 클라이언트 세션 생성

            lock (_lock)
            {
                ServerSession session = new ServerSession();
                _sessions.Add(session);

                return session;
            }
        }

        public void SendForEach()
        {
            lock (_lock)
            {
                foreach (ServerSession session in _sessions)
                {
                    C_PlayerChat p = new C_PlayerChat() { playerId = 1, playerName = "Test", contents = "채팅 내용 입니다." };

                    session.Send(p.Write());
                }
            }
        }
    }
}
