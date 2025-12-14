using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using CommonLibrary;
using System.Timers;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;

namespace MotorLibrary
{
    //定义事件委托
    //public delegate int gpEventHandler(out int XPos, out int ZPos);
    public delegate int pnEventHandler(int ID);
    public delegate int tnEventHandler(int para);
    public class MotorClass : PortParSet
    {
        //PC端串口向控制器发送取状态信息定时器
        System.Timers.Timer GetStatusTimer = new System.Timers.Timer();
        private ConfigClass motor_newConfig = new ConfigClass();
        private Thread InitMotorThr;//用于测试  运动到某个位置停止  线程
        private Thread TriggerMotorThr;//用于测试  运动到某个位置停止  线程
        private bool blOpenXAxis = false;
        private bool blOpenZAxis = false;
        // 内部 使用
        private SerialPort m_serialX = new SerialPort();
        // 内部使用
        private SerialPort m_serialZ = new SerialPort();
        private SerialPort m_serialRelay = new SerialPort();

        const int DEFAULT_INTERVAL = 40;

        // 你不要用
        private int m_currentX = 0;
        // 你不要用
        private int m_currentZ = 0;
        // 光源控制，暂时还放在你这里
        private OPTControllerAPI m_oPTController = null;
        // 定义一个显示事件
        public event smEventHandler showMessageEvent;
        // 定义一个获取电机位置事件
        //public event gpEventHandler getPoseEvent;
        // 管子到位触发事件
        public event pnEventHandler triggerNotifyEvent;//红外传感器触发之后  执行该事件
        // 电机位置改变事件
        public event pnEventHandler poseNotifyEvent;//0代表 X轴  1代表Z轴
        // 你用这个,给CurrentX赋值我那边就会同时更显编码器值
        public int CurrentX
        {
            get
            {
                return m_currentX;
            }
            set
            {
                m_currentX = value;
                poseNotifyEvent(0);
            }
        }
        // 你用这个,给CurrentZ赋值我那边就会同时更显编码器值
        public int CurrentZ
        {
            get
            {
                return m_currentZ;
            }
            set
            {
                m_currentZ = value;
                poseNotifyEvent(1);
            }
        }
        // 光源控制接口
        public OPTControllerAPI OPTController
        {
            set
            {
                m_oPTController = value;
            }
        }
        // 由你判断管子就绪了,执行这个函数
        public int triggerNotify()
        {
            // 管子就绪触发事件
            triggerNotifyEvent(0);
            return 0;
        }
        public bool blIsIoComOpen = false;
        public int OpenCOM(string portNum)
        {
            PortRelayParSetting(ref m_serialRelay, portNum);
            m_serialRelay.Close();
            try
            {
                if (!m_serialRelay.IsOpen)
                {
                    m_serialRelay.Open();
                    blIsIoComOpen = true;
                    OpenGetStatusTimer();
                    m_serialRelay.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.m_serialRelay_DataReceived);
                    showMessageEvent("串口" + portNum + "打开成功！", InfoType.Info);
                   
                    return 0;
                }
            }
            catch (Exception exception)
            {
                showMessageEvent("串口" + portNum + "打开失败！" + exception.Message, InfoType.Warn);
            }
            return -1;
        }
        public int CloseCOM(string portNum)
        {
            try
            {
                if (m_serialRelay.IsOpen)
                {
                    m_serialRelay.Close();
                    showMessageEvent("串口" + portNum + "关闭成功！", InfoType.Info);
                    blIsIoComOpen = false;
                    return 0;
                }
            }
            catch (Exception exception)
            {
                blIsIoComOpen = false;
                showMessageEvent(exception.Message, InfoType.Warn);
            }
            return -1;
        }
     

