using CommonLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows.Forms;

namespace MotorLibrary
{
    //***************************************************************
    //
    //             用于上位机与控制器的串口通信协议
    //                                                  zhang yonghao
    //                                              2020.12.29
    //
    //****************************************************************
    public class PortParSet 
    {
       
        //用于串口参数设置
        private SerialPort SPortX, SPortZ,SPortRelay;
        private DataFormatConvertion myDataFormatConvertion  = new DataFormatConvertion();
        //电机参数
        private int MaxVel = 30000;//电机最大速度


        private Int32 Encoder;//显示电机编码器值
        //XZ轴的零点和限位 以及红外传感器
        public bool blPosLimitX, blNegLimitX, blZeroLimitX,blZeroLimitZ, blNegLimitZ, blPosLimitZ, blSensorTrigger,blXMotorIsStop, blZMotorIsStop;
        public bool blXFPosIsZero = false;
        public bool blZFPosIsZero = false;
        #region 串口参数设置
        //以下是串口参数设置
        private int baudRate = 115200;
        private StopBits stopBits = StopBits.One;
        private Parity parity = Parity.None;
        private int dataBits = 8;
        #endregion

        
        #region CS20-1电机控制器的串口指令
        //以下是CS20-1电机控制器的串口指令
        //正向移动和负向移动至零位
        private string[] conPosStr = { "0x55", "0xaa", "0x0b", "0x09", "", "", "0x00", "0x00", "0x00", "0xc3" };
        private string[] conNegStr = { "0x55", "0xaa", "0x0b", "0x0a", "", "", "0x00", "0x00", "0x00", "0xc3" };
        //回零点
        private string[] conToZeroStr = { "0x55", "0xaa", "0x07", "", "", "0x00", "0x00", "0x00", "0x00", "0xc3" };
        //暂停运动
        private string[] conPauseMoveStr = { "0x55", "0xaa", "0x02", "0x00", "0x00", "0x00", "0x00", "0x00", "0x00", "0xc3" };
        //增量运动
        private string[] conRelMoveStr = { "0x55", "0xaa", "0x08", "", "", "", "", "", "", "0xc3" };
        //绝对运动
        private string[] conAbsMoveStr = { "0x55", "0xaa", "0x07", "", "", "", "", "", "", "0xc3" };
        //获取状态
        public string[] conGetStatusStr = { "0x55", "0xaa", "0x0c", "0x00", "0x00", "0x00", "0x00", "0x00", "0x00", "0xc3" };
        //正向运动不指定终点
        private string[] conPosMoveNoDesStr = { "0x55", "0xaa", "0x06", "0x09", "", "", "0x00", "0x00", "0x00", "0xc3" };
        //负向运动不指定终点
        private string[] conNegMoveNoDesStr = { "0x55", "0xaa", "0x06", "0x0A", "", "", "0x00", "0x00", "0x00", "0xc3" };
        #endregion

        #region 四路继电器串口控制指令
        //获取继电器状态
        public string[] conGetRelayStatusStr = { "0x01", "0x01", "0x00", "0x00", "0x00", "0x04", "0x3d", "0xc9"};

        //获取X1-X4输入量
        public string[] conGetXInputStatusStr = { "0x01", "0x02", "0x00", "0x00", "0x00", "0x04", "0x79", "0xc9" };

        //Y1-Y4继电器开关的状态控制
        //public string[] conOpenY1RelayStr = { "0x01", "0x05", "0x00", "0x00", "0xff", "0x00", "0x8c", "0x3a" };
        public string[] conOpenY1RelayStr = { "0x01", "0x05", "0x00", "0x00", "0xff", "0x00", "", "" };
        public string[] conCloseY1RelayStr = { "0x01", "0x05", "0x00", "0x00", "0x00", "0x00", "0xcd", "0xca" };

        public string[] conOpenY2RelayStr = { "0x01", "0x05", "0x00", "0x1", "0xff", "0x00", "0xdd", "0xfa" };
        public string[] conCloseY2RelayStr = { "0x01", "0x05", "0x00", "0x1", "0x00", "0x00", "0x9c", "0x0a" };

