using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerCore
{
    public abstract class Session
    {
        Socket _socket;
        int _disconnected = 0;
        object _lock = new object();

        SocketAsyncEventArgs _sendArgs = new SocketAsyncEventArgs();
        SocketAsyncEventArgs _recvArgs = new SocketAsyncEventArgs();
        Queue<ArraySegment<byte>> _sendQueue = new Queue<ArraySegment<byte>>();
        List<ArraySegment<byte>> _pendingList = new List<ArraySegment<byte>>();

        // server, client session에서 상속받을 기능 
        public abstract void OnConnected(EndPoint endPoint);
        public abstract void OnRecv(ArraySegment<byte> buffer);
        public abstract void OnSend(int numOfBytes);
        public abstract void OnDisconnected(EndPoint endPoint);


        public void Init(Socket socket)
        {
            // 소켓 등록 및 활성화

            _socket = socket;
            // send receive를 성공적으로 수행했다면 이벤트를 실행하도록 설정
            _sendArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnSendCompleted);
            _recvArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnRecvCompleted);
            //최초 1회 등록
            RegisterRecv();

        }

        void Clear()
        {
            // sendQueue, pendingList 초기화

            lock (_lock)
            {
                _pendingList.Clear();
                _sendQueue.Clear();
            }
        }

        public void Disconnect()
        {
            // 연결 해제

            // interloacked를 이용하여 현재상태가 disconnected(0) 인지 확인
            // disconnected 일 경우 connected(1)로 변경 후 리턴
            if (Interlocked.CompareExchange(ref _disconnected, 1, 0) == 1)
                return;

            _socket.Shutdown(SocketShutdown.Both);
            _socket.Close();
            Clear();

        }

        public void Send(ArraySegment<byte> buffer)
        {
            lock(_lock)
            {
                _sendQueue.Enqueue(buffer);
                if (_pendingList.Count == 0)
                    RegisterSend();
            }
        }

#region 네트워크 통신
        void RegisterSend()
        {
            // send를 비동기로 등록하고 대기

            if (_socket == null || _disconnected == 1) 
                return;
            while (_sendQueue.Count > 0) 
            {
                ArraySegment<byte> buff = _sendQueue.Dequeue();
                _pendingList.Add(buff);  // queue에 있던 정보를 _pendingList 저장
            }

            _sendArgs.BufferList = _pendingList; // _pendingList 정보를 sendArgs에 저장

            try
            {
                bool pending = _socket.SendAsync(_sendArgs); // 비동기 send
                if (pending == false)
                    OnSendCompleted(null, _sendArgs);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"RegisterSend Failed {ex}");
            }
        }
         void OnSendCompleted(object sender, SocketAsyncEventArgs args)
        {
            // Send가 정상적으로 수행되었다면 호출되는 이벤트

            lock (_lock)
            {
                // 전송된 바이트가 1이상이고 Success 상태 -> 정상통신
                if (args.BytesTransferred > 0 && args.SocketError == SocketError.Success)
                {
                    try
                    {
                        _sendArgs.BufferList = null; //Send가 정상적으로 이루어졌기 때문에 BufferList를 비운다
                        _pendingList.Clear(); // 위와 동일하게 pendingList를 비운다

                        OnSend(_sendArgs.BytesTransferred); // send 성공후 이벤트 호출

                        if (_sendQueue.Count > 0) // Send 완료시점에 sendQueue에 새로운 버퍼가 대기중이라면 send 등록
                            RegisterSend();
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine($"OnSendCompleted Failed {ex}");
                    }
                }
                else
                {
                    Disconnect();
                }
            }
        }

        void RegisterRecv()
        {
            // receive를 비동기로 등록하고 대기

            if (_socket == null || _disconnected == 1)
                return;

            _recvArgs.SetBuffer(new byte[4096], 0 , 4096); // 4096 사이즈 버퍼설정

            try
            {
                bool pending = _socket.ReceiveAsync(_recvArgs); // 비동기 send
                if (pending == false)
                    OnRecvCompleted(null, _sendArgs);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"RegisterRecv Failed {ex}");
            }

        }
        void OnRecvCompleted(object sender, SocketAsyncEventArgs args)
        {
            // Receive가 정상적으로 수행되었다면 호출되는 이벤트

                // 전송된 바이트가 1이상이고 Success 상태 -> 정상통신
                if (args.BytesTransferred > 0 && args.SocketError == SocketError.Success)
                {
                    try
                    {
                        OnRecv(new ArraySegment<byte>(args.Buffer, args.Offset, args.BytesTransferred)); // receive 성공후 이벤트 호출
                        RegisterRecv();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"OnSendCompleted Failed {ex}");
                    }
                }
                else
                {
                    Disconnect();
                }
            }
        

        #endregion

    }
}