        // 返回0：表示成功（加上初始化）
        public int OpenMotorX(string motorAddressX)
        {
            PortXParSetting(ref m_serialX, motorAddressX);
            m_serialX.Close();
            try
            {
                if (!m_serialX.IsOpen)
                {
                    m_serialX.Open();
                    OpenGetStatusTimer();
                    m_serialX.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.serialPortX_DataReceived);
                    showMessageEvent("X轴初始化成功", InfoType.Info);
                    blOpenXAxis = true;//打开X轴标志位
                    return 0;
                }
            }
            catch (Exception exception)
            {
                // MessageBox.Show(exception.Message, "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CloseGetStatusTimer();
                showMessageEvent(exception.Message, InfoType.Warn);
            }
            return -1;
        }
        // 返回0：表示成功（加上初始化）
        public int OpenMotorZ(string motorAddressZ)
        {
            PortZParSetting(ref m_serialZ, motorAddressZ);
            m_serialZ.Close();
            try
            {
                if (!m_serialZ.IsOpen)
                {
                    m_serialZ.Open();
                    OpenGetStatusTimer();
                    m_serialZ.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.serialPortZ_DataReceived);
                    showMessageEvent("Z轴初始化成功", InfoType.Info);
                    blOpenZAxis = true;//打开Z轴标志位
                    return 0;
                }
            }
            catch (Exception exception)
            {
                // MessageBox.Show(exception.Message, "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CloseGetStatusTimer();
                showMessageEvent(exception.Message, InfoType.Warn);
            }
            return -1;
        }
        // 返回0：表示成功
        public int CloseMotorX()
        {
            try
            {
                if (m_serialX.IsOpen)
                {
                    m_serialX.Close();
                    CloseGetStatusTimer();
                    showMessageEvent("X轴关闭成功", InfoType.Info);
                    return 0;
                }
            }
            catch (Exception exception)
            {
                // MessageBox.Show(exception.Message, "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                showMessageEvent(exception.Message, InfoType.Warn);
            }
            return -1;
        }
        // 返回0：表示成功
        public int CloseMotorZ()
        {
            try
            {
                if (m_serialZ.IsOpen)
                {
                    m_serialZ.Close();
                    CloseGetStatusTimer();
                    showMessageEvent("Z轴关闭成功", InfoType.Info);
                    return 0;
                }
            }
            catch (Exception exception)
            {
                // MessageBox.Show(exception.Message, "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                showMessageEvent(exception.Message, InfoType.Warn);
            }
            return -1;
        }

        private void serialPortX_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Byte[] receivedData = ReceiveControlData(m_serialX);
            if (receivedData != null)
            {
                Int64 pos = GetFPosition(receivedData);//得到编码器的值
                CurrentX = (int)pos;
                if (CurrentX == 0)
                    blXFPosIsZero = true;
                else
                    blXFPosIsZero = false;
                GetXIoStatus(receivedData);//检测IO口的状态
                //GetIoStatus(receivedData, 0);//检测IO口的状态
            }
        }
        public int tunelIsInpos = 0;//表示油管是否到位  不到位为0 到位状态为1
        Byte[] receivedData_temp = new Byte[6];//用于接收串口返回至 主机的数据
        public int TunelCnt=0;
        private void m_serialRelay_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Byte[] receivedData = ReceiveControlData(m_serialRelay);
            if (receivedData != null)
            {
                for (int i = 0; i < receivedData.Length; i++)
                {
                    if (i + 5 <= receivedData.Length)
                    {
                        if (receivedData[i] == 0x01 && receivedData[i + 1] == 0x02 && receivedData[i + 2] == 0x01)
                        {
                            receivedData_temp[0] = receivedData[i];
                            receivedData_temp[1] = receivedData[i + 1];
                            receivedData_temp[2] = receivedData[i + 2];

                            receivedData_temp[3] = receivedData[i + 3];
                            receivedData_temp[4] = receivedData[i + 4];
                            receivedData_temp[5] = receivedData[i + 5];
                            break;
                        }
                    }
                }
            }
            if (receivedData_temp != null)
            {
                int redinf = (int)receivedData_temp[3];//表示x1-x4的状态 0001 表示 x1有输入
                if (redinf == 1)
                {
                    tunelIsInpos = 1;//表示到位
                    //TunelCnt++;
                }
                    
                else
                    tunelIsInpos = 0;
            }
        }
        private void serialPortZ_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Byte[] receivedData = ReceiveControlData(m_serialZ);
            if (receivedData != null)
            {
                Int64 pos = GetFPosition(receivedData);//得到编码器的值
                CurrentZ = (int)pos;
                if (CurrentZ == 0)
                    blZFPosIsZero = true;
                else
                    blZFPosIsZero = false;
                GetZIoStatus(receivedData);//检测IO口的状态
                //GetIoStatus(receivedData,1);//检测IO口的状态
            }
        }
        private Byte[] ReceiveControlData(SerialPort sp)
        {
            if (sp.IsOpen == false)//若未打开串口
            {
                showMessageEvent(sp.PortName + "串口未打开！！", InfoType.Warn);
            }
            try
            {
                if (sp.BytesToRead >= 6)
                {
                    Byte[] receivedData = new Byte[sp.BytesToRead];        //创建接收字节数组
                    sp.Read(receivedData, 0, receivedData.Length);         //读取数据                                                                                                   
                    sp.DiscardInBuffer();                                  //清空SerialPort控件的Buffer
                    return receivedData;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                showMessageEvent(ex.Message + sp.PortName + "Io板串口接收数据错误！！", InfoType.Error);
                return null;
            }
        }
        // 自动根据传感器寻找合适的拍照位置 0：找到 -1：找不到 -2：异常
        public int autoLocate()
        {
            //int XPos;
            //int ZPos;
            // 让相机告诉你到哪
            //getPoseEvent(out XPos, out ZPos);
            // 移动到哪
            // 返回我移动到合适位置了true，或者我实在是到不了合适位置false
            showMessageEvent("移动到合适位置");
            showMessageEvent("无法移动到合适位置", InfoType.Warn);
            return 0;
        }
        // 主程序调用该接口回传检测结果
        public int markResult(int level)
        {
            return 0;
        }
        // 紧急停止
        public int Stop()
        {
            if (blOpenXAxis)
            {
                MoveParFlag(3, 0, motor_newConfig);
            }
            if (blOpenZAxis)
            {
                MoveParFlag(3, 1, motor_newConfig);
            }
            showMessageEvent("紧急停止成功");
            return 0;
        }

        public int TurnOnLight()
        {
            if (m_oPTController == null)
            {
                showMessageEvent("光源未初始化！", InfoType.Warn);
                return -2;
            }
            if (m_oPTController.TurnOnChannel(1) == 0)
            {
                showMessageEvent("光源打开成功！");
                return 0;
            }
            else
            {
                showMessageEvent("光源打开失败！", InfoType.Warn);
                return -1;
            }
        }

        public int TurnOffLight()
        {
            if (m_oPTController == null)
            {
                showMessageEvent("光源未初始化！", InfoType.Warn);
                return -2;
            }
            if (m_oPTController.TurnOffChannel(1) == 0)
            {
                showMessageEvent("光源关闭成功！");
                return 0;
            }
            else
            {
                showMessageEvent("光源关闭失败！", InfoType.Warn);
                return -1;
            }
        }

        public int XXX()
        {
            return 0;
        }
        public void MoveParFlag(int dir, int XorZ, ConfigClass config)
        {
            if (XorZ == 0)
            {
                blqueryXStaus = true;
            }
            else if (XorZ == 1)
            {
                blqueryZStaus = true;
            }
            MovePar(dir, XorZ, config);
        }
        /// <summary>
        /// 电机不同运动形式命令
        /// </summary>
        /// <param name="dir"> 几种操作</param>
        /// <param name="XorZ"> 0:X   1:Z   2:X和Z</param>
        /// <param name="config"> 所有配置</param>
        public void MovePar(int dir, int XorZ, ConfigClass config)
        {
            string VelX = config.m_motorSpeedX.ToString();
            string VelZ = config.m_motorSpeedZ.ToString();

            if (XorZ == 0)//X
            {
                blXMotorIsStop = false;//发送运动指令时，将运动标志位改变
                string[] strMoveParFlagX = GetProtocolPar(VelX, dir, 0, config);
                SendRSData(strMoveParFlagX, m_serialX);
                //config.Delay(50);
            }
            else if (XorZ == 1)
            {
                blZMotorIsStop = false;//发送运动指令时，将运动标志位改变
                string[] strMoveParFlagZ = GetProtocolPar(VelZ, dir, 1, config);
                SendRSData(strMoveParFlagZ, m_serialZ);
            }
        }

        /*
         定义：输出不同的颜色对应打开不同的继电器
                       Y0 Y1  Y2  Y3
                红色   1  0   0   0        01
                黄色   0  1   0   0        02
                蓝色   0  0   1   0        04
                绿色   0  0   0   1        08  
                白色   0  0   1   1        0B       
                默认   0  0   0   0        00
        */
        /// <summary>
        /// 用于控制Y1-Y4的继电器状态
        /// </summary>
        /// <param name="strColor">表示 红黄蓝绿白 五种颜色 CloseAll表示关闭所有继电器</param>
        /// <returns>1：表示成功 -1表示不成功</returns>
        public int OutputColorResult(string strColor)
        {
            string[] color;
            //if (strColor == "Red")
            //    color = CalCRCPariy(conCloseAllRelayStr);
            //else if (strColor == "Green")
            //    color = conOpenY1RelayStr;
            //else if (strColor == "CloseAll")
            //    color = CalCRCPariy(conCloseAllRelayStr);
            //else if (strColor == "无色环")
            //    color = CalCRCPariy(conCloseAllRelayStr);
            //else
            //    color = null;
            if (strColor == "Red")
                color = CalCRCPariy(conOpenY1RelayStr);
            else if (strColor == "Yellow")
                color = CalCRCPariy(conCloseAllRelayStr);
            else if (strColor == "Blue")
                color = CalCRCPariy(conCloseAllRelayStr);
            else if (strColor == "Green")
                color = CalCRCPariy(conCloseAllRelayStr);
            else if (strColor == "White")
                color = CalCRCPariy(conCloseAllRelayStr);
            else if (strColor == "CloseAll")
                color = CalCRCPariy(conCloseAllRelayStr);
            else if (strColor == "无色环")
                color = CalCRCPariy(conCloseAllRelayStr);
            else
                color = null;
            if (color != null)
            {
                SendRSData(color, m_serialRelay);
                return 1;
            }
            else
            {
                return -1;
            }

        }

        /// <summary>
        /// 根据输出指令名称控制继电器（可配置版本）
        /// </summary>
        /// <param name="command">Y1/Y2/Y3/Y4/Y3&Y4/CloseAll</param>
        /// <returns>1：成功 -1：失败</returns>
        public int OutputByCommand(string command)
        {
            string[] cmdData;
            switch (command)
            {
                case "Y1":
                    cmdData = CalCRCPariy(conOpenY1RelayStr);
                    break;
                case "Y2":
                    cmdData = CalCRCPariy(conOpenY2RelayStr);
                    break;
                case "Y3":
                    cmdData = CalCRCPariy(conOpenY3RelayStr);
                    break;
                case "Y4":
                    cmdData = CalCRCPariy(conOpenY4RelayStr);
                    break;
                case "Y3&Y4":
                    cmdData = CalCRCPariy(conOpenY3AndY4RelayStr);
                    break;
                case "CloseAll":
                    cmdData = CalCRCPariy(conCloseAllRelayStr);
                    break;
                default:
                    cmdData = null;
                    break;
            }
            if (cmdData != null)
            {
                SendRSData(cmdData, m_serialRelay);
                return 1;
            }
            else
            {
                return -1;
            }
        }
        public void OpenY1ContinuTime(int time)
        {
            string[] Y1ConStr = GeynerateOpenContinueTimeY1Con(time);
            SendRSData(Y1ConStr, m_serialRelay);
        }
        private void theout(object source, System.Timers.ElapsedEventArgs e)
        {
            //if (blOpenXAxis && blqueryXStaus)
            //{
            //    SendRSData(conGetStatusStr, m_serialX);
            //}
            //if (blOpenZAxis && blqueryZStaus)
            //{
            //    SendRSData(conGetStatusStr, m_serialZ);
            //}
            //SendRSData(conGetRelayStatusStr, m_serialRelay);
            if(blIsIoComOpen)
            {
                SendRSData(conGetXInputStatusStr, m_serialRelay);
            }

        }
        //----------串口发送数据（给控制器发送指令）------------------
        public void SendRSData(string[] strMoveParFlag, SerialPort serialport)
        {
            //处理数字转换
            string[] strArray = strMoveParFlag;
            int byteBufferCnt = 0;
            int byteBufferLength = strArray.Length;
            byte[] byteBuffer = new byte[byteBufferLength];
            try
            {
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
                    byteBuffer[byteBufferCnt] = Convert.ToByte(decNum);

                    byteBufferCnt++;
                }
                serialport.Write(byteBuffer, 0, byteBuffer.Length);
            }
            catch (Exception ex)
            {
                showMessageEvent(ex + "串口发送数据出错！", InfoType.Error);
                return;
            }
        }

        private void OpenGetStatusTimer()
        {
            GetStatusTimer.Elapsed += new ElapsedEventHandler(theout);  //到达时间的时候执行事件；
            GetStatusTimer.Interval = 200;
            GetStatusTimer.Enabled = true;
        }
        private void CloseGetStatusTimer()
        {
            GetStatusTimer.Enabled = false;
            blOpenXAxis = false;
            blOpenZAxis = false;
        }
        bool bl, blx, blz;
        public void InitMotor(ConfigClass m_newConfig)
        {
            MoveParFlag(2, 0, m_newConfig);//x轴左
            MoveParFlag(2, 1, m_newConfig);//z轴下
            motor_newConfig = m_newConfig;
            bl = false;
            blx = true;
            blz = true;
            InitMotorThr = new Thread(new ThreadStart(InMotor));
            InitMotorThr.Name = "上电初始化线程";
            InitMotorThr.Start();
        }
        public void TransforConfig(ConfigClass m_newConfig)
        {
            motor_newConfig = m_newConfig;
        }
        bool bltest = false;
        int cnt = 0;
        public void TestXZAxisBackAndForth()
        {
            MoveParFlag(2, 0, motor_newConfig);//x轴左
            MoveParFlag(2, 1, motor_newConfig);//z轴下
            bl = false;
            bltest = false; ;
            InitMotorThr = new Thread(new ThreadStart(TestMotor));
            InitMotorThr.Name = "上电初始化线程";
            InitMotorThr.Start();
        }
        public void StopTestXZAxisBackAndForth()
        {
            cnt = 0;
            bl = true;
            InitMotorThr.Abort();
            MoveParFlag(3, 0, motor_newConfig);
            MoveParFlag(3, 1, motor_newConfig);
        }

        private void TestMotor()
        {
            while (!bl)
            {
                //当停止运动时，X回待机点
                if (blXMotorIsStop && blZMotorIsStop && !bltest)
                {
                    m_oPTController.TurnOnChannel(1);
                    Thread.Sleep(400);
                    m_oPTController.TurnOffChannel(1);
                    Thread.Sleep(400);
                    MoveParFlag(8, 0, motor_newConfig);
                    MoveParFlag(8, 1, motor_newConfig);
                    bltest = true;
                    cnt++;
                }
                if (cnt == 1 && bltest && blXMotorIsStop && blZMotorIsStop)
                {
                    if (blXMotorIsStop && blZMotorIsStop)
                    {
                        m_oPTController.TurnOnChannel(1);
                        Thread.Sleep(200);
                        m_oPTController.TurnOffChannel(1);
                        Thread.Sleep(200);
                        Debug.WriteLine(cnt.ToString());
                    }
                    MoveParFlag(1, 0, motor_newConfig);
                    MoveParFlag(1, 1, motor_newConfig);
                    cnt++;
                }
                if (cnt == 2 && bltest && blXMotorIsStop && blZMotorIsStop)
                {
                    if (blXMotorIsStop && blZMotorIsStop)
                    {
                        m_oPTController.TurnOnChannel(1);
                        Thread.Sleep(200);
                        m_oPTController.TurnOffChannel(1);
                        Thread.Sleep(200);
                        Debug.WriteLine(cnt.ToString());
                    }
                    MoveParFlag(8, 1, motor_newConfig);
                    MoveParFlag(8, 0, motor_newConfig);
                    cnt = 1;
                }
            }
        }

        private void InMotor()
        {
            while (!bl)
            {
                //当停止运动时，X回待机点
                if (blXMotorIsStop && blx)
                {
                    MoveParFlag(8, 0, motor_newConfig);
                    blx = false;
                }
                if (blZMotorIsStop && blz)
                {
                    MoveParFlag(8, 1, motor_newConfig);
                    blz = false;
                }

                if (blZMotorIsStop && blXMotorIsStop && !blx && !blz)
                {
                    bl = true;
                    //MoveParFlag(3, 1, motor_newConfig);
                    //MoveParFlag(3, 0, motor_newConfig);
                    InitMotorThr.Abort();
                }
            }
            // InitMotorThr.Abort();
        }
        private bool blTesttigger = false;
        public void TriggerMotor(ConfigClass m_newConfig)
        {
            motor_newConfig = m_newConfig;
            MoveParFlag(1, 0, motor_newConfig);//X轴往右移动
            blTesttigger = true;
            TriggerMotorThr = new Thread(new ThreadStart(TestTriggerMotor));
            TriggerMotorThr.Name = "测试红外触发停止运动线程";
            TriggerMotorThr.Start();

        }

        private void TestTriggerMotor()
        {
            while (blTesttigger)
            {
                if (blSensorTrigger)//检测到红外触发
                {
                    MoveParFlag(3, 0, motor_newConfig);//停止运动
                    blTesttigger = false;
                    TriggerMotorThr.Abort();
                }
                if (blPosLimitX)//若没检测到红外触发  运动到了正限位
                {
                    MoveParFlag(3, 0, motor_newConfig);//停止运动
                    blTesttigger = false;
                    TriggerMotorThr.Abort();
                    MessageBox.Show("未检测到触发信号");
                }
            }
        }


    }
}