        public string[] conOpenY3RelayStr = { "0x01", "0x05", "0x00", "0x2", "0xff", "0x00", "0x2d", "0xfa" };
        public string[] conCloseY3RelayStr = { "0x01", "0x05", "0x00", "0x2", "0x00", "0x00", "0x6c", "0x0a" };

        public string[] conOpenY4RelayStr = { "0x01", "0x05", "0x00", "0x3", "0xff", "0x00", "0x7c", "0x3a" };
        public string[] conCloseY4RelayStr = { "0x01", "0x05", "0x00", "0x3", "0x00", "0x00", "0x3d", "0xca" };

        //同步打开多个继电器状态
        public string[] conOpenY3AndY4RelayStr = { "0x01", "0x0f", "0x00", "0x00", "0x00", "0x0c", "", "" };
        public string[] conCloseAllRelayStr = { "0x01", "0x0f", "0x00", "0x00", "0x00", "0x00", "", "" };

        //设置继电器点动开关（延时指定时间关闭）
        public int Y1ContinueTime = 3;
        public string[] conOutputY1TimeRelayStr = { "0x01", "0x30", "0x00", "0x00", "", "", "", "" };
        public string[] conOutputY2TimeRelayStr = { "0x01", "0x30", "0x00", "0x01", "", "", "", "" };
        public string[] conOutputY3TimeRelayStr = { "0x01", "0x30", "0x00", "0x02", "", "", "", "" };
        public string[] conOutputY4TimeRelayStr = { "0x01", "0x30", "0x00", "0x03", "", "", "", "" };
        #endregion

        #region 四路继电器串口校验值

        //--------------------------------------------------------------------
        // CRC 高位字节值表
        //--------------------------------------------------------------------
        byte[] auchCRCHi = new byte[] {
            0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0,
            0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
            0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0,
            0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
            0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1,
            0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
            0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1,
            0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
            0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0,
            0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40,
            0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1,
            0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
            0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0,
            0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40,
            0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0,
            0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
            0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0,
            0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
            0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0,
            0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
            0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0,
            0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40,
            0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1,
            0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
            0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0,
            0x80, 0x41, 0x00, 0xC1, 0x81, 0x40
            };

        //--------------------------------------------------------------------
        // CRC 低位字节值表
        //--------------------------------------------------------------------
        byte[] auchCRCLo = new byte[] {
            0x00, 0xC0, 0xC1, 0x01, 0xC3, 0x03, 0x02, 0xC2, 0xC6, 0x06,
            0x07, 0xC7, 0x05, 0xC5, 0xC4, 0x04, 0xCC, 0x0C, 0x0D, 0xCD,
            0x0F, 0xCF, 0xCE, 0x0E, 0x0A, 0xCA, 0xCB, 0x0B, 0xC9, 0x09,
            0x08, 0xC8, 0xD8, 0x18, 0x19, 0xD9, 0x1B, 0xDB, 0xDA, 0x1A,
            0x1E, 0xDE, 0xDF, 0x1F, 0xDD, 0x1D, 0x1C, 0xDC, 0x14, 0xD4,
            0xD5, 0x15, 0xD7, 0x17, 0x16, 0xD6, 0xD2, 0x12, 0x13, 0xD3,
            0x11, 0xD1, 0xD0, 0x10, 0xF0, 0x30, 0x31, 0xF1, 0x33, 0xF3,
            0xF2, 0x32, 0x36, 0xF6, 0xF7, 0x37, 0xF5, 0x35, 0x34, 0xF4,
            0x3C, 0xFC, 0xFD, 0x3D, 0xFF, 0x3F, 0x3E, 0xFE, 0xFA, 0x3A,
            0x3B, 0xFB, 0x39, 0xF9, 0xF8, 0x38, 0x28, 0xE8, 0xE9, 0x29,
            0xEB, 0x2B, 0x2A, 0xEA, 0xEE, 0x2E, 0x2F, 0xEF, 0x2D, 0xED,
            0xEC, 0x2C, 0xE4, 0x24, 0x25, 0xE5, 0x27, 0xE7, 0xE6, 0x26,
            0x22, 0xE2, 0xE3, 0x23, 0xE1, 0x21, 0x20, 0xE0, 0xA0, 0x60,
            0x61, 0xA1, 0x63, 0xA3, 0xA2, 0x62, 0x66, 0xA6, 0xA7, 0x67,
            0xA5, 0x65, 0x64, 0xA4, 0x6C, 0xAC, 0xAD, 0x6D, 0xAF, 0x6F,
            0x6E, 0xAE, 0xAA, 0x6A, 0x6B, 0xAB, 0x69, 0xA9, 0xA8, 0x68,
            0x78, 0xB8, 0xB9, 0x79, 0xBB, 0x7B, 0x7A, 0xBA, 0xBE, 0x7E,
            0x7F, 0xBF, 0x7D, 0xBD, 0xBC, 0x7C, 0xB4, 0x74, 0x75, 0xB5,
            0x77, 0xB7, 0xB6, 0x76, 0x72, 0xB2, 0xB3, 0x73, 0xB1, 0x71,
            0x70, 0xB0, 0x50, 0x90, 0x91, 0x51, 0x93, 0x53, 0x52, 0x92,
            0x96, 0x56, 0x57, 0x97, 0x55, 0x95, 0x94, 0x54, 0x9C, 0x5C,
            0x5D, 0x9D, 0x5F, 0x9F, 0x9E, 0x5E, 0x5A, 0x9A, 0x9B, 0x5B,
            0x99, 0x59, 0x58, 0x98, 0x88, 0x48, 0x49, 0x89, 0x4B, 0x8B,
            0x8A, 0x4A, 0x4E, 0x8E, 0x8F, 0x4F, 0x8D, 0x4D, 0x4C, 0x8C,
            0x44, 0x84, 0x85, 0x45, 0x87, 0x47, 0x46, 0x86, 0x82, 0x42,
            0x43, 0x83, 0x41, 0x81, 0x80, 0x40
            };
        #endregion


