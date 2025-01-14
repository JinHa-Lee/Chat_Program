using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PacketGenerator
{
    internal class PacketGenerator
    {
        static string _genPackets;
        static ushort packetId;
        static string packetEnums;


        static void Main(string[] args)
        {
            string pdlPath = "../PDL.xml";

            XmlReaderSettings settings = new XmlReaderSettings()
            {
                IgnoreComments = true, // 주석 무시
                IgnoreWhitespace = true, // 스페이스바 무시
            };

            // input 인자를 받으면 해당 값을 경로로 사용
            if (args.Length >= 1)
                pdlPath = args[0];

            using (XmlReader r = XmlReader.Create(pdlPath, settings))
            {
                r.MoveToContent(); // Header를 건너 뛰고 내용부터 시작

                while (r.Read()) // string방식으로 읽기 (한줄씩)
                {
                    if (r.Depth == 1 && r.NodeType == XmlNodeType.Element)
                        ParsePacket(r);
                }
                // 생성된 패킷기능을 파일로 저장
                string fileText = string.Format(PacketFormat.flieFormat, packetEnums, _genPackets);
                File.WriteAllText("GenPackets.cs", fileText);
            }
        }

        public static void ParsePacket(XmlReader r)
        {
            // 패킷 파싱

            if (r.NodeType == XmlNodeType.EndElement)
                return;

            if (r.Name.ToLower() != "packet") // 패킷이 아닐경우 생략
            {
                Console.WriteLine("Invalid packet node");
                return;
            }

            string packetName = r["name"];
            if (string.IsNullOrEmpty(packetName)) // 이름 없는 경우 생략
            {
                Console.WriteLine("Packet without name");
                return;
            }
            Tuple<string, string, string> result_t = ParseMembers(r);

            _genPackets += string.Format(PacketFormat.packetFormat, packetName, result_t.Item1, result_t.Item2, result_t.Item3);
            packetEnums += string.Format(PacketFormat.packetEnumFormat, packetName, ++packetId) + Environment.NewLine + "\t";
        }

        public static Tuple<string, string, string> ParseMembers(XmlReader r)
        {
            // 패킷 내부 멤버 파싱

            string packetName = r["name"]; // 패킷 이름 확인
            int depth = r.Depth + 1; // 패킷 이름 다음 깊이 정보 확인

            string memberCode = ""; // {1} 멤버 변수들
            string readCode = ""; // {2} 멤버 변수 Read
            string writeCode = ""; // {3} 멤버 변수 Write


            while (r.Read())
            {
                if (r.Depth != depth) // 깊이가 다른 경우 > 패킷내용이 끝나는 경우
                    break;
                string memberName = r["name"]; // 멤버 이름 확인
                Console.WriteLine(memberName);
                if (string.IsNullOrEmpty(memberName)) // 멤버 이름이 유효하지 않을 경우
                {
                    Console.WriteLine("Member without name");
                    return null;
                }

                // 이미 내용이 있다면 줄바꿈 추가
                if (string.IsNullOrEmpty(memberCode) == false)
                    memberCode += Environment.NewLine;
                if (string.IsNullOrEmpty(readCode) == false)
                    readCode += Environment.NewLine;
                if (string.IsNullOrEmpty(writeCode) == false)
                    writeCode += Environment.NewLine;

                string memberType = r.Name.ToLower(); // 멤버 타입 확인

                switch (memberType) // 멤버 타입에 따른 패킷생성함수 실행
                {
                    case "byte":
                    case "sbyte":
                        memberCode += string.Format(PacketFormat.memberFormat, memberType, memberName);
                        readCode += string.Format(PacketFormat.readByteFormat, memberName, memberType);
                        writeCode += string.Format(PacketFormat.writeByteFormat, memberName, memberType);
                        break;
                    case "bool":
                    case "short":
                    case "ushort":
                    case "int":
                    case "long":
                    case "float":
                    case "double":
                        memberCode += string.Format(PacketFormat.memberFormat, memberType, memberName);
                        readCode += string.Format(PacketFormat.readFormat, memberName, ToMemberType(memberType), memberType);
                        writeCode += string.Format(PacketFormat.writeFormat, memberName, memberType);
                        break;
                    case "string":
                        memberCode += string.Format(PacketFormat.memberFormat, memberType, memberName);
                        readCode += string.Format(PacketFormat.readStringFormat, memberName);
                        writeCode += string.Format(PacketFormat.writeStringFormat, memberName);
                        break;
                    case "list":
                        Tuple<string, string, string> t = ParseList(r);
                        memberCode += t.Item1;
                        readCode += t.Item2;
                        writeCode += t.Item3;
                        break;
                    default:
                        break;
                }
            }


            memberCode = memberCode.Replace("\n", "\n\t");
            readCode = readCode.Replace("\n", "\n\t\t");
            writeCode = writeCode.Replace("\n", "\n\t\t");
            return new Tuple<string, string, string>(memberCode, readCode, writeCode);
        }
        public static Tuple<string, string, string> ParseList(XmlReader r)
        {
            // 패킷 내부 List 파싱

            string listName = r["name"];
            if (string.IsNullOrEmpty(listName)) // List 이름이 유효하지 않을 경우
            {
                Console.WriteLine("List without name");
                return null;
            }

            Tuple<string, string, string> t = ParseMembers(r); // List 내부 멤버 파싱

            string memberCode = string.Format(PacketFormat.memberListFormat,
                FirstCharToUpper(listName),
                FirstCharToLower(listName),
                t.Item1,
                t.Item2,
                t.Item3
                );
            
            string readCode = string.Format(PacketFormat.readListFormat,
                FirstCharToUpper(listName),
                FirstCharToLower(listName));
            string writeCode = string.Format(PacketFormat.writeListFormat,
                FirstCharToUpper(listName),
                FirstCharToLower(listName));
            return new Tuple<string, string, string>(memberCode, readCode, writeCode);
        }

        public static string ToMemberType(string memberType)
        {
            // read 실행시 변수 타입에 따른 메서드 지정

            switch (memberType)
            {
                case "bool":
                    return "ToBoolean";
                case "short":
                    return "ToInt16";
                case "ushort":
                    return "ToUInt16";
                case "int":
                    return "ToInt32";
                case "long":
                    return "ToInt64";
                case "float":
                    return "ToSingle";
                case "double":
                    return "ToDouble";
                default:
                    return "";
            }
        }

        public static string FirstCharToUpper(string input)
        {
            // 기존의 변수 명명 형식에 따른 대문자 변경

            if (string.IsNullOrEmpty(input))
                return "";
            return input[0].ToString().ToUpper() + input.Substring(1);
        }

        public static string FirstCharToLower(string input)
        {
            // 기존의 변수 명명 형식에 따른 소문자 변경

            if (string.IsNullOrEmpty(input))
                return "";
            return input[0].ToString().ToLower() + input.Substring(1);
        }
    }
}
