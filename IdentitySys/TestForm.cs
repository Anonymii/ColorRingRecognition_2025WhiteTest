using System;
using System.IO.Ports;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Threading;

namespace IdentitySys
{
    public partial class TestForm : Form
    {
        //SerialPort serialPortX = new SerialPort();//代表串口1
        //SerialPort sp2 = new SerialPort();//代表串口2

        private SerialPort m_serialX;
        private SerialPort m_serialZ;
        private Int32 xEncoder;
        private Int32 zEncoder;

        private Thread backAndForyhThrX;//往复运动测试线程
        private Thread backAndForyhThrZ;//往复运动测试线程

        private Thread testPosThr;//用于测试  运动到某个位置停止  线程
        private Thread TestIoStatusThr;//实时监测IO口状态线程
        private bool blInfSensor=false;//用于测试红外传感器触发

        private bool blTestPortOneIoStatus= false;
        private bool blMotorIsInpos = false;
        private bool blmotorStop = false;//用于测试  运动停止
        private bool blbackForthTest = false;
        System.Windows.Forms.Timer GetStatueTimer = new System.Windows.Forms.Timer();
        //serialPortX.ReceivedBytesThreshold = 1;//只要有1个字符送达端口时便触发DataReceived事件 
        private int MaxVel = 8000;
        //正向移动和负向移动至零位
        private string[] conPosStr = {"0x55", "0xaa", "0x0b", "0x09", "", "", "0x00", "0x00", "0x00", "0xc3" };
        private string[] conNegStr = {"0x55", "0xaa", "0x0b", "0x0a", "", "", "0x00", "0x00", "0x00", "0xc3" };
        //回零点
        private string[] conToZeroStr = {"0x55", "0xaa", "0x07", "", "", "0x00", "0x00", "0x00", "0x00", "0xc3" };
        //暂停运动
        private string[] conPauseMoveStr = {"0x55", "0xaa", "0x02", "0x00", "0x00", "0x00", "0x00", "0x00", "0x00", "0xc3" };
        //增量运动
        private string[] conRelMoveStr = { "0x55", "0xaa", "0x08", "", "", "", "", "", "", "0xc3" };
        //绝对运动
        private string[] conAbsMoveStr = { "0x55", "0xaa", "0x07", "", "", "", "", "", "", "0xc3" };
        //获取状态
        private string[] conGetStatusStr = { "0x55", "0xaa", "0x0c", "0x00", "0x00", "0x00", "0x00", "0x00", "0x00", "0xc3" };
        //正向运动不指定终点
        private string[] conPosMoveNoDesStr = { "0x55", "0xaa", "0x06", "0x09", "", "", "0x00", "0x00", "0x00", "0xc3" };
        //负向运动不指定终点
        private string[] conNegMoveNoDesStr = { "0x55", "0xaa", "0x06", "0x0A", "", "", "0x00", "0x00", "0x00", "0xc3" };


        public SerialPort SerialX
        {
            set
            {
                m_serialX = value;        
            }
        }
        public SerialPort SerialZ
        {
            set
            {
                m_serialZ = value;
            }
        }