        private int crc16p(byte[] puchMsg, int usDataLen)
        {
            byte uchCRCHi = 0xFF; /* 高CRC 字节初始化*/
            byte uchCRCLo = 0xFF; /* 低CRC 字节初始化*/
            uint uIndex = 0; /* CRC 循环中的索引*/
            int i = 0;
            while ((usDataLen--) > 0) /* 传输消息缓冲区*/
            {
                uIndex = (uint)(uchCRCHi ^ puchMsg[i++]); /* 计算CRC */
                uchCRCHi = (byte)(uchCRCLo ^ auchCRCHi[uIndex]);
                uchCRCLo = auchCRCLo[uIndex];
            }
            return (int)(uchCRCHi << 8 | uchCRCLo);
        }

        public byte crcHigh, crcLow;
        public int crchl;
        private int ByteLength=8;
        public string[] CalCRCPariy(string[] strConRalay)
        {
            //处理数字转换
            string[] strArray = strConRalay;
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
                byteBuffer[byteBufferCnt] = Convert.ToByte(decNum);
                byteBufferCnt++;
            }

            byte[] Buffer = new byte[6];
            for (int i = 0; i < 6; i++)
            {
                Buffer[i] = byteBuffer[i];
                Debug.WriteLine(Buffer[i]);
            }
            crchl = crc16p(Buffer, 6);
            crcHigh = (byte)((crchl & 0xff00) >> 8);
            crcLow = (byte)(crchl & 0xff);
            
            Debug.WriteLine(crcHigh);
            Debug.WriteLine(crcLow);

            strConRalay[6] = crcHigh.ToString("x2");
            strConRalay[7] = crcLow.ToString("x2");
            return strConRalay;
        }
        /// <summary>
        /// 设置X轴电机的串口参数
        /// </summary>
        public void PortXParSetting(ref SerialPort PortX,string motorAddressX)
        {
            //X轴电机     
            PortX.PortName = motorAddressX;
            PortX.BaudRate = baudRate;
            PortX.StopBits = stopBits;
            PortX.Parity = parity;
            PortX.DataBits = dataBits;
            SPortX = PortX;
        }

