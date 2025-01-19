using client;
using server;
using System.Net;

namespace WinFormsClient
{
    public partial class Form1 : Form
    {
        string serverIP = string.Empty;
        int port = 7777;
        string userName = string.Empty;
        static string connectStatus = "Disconnect";
        string message = string.Empty;
        ServerSession _session = SessionManager.Instance.Generate();


        public Form1()
        {
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "Client";
            this.txtIP.Text = serverIP.ToString();
            this.txtPORT.Text = port.ToString();
            this.txtUsername.Text = userName.ToString();
            this.txtStatus.Text = connectStatus.ToString();

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
                    
                    Connector connector = new Connector();
                    connector.Connect(endPoint, () => { return _session; });
                    connectStatus = "Connect";
                }
                else
                {
                    message = "�̹� ������ ����Ǿ� �ֽ��ϴ�.";
                    DisplayText(message);
                    return;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("�������ῡ �����Ͽ����ϴ�.");
            }
            message = "������ ����Ǿ����ϴ�.";
            DisplayText(message);
            txtStatus.Text = connectStatus;

        }

        private void buttonDisconnect_Click(object sender, EventArgs e)
        {
            try
            {
                if (connectStatus.Equals("Connect"))
                {
                    _session.Disconnect();
                    
                    connectStatus = "Diconnect";
                }
                else
                {
                    message = "������ ����Ǿ� ���� �ʽ��ϴ�.";
                    DisplayText(message);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("�������������� �����Ͽ����ϴ�.");
            }
            message = "������ �������� �Ͽ����ϴ�.";
            DisplayText(message);
            txtStatus.Text = connectStatus;
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {

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
            // �����͸� ����â�� ǥ��, invoke�� ����Ͽ� �浹���ϱ�
            richTextBox1.Invoke((MethodInvoker)delegate { richTextBox1.AppendText(text + "\r\n"); });
            // ��ũ���� ���� �Ʒ���
            richTextBox1.Invoke((MethodInvoker)delegate { richTextBox1.ScrollToCaret(); });

        }

        private void txtStatus_TextChanged(object sender, EventArgs e)
        {
            txtStatus.Text = connectStatus;
        }
    }
}
