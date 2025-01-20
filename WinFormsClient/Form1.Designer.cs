namespace WinFormsClient
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            textBox3 = new TextBox();
            txtStatus = new TextBox();
            txtMessage = new TextBox();
            buttonConnect = new Button();
            buttonDisconnect = new Button();
            buttonSend = new Button();
            txtUsername = new TextBox();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            richTextBox1 = new RichTextBox();
            Login = new Button();
            Logout = new Button();
            SuspendLayout();
            // 
            // textBox3
            // 
            textBox3.Location = new Point(623, 103);
            textBox3.Multiline = true;
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(158, 282);
            textBox3.TabIndex = 2;
            textBox3.TextChanged += textBox3_TextChanged;
            // 
            // txtStatus
            // 
            txtStatus.Location = new Point(12, 39);
            txtStatus.Name = "txtStatus";
            txtStatus.Size = new Size(133, 27);
            txtStatus.TabIndex = 4;
            txtStatus.TextChanged += txtStatus_TextChanged;
            // 
            // txtMessage
            // 
            txtMessage.Location = new Point(12, 404);
            txtMessage.Name = "txtMessage";
            txtMessage.Size = new Size(605, 27);
            txtMessage.TabIndex = 5;
            txtMessage.TextChanged += txtMessage_TextChanged;
            // 
            // buttonConnect
            // 
            buttonConnect.Location = new Point(170, 35);
            buttonConnect.Name = "buttonConnect";
            buttonConnect.Size = new Size(94, 35);
            buttonConnect.TabIndex = 6;
            buttonConnect.Text = "CONNECT";
            buttonConnect.UseVisualStyleBackColor = true;
            buttonConnect.Click += buttonConnect_Click;
            // 
            // buttonDisconnect
            // 
            buttonDisconnect.Location = new Point(279, 35);
            buttonDisconnect.Name = "buttonDisconnect";
            buttonDisconnect.Size = new Size(113, 35);
            buttonDisconnect.TabIndex = 7;
            buttonDisconnect.Text = "DISCONNECT";
            buttonDisconnect.UseVisualStyleBackColor = true;
            buttonDisconnect.Click += buttonDisconnect_Click;
            // 
            // buttonSend
            // 
            buttonSend.Location = new Point(650, 400);
            buttonSend.Name = "buttonSend";
            buttonSend.Size = new Size(76, 35);
            buttonSend.TabIndex = 8;
            buttonSend.Text = "send";
            buttonSend.UseVisualStyleBackColor = true;
            buttonSend.Click += buttonSend_Click;
            // 
            // txtUsername
            // 
            txtUsername.Location = new Point(422, 39);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(158, 27);
            txtUsername.TabIndex = 9;
            txtUsername.TextChanged += textBox7_TextChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 16);
            label3.Name = "label3";
            label3.Size = new Size(62, 20);
            label3.TabIndex = 12;
            label3.Text = "STATUS";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(623, 80);
            label4.Name = "label4";
            label4.Size = new Size(66, 20);
            label4.TabIndex = 13;
            label4.Text = "User List";
            label4.Click += label4_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(422, 16);
            label5.Name = "label5";
            label5.Size = new Size(49, 20);
            label5.TabIndex = 14;
            label5.Text = "Name";
            // 
            // richTextBox1
            // 
            richTextBox1.Location = new Point(12, 103);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(583, 282);
            richTextBox1.TabIndex = 15;
            richTextBox1.Text = "";
            richTextBox1.TextChanged += richTextBox1_TextChanged;
            // 
            // Login
            // 
            Login.Location = new Point(599, 36);
            Login.Name = "Login";
            Login.Size = new Size(80, 32);
            Login.TabIndex = 16;
            Login.Text = "Login";
            Login.UseVisualStyleBackColor = true;
            Login.Click += Login_Click;
            // 
            // Logout
            // 
            Logout.Location = new Point(685, 36);
            Logout.Name = "Logout";
            Logout.Size = new Size(80, 32);
            Logout.TabIndex = 17;
            Logout.Text = "Logout";
            Logout.UseVisualStyleBackColor = true;
            Logout.Click += Logout_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(821, 466);
            Controls.Add(Logout);
            Controls.Add(Login);
            Controls.Add(richTextBox1);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(txtUsername);
            Controls.Add(buttonSend);
            Controls.Add(buttonDisconnect);
            Controls.Add(buttonConnect);
            Controls.Add(txtMessage);
            Controls.Add(txtStatus);
            Controls.Add(textBox3);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private TextBox textBox3;
        private TextBox txtStatus;
        private TextBox txtMessage;
        private Button buttonConnect;
        private Button buttonDisconnect;
        private Button buttonSend;
        private TextBox txtUsername;
        private Label label3;
        private Label label4;
        private Label label5;
        private RichTextBox richTextBox1;
        private Button Login;
        private Button Logout;
    }
}
