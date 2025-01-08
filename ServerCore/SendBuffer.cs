using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerCore
{
    public class SendBuffer
    {
        byte[] _buffer;
        int _usedSize = 0; // write 커서

        public int FreeSize { get { return _usedSize; } }

        public SendBuffer(int chunkSize)
        {
            // 입력한 크기의 버퍼 생성

            _buffer = new byte[chunkSize];
        }

        public ArraySegment<byte> Open(int reserveSize)
        {
            // send buffer 에 데이터를 담기 전 실행

            // 여유 크기 보다 큰 사이즈를 요청할 경우 null 반환 
            if (reserveSize > FreeSize)
                return null;

            // 요청 크기만큼 빈 버퍼 반환
            return new ArraySegment<byte>(_buffer, _usedSize, reserveSize);
        }

        public ArraySegment<byte> Close(int usedSize)
        {
            // send buffer 에 데이터를 담은 후 실행
            
            // 데이터가 담긴 버퍼 반환
            ArraySegment<byte> segment = new ArraySegment<byte>(_buffer, _usedSize, usedSize);
            _usedSize += usedSize;
            return segment;
        }
    }

    public class SendBufferHelper
    {
        // sendBuffer 사용에 도움이 되는 기능 제공

        // 현재 쓰레드에서 사용할 sendBuffer 참조값 보관
       public static ThreadLocal<SendBuffer> currentBuffer = new ThreadLocal<SendBuffer>(() => { return null; });

        // 생성할 sendBuffer 크기
        public static int ChunkSize { get; set; } = 65535;

        public static ArraySegment<byte> Open(int reserveSize)
        {
            // currentBuffer 에 데이터를 담기 전 실행

            // 현재 쓰레드에 sendBuffer가 없거나 예약크기보다 여유롭다면 sendBuffer 생성
            if (currentBuffer.Value == null)
                currentBuffer.Value = new SendBuffer(ChunkSize);
            if (currentBuffer.Value.FreeSize < reserveSize)
                currentBuffer.Value = new SendBuffer(ChunkSize);
            // 생성된 sendBuffer 기능 실행
            return currentBuffer.Value.Open(reserveSize);
        }

        public static ArraySegment<byte> Close(int usedSize)
        {
            // currentBuffer 에 데이터를 담은 후 실행

            // 생성된 sendBuffer 기능 실행
            return currentBuffer.Value.Close(usedSize);
        }

    }
}
