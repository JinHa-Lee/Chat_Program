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
                if (connectStatus.Equals("Disconnect"))
                {
                    //DNS (Domain Name System) ���
                    string host = Dns.GetHostName();   // ���� ���� ȣ��Ʈ ����
                    IPHostEntry ipHost = Dns.GetHostEntry(host);
                    IPAddress ipAddr = ipHost.AddressList[0];
                    serverIP = ipHost.AddressList[0].ToString();
                    IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777); //endPoint - ip, port �ּ�
                    clientSocket.Connect(endPoint);
                    stream = clientSocket.GetStream();
                    //Connector connector = new Connector();
                    //connector.Connect(endPoint, () => { return _session; });
                    connectStatus = "Connect";
                }
                else
                {
                    DisplayText("�̹� ������ ����Ǿ� �ֽ��ϴ�.");
                    return;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("�������ῡ �����Ͽ����ϴ�.");
            }
            DisplayText("������ ����Ǿ����ϴ�.");
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
                    DisplayText("������ ����Ǿ� ���� �ʽ��ϴ�.");
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("�������������� �����Ͽ����ϴ�.");
            }
            DisplayText("������ �������� �Ͽ����ϴ�.");
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

            C_PlayerChat p = new C_PlayerChat() { contents = txtMessage.Text };
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
        public void DisplayText(string text)
        {
            // �����͸� ����â�� ǥ��, invoke�� ����Ͽ� �浹���ϱ�
            richTextBox1.Invoke((MethodInvoker)delegate { richTextBox1.AppendText(text + "\r\n"); });
            // ��ũ���� ���� �Ʒ���
            richTextBox1.Invoke((MethodInvoker)delegate { richTextBox1.ScrollToCaret(); });

        }

        private void txtStatus_TextChanged(object sender, EventArgs e)
        {
            txtStatus.Text = connectStatus;
        }

        private void Login_Click(object sender, EventArgs e)
        {
            if (connectStatus.Equals("Connect"))
            {
                userName = txtUsername.Text;
                txtUsername.Enabled = false;
                C_PlayerName p = new C_PlayerName() { playerName = userName };
                ArraySegment<byte> buffer = p.Write();
                stream.Write(p.Write());
            }
            else
            {
                MessageBox.Show("������ �������ֽʽÿ�.");
                return;
            }
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
                PacketManager.Instance.OnRecvPacket(_session, buffer, this);

            }
        }

        private void txtMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                buttonSend_Click(sender, e);
        }

        public void Add_User(string text)
        {
            text += "\r\n";
            if (userList.InvokeRequired)
                userList.Invoke(new MethodInvoker(delegate { userList.Text += text;  }));
            else
                userList.Text += text ;
        }
        public void read_UserList(string text)
        {
            text += "\r\n";
            if (userList.InvokeRequired)
                userList.Invoke(new MethodInvoker(delegate { userList.Text += text; }));
            else
                userList.Text += text;

        }
    }

}
