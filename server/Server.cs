using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class chat_Server
{
    static void Main(string[] args)
    {
        MyServer server = new MyServer();
    }
}


class MyServer
{
    public MyServer()
    {
        // 서버 시작
        AsyncServerStart();
    }

    private void AsyncServerStart()
    {
        Console.WriteLine("서버콘솔창 \n\n");

        // 서버 포트 설정 및 시작
        TcpListener server = new TcpListener(new IPEndPoint(IPAddress.Any, 9999));
        server.Start();
        Console.WriteLine("서버를 시작합니다. 클라이언트의 접속을 기다립니다.");

        // 클라이언트 객체 생성 후 연결
        // 클라이언트가 연결될때가지 서버는 블락처리
        TcpClient client = server.AcceptTcpClient();
        Console.WriteLine("클라이언트가 접속하였습니다.");

        // ClientData의 객체를 생성해주고 연결된 클라이언트를 ClientData의 멤버로 설정해준다
        ClientData clientData = new ClientData(client);

        // BeginRead를 통해 비동기로 읽기
        clientData.client.GetStream().BeginRead(clientData.readByteData, 0, clientData.readByteData.Length, new AsyncCallback(DataReceived), clientData);

        // 데이터를 읽든 못읽든 해당 로직 실행
        while (true)
        {
            Console.WriteLine("서버 구동중");
            Thread.Sleep(1000);
        }


    }

    private void DataReceived(IAsyncResult ar)
    {
        // 콜백 메서드
        // 데이터를 읽었을 경우 실행

        // 콜백으로 받아온 DATA를 형변환
        ClientData callbackClient = ar.AsyncState as ClientData;

        // 실제로 넘어온 크기를 받아옴
        int bytesRead = callbackClient.client.GetStream().EndRead(ar);

        // 문자열로 넘어온 데이터를 파싱하여 출력
        string readString = Encoding.Default.GetString(callbackClient.readByteData, 0, bytesRead);

        Console.WriteLine(readString);

        // 비동기서버의 핵심
        // 비동기서버는 while문을 돌리지 않고 콜백메서드에서 다시 읽으라고 비동기 명령을 내림
        callbackClient.client.GetStream().BeginRead(callbackClient.readByteData, 0, callbackClient.readByteData.Length,new AsyncCallback(DataReceived), callbackClient);    

    }


}

class ClientData
{
    // 연결이 확인된 클라이언트를 넣어줄 클래스입니다.
    // readByteData는 stream데이터를 읽을 객체
    public TcpClient client { get; set; }
    public byte[] readByteData { get; set; }

    public ClientData(TcpClient client)
    {
        this.client = client;
        this.readByteData = new byte[1024];
    
    }
}