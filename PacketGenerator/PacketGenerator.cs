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



        static void Main(string[] args)
        {
            XmlReaderSettings settings = new XmlReaderSettings()
            {
                IgnoreComments = true, // 주석 무시
                IgnoreWhitespace = true, // 스페이스바 무시
            };

            using (XmlReader r = XmlReader.Create("../../../PDL.xml", settings))
            {
                r.MoveToContent(); // Header를 건너 뛰고 내용부터 시작

                while (r.Read()) // string방식으로 읽기 (한줄씩)
                {
                    if (r.Depth == 1 && r.NodeType == XmlNodeType.Element)
                        ParsePacket(r);
                }
            }
        }

        public static void ParsePacket(XmlReader r)
        {
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
            ParseMembers(r);
            //Console.WriteLine(r.Name + " " + r["name"]);

        }

        public static void ParseMembers(XmlReader r)
        {
            string packetName = r["name"]; // 패킷 이름 확인
            int depth = r.Depth + 1; // 패킷 이름 다음 깊이 정보 확인


            while (r.Read())
            {
                if (r.Depth != depth) // 깊이가 다른 경우 > 패킷내용이 끝나는 경우
                    break;
                string memberName = r["name"]; // 멤버 이름 확인
                Console.WriteLine(memberName);
                if (string.IsNullOrEmpty(memberName)) // 멤버 이름이 유효하지 않을 경우
                {
                    Console.WriteLine("Member without name");
                    break;
                }

                string memberType = r.Name.ToLower(); // 멤버 타입 확인

                switch (memberType) // 멤버 타입에 따른 패킷생성함수 실행
                {
                    case "byte":
                    case "sbyte":
                    case "bool":
                    case "short":
                    case "ushort":
                    case "int":
                    case "long":
                    case "float":
                    case "double":
                    case "string":
                    case "list":
                    default:
                        break;
                }
            }

        }
    }
}
