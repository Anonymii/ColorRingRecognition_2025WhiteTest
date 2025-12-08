namespace IdentitySys
{
    partial class TestForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label16 = new System.Windows.Forms.Label();
            this.btnSwitchTwo = new System.Windows.Forms.Button();
            this.cbSerialTwo = new System.Windows.Forms.ComboBox();
            this.cboSerialChoose = new System.Windows.Forms.ComboBox();
            this.txtSend = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cbParity = new System.Windows.Forms.ComboBox();
            this.cbStop = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.cbDataBits = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cbBaudRate = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnSend = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtSecond = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbTimeSend = new System.Windows.Forms.CheckBox();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.rbRcvStr = new System.Windows.Forms.RadioButton();
            this.rbRcv16 = new System.Windows.Forms.RadioButton();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.radio1 = new System.Windows.Forms.RadioButton();
            this.rdSendStr = new System.Windows.Forms.RadioButton();
            this.btnSwitch = new System.Windows.Forms.Button();
            this.cbSerial = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtReceive = new System.Windows.Forms.RichTextBox();
            this.btnReceive = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.tmSend = new System.Windows.Forms.Timer(this.components);
            this.serialPortX = new System.IO.Ports.SerialPort(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tsSpNum = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsBaudRate = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsDataBits = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsStopBits = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsParity = new System.Windows.Forms.ToolStripStatusLabel();
            this.label10 = new System.Windows.Forms.Label();
            this.txtVel = new System.Windows.Forms.TextBox();
            this.btnPosMoveToLimit = new System.Windows.Forms.Button();
            this.btnNegMoveToLimit = new System.Windows.Forms.Button();
            this.btnMoveToZero = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btnTwoAxisStop = new System.Windows.Forms.Button();
            this.btnMotorIsRet = new System.Windows.Forms.Button();
            this.btnMotorStop = new System.Windows.Forms.Button();
            this.btnNegMoveNoDes = new System.Windows.Forms.Button();
            this.btnPosMoveNoDes = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btnIn4 = new System.Windows.Forms.Button();
            this.btnZero = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.btnPosLimit = new System.Windows.Forms.Button();
            this.btnBackAndForthTest = new System.Windows.Forms.Button();
            this.btnNegLimit = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.btnGetStatus = new System.Windows.Forms.Button();
            this.btnPauseMove = new System.Windows.Forms.Button();
            this.btnAbsMove = new System.Windows.Forms.Button();
            this.txtAbsPos = new System.Windows.Forms.TextBox();
            this.txtEncoder = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.btnRelMove = new System.Windows.Forms.Button();
            this.txtIncrement = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.btnZMotorIsRet = new System.Windows.Forms.Button();
            this.btnZMotorStop = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.btnZIn4 = new System.Windows.Forms.Button();
            this.btnZZero = new System.Windows.Forms.Button();
            this.label17 = new System.Windows.Forms.Label();
            this.btnZPosLimit = new System.Windows.Forms.Button();
            this.btnZBackAndForthTest = new System.Windows.Forms.Button();
            this.btnZNegLimit = new System.Windows.Forms.Button();
            this.label18 = new System.Windows.Forms.Label();
            this.btnZGetStatus = new System.Windows.Forms.Button();
            this.btnZPauseMove = new System.Windows.Forms.Button();
            this.btnZAbsMove = new System.Windows.Forms.Button();
            this.txtZAbsPos = new System.Windows.Forms.TextBox();
            this.txtZEncoder = new System.Windows.Forms.TextBox();
            this.btnZMoveToZero = new System.Windows.Forms.Button();
            this.label19 = new System.Windows.Forms.Label();
            this.btnZNegMoveToLimit = new System.Windows.Forms.Button();
            this.btnZRelMove = new System.Windows.Forms.Button();
            this.btnZPosMoveToLimit = new System.Windows.Forms.Button();
            this.txtZIncrement = new System.Windows.Forms.TextBox();
            this.txtZVel = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.serialPortZ = new System.IO.Ports.SerialPort(this.components);
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.btnSwitchTwo);
            this.groupBox1.Controls.Add(this.cbSerialTwo);
            this.groupBox1.Controls.Add(this.cboSerialChoose);
            this.groupBox1.Controls.Add(this.txtSend);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.btnSend);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtSecond);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cbTimeSend);
            this.groupBox1.Controls.Add(this.groupBox8);
            this.groupBox1.Controls.Add(this.groupBox7);
            this.groupBox1.Controls.Add(this.btnSwitch);
            this.groupBox1.Controls.Add(this.cbSerial);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(5, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(313, 311);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "串口：发送方";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label16.Location = new System.Drawing.Point(6, 146);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(119, 14);
            this.label16.TabIndex = 35;
            this.label16.Text = "请选择测试串口：";
            // 
            // btnSwitchTwo
            // 
            this.btnSwitchTwo.Location = new System.Drawing.Point(208, 41);
            this.btnSwitchTwo.Name = "btnSwitchTwo";
            this.btnSwitchTwo.Size = new System.Drawing.Size(75, 21);
            this.btnSwitchTwo.TabIndex = 11;
            this.btnSwitchTwo.Text = "打开串口";
            this.btnSwitchTwo.UseVisualStyleBackColor = true;
            this.btnSwitchTwo.Click += new System.EventHandler(this.btnSwitchTwo_Click);
            // 
            // cbSerialTwo
            // 
            this.cbSerialTwo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSerialTwo.FormattingEnabled = true;
            this.cbSerialTwo.Location = new System.Drawing.Point(134, 42);
            this.cbSerialTwo.Name = "cbSerialTwo";
            this.cbSerialTwo.Size = new System.Drawing.Size(68, 20);
            this.cbSerialTwo.TabIndex = 10;
            // 
            // cboSerialChoose
            // 
            this.cboSerialChoose.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSerialChoose.FormattingEnabled = true;
            this.cboSerialChoose.Location = new System.Drawing.Point(128, 142);
            this.cboSerialChoose.Name = "cboSerialChoose";
            this.cboSerialChoose.Size = new System.Drawing.Size(68, 20);
            this.cboSerialChoose.TabIndex = 34;
            // 
            // txtSend
            // 
            this.txtSend.Location = new System.Drawing.Point(3, 254);
            this.txtSend.Name = "txtSend";
            this.txtSend.Size = new System.Drawing.Size(259, 21);
            this.txtSend.TabIndex = 33;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label15.Location = new System.Drawing.Point(5, 44);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(119, 14);
            this.label15.TabIndex = 9;
            this.label15.Text = "串口2--Z轴Com6：";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.label9.Location = new System.Drawing.Point(1, 280);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(221, 12);
            this.label9.TabIndex = 31;
            this.label9.Text = "(16进制时，用空格或“，”将字节隔开)";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cbParity);
            this.groupBox3.Controls.Add(this.cbStop);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.cbDataBits);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.cbBaudRate);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Location = new System.Drawing.Point(3, 68);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(278, 71);
            this.groupBox3.TabIndex = 30;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "串口参数设置";
            // 
            // cbParity
            // 
            this.cbParity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbParity.FormattingEnabled = true;
            this.cbParity.Items.AddRange(new object[] {
            "无",
            "奇校验",
            "偶校验"});
            this.cbParity.Location = new System.Drawing.Point(202, 44);
            this.cbParity.Name = "cbParity";
            this.cbParity.Size = new System.Drawing.Size(68, 20);
            this.cbParity.TabIndex = 29;
            // 
            // cbStop
            // 
            this.cbStop.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbStop.FormattingEnabled = true;
            this.cbStop.Items.AddRange(new object[] {
            "1",
            "1.5",
            "2"});
            this.cbStop.Location = new System.Drawing.Point(61, 44);
            this.cbStop.Name = "cbStop";
            this.cbStop.Size = new System.Drawing.Size(63, 20);
            this.cbStop.TabIndex = 28;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.Location = new System.Drawing.Point(138, 47);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(63, 14);
            this.label8.TabIndex = 27;
            this.label8.Text = "校验位：";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(8, 47);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 14);
            this.label7.TabIndex = 27;
            this.label7.Text = "停止位：";
            // 
            // cbDataBits
            // 
            this.cbDataBits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDataBits.FormattingEnabled = true;
            this.cbDataBits.Items.AddRange(new object[] {
            "5",
            "6",
            "7",
            "8"});
            this.cbDataBits.Location = new System.Drawing.Point(202, 18);
            this.cbDataBits.Name = "cbDataBits";
            this.cbDataBits.Size = new System.Drawing.Size(68, 20);
            this.cbDataBits.TabIndex = 26;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(139, 21);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(63, 14);
            this.label6.TabIndex = 25;
            this.label6.Text = "数据位：";
            // 
            // cbBaudRate
            // 
            this.cbBaudRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBaudRate.FormattingEnabled = true;
            this.cbBaudRate.Items.AddRange(new object[] {
            "300",
            "600",
            "1200",
            "2400",
            "4800",
            "9600",
            "19200",
            "38400",
            "115200"});
            this.cbBaudRate.Location = new System.Drawing.Point(61, 18);
            this.cbBaudRate.Name = "cbBaudRate";
            this.cbBaudRate.Size = new System.Drawing.Size(63, 20);
            this.cbBaudRate.TabIndex = 24;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(8, 21);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 14);
            this.label5.TabIndex = 23;
            this.label5.Text = "波特率:";
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(222, 281);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(60, 25);
            this.btnSend.TabIndex = 22;
            this.btnSend.Text = "发送";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 239);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 20;
            this.label4.Text = "发送数据：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(245, 219);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 12);
            this.label3.TabIndex = 19;
            this.label3.Text = "秒";
            // 
            // txtSecond
            // 
            this.txtSecond.Location = new System.Drawing.Point(193, 214);
            this.txtSecond.Name = "txtSecond";
            this.txtSecond.Size = new System.Drawing.Size(44, 21);
            this.txtSecond.TabIndex = 18;
            this.txtSecond.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSecond_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(126, 219);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 17;
            this.label2.Text = "时间间隔：";
            // 
            // cbTimeSend
            // 
            this.cbTimeSend.AutoSize = true;
            this.cbTimeSend.Location = new System.Drawing.Point(4, 219);
            this.cbTimeSend.Name = "cbTimeSend";
            this.cbTimeSend.Size = new System.Drawing.Size(96, 16);
            this.cbTimeSend.TabIndex = 16;
            this.cbTimeSend.Text = "定时发送数据";
            this.cbTimeSend.UseVisualStyleBackColor = true;
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.rbRcvStr);
            this.groupBox8.Controls.Add(this.rbRcv16);
            this.groupBox8.Location = new System.Drawing.Point(138, 171);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(142, 36);
            this.groupBox8.TabIndex = 15;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "接收数据格式";
            // 
            // rbRcvStr
            // 
            this.rbRcvStr.AutoSize = true;
            this.rbRcvStr.Location = new System.Drawing.Point(72, 14);
            this.rbRcvStr.Name = "rbRcvStr";
            this.rbRcvStr.Size = new System.Drawing.Size(59, 16);
            this.rbRcvStr.TabIndex = 2;
            this.rbRcvStr.TabStop = true;
            this.rbRcvStr.Text = "字符串";
            this.rbRcvStr.UseVisualStyleBackColor = true;
            // 
            // rbRcv16
            // 
            this.rbRcv16.AutoSize = true;
            this.rbRcv16.Location = new System.Drawing.Point(9, 14);
            this.rbRcv16.Name = "rbRcv16";
            this.rbRcv16.Size = new System.Drawing.Size(59, 16);
            this.rbRcv16.TabIndex = 1;
            this.rbRcv16.TabStop = true;
            this.rbRcv16.Text = "16进制";
            this.rbRcv16.UseVisualStyleBackColor = true;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.radio1);
            this.groupBox7.Controls.Add(this.rdSendStr);
            this.groupBox7.Location = new System.Drawing.Point(0, 171);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(134, 37);
            this.groupBox7.TabIndex = 14;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "发送数据格式";
            // 
            // radio1
            // 
            this.radio1.AutoSize = true;
            this.radio1.Location = new System.Drawing.Point(9, 15);
            this.radio1.Name = "radio1";
            this.radio1.Size = new System.Drawing.Size(59, 16);
            this.radio1.TabIndex = 7;
            this.radio1.TabStop = true;
            this.radio1.Text = "16进制";
            this.radio1.UseVisualStyleBackColor = true;
            // 
            // rdSendStr
            // 
            this.rdSendStr.AutoSize = true;
            this.rdSendStr.Location = new System.Drawing.Point(73, 15);
            this.rdSendStr.Name = "rdSendStr";
            this.rdSendStr.Size = new System.Drawing.Size(59, 16);
            this.rdSendStr.TabIndex = 6;
            this.rdSendStr.TabStop = true;
            this.rdSendStr.Text = "字符串";
            this.rdSendStr.UseVisualStyleBackColor = true;
            // 
            // btnSwitch
            // 
            this.btnSwitch.Location = new System.Drawing.Point(208, 14);
            this.btnSwitch.Name = "btnSwitch";
            this.btnSwitch.Size = new System.Drawing.Size(75, 21);
            this.btnSwitch.TabIndex = 9;
            this.btnSwitch.Text = "打开串口";
            this.btnSwitch.UseVisualStyleBackColor = true;
            this.btnSwitch.Click += new System.EventHandler(this.btnSwitch_Click);
            // 
            // cbSerial
            // 
            this.cbSerial.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSerial.FormattingEnabled = true;
            this.cbSerial.Location = new System.Drawing.Point(134, 14);
            this.cbSerial.Name = "cbSerial";
            this.cbSerial.Size = new System.Drawing.Size(68, 20);
            this.cbSerial.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(7, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(119, 14);
            this.label1.TabIndex = 7;
            this.label1.Text = "串口1--X轴COM7：";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtReceive);
            this.groupBox2.Controls.Add(this.btnReceive);
            this.groupBox2.Controls.Add(this.btnClear);
            this.groupBox2.Controls.Add(this.btnExit);
            this.groupBox2.Location = new System.Drawing.Point(0, 322);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(323, 269);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "数据接收方";
            // 
            // txtReceive
            // 
            this.txtReceive.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.txtReceive.Location = new System.Drawing.Point(6, 21);
            this.txtReceive.Name = "txtReceive";
            this.txtReceive.ReadOnly = true;
            this.txtReceive.Size = new System.Drawing.Size(311, 194);
            this.txtReceive.TabIndex = 0;
            this.txtReceive.Text = "";
            // 
            // btnReceive
            // 
            this.btnReceive.Location = new System.Drawing.Point(6, 222);
            this.btnReceive.Name = "btnReceive";
            this.btnReceive.Size = new System.Drawing.Size(81, 23);
            this.btnReceive.TabIndex = 13;
            this.btnReceive.Text = "接收数据";
            this.btnReceive.UseVisualStyleBackColor = true;
            this.btnReceive.Click += new System.EventHandler(this.btnReceive_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(102, 222);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(59, 23);
            this.btnClear.TabIndex = 10;
            this.btnClear.Text = "清空";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(167, 222);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(59, 23);
            this.btnExit.TabIndex = 10;
            this.btnExit.Text = "退出";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // tmSend
            // 
            this.tmSend.Tick += new System.EventHandler(this.tmSend_Tick);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsSpNum,
            this.tsBaudRate,
            this.tsDataBits,
            this.tsStopBits,
            this.tsParity});
            this.statusStrip1.Location = new System.Drawing.Point(0, 609);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(920, 22);
            this.statusStrip1.TabIndex = 12;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tsSpNum
            // 
            this.tsSpNum.Name = "tsSpNum";
            this.tsSpNum.Size = new System.Drawing.Size(95, 17);
            this.tsSpNum.Text = "串口号：未指定|";
            // 
            // tsBaudRate
            // 
            this.tsBaudRate.Name = "tsBaudRate";
            this.tsBaudRate.Size = new System.Drawing.Size(86, 17);
            this.tsBaudRate.Text = "波特率:未指定|";
            // 
            // tsDataBits
            // 
            this.tsDataBits.Name = "tsDataBits";
            this.tsDataBits.Size = new System.Drawing.Size(86, 17);
            this.tsDataBits.Text = "数据位:未指定|";
            // 
            // tsStopBits
            // 
            this.tsStopBits.Name = "tsStopBits";
            this.tsStopBits.Size = new System.Drawing.Size(86, 17);
            this.tsStopBits.Text = "停止位:未指定|";
            // 
            // tsParity
            // 
            this.tsParity.Name = "tsParity";
            this.tsParity.Size = new System.Drawing.Size(86, 17);
            this.tsParity.Text = "停止位:未指定|";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label10.Location = new System.Drawing.Point(6, 105);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(77, 14);
            this.label10.TabIndex = 14;
            this.label10.Text = "速度设定：";
            // 
            // txtVel
            // 
            this.txtVel.Location = new System.Drawing.Point(83, 98);
            this.txtVel.Name = "txtVel";
            this.txtVel.Size = new System.Drawing.Size(67, 21);
            this.txtVel.TabIndex = 34;
            this.txtVel.Text = "500";
            // 
            // btnPosMoveToLimit
            // 
            this.btnPosMoveToLimit.Location = new System.Drawing.Point(9, 136);
            this.btnPosMoveToLimit.Name = "btnPosMoveToLimit";
            this.btnPosMoveToLimit.Size = new System.Drawing.Size(74, 25);
            this.btnPosMoveToLimit.TabIndex = 35;
            this.btnPosMoveToLimit.Text = "正移到限位";
            this.btnPosMoveToLimit.UseVisualStyleBackColor = true;
            this.btnPosMoveToLimit.Click += new System.EventHandler(this.btnPosMoveToLimit_Click);
            // 
            // btnNegMoveToLimit
            // 
            this.btnNegMoveToLimit.Location = new System.Drawing.Point(89, 136);
            this.btnNegMoveToLimit.Name = "btnNegMoveToLimit";
            this.btnNegMoveToLimit.Size = new System.Drawing.Size(82, 25);
            this.btnNegMoveToLimit.TabIndex = 36;
            this.btnNegMoveToLimit.Text = "逆移到限位";
            this.btnNegMoveToLimit.UseVisualStyleBackColor = true;
            this.btnNegMoveToLimit.Click += new System.EventHandler(this.btnNegMoveToLimit_Click);
            // 
            // btnMoveToZero
            // 
            this.btnMoveToZero.Location = new System.Drawing.Point(177, 136);
            this.btnMoveToZero.Name = "btnMoveToZero";
            this.btnMoveToZero.Size = new System.Drawing.Size(74, 25);
            this.btnMoveToZero.TabIndex = 37;
            this.btnMoveToZero.Text = "回零点";
            this.btnMoveToZero.UseVisualStyleBackColor = true;
            this.btnMoveToZero.Click += new System.EventHandler(this.btnMoveToZero_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.button2);
            this.groupBox4.Controls.Add(this.btnTwoAxisStop);
            this.groupBox4.Controls.Add(this.btnMotorIsRet);
            this.groupBox4.Controls.Add(this.btnMotorStop);
            this.groupBox4.Controls.Add(this.btnNegMoveNoDes);
            this.groupBox4.Controls.Add(this.btnPosMoveNoDes);
            this.groupBox4.Controls.Add(this.button1);
            this.groupBox4.Controls.Add(this.btnIn4);
            this.groupBox4.Controls.Add(this.btnZero);
            this.groupBox4.Controls.Add(this.label14);
            this.groupBox4.Controls.Add(this.btnPosLimit);
            this.groupBox4.Controls.Add(this.btnBackAndForthTest);
            this.groupBox4.Controls.Add(this.btnNegLimit);
            this.groupBox4.Controls.Add(this.label13);
            this.groupBox4.Controls.Add(this.btnGetStatus);
            this.groupBox4.Controls.Add(this.btnPauseMove);
            this.groupBox4.Controls.Add(this.btnAbsMove);
            this.groupBox4.Controls.Add(this.txtAbsPos);
            this.groupBox4.Controls.Add(this.txtEncoder);
            this.groupBox4.Controls.Add(this.btnMoveToZero);
            this.groupBox4.Controls.Add(this.label12);
            this.groupBox4.Controls.Add(this.btnNegMoveToLimit);
            this.groupBox4.Controls.Add(this.btnRelMove);
            this.groupBox4.Controls.Add(this.btnPosMoveToLimit);
            this.groupBox4.Controls.Add(this.txtIncrement);
            this.groupBox4.Controls.Add(this.txtVel);
            this.groupBox4.Controls.Add(this.label10);
            this.groupBox4.Controls.Add(this.label11);
            this.groupBox4.Location = new System.Drawing.Point(324, 11);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(575, 269);
            this.groupBox4.TabIndex = 38;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "X轴伺服电机控制";
            // 
            // btnTwoAxisStop
            // 
            this.btnTwoAxisStop.BackColor = System.Drawing.Color.Red;
            this.btnTwoAxisStop.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnTwoAxisStop.Location = new System.Drawing.Point(389, 3);
            this.btnTwoAxisStop.Name = "btnTwoAxisStop";
            this.btnTwoAxisStop.Size = new System.Drawing.Size(159, 72);
            this.btnTwoAxisStop.TabIndex = 56;
            this.btnTwoAxisStop.Text = "双轴急停";
            this.btnTwoAxisStop.UseVisualStyleBackColor = false;
            this.btnTwoAxisStop.Click += new System.EventHandler(this.btnTwoAxisStop_Click);
            // 
            // btnMotorIsRet
            // 
            this.btnMotorIsRet.Location = new System.Drawing.Point(486, 241);
            this.btnMotorIsRet.Name = "btnMotorIsRet";
            this.btnMotorIsRet.Size = new System.Drawing.Size(81, 23);
            this.btnMotorIsRet.TabIndex = 56;
            this.btnMotorIsRet.Text = "检测到复位";
            this.btnMotorIsRet.UseVisualStyleBackColor = true;
            // 
            // btnMotorStop
            // 
            this.btnMotorStop.Location = new System.Drawing.Point(389, 241);
            this.btnMotorStop.Name = "btnMotorStop";
            this.btnMotorStop.Size = new System.Drawing.Size(81, 23);
            this.btnMotorStop.TabIndex = 55;
            this.btnMotorStop.Text = "检测到停止";
            this.btnMotorStop.UseVisualStyleBackColor = true;
            // 
            // btnNegMoveNoDes
            // 
            this.btnNegMoveNoDes.Location = new System.Drawing.Point(337, 136);
            this.btnNegMoveNoDes.Name = "btnNegMoveNoDes";
            this.btnNegMoveNoDes.Size = new System.Drawing.Size(74, 25);
            this.btnNegMoveNoDes.TabIndex = 54;
            this.btnNegMoveNoDes.Text = "逆  移";
            this.btnNegMoveNoDes.UseVisualStyleBackColor = true;
            this.btnNegMoveNoDes.Click += new System.EventHandler(this.btnNegMoveNoDes_Click);
            // 
            // btnPosMoveNoDes
            // 
            this.btnPosMoveNoDes.Location = new System.Drawing.Point(257, 136);
            this.btnPosMoveNoDes.Name = "btnPosMoveNoDes";
            this.btnPosMoveNoDes.Size = new System.Drawing.Size(74, 25);
            this.btnPosMoveNoDes.TabIndex = 53;
            this.btnPosMoveNoDes.Text = "正   移";
            this.btnPosMoveNoDes.UseVisualStyleBackColor = true;
            this.btnPosMoveNoDes.Click += new System.EventHandler(this.btnPosMoveNoDes_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(293, 196);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(203, 37);
            this.button1.TabIndex = 52;
            this.button1.Text = "测试运动到红外位置停止";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnIn4
            // 
            this.btnIn4.Location = new System.Drawing.Point(293, 241);
            this.btnIn4.Name = "btnIn4";
            this.btnIn4.Size = new System.Drawing.Size(81, 23);
            this.btnIn4.TabIndex = 48;
            this.btnIn4.Text = "入四";
            this.btnIn4.UseVisualStyleBackColor = true;
            // 
            // btnZero
            // 
            this.btnZero.Location = new System.Drawing.Point(196, 241);
            this.btnZero.Name = "btnZero";
            this.btnZero.Size = new System.Drawing.Size(81, 23);
            this.btnZero.TabIndex = 47;
            this.btnZero.Text = "零位";
            this.btnZero.UseVisualStyleBackColor = true;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label14.Location = new System.Drawing.Point(15, 179);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(329, 14);
            this.label14.TabIndex = 51;
            this.label14.Text = "循环测试：（保证限位正常连接，否则有安全问题）";
            // 
            // btnPosLimit
            // 
            this.btnPosLimit.Location = new System.Drawing.Point(105, 241);
            this.btnPosLimit.Name = "btnPosLimit";
            this.btnPosLimit.Size = new System.Drawing.Size(81, 23);
            this.btnPosLimit.TabIndex = 46;
            this.btnPosLimit.Text = "正限位";
            this.btnPosLimit.UseVisualStyleBackColor = true;
            // 
            // btnBackAndForthTest
            // 
            this.btnBackAndForthTest.Location = new System.Drawing.Point(58, 196);
            this.btnBackAndForthTest.Name = "btnBackAndForthTest";
            this.btnBackAndForthTest.Size = new System.Drawing.Size(203, 37);
            this.btnBackAndForthTest.TabIndex = 50;
            this.btnBackAndForthTest.Text = "往复移动循环测试";
            this.btnBackAndForthTest.UseVisualStyleBackColor = true;
            this.btnBackAndForthTest.Click += new System.EventHandler(this.btnBackAndForthTest_Click);
            // 
            // btnNegLimit
            // 
            this.btnNegLimit.Location = new System.Drawing.Point(18, 241);
            this.btnNegLimit.Name = "btnNegLimit";
            this.btnNegLimit.Size = new System.Drawing.Size(81, 23);
            this.btnNegLimit.TabIndex = 45;
            this.btnNegLimit.Text = "负限位";
            this.btnNegLimit.UseVisualStyleBackColor = true;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label13.Location = new System.Drawing.Point(265, 81);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(77, 14);
            this.label13.TabIndex = 49;
            this.label13.Text = "编码器值：";
            // 
            // btnGetStatus
            // 
            this.btnGetStatus.Location = new System.Drawing.Point(161, 94);
            this.btnGetStatus.Name = "btnGetStatus";
            this.btnGetStatus.Size = new System.Drawing.Size(74, 25);
            this.btnGetStatus.TabIndex = 39;
            this.btnGetStatus.Text = "获取状态";
            this.btnGetStatus.UseVisualStyleBackColor = true;
            this.btnGetStatus.Click += new System.EventHandler(this.btnGetStatus_Click);
            // 
            // btnPauseMove
            // 
            this.btnPauseMove.Location = new System.Drawing.Point(257, 8);
            this.btnPauseMove.Name = "btnPauseMove";
            this.btnPauseMove.Size = new System.Drawing.Size(98, 65);
            this.btnPauseMove.TabIndex = 39;
            this.btnPauseMove.Text = "停止运动";
            this.btnPauseMove.UseVisualStyleBackColor = true;
            this.btnPauseMove.Click += new System.EventHandler(this.btnPauseMove_Click);
            // 
            // btnAbsMove
            // 
            this.btnAbsMove.Location = new System.Drawing.Point(161, 54);
            this.btnAbsMove.Name = "btnAbsMove";
            this.btnAbsMove.Size = new System.Drawing.Size(74, 25);
            this.btnAbsMove.TabIndex = 43;
            this.btnAbsMove.Text = "绝对运动";
            this.btnAbsMove.UseVisualStyleBackColor = true;
            this.btnAbsMove.Click += new System.EventHandler(this.btnAbsMove_Click);
            // 
            // txtAbsPos
            // 
            this.txtAbsPos.Location = new System.Drawing.Point(83, 54);
            this.txtAbsPos.Name = "txtAbsPos";
            this.txtAbsPos.Size = new System.Drawing.Size(63, 21);
            this.txtAbsPos.TabIndex = 42;
            this.txtAbsPos.Text = "10000";
            // 
            // txtEncoder
            // 
            this.txtEncoder.Location = new System.Drawing.Point(268, 98);
            this.txtEncoder.Name = "txtEncoder";
            this.txtEncoder.Size = new System.Drawing.Size(63, 21);
            this.txtEncoder.TabIndex = 44;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label12.Location = new System.Drawing.Point(6, 61);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(77, 14);
            this.label12.TabIndex = 41;
            this.label12.Text = "绝对位置：";
            // 
            // btnRelMove
            // 
            this.btnRelMove.Location = new System.Drawing.Point(161, 23);
            this.btnRelMove.Name = "btnRelMove";
            this.btnRelMove.Size = new System.Drawing.Size(74, 25);
            this.btnRelMove.TabIndex = 40;
            this.btnRelMove.Text = "增量运动";
            this.btnRelMove.UseVisualStyleBackColor = true;
            this.btnRelMove.Click += new System.EventHandler(this.btnRelMove_Click);
            // 
            // txtIncrement
            // 
            this.txtIncrement.Location = new System.Drawing.Point(83, 23);
            this.txtIncrement.Name = "txtIncrement";
            this.txtIncrement.Size = new System.Drawing.Size(63, 21);
            this.txtIncrement.TabIndex = 36;
            this.txtIncrement.Text = "1000";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label11.Location = new System.Drawing.Point(6, 30);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(70, 14);
            this.label11.TabIndex = 35;
            this.label11.Text = "增   量：";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.btnZMotorIsRet);
            this.groupBox5.Controls.Add(this.btnZMotorStop);
            this.groupBox5.Controls.Add(this.button6);
            this.groupBox5.Controls.Add(this.btnZIn4);
            this.groupBox5.Controls.Add(this.btnZZero);
            this.groupBox5.Controls.Add(this.label17);
            this.groupBox5.Controls.Add(this.btnZPosLimit);
            this.groupBox5.Controls.Add(this.btnZBackAndForthTest);
            this.groupBox5.Controls.Add(this.btnZNegLimit);
            this.groupBox5.Controls.Add(this.label18);
            this.groupBox5.Controls.Add(this.btnZGetStatus);
            this.groupBox5.Controls.Add(this.btnZPauseMove);
            this.groupBox5.Controls.Add(this.btnZAbsMove);
            this.groupBox5.Controls.Add(this.txtZAbsPos);
            this.groupBox5.Controls.Add(this.txtZEncoder);
            this.groupBox5.Controls.Add(this.btnZMoveToZero);
            this.groupBox5.Controls.Add(this.label19);
            this.groupBox5.Controls.Add(this.btnZNegMoveToLimit);
            this.groupBox5.Controls.Add(this.btnZRelMove);
            this.groupBox5.Controls.Add(this.btnZPosMoveToLimit);
            this.groupBox5.Controls.Add(this.txtZIncrement);
            this.groupBox5.Controls.Add(this.txtZVel);
            this.groupBox5.Controls.Add(this.label20);
            this.groupBox5.Controls.Add(this.label21);
            this.groupBox5.Location = new System.Drawing.Point(324, 322);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(575, 269);
            this.groupBox5.TabIndex = 39;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Z轴步进电机控制";
            // 
            // btnZMotorIsRet
            // 
            this.btnZMotorIsRet.Location = new System.Drawing.Point(257, 138);
            this.btnZMotorIsRet.Name = "btnZMotorIsRet";
            this.btnZMotorIsRet.Size = new System.Drawing.Size(81, 23);
            this.btnZMotorIsRet.TabIndex = 57;
            this.btnZMotorIsRet.Text = "检测到复位";
            this.btnZMotorIsRet.UseVisualStyleBackColor = true;
            // 
            // btnZMotorStop
            // 
            this.btnZMotorStop.Location = new System.Drawing.Point(389, 241);
            this.btnZMotorStop.Name = "btnZMotorStop";
            this.btnZMotorStop.Size = new System.Drawing.Size(81, 23);
            this.btnZMotorStop.TabIndex = 55;
            this.btnZMotorStop.Text = "检测到停止";
            this.btnZMotorStop.UseVisualStyleBackColor = true;
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(293, 196);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(203, 37);
            this.button6.TabIndex = 52;
            this.button6.Text = "测试运动到红外位置停止";
            this.button6.UseVisualStyleBackColor = true;
            // 
            // btnZIn4
            // 
            this.btnZIn4.Location = new System.Drawing.Point(293, 241);
            this.btnZIn4.Name = "btnZIn4";
            this.btnZIn4.Size = new System.Drawing.Size(81, 23);
            this.btnZIn4.TabIndex = 48;
            this.btnZIn4.Text = "入四";
            this.btnZIn4.UseVisualStyleBackColor = true;
            // 
            // btnZZero
            // 
            this.btnZZero.Location = new System.Drawing.Point(196, 241);
            this.btnZZero.Name = "btnZZero";
            this.btnZZero.Size = new System.Drawing.Size(81, 23);
            this.btnZZero.TabIndex = 47;
            this.btnZZero.Text = "零位";
            this.btnZZero.UseVisualStyleBackColor = true;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label17.Location = new System.Drawing.Point(15, 179);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(329, 14);
            this.label17.TabIndex = 51;
            this.label17.Text = "循环测试：（保证限位正常连接，否则有安全问题）";
            // 
            // btnZPosLimit
            // 
            this.btnZPosLimit.Location = new System.Drawing.Point(105, 241);
            this.btnZPosLimit.Name = "btnZPosLimit";
            this.btnZPosLimit.Size = new System.Drawing.Size(81, 23);
            this.btnZPosLimit.TabIndex = 46;
            this.btnZPosLimit.Text = "正限位";
            this.btnZPosLimit.UseVisualStyleBackColor = true;
            // 
            // btnZBackAndForthTest
            // 
            this.btnZBackAndForthTest.Location = new System.Drawing.Point(58, 196);
            this.btnZBackAndForthTest.Name = "btnZBackAndForthTest";
            this.btnZBackAndForthTest.Size = new System.Drawing.Size(203, 37);
            this.btnZBackAndForthTest.TabIndex = 50;
            this.btnZBackAndForthTest.Text = "往复移动循环测试";
            this.btnZBackAndForthTest.UseVisualStyleBackColor = true;
            this.btnZBackAndForthTest.Click += new System.EventHandler(this.btnZBackAndForthTest_Click);
            // 
            // btnZNegLimit
            // 
            this.btnZNegLimit.Location = new System.Drawing.Point(18, 241);
            this.btnZNegLimit.Name = "btnZNegLimit";
            this.btnZNegLimit.Size = new System.Drawing.Size(81, 23);
            this.btnZNegLimit.TabIndex = 45;
            this.btnZNegLimit.Text = "负限位";
            this.btnZNegLimit.UseVisualStyleBackColor = true;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label18.Location = new System.Drawing.Point(265, 81);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(77, 14);
            this.label18.TabIndex = 49;
            this.label18.Text = "编码器值：";
            // 
            // btnZGetStatus
            // 
            this.btnZGetStatus.Location = new System.Drawing.Point(161, 94);
            this.btnZGetStatus.Name = "btnZGetStatus";
            this.btnZGetStatus.Size = new System.Drawing.Size(74, 25);
            this.btnZGetStatus.TabIndex = 39;
            this.btnZGetStatus.Text = "获取状态";
            this.btnZGetStatus.UseVisualStyleBackColor = true;
            this.btnZGetStatus.Click += new System.EventHandler(this.btnZGetStatus_Click);
            // 
            // btnZPauseMove
            // 
            this.btnZPauseMove.Location = new System.Drawing.Point(348, 54);
            this.btnZPauseMove.Name = "btnZPauseMove";
            this.btnZPauseMove.Size = new System.Drawing.Size(98, 65);
            this.btnZPauseMove.TabIndex = 39;
            this.btnZPauseMove.Text = "停止运动";
            this.btnZPauseMove.UseVisualStyleBackColor = true;
            this.btnZPauseMove.Click += new System.EventHandler(this.btnZPauseMove_Click);
            // 
            // btnZAbsMove
            // 
            this.btnZAbsMove.Location = new System.Drawing.Point(161, 54);
            this.btnZAbsMove.Name = "btnZAbsMove";
            this.btnZAbsMove.Size = new System.Drawing.Size(74, 25);
            this.btnZAbsMove.TabIndex = 43;
            this.btnZAbsMove.Text = "绝对运动";
            this.btnZAbsMove.UseVisualStyleBackColor = true;
            this.btnZAbsMove.Click += new System.EventHandler(this.btnZAbsMove_Click);
            // 
            // txtZAbsPos
            // 
            this.txtZAbsPos.Location = new System.Drawing.Point(83, 54);
            this.txtZAbsPos.Name = "txtZAbsPos";
            this.txtZAbsPos.Size = new System.Drawing.Size(63, 21);
            this.txtZAbsPos.TabIndex = 42;
            this.txtZAbsPos.Text = "10000";
            // 
            // txtZEncoder
            // 
            this.txtZEncoder.Location = new System.Drawing.Point(268, 98);
            this.txtZEncoder.Name = "txtZEncoder";
            this.txtZEncoder.Size = new System.Drawing.Size(63, 21);
            this.txtZEncoder.TabIndex = 44;
            // 
            // btnZMoveToZero
            // 
            this.btnZMoveToZero.Location = new System.Drawing.Point(177, 136);
            this.btnZMoveToZero.Name = "btnZMoveToZero";
            this.btnZMoveToZero.Size = new System.Drawing.Size(74, 25);
            this.btnZMoveToZero.TabIndex = 37;
            this.btnZMoveToZero.Text = "回零点";
            this.btnZMoveToZero.UseVisualStyleBackColor = true;
            this.btnZMoveToZero.Click += new System.EventHandler(this.btnZMoveToZero_Click);
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label19.Location = new System.Drawing.Point(6, 61);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(77, 14);
            this.label19.TabIndex = 41;
            this.label19.Text = "绝对位置：";
            // 
            // btnZNegMoveToLimit
            // 
            this.btnZNegMoveToLimit.Location = new System.Drawing.Point(89, 136);
            this.btnZNegMoveToLimit.Name = "btnZNegMoveToLimit";
            this.btnZNegMoveToLimit.Size = new System.Drawing.Size(82, 25);
            this.btnZNegMoveToLimit.TabIndex = 36;
            this.btnZNegMoveToLimit.Text = "逆移到限位";
            this.btnZNegMoveToLimit.UseVisualStyleBackColor = true;
            this.btnZNegMoveToLimit.Click += new System.EventHandler(this.btnZNegMoveToLimit_Click);
            // 
            // btnZRelMove
            // 
            this.btnZRelMove.Location = new System.Drawing.Point(161, 23);
            this.btnZRelMove.Name = "btnZRelMove";
            this.btnZRelMove.Size = new System.Drawing.Size(74, 25);
            this.btnZRelMove.TabIndex = 40;
            this.btnZRelMove.Text = "增量运动";
            this.btnZRelMove.UseVisualStyleBackColor = true;
            this.btnZRelMove.Click += new System.EventHandler(this.btnZRelMove_Click);
            // 
            // btnZPosMoveToLimit
            // 
            this.btnZPosMoveToLimit.Location = new System.Drawing.Point(9, 136);
            this.btnZPosMoveToLimit.Name = "btnZPosMoveToLimit";
            this.btnZPosMoveToLimit.Size = new System.Drawing.Size(74, 25);
            this.btnZPosMoveToLimit.TabIndex = 35;
            this.btnZPosMoveToLimit.Text = "正移到限位";
            this.btnZPosMoveToLimit.UseVisualStyleBackColor = true;
            this.btnZPosMoveToLimit.Click += new System.EventHandler(this.btnZPosMoveToLimit_Click);
            // 
            // txtZIncrement
            // 
            this.txtZIncrement.Location = new System.Drawing.Point(83, 23);
            this.txtZIncrement.Name = "txtZIncrement";
            this.txtZIncrement.Size = new System.Drawing.Size(63, 21);
            this.txtZIncrement.TabIndex = 36;
            this.txtZIncrement.Text = "1000";
            // 
            // txtZVel
            // 
            this.txtZVel.Location = new System.Drawing.Point(83, 98);
            this.txtZVel.Name = "txtZVel";
            this.txtZVel.Size = new System.Drawing.Size(67, 21);
            this.txtZVel.TabIndex = 34;
            this.txtZVel.Text = "1500";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label20.Location = new System.Drawing.Point(6, 105);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(77, 14);
            this.label20.TabIndex = 14;
            this.label20.Text = "速度设定：";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label21.Location = new System.Drawing.Point(6, 30);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(70, 14);
            this.label21.TabIndex = 35;
            this.label21.Text = "增   量：";
            // 
            // serialPortZ
            // 
            this.serialPortZ.PortName = "COM2";
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.button2.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button2.Location = new System.Drawing.Point(389, 80);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(159, 52);
            this.button2.TabIndex = 57;
            this.button2.Text = "上电初始化";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(920, 631);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Name = "TestForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "串口通信助手";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox cbSerial;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSwitch;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.RadioButton rbRcvStr;
        private System.Windows.Forms.RadioButton rbRcv16;
        private System.Windows.Forms.RadioButton rdSendStr;
        //private System.Windows.Forms.RadioButton rdse;
        private System.Windows.Forms.TextBox txtSecond;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox cbTimeSend;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.RadioButton radio1;
        private System.Windows.Forms.RichTextBox txtReceive;
        private System.Windows.Forms.Timer tmSend;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.ComboBox cbBaudRate;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cbDataBits;
        private System.Windows.Forms.Label label6;
        private System.IO.Ports.SerialPort serialPortX;
        private System.Windows.Forms.ComboBox cbStop;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cbParity;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tsSpNum;
        private System.Windows.Forms.ToolStripStatusLabel tsBaudRate;
        private System.Windows.Forms.ToolStripStatusLabel tsDataBits;
        private System.Windows.Forms.ToolStripStatusLabel tsStopBits;
        private System.Windows.Forms.ToolStripStatusLabel tsParity;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnReceive;
        private System.Windows.Forms.TextBox txtSend;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtVel;
        private System.Windows.Forms.Button btnPosMoveToLimit;
        private System.Windows.Forms.Button btnNegMoveToLimit;
        private System.Windows.Forms.Button btnMoveToZero;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox txtIncrement;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button btnPauseMove;
        private System.Windows.Forms.Button btnRelMove;
        private System.Windows.Forms.Button btnAbsMove;
        private System.Windows.Forms.TextBox txtAbsPos;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button btnGetStatus;
        private System.Windows.Forms.TextBox txtEncoder;
        private System.Windows.Forms.Button btnNegLimit;
        private System.Windows.Forms.Button btnPosLimit;
        private System.Windows.Forms.Button btnZero;
        private System.Windows.Forms.Button btnIn4;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button btnBackAndForthTest;
        private System.Windows.Forms.Button btnSwitchTwo;
        private System.Windows.Forms.ComboBox cbSerialTwo;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.ComboBox cboSerialChoose;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnNegMoveNoDes;
        private System.Windows.Forms.Button btnPosMoveNoDes;
        private System.Windows.Forms.Button btnMotorStop;
        private System.Windows.Forms.Button btnMotorIsRet;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button btnZMotorStop;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button btnZIn4;
        private System.Windows.Forms.Button btnZZero;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Button btnZPosLimit;
        private System.Windows.Forms.Button btnZBackAndForthTest;
        private System.Windows.Forms.Button btnZNegLimit;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Button btnZGetStatus;
        private System.Windows.Forms.Button btnZPauseMove;
        private System.Windows.Forms.Button btnZAbsMove;
        private System.Windows.Forms.TextBox txtZAbsPos;
        private System.Windows.Forms.TextBox txtZEncoder;
        private System.Windows.Forms.Button btnZMoveToZero;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Button btnZNegMoveToLimit;
        private System.Windows.Forms.Button btnZRelMove;
        private System.Windows.Forms.Button btnZPosMoveToLimit;
        private System.Windows.Forms.TextBox txtZIncrement;
        private System.Windows.Forms.TextBox txtZVel;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Button btnTwoAxisStop;
        private System.IO.Ports.SerialPort serialPortZ;
        private System.Windows.Forms.Button btnZMotorIsRet;
        private System.Windows.Forms.Button button2;
    }
}

