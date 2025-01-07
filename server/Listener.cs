using Microsoft.Win32;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace server
{
    internal class Listener
    {

        Socket _listenSocket;
        Func<Session> _sessionFactory; // 클라이언트와 연결할 세션을 만들 함수

        // 리스너 초기화
        public void Init(IPEndPoint endPoint, Func<Session> sessionFactory, int register = 10)
        {
            // 리슨 소켓 생성
            _listenSocket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            // 수행할 함수 추가
            _sessionFactory = sessionFactory;

            // 리슨소켓 설정
            _listenSocket.Bind(endPoint);

            // 최대 대기수 설정
            _listenSocket.Listen(5);

            // 많은 접속이 예상될 경우 args를 여러개 생성하여 작업을 진행할 수도 있다.
            //for (int i = 0; i < register; i++)
            {
                // 클라이언트의 승인 요청을 받는 작업을 진행할 비동기소켓작업 객체를 생성하고
                // 작업이 끝나면 OnAcceptCompleted()를 호출하도록 이벤트 핸들러에 등록
                SocketAsyncEventArgs args = new SocketAsyncEventArgs();
                args.Completed += new EventHandler<SocketAsyncEventArgs>(OnAcceptCompleted);
                // 최초 1회 등록
                RegisterAccept(args);
            }
        }

        void RegisterAccept(SocketAsyncEventArgs args)
        {
            // 이전 소켓에 대한 정보 초기화
            args.AcceptSocket = null;

            // 서버는 승인요청을 받는 작업을 할 args 객체와 함께
            // 클라이언트의 요청이 들어오면 승인작업을 하도록 처리
            bool pending = _listenSocket.AcceptAsync(args); // 비동기 accept
            // pending이 true면 작업이 보류중상태 작업이 완료되면 args에서 complete 이벤트 발생

            // 즉시 연결된 경우 이벤트 발생 (pending 상태가 아닌경우)
            if (pending == false)
            {
                OnAcceptCompleted(null, args);
            }
        }

        void OnAcceptCompleted(object sender, SocketAsyncEventArgs args)
        {
            if (args.SocketError == SocketError.Success)
            {
                // 세션의 소켓을 인자로 넣어 호출
                Session session = _sessionFactory.Invoke();
                session.Init(args.AcceptSocket);
                session.OnConnected(args.AcceptSocket.RemoteEndPoint);

            }
            else
            {
                Console.WriteLine(args.SocketError.ToString());
            }
            // 새로운 승인을 기다린다
            // 승인작업을 가지고 온 args객체가 작업을 마치고
            // 다시 승인작업을 하러 넘어감
            RegisterAccept(args);
        }
    }
}
