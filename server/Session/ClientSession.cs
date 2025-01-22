using Server;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace server
{
    internal class ClientSession : PacketSession
    {
        public Room Room { get; set; }
        public int sessionId { get; set; }
        public string playerName { get; set; }

        public override void OnConnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnConnected : {endPoint}");

            // 연결과 동시에 방 입장
            chat_Server.Room.Push(() => { chat_Server.Room.Enter(this); });
            

        }

        public override void OnDisconnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnDisconnected : {endPoint}");
            SessionManager.Instance.Remove(this);
            if (Room == null)
            {
                // null 크래시 방지
                Room room = Room;
                room.Push(() => { room.Leave(this); });
                Room = null;
            }

        }

        public override void OnRecvPacket(ArraySegment<byte> buffer)
        {
            PacketManager.Instance.OnRecvPacket(this,buffer);

        }

        public override void OnSend(int numOfBytes)
        {
            Console.WriteLine($"Transferred bytes : {numOfBytes}");
        }
    }
}
