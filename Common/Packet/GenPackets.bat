START ../../PacketGenerator/bin/PacketGenerator.exe ../../PacketGenerator/PDL.xml
::/Y �ɼ��� ������ �����ϸ� �����
XCOPY /Y GenPackets.cs "../../client/Packets"
XCOPY /Y GenPackets.cs "../../WinFormsClient/Packets"
XCOPY /Y GenPackets.cs "../../server/Packets"
XCOPY /Y ClientPacketManager.cs "../../client/Packets"
XCOPY /Y ClientPacketManager.cs "../../WinFormsClient/Packets"
XCOPY /Y ServerPacketManager.cs "../../server/Packets"