using ServerCore;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

public enum PacketID
{
    C_PlayerInfo = 1,
	C_PlayerChat = 2,
	
}
public interface IPacket
{
	ushort Protocol { get; }
	void Read(ArraySegment<byte> segment);
	ArraySegment<byte> Write();
}

class C_PlayerInfo : IPacket
{

    public long playerId;
	public string playerName;
	public class SkillInfo
	{
	    public int id;
		public short level;
		public float duration;
	
	    public void Read(ReadOnlySpan<byte> s, ref ushort count)
	    {
	        this.id = BitConverter.ToInt32(s.Slice(count, s.Length - count));
			count += sizeof(int);
			this.level = BitConverter.ToInt16(s.Slice(count, s.Length - count));
			count += sizeof(short);
			this.duration = BitConverter.ToSingle(s.Slice(count, s.Length - count));
			count += sizeof(float);
	    }
	
	    public bool Write(Span<byte> s, ref ushort count)
	    {
	        bool success = true;
	        success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), this.id);
			count += sizeof(int);
			success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), this.level);
			count += sizeof(short);
			success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), this.duration);
			count += sizeof(float);
	
	        return success;
	    }
	
	
	}
	public List<SkillInfo> skillInfos = new List<SkillInfo>();

    public ushort Protocol { get { return (ushort)PacketID.C_PlayerInfo; } }

    public void Read(ArraySegment<byte> segment)
    {
        ReadOnlySpan<byte> s = new ReadOnlySpan<byte>(segment.Array, segment.Offset, segment.Count);
        ushort count = 0;
        count += sizeof(ushort); // size 크기
        count += sizeof(ushort); // packetId 크기
        this.playerId = BitConverter.ToInt64(s.Slice(count, s.Length - count));
		count += sizeof(long);
		ushort playerNameLen = BitConverter.ToUInt16(s.Slice(count, s.Length - count));
		count += sizeof(ushort);
		this.playerName = Encoding.Unicode.GetString(s.Slice(count, playerNameLen));
		count += playerNameLen;
		this.skillInfos.Clear();
		ushort skillInfoLen = BitConverter.ToUInt16(s.Slice(count, s.Length - count));
		count += sizeof(ushort);
		for (int i = 0; i < skillInfoLen; i++)
		{
		    SkillInfo skillInfo = new SkillInfo();
		    skillInfo.Read(s, ref count);
		    skillInfos.Add(skillInfo);
		}
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;
        bool success = true;
        Span<byte> s = new Span<byte>(segment.Array, segment.Offset, segment.Count);
        count += sizeof(ushort);
        success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), (ushort)PacketID.C_PlayerInfo);
        count += sizeof(ushort);
        success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), this.playerId);
		count += sizeof(long);
		ushort playerNameLen = (ushort)Encoding.Unicode.GetBytes(this.playerName, 0, playerName.Length, segment.Array, segment.Offset + count + sizeof(ushort));
		success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), playerNameLen);
		count += sizeof(ushort);
		count += playerNameLen;
		success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), (ushort)this.skillInfos.Count);
		count += sizeof(ushort);
		foreach (SkillInfo skillInfo in this.skillInfos)
		{
		    success &= skillInfo.Write(s, ref count);
		}
        success &= BitConverter.TryWriteBytes(s, count);
        if (success == false)
            return null;
        return SendBufferHelper.Close(count);
    }
}
class C_PlayerChat : IPacket
{

    public long playerId;
	public string playerName;
	public class ChatInfo
	{
	    public int ChatId;
	
	    public void Read(ReadOnlySpan<byte> s, ref ushort count)
	    {
	        this.ChatId = BitConverter.ToInt32(s.Slice(count, s.Length - count));
			count += sizeof(int);
	    }
	
	    public bool Write(Span<byte> s, ref ushort count)
	    {
	        bool success = true;
	        success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), this.ChatId);
			count += sizeof(int);
	
	        return success;
	    }
	
	
	}
	public List<ChatInfo> chatInfos = new List<ChatInfo>();
	public string contents;
	public byte ChatByte;

    public ushort Protocol { get { return (ushort)PacketID.C_PlayerChat; } }

    public void Read(ArraySegment<byte> segment)
    {
        ReadOnlySpan<byte> s = new ReadOnlySpan<byte>(segment.Array, segment.Offset, segment.Count);
        ushort count = 0;
        count += sizeof(ushort); // size 크기
        count += sizeof(ushort); // packetId 크기
        this.playerId = BitConverter.ToInt64(s.Slice(count, s.Length - count));
		count += sizeof(long);
		ushort playerNameLen = BitConverter.ToUInt16(s.Slice(count, s.Length - count));
		count += sizeof(ushort);
		this.playerName = Encoding.Unicode.GetString(s.Slice(count, playerNameLen));
		count += playerNameLen;
		this.chatInfos.Clear();
		ushort chatInfoLen = BitConverter.ToUInt16(s.Slice(count, s.Length - count));
		count += sizeof(ushort);
		for (int i = 0; i < chatInfoLen; i++)
		{
		    ChatInfo chatInfo = new ChatInfo();
		    chatInfo.Read(s, ref count);
		    chatInfos.Add(chatInfo);
		}
		ushort contentsLen = BitConverter.ToUInt16(s.Slice(count, s.Length - count));
		count += sizeof(ushort);
		this.contents = Encoding.Unicode.GetString(s.Slice(count, contentsLen));
		count += contentsLen;
		this.ChatByte = (byte)segment.Array[segment.Offset + count];
		count += sizeof(byte);
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
        success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), this.playerId);
		count += sizeof(long);
		ushort playerNameLen = (ushort)Encoding.Unicode.GetBytes(this.playerName, 0, playerName.Length, segment.Array, segment.Offset + count + sizeof(ushort));
		success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), playerNameLen);
		count += sizeof(ushort);
		count += playerNameLen;
		success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), (ushort)this.chatInfos.Count);
		count += sizeof(ushort);
		foreach (ChatInfo chatInfo in this.chatInfos)
		{
		    success &= chatInfo.Write(s, ref count);
		}
		ushort contentsLen = (ushort)Encoding.Unicode.GetBytes(this.contents, 0, contents.Length, segment.Array, segment.Offset + count + sizeof(ushort));
		success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), contentsLen);
		count += sizeof(ushort);
		count += contentsLen;
		segment.Array[segment.Offset + count] = (byte)this.ChatByte;
		count += sizeof(byte);
        success &= BitConverter.TryWriteBytes(s, count);
        if (success == false)
            return null;
        return SendBufferHelper.Close(count);
    }
}
