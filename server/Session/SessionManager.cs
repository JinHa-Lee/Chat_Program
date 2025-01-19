using ServerCore;
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

        int _sessionId = 0;
        Dictionary<int, ClientSession> _sessions = new Dictionary<int, ClientSession>();
        object _lock = new object();

        public ClientSession Generate()
        {
            // 클라이언트 세션 생성

            lock (_lock)
            {
                int sessionId = ++_sessionId;

                ClientSession session = new ClientSession();
                session.sessionId = sessionId;
                _sessions.Add(sessionId, session);

                Console.WriteLine($"Connected {sessionId}");

                return session;
            }
        }

        public void Remove(ClientSession session)
        {
            // 클라이언트 세션 삭제

            lock (_lock)
            {
                Console.WriteLine($"Disconnected {session.sessionId}");
                _sessions.Remove(session.sessionId);

            }
        }

        public ClientSession Find(int id)
        {
            // 클라이언트 세션 탐색

            lock(_lock)
            {
                ClientSession session = null;
                _sessions.TryGetValue(id, out session);
                return session;
            }
        }
    }
}
