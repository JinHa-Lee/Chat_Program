using client;
using server;
using ServerCore;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace WinFormsClient
{
    public partial class Form1 : Form
    {
        TcpClient clientSocket = new TcpClient();
        NetworkStream stream;
        string serverIP = string.Empty;
        int port = 7777;
        string userName = string.Empty;
        static string connectStatus = "Disconnect";
        string _message = string.Empty;
        ServerSession _session = new ServerSession();
        Thread t_handler;
        public string recvuserName = string.Empty;
        public string recvmessage = string.Empty;


        public Form1()
        {
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "Client";
            this.txtUsername.Text = userName.ToString();
            this.txtStatus.Text = connectStatus.ToString();
            this.txtStatus.Enabled = false;

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(userName))
                {
                    MessageBox.Show("사용자를 등록해주십시오.");
                    return;
                }
                if (connectStatus.Equals("Disconnect"))
                {
                    //DNS (Domain Name System) 사용
                    string host = Dns.GetHostName();   // 현재 로컬 호스트 저장
                    IPHostEntry ipHost = Dns.GetHostEntry(host);
                    IPAddress ipAddr = ipHost.AddressList[0];
                    serverIP = ipHost.AddressList[0].ToString();
                    IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777); //endPoint - ip, port 주소
                    clientSocket.Connect(endPoint);
                    stream = clientSocket.GetStream();
                    //Connector connector = new Connector();
                    //connector.Connect(endPoint, () => { return _session; });
                    connectStatus = "Connect";
                }
                else
                {
                    DisplayText("이미 서버에 연결되어 있습니다.");
                    return;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("서버연결에 실패하였습니다.");
            }
            DisplayText("서버에 연결되었습니다.");
            txtStatus.Text = connectStatus;

            t_handler = new Thread(GetMessage);
            t_handler.IsBackground = true;
            t_handler.Start();

        }

        private void buttonDisconnect_Click(object sender, EventArgs e)
        {
            try
            {
                if (connectStatus.Equals("Connect"))
                {
                    Disconnect();
                }
                else
                {
                    DisplayText("서버에 연결되어 있지 않습니다.");
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("서버연결해제에 실패하였습니다.");
            }
            DisplayText("서버와 연결해제 하였습니다.");
            txtStatus.Text = connectStatus;
        }

        private void Disconnect()
        {
            clientSocket = new TcpClient();
            C_Disconnect p = new C_Disconnect();
            stream.Write(p.Write());
            stream.Flush();
            connectStatus = "Disconnect";
            
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Disconnect();
            Application.ExitThread();
            Environment.Exit(0);
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {

            C_PlayerChat p = new C_PlayerChat() { playerName = txtUsername.Text, contents = txtMessage.Text };
            ArraySegment<byte> buffer = p.Write();

            stream.Write(p.Write());
            txtMessage.Text = "";
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
        private void DisplayText(string text)
        {
            // 데이터를 수신창에 표시, invoke를 사용하여 충돌피하기
            richTextBox1.Invoke((MethodInvoker)delegate { richTextBox1.AppendText(text + "\r\n"); });
            // 스크롤을 제일 아래로
            richTextBox1.Invoke((MethodInvoker)delegate { richTextBox1.ScrollToCaret(); });

        }

        private void txtStatus_TextChanged(object sender, EventArgs e)
        {
            txtStatus.Text = connectStatus;
        }

        private void Login_Click(object sender, EventArgs e)
        {
            userName = txtUsername.Text;
            txtUsername.Enabled = false;
        }

        private void Logout_Click(object sender, EventArgs e)
        {
            txtUsername.Enabled = true;
        }

        private void txtMessage_TextChanged(object sender, EventArgs e)
        {

        }

        private void GetMessage()
        {
            while (connectStatus.Equals("Connect"))
            {
                stream = clientSocket.GetStream();
                int bufferSize = clientSocket.ReceiveBufferSize;
                byte[] buffer = new byte[bufferSize];
                int bytes = stream.Read(buffer, 0, bufferSize);
                S_BroadcastChat s = new S_BroadcastChat();
                s.Read(buffer);
                DisplayText($"<{s.playerName}>{s.contents}");

            }
        }

        private void txtMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                buttonSend_Click(sender, e);
        }
    }

}
