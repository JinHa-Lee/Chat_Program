using ServerCore;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

public enum PacketID
{
    C_PlayerChat = 1,
	S_BroadcastChat = 2,
	C_Disconnect = 3,
	S_BroadcastDisconnect = 4,
	
}
public interface IPacket
{
	ushort PacketId { get; }
	void Read(ArraySegment<byte> segment);
	ArraySegment<byte> Write();
}

class C_PlayerChat : IPacket
{

    public string playerName;
	public string contents;

    public ushort PacketId { get { return (ushort)PacketID.C_PlayerChat; } }

    public void Read(ArraySegment<byte> segment)
    {
        ReadOnlySpan<byte> s = new ReadOnlySpan<byte>(segment.Array, segment.Offset, segment.Count);
        ushort count = 0;
        count += sizeof(ushort); // size 크기
        count += sizeof(ushort); // packetId 크기
        ushort playerNameLen = BitConverter.ToUInt16(s.Slice(count, s.Length - count));
		count += sizeof(ushort);
		this.playerName = Encoding.Unicode.GetString(s.Slice(count, playerNameLen));
		count += playerNameLen;
		ushort contentsLen = BitConverter.ToUInt16(s.Slice(count, s.Length - count));
		count += sizeof(ushort);
		this.contents = Encoding.Unicode.GetString(s.Slice(count, contentsLen));
		count += contentsLen;
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;
        bool success = true;
        Span<byte> s = new Span<byte>(segment.Array, segment.Offset, segment.Count);
        count += sizeof(ushort);
        success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), (ushort)PacketID.C_PlayerChat);
        count += sizeof(ushort);
        ushort playerNameLen = (ushort)Encoding.Unicode.GetBytes(this.playerName, 0, playerName.Length, segment.Array, segment.Offset + count + sizeof(ushort));
		success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), playerNameLen);
		count += sizeof(ushort);
		count += playerNameLen;
		ushort contentsLen = (ushort)Encoding.Unicode.GetBytes(this.contents, 0, contents.Length, segment.Array, segment.Offset + count + sizeof(ushort));
		success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), contentsLen);
		count += sizeof(ushort);
		count += contentsLen;
        success &= BitConverter.TryWriteBytes(s, count);
        if (success == false)
            return null;
        return SendBufferHelper.Close(count);
    }
}
class S_BroadcastChat : IPacket
{

    public string playerName;
	public string contents;

    public ushort PacketId { get { return (ushort)PacketID.S_BroadcastChat; } }

    public void Read(ArraySegment<byte> segment)
    {
        ReadOnlySpan<byte> s = new ReadOnlySpan<byte>(segment.Array, segment.Offset, segment.Count);
        ushort count = 0;
        count += sizeof(ushort); // size 크기
        count += sizeof(ushort); // packetId 크기
        ushort playerNameLen = BitConverter.ToUInt16(s.Slice(count, s.Length - count));
		count += sizeof(ushort);
		this.playerName = Encoding.Unicode.GetString(s.Slice(count, playerNameLen));
		count += playerNameLen;
		ushort contentsLen = BitConverter.ToUInt16(s.Slice(count, s.Length - count));
		count += sizeof(ushort);
		this.contents = Encoding.Unicode.GetString(s.Slice(count, contentsLen));
		count += contentsLen;
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;
        bool success = true;
        Span<byte> s = new Span<byte>(segment.Array, segment.Offset, segment.Count);
        count += sizeof(ushort);
        success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), (ushort)PacketID.S_BroadcastChat);
        count += sizeof(ushort);
        ushort playerNameLen = (ushort)Encoding.Unicode.GetBytes(this.playerName, 0, playerName.Length, segment.Array, segment.Offset + count + sizeof(ushort));
		success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), playerNameLen);
		count += sizeof(ushort);
		count += playerNameLen;
		ushort contentsLen = (ushort)Encoding.Unicode.GetBytes(this.contents, 0, contents.Length, segment.Array, segment.Offset + count + sizeof(ushort));
		success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), contentsLen);
		count += sizeof(ushort);
		count += contentsLen;
        success &= BitConverter.TryWriteBytes(s, count);
        if (success == false)
            return null;
        return SendBufferHelper.Close(count);
    }
}
class C_Disconnect : IPacket
{

    

    public ushort PacketId { get { return (ushort)PacketID.C_Disconnect; } }

    public void Read(ArraySegment<byte> segment)
    {
        ReadOnlySpan<byte> s = new ReadOnlySpan<byte>(segment.Array, segment.Offset, segment.Count);
        ushort count = 0;
        count += sizeof(ushort); // size 크기
        count += sizeof(ushort); // packetId 크기
        
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;
        bool success = true;
        Span<byte> s = new Span<byte>(segment.Array, segment.Offset, segment.Count);
        count += sizeof(ushort);
        success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), (ushort)PacketID.C_Disconnect);
        count += sizeof(ushort);
        
        success &= BitConverter.TryWriteBytes(s, count);
        if (success == false)
            return null;
        return SendBufferHelper.Close(count);
    }
}
class S_BroadcastDisconnect : IPacket
{

    public string playerName;

    public ushort PacketId { get { return (ushort)PacketID.S_BroadcastDisconnect; } }

    public void Read(ArraySegment<byte> segment)
    {
        ReadOnlySpan<byte> s = new ReadOnlySpan<byte>(segment.Array, segment.Offset, segment.Count);
        ushort count = 0;
        count += sizeof(ushort); // size 크기
        count += sizeof(ushort); // packetId 크기
        ushort playerNameLen = BitConverter.ToUInt16(s.Slice(count, s.Length - count));
		count += sizeof(ushort);
		this.playerName = Encoding.Unicode.GetString(s.Slice(count, playerNameLen));
		count += playerNameLen;
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;
        bool success = true;
        Span<byte> s = new Span<byte>(segment.Array, segment.Offset, segment.Count);
        count += sizeof(ushort);
        success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), (ushort)PacketID.S_BroadcastDisconnect);
        count += sizeof(ushort);
        ushort playerNameLen = (ushort)Encoding.Unicode.GetBytes(this.playerName, 0, playerName.Length, segment.Array, segment.Offset + count + sizeof(ushort));
		success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), playerNameLen);
		count += sizeof(ushort);
		count += playerNameLen;
        success &= BitConverter.TryWriteBytes(s, count);
        if (success == false)
            return null;
        return SendBufferHelper.Close(count);
    }
}
