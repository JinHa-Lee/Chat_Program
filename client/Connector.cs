using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ServerCore;

namespace client
{
    internal class Connector
    {

        Func<Session> _sessionFactory; // 소켓을 받으면 실행하는 함수

        public void Connect(IPEndPoint endPoint, Func<Session> sessionFactory)
        {
            // 리슨 소켓 생성
            Socket socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            // 수행할 함수 추가
            _sessionFactory = sessionFactory;
            // 서버측과 유사하게 이벤트 등록
            SocketAsyncEventArgs args = new SocketAsyncEventArgs();
            args.Completed += OnAcceptCompleted;
            args.RemoteEndPoint = endPoint;
            args.UserToken = socket;
            // 최초 1회 등록
            RegisterAccept(args);

        }

        void RegisterAccept(SocketAsyncEventArgs args)
        {
            // 이전 소켓에 대한 정보 초기화
            Socket socket = args.UserToken as Socket;
            if (socket == null)
                return;

            // 서버의 요청이 들어오면 승인작업을 하도록 처리
            bool pending = socket.ConnectAsync(args); // 비동기 connect
            // pending이 true면 작업이 보류중상태 작업이 완료되면 args에서 complete 이벤트 발생

            // 즉시 연결된 경우 이벤트 발생 (pending 상태가 아닌경우)
            if (pending == false)
                OnAcceptCompleted(null, args);
            
        }

        void OnAcceptCompleted(object sender, SocketAsyncEventArgs args)
        {
            if (args.SocketError == SocketError.Success)
            {
                // 세션의 소켓을 인자로 넣어 호출
                Session session = _sessionFactory.Invoke();
                session.Init(args.ConnectSocket);
                session.OnConnected(args.RemoteEndPoint);

            }
            else
            {
                Console.WriteLine(args.SocketError.ToString());
            }
        }
    }
}
