START ../../PacketGenerator/bin/PacketGenerator.exe ../../PacketGenerator/PDL.xml
::/Y 옵션은 파일이 존재하면 덮어쓴다
XCOPY /Y GenPackets.cs "../../client/Packets"
XCOPY /Y GenPackets.cs "../../server/Packets"
XCOPY /Y ClientPacketManager.cs "../../client/Packets"
XCOPY /Y ServerPacketManager.cs "../../server/Packets"