        public TestForm()
        {
            InitializeComponent();
        }
        private void GetStatueTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (serialPortX.IsOpen)
                {
                    SendRSData(conGetStatusStr);
                   
                }
                if (serialPortZ.IsOpen)
                {
                    SendZRSData(conGetStatusStr);
     
                }
            }
            catch (Exception)
            {
                GetStatueTimer.Enabled = false;
                MoveParFlag(3,serialPortX);
                MoveParFlag(3, serialPortZ);
                blbackForthTest = false;          
            }          
        }
        private void Form1_Load(object sender, EventArgs e)
        {                  
            Init();
        }

        private void Init()
        {
            GetStatueTimer.Interval = 200;
            GetStatueTimer.Tick += new EventHandler(GetStatueTimer_Tick);
            GetStatueTimer.Enabled = false;

            //检查是否含有串口
            string[] str = SerialPort.GetPortNames();
            if (str.Length == 0)
            {
                MessageBox.Show("没有检测到本地串口！", "错误提示！！");
                return;
            }

            //添加串口项目
            foreach (string s in System.IO.Ports.SerialPort.GetPortNames())
            {
                string ss = s;
                //获取有多少个COM口
                cbSerial.Items.Add(s);
                cbSerialTwo.Items.Add(s);
                cboSerialChoose.Items.Add(s);
            }

            //串口设置默认选择项
            cbSerial.SelectedIndex = 0;
            cbSerialTwo.SelectedIndex = 0;
            cboSerialChoose.SelectedIndex = 0;
            cbBaudRate.SelectedIndex = 8;
            cbDataBits.SelectedIndex = 3;
            cbStop.SelectedIndex = 0;
            cbParity.SelectedIndex = 0;


            //不检查跨线程的调用是否合法
            Control.CheckForIllegalCrossThreadCalls = false;
            
            serialPortX.DataReceived += new SerialDataReceivedEventHandler(serialPortX_DataReceived);
            serialPortX.ReceivedBytesThreshold = 8; //serialPortX参数设置,这里是接收1个数据便促发接收处理程序

            serialPortZ.DataReceived += new SerialDataReceivedEventHandler(serialPortZ_DataReceived);
            serialPortZ.ReceivedBytesThreshold = 8; //serialPortX参数设置,这里是接收1个数据便促发接收处理程序

            radio1.Checked = true;  //单选按钮默认是选中的
            rbRcv16.Checked = true;

            //准备就绪              
            serialPortX.DtrEnable = true;
            serialPortX.RtsEnable = true;
            //设置数据读取超时为1秒
            serialPortX.ReadTimeout = 1000;

            //准备就绪              
            serialPortZ.DtrEnable = true;
            serialPortZ.RtsEnable = true;
            //设置数据读取超时为1秒
            serialPortZ.ReadTimeout = 1000;

            serialPortX.Close();
            serialPortZ.Close();

            TestIoStatusThr = new Thread(new ThreadStart(DoTestIoStatusThr));
            TestIoStatusThr.IsBackground = true;
            TestIoStatusThr.Start();
        }

       

        private void DoTestIoStatusThr()
        {
            while(blTestPortOneIoStatus)//串口打开之后，便一直执行检测线程
            {
                if(blMotorIsInpos)
                {
                    MoveParFlag(7, serialPortX);//如果运动到检测的位置 则停止运动
                    blMotorIsInpos = false;
                }
            }
        }
        public Int32 GetFPosition(SerialPort sp)
        {
            Int32 pos;
            pos = (sp.PortName == serialPortX.PortName) ? xEncoder : zEncoder;
            return pos;
        }
       
        private void XReceiveData(SerialPort sp)
        {
            if (sp.IsOpen)     //判断是否打开串口
            {
                try
                {
                    if (sp.BytesToRead >= 8)//每次读到8个字节以上 才开始接受数据zyh
                    {
                        Byte[] receivedData = new Byte[sp.BytesToRead];        //创建接收字节数组
                        sp.Read(receivedData, 0, receivedData.Length);         //读取数据                                                                            
                        sp.DiscardInBuffer();                                  //清空SerialPort控件的Buffer

                        if (receivedData != null)
                        {
                            //用于显示编码器的值（receivedData前四个字节表示long型的坐标）
                            string byte1 = receivedData[0].ToString("x2");
                            string byte2 = receivedData[1].ToString("x2");
                            string byte3 = receivedData[2].ToString("x2");
                            string byte4 = receivedData[3].ToString("x2");
                            //将四个字节拼接并转化为 10进制
                            uint cc = Convert.ToUInt32(byte1 + byte2 + byte3 + byte4, 16);
                            xEncoder = (int)cc;
                            if (xEncoder.ToString() == txtAbsPos.Text)
                                blMotorIsInpos = true;
                            txtEncoder.Text = xEncoder.ToString();

                            //第七字节 用于获取输入状态bit2:正限  bit3: 零点  bit4:负限  bit5:入四
                            int status = receivedData[6];
                            int testInput = receivedData[7];
                            //int status = Convert.ToInt16((byte7.ToString("X2")), 16);
                            if (status == 221)   // 1101 1101  入四  
                            {
                                btnIn4.BackColor = Color.Red;
                                btnNegLimit.BackColor = Color.White;
                                btnPosLimit.BackColor = Color.White;
                                btnZero.BackColor = Color.White;
                                blInfSensor = true;
                            }
                            else if (status == 237) //1110 1101 负限位
                            {
                                btnIn4.BackColor = Color.White;
                                btnNegLimit.BackColor = Color.Red;
                                btnPosLimit.BackColor = Color.White;
                                btnZero.BackColor = Color.White;
                            }
                            else if (status == 245) // 1111 0101 零位  
                            {
                                btnIn4.BackColor = Color.White;
                                btnNegLimit.BackColor = Color.White;
                                btnPosLimit.BackColor = Color.White;
                                btnZero.BackColor = Color.Red;
                            }
                            else if (status == 249)  // 1111 1001 正限位  
                            {
                                btnIn4.BackColor = Color.White;
                                btnNegLimit.BackColor = Color.White;
                                btnPosLimit.BackColor = Color.Red;
                                btnZero.BackColor = Color.White;
                            }
                            else
                            {
                                btnIn4.BackColor = Color.White;
                                btnNegLimit.BackColor = Color.White;
                                btnPosLimit.BackColor = Color.White;
                                btnZero.BackColor = Color.White;
                            }
                            if (testInput == 32)  //0x32 表示停止
                            {
                                btnMotorStop.BackColor = Color.Red;
                                btnMotorIsRet.BackColor = Color.White;
                                blmotorStop = true;
                            }
                            else if (testInput == 0)//0x00  表示上电复位
                            {
                                btnMotorStop.BackColor = Color.White;
                                btnMotorIsRet.BackColor = Color.Red;
                            }
                            else
                            {
                                btnMotorStop.BackColor = Color.White;
                                btnMotorIsRet.BackColor = Color.White;
                            }
                        }
                        //string strRcv = null;
                        //string strRcv_temp = null;
                        //txtReceive.Text += "\r\n";
                        ////int decNum = 0;//存储十进制
                        //for (int i = 0; i < receivedData.Length; i++) //窗体显示
                        //{
                        //    strRcv += receivedData[i].ToString("x2");  //表示转化为小写16进制显示
                        //}
                        //strRcv_temp = InsertFormat(strRcv, 2, ",");
                        //txtReceive.Text += strRcv_temp;

                    }
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.Message, "出错提示");
                    txtSend.Text = "";
                }
            }
            else
            {
                MessageBox.Show("请打开某个串口", "错误提示");
            }
        }
        private void serialPortZ_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            ZReceiveData(serialPortZ);
        }
        private void serialPortX_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
           XReceiveData(serialPortX);
        }
        private void ZReceiveData(SerialPort sp)
        {
            if (sp.IsOpen)     //判断是否打开串口
            {
                try
                {
                    if (sp.BytesToRead >= 8)//每次读到8个字节以上 才开始接受数据zyh
                    {
                        Byte[] receivedData = new Byte[sp.BytesToRead];        //创建接收字节数组
                        sp.Read(receivedData, 0, receivedData.Length);         //读取数据                                                                            
                        sp.DiscardInBuffer();                                  //清空SerialPort控件的Buffer

                        if (receivedData != null)
                        {
                            //用于显示编码器的值（receivedData前四个字节表示long型的坐标）
                            string byte1 = receivedData[0].ToString("x2");
                            string byte2 = receivedData[1].ToString("x2");
                            string byte3 = receivedData[2].ToString("x2");
                            string byte4 = receivedData[3].ToString("x2");
                            //将四个字节拼接并转化为 10进制
                            uint cc = Convert.ToUInt32(byte1 + byte2 + byte3 + byte4, 16);
                            zEncoder = (int)cc;
                            if (zEncoder.ToString() == txtAbsPos.Text)
                                blMotorIsInpos = true;
                            txtZEncoder.Text = zEncoder.ToString();

                            //第七字节 用于获取输入状态bit2:正限  bit3: 零点  bit4:负限  bit5:入四
                            int status = receivedData[6];
                            int testInput = receivedData[7];
                            //int status = Convert.ToInt16((byte7.ToString("X2")), 16);
                            if (status == 221)   // 1101 1101  入四  
                            {
                                btnZIn4.BackColor = Color.Red;
                                btnZNegLimit.BackColor = Color.White;
                                btnZPosLimit.BackColor = Color.White;
                                btnZZero.BackColor = Color.White;
                                blInfSensor = true;
                            }
                            else if (status == 237) //1110 1101 负限位
                            {
                                btnZIn4.BackColor = Color.White;
                                btnZNegLimit.BackColor = Color.Red;
                                btnZPosLimit.BackColor = Color.White;
                                btnZZero.BackColor = Color.White;
                            }
                            else if (status == 245) // 1111 0101 零位  
                            {
                                btnZIn4.BackColor = Color.White;
                                btnZNegLimit.BackColor = Color.White;
                                btnZPosLimit.BackColor = Color.White;
                                btnZZero.BackColor = Color.Red;
                            }
                            else if (status == 249)  // 1111 1001 正限位  
                            {
                                btnZIn4.BackColor = Color.White;
                                btnZNegLimit.BackColor = Color.White;
                                btnZPosLimit.BackColor = Color.Red;
                                btnZZero.BackColor = Color.White;
                            }
                            else
                            {
                                btnZIn4.BackColor = Color.White;
                                btnZNegLimit.BackColor = Color.White;
                                btnZPosLimit.BackColor = Color.White;
                                btnZZero.BackColor = Color.White;
                            }
                            if (testInput == 32)  //0x32 表示停止
                            {
                                btnZMotorStop.BackColor = Color.Red;
                                btnZMotorIsRet.BackColor = Color.White;
                                blmotorStop = true;
                            }
                            else if (testInput == 0)//0x00  表示上电复位
                            {
                                btnZMotorStop.BackColor = Color.White;
                                btnZMotorIsRet.BackColor = Color.Red;
                            }
                            else
                            {
                                btnZMotorStop.BackColor = Color.White;
                            }


                        }
                        //string strRcv = null;
                        //string strRcv_temp = null;
                        //txtReceive.Text += "\r\n";
                        ////int decNum = 0;//存储十进制
                        //for (int i = 0; i < receivedData.Length; i++) //窗体显示
                        //{
                        //    strRcv += receivedData[i].ToString("x2");  //表示转化为小写16进制显示
                        //}
                        //strRcv_temp = InsertFormat(strRcv, 2, ",");
                        //txtReceive.Text += strRcv_temp;

                    }
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.Message, "出错提示");
                    txtSend.Text = "";
                }
            }
            else
            {
                MessageBox.Show("请打开某个串口", "错误提示");
            }
        }

        //发送按钮
        private void btnSend_Click(object sender, EventArgs e)
        {
            string PortName = cboSerialChoose.SelectedItem.ToString();
            SerialPort sp = ConfirmPort(PortName);
            SendDataForClickBtn(sp);
        }

        private SerialPort ConfirmPort(string PortName)
        {
            SerialPort sp = new SerialPort();
            if (PortName == serialPortX.PortName)
                sp = serialPortX;
            else if (PortName == serialPortZ.PortName)
                sp = serialPortZ;
            else
            {
                MessageBox.Show("串口信息错误，请检查！", "错误提示！");
            }
            return sp; 
        }
        /// <summary>
        /// 根据选择的端口名称发送数据
        /// </summary>
        /// <param name="sp"></param>
        /// <param name="PortName"></param>
        private void SendDataForClickBtn(SerialPort sp)
        {
            tmSend.Enabled = cbTimeSend.Checked ? true : false;
            if (!sp.IsOpen) //如果没打开
            {
                MessageBox.Show("请先打开串口！", "Error");
                return;
            }
            String strSend = txtSend.Text;
            if (radio1.Checked == true)	//“HEX发送” 按钮 
            {
                //处理数字转换
                string sendBuf = strSend;
                string sendnoNull = sendBuf.Trim();
                string sendNOComma = sendnoNull.Replace(',', ' ');    //去掉英文逗号
                string sendNOComma1 = sendNOComma.Replace('，', ' '); //去掉中文逗号
                string strSendNoComma2 = sendNOComma1.Replace("0x", "");   //去掉0x
                //strSendNoComma2.Replace("0X", "");   //去掉0X
                string[] strArray = strSendNoComma2.Split(' ');
                int ii = 0;
                int byteBufferLength = strArray.Length;
                byte[] byteBuffer = new byte[byteBufferLength];
                for (int i = 0; i < strArray.Length; i++)
                {
                    Byte[] bytesOfStr = Encoding.Default.GetBytes(strArray[i]);
                    int decNum = 0;
                    if (strArray[i] == "")
                    {
                        continue;
                    }
                    else
                    {
                        decNum = Convert.ToInt32(strArray[i], 16);
                    }
                    try    //防止输错，使其只能输入一个字节的字符
                    {
                        byteBuffer[ii] = Convert.ToByte(decNum);
                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.Show("字节越界，请逐个字节输入！", "Error");
                        tmSend.Enabled = false;
                        return;
                    }
                    ii++;
                }
                sp.Write(byteBuffer, 0, byteBuffer.Length);
            }
        }

        //开关按钮
        private void btnSwitch_Click(object sender, EventArgs e)
        {        
            string serialName = cbSerial.SelectedItem.ToString();
            OpenCOM(serialPortX, serialName);
        }

        //清空按钮
        private void btnClear_Click(object sender, EventArgs e)
        {
            txtReceive.Text = "";       //清空文本
        }
        //退出按钮
        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        //关闭时事件
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            serialPortX.Close();
            serialPortZ.Close();
        }

        //定时器
        private void tmSend_Tick(object sender, EventArgs e)
        {
            //转换时间间隔
            string strSecond = txtSecond.Text;
            try
            {
                int isecond = int.Parse(strSecond) * 1000;//Interval以微秒为单位
                tmSend.Interval = isecond;
                if (tmSend.Enabled == true)
                {
                    btnSend.PerformClick();
                }
            }
            catch (System.Exception ex)
            {
                tmSend.Enabled = false;
                MessageBox.Show("错误的定时输入！", "Error");
            }
            
        }

        private void txtSecond_KeyPress(object sender, KeyPressEventArgs e)
        {
            string patten = "[0-9]|\b"; //“\b”：退格键
            Regex r = new Regex(patten);
            Match m = r.Match(e.KeyChar.ToString());

            if (m.Success)
            {
                e.Handled = false;   //没操作“过”，系统会处理事件    
            }
            else
            {
                e.Handled = true;
            }
        }

        private void btnReceive_Click(object sender, EventArgs e)
        {
            string PortName = cboSerialChoose.SelectedItem.ToString();
            SerialPort sp = new SerialPort();
            if (PortName == serialPortX.PortName)
                sp = serialPortX;
            else if (PortName == serialPortZ.PortName)
                sp = serialPortZ;
            else
                MessageBox.Show("串口信息错误，请检查！", "错误提示！");
            RecicveData(sp, PortName);
        }
        private void RecicveData(SerialPort sp, string PortName)
        {
            if (sp.IsOpen)     //判断是否打开串口
            {
                if (rdSendStr.Checked)                          //'发送字符串'单选按钮
                {
                    txtReceive.Text += serialPortX.ReadLine() + "\r\n"; //注意：回车换行必须这样写，单独使用"\r"和"\n"都不会有效果
                    sp.DiscardInBuffer();                      //清空SerialPort控件的Buffer 
                }
                else                                            //'发送16进制按钮'
                {
                    try
                    {
                        txtReceive.Text += "\r\n";
                        Byte[] receivedData = new Byte[sp.BytesToRead];        //创建接收字节数组
                        sp.Read(receivedData, 0, receivedData.Length);         //读取数据
                        //string text = serialPortX.Read();   //Encoding.ASCII.GetString(receivedData);
                        sp.DiscardInBuffer();                                  //清空SerialPort控件的Buffer
                        if (receivedData.Length == 0)
                        {
                            MessageBox.Show("没有接收到数据，请检查");
                            return;
                        }
                        //这是用以显示字符串
                        //    string strRcv = null;
                        //    for (int i = 0; i < receivedData.Length; i++ )
                        //    {
                        //        strRcv += ((char)Convert.ToInt32(receivedData[i])) ;
                        //    }
                        //    txtReceive.Text += strRcv + "\r\n";             //显示信息
                        //}


                        string strRcv = null;
                        string strRcv_temp = null;
                        //int decNum = 0;//存储十进制
                        for (int i = 0; i < receivedData.Length; i++) //窗体显示
                        {
                            strRcv += receivedData[i].ToString("x2");  //表示转化为小写16进制显示
                        }
                        strRcv_temp = InsertFormat(strRcv, 2, ",");
                        txtReceive.Text += strRcv_temp;

                        txtReceive.Text += "\r\n";
                        if (receivedData != null)
                        {
                            //用于显示编码器的值（receivedData前四个字节表示long型的坐标）
                            string byte1 = receivedData[0].ToString("x2");
                            string byte2 = receivedData[1].ToString("x2");
                            string byte3 = receivedData[2].ToString("x2");
                            string byte4 = receivedData[3].ToString("x2");
                            //将四个字节拼接并转化为 10进制
                            //Int64 cc = Convert.ToInt64(byte1 + byte2 + byte3 + byte4, 16);
                            uint cc = Convert.ToUInt32(byte1 + byte2 + byte3 + byte4, 16);
                            Int32 wd = (int)cc;
                            txtReceive.Text += wd.ToString();

                        }
                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.Show(ex.Message, "出错提示");
                        txtSend.Text = "";
                    }
                }
            }
            else
            {
                MessageBox.Show("请打开某个串口", "错误提示");
            }
        }
        //---每隔n个字符插入一个字符-----
        public static string InsertFormat(string input, int interval, string value)
        {
            for (int i = interval; i < input.Length; i += interval + 1)
                input = input.Insert(i, value);
            return input;
        }

        //将界面速度值转化为2个十六进制数  例如 十进制2400 = 0x0960 转化为 0x60,0x09
        private string[] VelConvert(string vel)
        {
            string[] strVelRet = new string[2] {"","" };
            Int32 Vel = Convert.ToInt32(vel);
            string strVel = Vel.ToString("x4");// x表示转换的格式是16进制，4表示填充位为4位，不够4位补0。
            strVelRet[1] = strVel.Substring(0, 2);//高八位 09
            strVelRet[0] = strVel.Substring(2, 2);//低八位 60
                 
            return strVelRet;
        }

        //-----将速度参数组合进去获取串口协议参数---------------------------
        //---1代表正转动，0代表逆转，2代表回零点,3代表停止运动,4代表增量运动，5代表绝对运动
        private string[] GetProtocolPar(string vel,int dir, SerialPort sp)
        {
            string strIncPar = null;//增量运动
            string strAbsPar = null;//绝对运动
            if (sp.PortName == serialPortX.PortName)
            {
                strIncPar = txtIncrement.Text.Trim();
                strAbsPar = txtAbsPos.Text.Trim();
            }
            else if (sp.PortName == serialPortZ.PortName)
            {
                strIncPar = txtZIncrement.Text.Trim();
                strAbsPar = txtZAbsPos.Text.Trim();
            }
            string[] strVelRet = VelConvert(vel);
            if (dir == 1)//正向移动至限位
            {
                conPosStr[4] = strVelRet[0];
                conPosStr[5] = strVelRet[1];
                return conPosStr;
            }
            else if (dir == 0)//逆向移动至限位
            {
                conNegStr[4] = strVelRet[0];
                conNegStr[5] = strVelRet[1];
                return conNegStr;
            }
            else if (dir == 2)//回零点
            {
                conToZeroStr[3] = strVelRet[0];
                conToZeroStr[4] = strVelRet[1];
                return conToZeroStr;
            }
            else if (dir == 3) //暂停运动
            {
                return conPauseMoveStr;
            }
            else if (dir == 4) //增量运动
            {
                //速度设置
                conRelMoveStr[3] = strVelRet[0];
                conRelMoveStr[4] = strVelRet[1];              
                //增量设置
                string[] strIncParTemp = IncParConvert(strIncPar);
                if(strIncParTemp==null)
                {
                    MessageBox.Show("增量值设置不正确");
                }
                //---这个地方应该加判断  增量位置超出范围则。。。。。。注意：：
                for (int i = 0; i < strIncParTemp.Length; i++)
                {
                    conRelMoveStr[i + 5] = strIncParTemp[i];
                }
                return conRelMoveStr;

            }
            else if (dir == 5) //绝对运动
            {
                //速度设置
                conAbsMoveStr[3] = strVelRet[0];
                conAbsMoveStr[4] = strVelRet[1];
                string[] strAbsParTemp = IncParConvert(strAbsPar);
                for (int i = 0; i < strAbsParTemp.Length; i++)
                {
                    conAbsMoveStr[i+5] = strAbsParTemp[i];
                }
                return conAbsMoveStr;
            }
            else if (dir == 6) //正向运动无终点
            {
                conPosMoveNoDesStr[4] = strVelRet[0];
                conPosMoveNoDesStr[5] = strVelRet[1];
                return conPosMoveNoDesStr;
            }
            else if (dir == 7) //正向运动无终点
            {
                conNegMoveNoDesStr[4] = strVelRet[0];
                conNegMoveNoDesStr[5] = strVelRet[1];
                return conNegMoveNoDesStr;
            }
            else
            {
                return null;
            }                       
        }
        //-----用于增量运动和参数转换----------------------
        private string[] IncParConvert(string IncPar)
        {
 
            string[] strIncParRet = new string[4];
            Int32 Inc = Convert.ToInt32(IncPar);
            if(Inc>=0)
            {
                string strInc = Inc.ToString("x8");// x表示转换的格式是16进制，8表示填充位为8位，不够8位补0。    
                strIncParRet[3] = strInc.Substring(0, 2);
                strIncParRet[2] = strInc.Substring(2, 2);
                strIncParRet[1] = strInc.Substring(4, 2);
                strIncParRet[0] = strInc.Substring(6, 2);
                return strIncParRet;
            }
            else  if(Inc<0)        //若增量是负数
            {
                string strInc = int.Parse(IncPar).ToString("X8");
                strIncParRet[3] = strInc.Substring(0, 2);
                strIncParRet[2] = strInc.Substring(2, 2);
                strIncParRet[1] = strInc.Substring(4, 2);
                strIncParRet[0] = strInc.Substring(6, 2);
                return strIncParRet;
            }
            else
            {
                return null;
            }
            
        }

        private void MoveParFlag(int dir,SerialPort sp)
        {
            String Vel = "";
            if (!sp.IsOpen) //如果没打开
            {
                MessageBox.Show("请先打开串口！", "Error");
                return;
            }
            Vel = (sp.PortName == serialPortX.PortName) ? txtVel.Text.Trim() : txtZVel.Text.Trim();
            if (Convert.ToInt32(Vel)>MaxVel)
            {
                MessageBox.Show("速度值设置过大！","安全提示！");
                return;
            }
            if (Convert.ToInt32(Vel) < 0)
            {
                MessageBox.Show("速度值设置格式不正确！", "错误提示！");
                return;
            }
            string[] strMoveParFlag = GetProtocolPar(Vel,dir,sp);
            if (strMoveParFlag != null)
            {
                SendRSData(strMoveParFlag,sp);
            }
            else
            {
                MessageBox.Show("速度或者位置参数组合失败！请检查！");
            }
        }
       
        //-----发送数据-----------------
        private void SendRSData(string[] strMoveParFlag)
        {
            //处理数字转换
            string[] strArray = strMoveParFlag;
            int byteBufferCnt = 0;
            int byteBufferLength = strArray.Length;
            byte[] byteBuffer = new byte[byteBufferLength];
            for (int i = 0; i < strArray.Length; i++)
            {
                int decNum = 0;
                if (strArray[i] == "")
                {
                    continue;
                }
                else
                {
                    decNum = Convert.ToInt32(strArray[i], 16);
                }
                try    //防止输错，使其只能输入一个字节的字符
                {
                    byteBuffer[byteBufferCnt] = Convert.ToByte(decNum);
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("字节越界，请逐个字节输入！", "Error");
                    tmSend.Enabled = false;
                    return;
                }
                byteBufferCnt++;
            }
            serialPortX.Write(byteBuffer, 0, byteBuffer.Length);

        }

        private void SendZRSData(string[] strMoveParFlag)
        {          
            //处理数字转换
            string[] strArray = strMoveParFlag;
            int byteBufferCnt = 0;
            int byteBufferLength = strArray.Length;
            byte[] byteBuffer = new byte[byteBufferLength];
            for (int i = 0; i < strArray.Length; i++)
            {
                int decNum = 0;
                if (strArray[i] == "")
                {
                    continue;
                }
                else
                {
                    decNum = Convert.ToInt32(strArray[i], 16);
                }
                try    //防止输错，使其只能输入一个字节的字符
                {
                    byteBuffer[byteBufferCnt] = Convert.ToByte(decNum);
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("字节越界，请逐个字节输入！", "Error");
                    tmSend.Enabled = false;
                    return;
                }
                byteBufferCnt++;
            }
            serialPortZ.Write(byteBuffer, 0, byteBuffer.Length);
        }
        //-----发送数据-----------------
        private void SendRSData(string[] strMoveParFlag, SerialPort serialport)
        {
            //处理数字转换
            string[] strArray = strMoveParFlag;
            int byteBufferCnt = 0;
            int byteBufferLength = strArray.Length;
            byte[] byteBuffer = new byte[byteBufferLength];
            for (int i = 0; i < strArray.Length; i++)
            {
                Byte[] bytesOfStr = Encoding.Default.GetBytes(strArray[i]);
                int decNum = 0;
                if (strArray[i] == "")
                {
                    continue;
                }
                else
                {
                    decNum = Convert.ToInt32(strArray[i], 16);
                }
                try    //防止输错，使其只能输入一个字节的字符
                {
                    byteBuffer[byteBufferCnt] = Convert.ToByte(decNum);
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("字节越界，请逐个字节输入！", "Error");
                    tmSend.Enabled = false;
                    return;
                }
                byteBufferCnt++;
            }
            serialport.Write(byteBuffer, 0, byteBuffer.Length);
        }

        private void btnMoveToZero_Click(object sender, EventArgs e)
        {
            MoveParFlag(2, serialPortX);
        }

        private void btnNegMoveToLimit_Click(object sender, EventArgs e)
        {
            MoveParFlag(0, serialPortX);
        }

        private void btnPosMoveToLimit_Click(object sender, EventArgs e)
        {
            MoveParFlag(1, serialPortX); 
        }

        private void btnPauseMove_Click(object sender, EventArgs e)
        {
            blbackForthTest = false;
            MoveParFlag(3, serialPortX);
        }

        private void btnRelMove_Click(object sender, EventArgs e)
        {
            MoveParFlag(4, serialPortX);
        }

        private void btnAbsMove_Click(object sender, EventArgs e)
        {
            MoveParFlag(5, serialPortX);
        }

        private void btnGetStatus_Click(object sender, EventArgs e)
        {
            //SendRSData(conGetStatusStr);
        }

        //------------延时函数-----------------（单位是ms）-------------
        public static void Delay(int delayTime)
        {
            long span;
            Stopwatch myWatch = new Stopwatch();
            myWatch.Start();
            long start = myWatch.ElapsedMilliseconds;
            do
            {
                span = myWatch.ElapsedMilliseconds - start;
            } while (span < delayTime);
        }

        private void btnBackAndForthTest_Click(object sender, EventArgs e)
        {
            blbackForthTest = true;
            backAndForyhThrX = new Thread(new ThreadStart(BackAndForthTest));
            
            backAndForyhThrX.Name = "点击往复移动测试";
            backAndForyhThrX.Start();
        }

        private int MotorXAction = 0;
        private int MotorZAction = 0;
        private void BackAndForthTest()
        {
            int cnt = 0;
            MotorXAction = 0;
            while (blbackForthTest)
            {
                if(MotorXAction == 0)
                {
                    MoveParFlag(1, serialPortX);//正限位 
                    MotorXAction++;
                }
                while(cnt<=100000 && blbackForthTest)
                {
                    if (MotorXAction == 1 && btnPosLimit.BackColor == Color.Red)
                    {
                        MoveParFlag(0, serialPortX);//负限位 
                        MotorXAction++;
                        cnt++;
                    }
                    if (MotorXAction == 2 && btnNegLimit.BackColor == Color.Red)
                    {
                        MoveParFlag(1, serialPortX);//正限位  
                        MotorXAction=1;
                        cnt++;
                    }                  
                }             
                if (!blbackForthTest)
                    MoveParFlag(3, serialPortX);                                     
            }                              
        }

        private void btnSwitchTwo_Click(object sender, EventArgs e)
        {
            string serialName = cbSerialTwo.SelectedItem.ToString();
            OpenCOM(serialPortZ, serialName);
        }
        
        /// <summary>
        /// 打开串口
        /// </summary>
        /// <param name="sp"></param> 代表是哪个串口
        /// <param name="portNum"></param> 串口序号
        private void OpenCOM(SerialPort sp, string portNum)
        {
            if (!sp.IsOpen)
            {
                try
                {
                    sp.PortName = portNum;
                    //设置各“串口设置”
                    string strBaudRate = cbBaudRate.Text;
                    string strDateBits = cbDataBits.Text;
                    string strStopBits = cbStop.Text;
                    Int32 iBaudRate = Convert.ToInt32(strBaudRate);
                    Int32 iDateBits = Convert.ToInt32(strDateBits);

                    sp.BaudRate = iBaudRate;       //波特率
                    sp.DataBits = iDateBits;       //数据位
                    switch (cbStop.Text)            //停止位
                    {
                        case "1":
                            sp.StopBits = StopBits.One;
                            break;
                        case "1.5":
                            sp.StopBits = StopBits.OnePointFive;
                            break;
                        case "2":
                            sp.StopBits = StopBits.Two;
                            break;
                        default:
                            MessageBox.Show("Error：参数不正确!", "Error");
                            break;
                    }
                    switch (cbParity.Text)             //校验位
                    {
                        case "无":
                            sp.Parity = Parity.None;
                            break;
                        case "奇校验":
                            sp.Parity = Parity.Odd;
                            break;
                        case "偶校验":
                            sp.Parity = Parity.Even;
                            break;
                        default:
                            MessageBox.Show("Error：参数不正确!", "Error");
                            break;
                    }

                    if (sp.IsOpen == true)//如果打开状态，则先关闭一下
                    {
                        sp.Close();
                    }
                    //状态栏设置                   
                    //tsSpNum.Text += "串口号：" + sp.PortName + "|";
                    tsBaudRate.Text = "波特率：" + sp.BaudRate + "|";
                    tsDataBits.Text = "数据位：" + sp.DataBits + "|";
                    tsStopBits.Text = "停止位：" + sp.StopBits + "|";
                    tsParity.Text = "校验位：" + sp.Parity + "|";

                    //设置必要控件不可用
                    //cbSerial.Enabled = false;
                    cbBaudRate.Enabled = false;
                    cbDataBits.Enabled = false;
                    cbStop.Enabled = false;
                    cbParity.Enabled = false;

                    sp.Open();     //打开串口
                    blTestPortOneIoStatus = true;
                    GetStatueTimer.Enabled = true;
                    if (portNum == cbSerial.SelectedItem.ToString())
                    {
                        cbSerial.Enabled = false;
                        btnSwitch.Text = "关闭串口";
                    }
                    else if(portNum == cbSerialTwo.SelectedItem.ToString())
                    {
                        cbSerialTwo.Enabled = false;
                        btnSwitchTwo.Text = "关闭串口";
                    }
                  
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("Error:" + ex.Message, "Error");
                    tmSend.Enabled = false;
                    return;
                }
            }
            else
            {
                //状态栏设置
                tsSpNum.Text = "串口号：未指定|";
                tsBaudRate.Text = "波特率：未指定|";
                tsDataBits.Text = "数据位：未指定|";
                tsStopBits.Text = "停止位：未指定|";
                tsParity.Text = "校验位：未指定|";
                //恢复控件功能
                cbSerial.Enabled = true;
                cbSerialTwo.Enabled = true;
                cbBaudRate.Enabled = true;
                cbDataBits.Enabled = true;
                cbStop.Enabled = true;
                cbParity.Enabled = true;

                sp.Close();                    //关闭串口
                if (portNum == cbSerial.SelectedItem.ToString())
                {
                    btnSwitch.Text = "打开串口";
                    btnSwitch.Enabled = true;
                }
                else if (portNum == cbSerialTwo.SelectedItem.ToString())
                {
                    btnSwitchTwo.Text = "打开串口";
                    btnSwitchTwo.Enabled = true;
                }
                blTestPortOneIoStatus = false;
                GetStatueTimer.Enabled = false;
                tmSend.Enabled = false;         //关闭计时器
                GetStatueTimer.Enabled = false;
            }
        }

        private bool  blTestPos = false;
        private void button1_Click(object sender, EventArgs e)
        {           
            //Delay(1000);
            MoveParFlag(0, serialPortX);
            blTestPos = true;
            
            testPosThr = new Thread(new ThreadStart(testPosOnly));
            testPosThr.Name = "测试移动到某个位置线程";
            testPosThr.Start();
        }
        private void testPosOnly()
        {
            
            while (blTestPos)
            {                       
               if(blInfSensor)
                {
                    MoveParFlag(3, serialPortX);//停止运动
                    blInfSensor = false;
                    blTestPos = false;
                }  
               if(btnNegLimit.BackColor == Color.Red || btnPosLimit.BackColor == Color.Red)
                {
                    MoveParFlag(3, serialPortX);//停止运动
                    blInfSensor = false;
                    blTestPos = false;
                }                           
            }
        }

        private void btnPosMoveNoDes_Click(object sender, EventArgs e)
        {
            MoveParFlag(6, serialPortX);
        }

        private void btnNegMoveNoDes_Click(object sender, EventArgs e)
        {
            MoveParFlag(7, serialPortX);
        }

        private void btnZRelMove_Click(object sender, EventArgs e)
        {
            MoveParFlag(4, serialPortZ);
        }

        private void btnZAbsMove_Click(object sender, EventArgs e)
        {
            MoveParFlag(5, serialPortZ);
        }

        private void btnZGetStatus_Click(object sender, EventArgs e)
        {
           
        }

        private void btnZPosMoveToLimit_Click(object sender, EventArgs e)
        {
            MoveParFlag(1, serialPortZ);
        }

        private void btnZNegMoveToLimit_Click(object sender, EventArgs e)
        {
            MoveParFlag(0, serialPortZ);
        }

        private void btnZMoveToZero_Click(object sender, EventArgs e)
        {
            MoveParFlag(2, serialPortZ);
        }

        private void btnZPauseMove_Click(object sender, EventArgs e)
        {
            blbackForthTest = false;
            MoveParFlag(3, serialPortZ);
        }

        private void btnZBackAndForthTest_Click(object sender, EventArgs e)
        {
            blbackForthTest = true;
            backAndForyhThrZ = new Thread(new ThreadStart(BackAndForthZTest));

            backAndForyhThrZ.Name = "点击往复移动测试";
            backAndForyhThrZ.Start();
        }
        private void BackAndForthZTest()
        {
            int cnt = 0;
            MotorZAction = 0;
            while (blbackForthTest)
            {
                if (MotorXAction == 0)
                {
                    MoveParFlag(1, serialPortZ);//正限位 
                    MotorZAction++;
                }
                while (cnt <= 100000 && blbackForthTest)
                {
                    if (MotorZAction == 1 && btnZPosLimit.BackColor == Color.Red)
                    {
                        MoveParFlag(0, serialPortZ);//负限位 
                        MotorZAction++;
                        cnt++;
                    }
                    if (MotorZAction == 2 && btnZNegLimit.BackColor == Color.Red)
                    {
                        MoveParFlag(1, serialPortZ);//正限位  
                        MotorZAction = 1;
                        cnt++;
                    }
                }
                if (!blbackForthTest)
                    MoveParFlag(3, serialPortZ);
            }
        }

        private void btnTwoAxisStop_Click(object sender, EventArgs e)
        {
            blbackForthTest = false;
            MoveParFlag(3, serialPortX);
            MoveParFlag(3, serialPortZ);
        }
    }
}