        /// <summary>
        /// 设置Z轴电机的串口参数
        /// </summary>
        public void PortZParSetting(ref SerialPort PortZ,string motorAddressZ)
        {
            //Z轴电机
            PortZ.PortName = motorAddressZ;
            PortZ.BaudRate = baudRate;
            PortZ.StopBits = stopBits;
            PortZ.Parity = parity;
            PortZ.DataBits = dataBits;
            SPortZ = PortZ;
        }


        public void PortRelayParSetting(ref SerialPort PortRelay, string portNum)
        {
            PortRelay.PortName = portNum;
            PortRelay.BaudRate = 9600;
            PortRelay.DataBits = 8;
            PortRelay.StopBits = StopBits.One;
            PortRelay.Parity = Parity.None;
            SPortRelay = PortRelay;
        }
        public string[] GeynerateOpenContinueTimeY2Con(int time)
        {
            try
            {
                string[] Y2Con = ContinueTimeCovert(time);
                conOutputY2TimeRelayStr[4] = Y2Con[1];
                conOutputY2TimeRelayStr[5] = Y2Con[0];
                string[] Y2ConStr = CalCRCPariy(conOutputY2TimeRelayStr);
                return Y2ConStr;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        private string[] ContinueTimeCovert(int Y1ContinueTime)
        {
            int time = Y1ContinueTime * 10;
            string[] strY1Con = myDataFormatConvertion.VelConvert(time.ToString());
            return strY1Con;
        }
        public string[] GeynerateOpenContinueTimeY1Con(int time)
        {
            try
            {
                string[] Y1Con = ContinueTimeCovert(time);
                conOutputY1TimeRelayStr[4] = Y1Con[1];
                conOutputY1TimeRelayStr[5] = Y1Con[0];
                string[] Y1ConStr = CalCRCPariy(conOutputY1TimeRelayStr);
                return Y1ConStr;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// 获取电机不同运动形式的指令
        /// </summary>
        /// <param name="vel">速度</param>
        /// <param name="dir">不同的运动方式</param>
        /// <param name="XorZ"> 0:X   1:Z   2:X和Z</param>
        /// <returns></returns>
        public string[] GetProtocolPar(string vel, int dir,int XorZ, ConfigClass config)
        {
            string[] strVelRet = myDataFormatConvertion.VelConvert(vel);

            string strAbsParX = config.m_motorGotoX.ToString();
            string strAbsParZ = config.m_motorGotoZ.ToString();

            string strIncParX = config.m_motorStepX.ToString();
            string strIncParZ = config.m_motorStepZ.ToString();

            string strSafeParX = config.m_motorSafeX.ToString();
            string strSafeParZ = config.m_motorSafeZ.ToString();

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
                if (XorZ == 0)
                    blXMotorIsStop = true;
                else if(XorZ == 1)
                    blZMotorIsStop = true;
                return conPauseMoveStr;
            }
            else if (dir == 4) //增量运动
            {               
               //速度设置
                conRelMoveStr[3] = strVelRet[0];
                conRelMoveStr[4] = strVelRet[1];
                string[] strIncParTempX, strIncParTempZ; //增量设置
                if (XorZ==0)
                {
                     strIncParTempX = myDataFormatConvertion.IncParConvert(strIncParX);
                    for (int i = 0; i < strIncParTempX.Length; i++)
                    {
                        conRelMoveStr[i + 5] = strIncParTempX[i];
                    }
                }
                else if (XorZ==1)
                {
                    strIncParTempZ = myDataFormatConvertion.IncParConvert(strIncParZ);
                    for (int i = 0; i < strIncParTempZ.Length; i++)
                    {
                        conRelMoveStr[i + 5] = strIncParTempZ[i];
                    }
                }                                           
                return conRelMoveStr;
            }
            else if (dir == 5) //绝对运动
            {                               
                //速度设置
                conAbsMoveStr[3] = strVelRet[0];
                conAbsMoveStr[4] = strVelRet[1];
                string[] strAbsParTempX, strAbsParTempZ;
                if (XorZ == 0)
                {
                    strAbsParTempX = myDataFormatConvertion.IncParConvert(strAbsParX);
                    for (int i = 0; i < strAbsParTempX.Length; i++)
                    {
                        conAbsMoveStr[i + 5] = strAbsParTempX[i];
                    }
                }
                else if (XorZ == 1)
                {
                    strAbsParTempZ = myDataFormatConvertion.IncParConvert(strAbsParZ);
                    for (int i = 0; i < strAbsParTempZ.Length; i++)
                    {
                        conAbsMoveStr[i + 5] = strAbsParTempZ[i];
                    }
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
            else if(dir == 8) //回待机点
            {
                conAbsMoveStr[3] = strVelRet[0];
                conAbsMoveStr[4] = strVelRet[1];
                string[] strSafeParTempX, strSafeParTempZ;
                if (XorZ == 0)
                {
                    strSafeParTempX = myDataFormatConvertion.IncParConvert(strSafeParX);
                    for (int i = 0; i < strSafeParTempX.Length; i++)
                    {
                        conAbsMoveStr[i + 5] = strSafeParTempX[i];
                    }
                }
                if (XorZ == 1)
                {
                    strSafeParTempZ = myDataFormatConvertion.IncParConvert(strSafeParZ);
                    for (int i = 0; i < strSafeParTempZ.Length; i++)
                    {
                        conAbsMoveStr[i + 5] = strSafeParTempZ[i];
                    }
                }
                return conAbsMoveStr;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 将接收到的数据转化为电机编码器的值
        /// </summary>
        /// <param name="receivedData"></param>
        /// <returns></returns>
        public Int64 GetFPosition(Byte[] receivedData)
        {
            string byte1 = receivedData[0].ToString("x2");
            string byte2 = receivedData[1].ToString("x2");
            string byte3 = receivedData[2].ToString("x2");
            string byte4 = receivedData[3].ToString("x2");
            uint cc = Convert.ToUInt32(byte1 + byte2 + byte3 + byte4, 16);
            Encoder = (int)cc;
           return Encoder;
        }

        public Int64 GeFRelayStatus(Byte[] receivedData)
        {
            //0x01 0x01 0x01 0x0B 0x10 0x4F  其中0x0B 表示继电器状态
            int status = receivedData[3];
            return status;
        }

        public bool blqueryXStaus = false, blqueryZStaus = false;

        /// <summary>
        /// 将接收到的数据转化为电机IO口的状态检测
        /// </summary>
        /// <param name="receivedData"></param>

        public void GetZIoStatus(Byte[] receivedData)
        {
            //第七字节 用于获取输入状态bit2:正限  bit3: 零点  bit4:负限  bit5:入四
            int status = receivedData[6];
            int testInput = receivedData[7];
            if (status == 237) //1110 1101 负限位
            {
                blNegLimitZ = true;
                //blqueryZStaus = false;
            }
            else if (status == 245) // 1111 0101 零位  
            {
                blZeroLimitZ = true;
                //blqueryZStaus = false;
            }
            else if (status == 249)  // 1111 1001 正限位  
            {
                blPosLimitZ = true;
                //blqueryZStaus = false;
            }
            else
            {
                blZeroLimitZ = blNegLimitZ = blPosLimitZ = false;
            }
            if (testInput == 32)  //0x32 表示停止
            {
                blZMotorIsStop = true;
                //blqueryZStaus = false;
            }
        }


        public void GetXIoStatus(Byte[] receivedData)
        {
            //第七字节 用于获取输入状态bit2:正限  bit3: 零点  bit4:负限  bit5:入四
            int status = receivedData[6];
            int testInput = receivedData[7];

            if (status == 221)   // 1101 1101  入四  
            {
                blSensorTrigger = true;
                //blqueryXStaus = false;
            }
            else if (status == 237) //1110 1101 负限位
            {
                blNegLimitX = true;
                //blqueryXStaus = false;
            }
            else if (status == 245) // 1111 0101 零位  
            {
                blZeroLimitX = true;
                //blqueryXStaus = false;
            }
            else if (status == 249)  // 1111 1001 正限位  
            {
                blPosLimitX = true;
                //blqueryXStaus = false;
            }
            else
            {
                blPosLimitX = blNegLimitX = blZeroLimitX = blSensorTrigger = false;
            }

            if (testInput == 32)  //0x32 表示停止
            {
                blXMotorIsStop = true;
                blqueryXStaus = false;
            }
        }


    }
}
