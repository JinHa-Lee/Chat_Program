using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerCore
{
    // receive buffer 를 객체로 만들어 재사용
    internal class RecvBuffer
    {
        ArraySegment<byte> _buffer;
        int _readPos;   // read 커서
        int _writePos;  // write 커서

        public RecvBuffer(int bufferSize)
        {
            // 입력한 크기의 버퍼 생성

            _buffer = new ArraySegment<byte>(new byte[bufferSize], 0, bufferSize);
        }

        public int DataSize { get { return _writePos - _readPos; } }
        public int FreeSize { get { return _buffer.Count - _writePos; } }

        // read 가능한(유효한) 범위의 버퍼 반환
        public ArraySegment<byte> ReadSegment
        {
            get { return new ArraySegment<byte>(_buffer.Array, _buffer.Offset + _readPos, DataSize); }
        }
        // write 가능한(유효한) 범위의 버퍼 반환
        public ArraySegment<byte> WriteSegment
        {
            get { return new ArraySegment<byte>(_buffer.Array, _buffer.Offset + _writePos, FreeSize); }
        }

        public void Clean()
        {
            // 버퍼 정리

            int dataSize = DataSize;
            if (dataSize == 0)
            {
                // 남은 데이터가 없다면 커서 위치 초기화
                _readPos = _writePos = 0;
            }
            else
            {
                // 남은 데이터가 있다면 데이터 위치, 커서를 정리
                Array.Copy(_buffer.Array, _buffer.Offset + _readPos, _buffer.Array, _buffer.Offset, dataSize);
                _readPos = 0;
                _writePos = dataSize;
            }
        }

        public bool OnRead(int numOfBytes)
        {
            // read 시 실행

            // 데이터 크기 보다 큰 사이즈를 읽으려할 경우 false 반환
            if (numOfBytes > DataSize)
                return false;
            // 커서위치 이동
            _readPos += numOfBytes;
            return true;
        }

        public bool OnWrite(int numOfBytes)
        {
            // write 시 실행

            // 여유 크기 보다 큰 사이즈를 쓸려할 경우 false 반환
            if (numOfBytes > FreeSize)
                return false;
            // 커서위치 이동
            _writePos += numOfBytes;
            return true;
        }

    }
}
