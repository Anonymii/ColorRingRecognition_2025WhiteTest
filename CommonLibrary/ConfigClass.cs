using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CommonLibrary
{
    public  class ConfigClass
    {
        //// 配置信息
        public bool m_bDebug = false;// 调试信息开关  
        public string m_imagePath = "";
        public string m_motorAddressX = "";
        public string m_motorAddressZ = "";
        public string m_IoComAddress = "";
        public string m_lightIPAddress = "";

        // 界面信息
        public int m_motorStepX = 1;
        public int m_motorStepZ = 1;
        public int m_motorGotoX = 10;
        public int m_motorGotoZ = 10;
        public int m_motorSpeedX = 30;
        public int m_motorSpeedZ = 30;
        public int m_motorSafeX = 10;
        public int m_motorSafeZ = 10;
        public int m_intensity = 122;
        public string m_imageFetchPath = "";
        public string m_colorImageSavePath = "";
        public int CameraDelayTime = 2;
        public int m_RedNumber = 0;
        public int m_GreenNumber = 0;
        public int m_BlueNumber = 0;
        public int m_YellowNumber = 0;
        public int m_WhiteNumber = 0;
        public int m_NoneNumber = 0;
        public int m_OtherColorNumber = 0;

        public int m_fetchInterval = 10;
        public int m_fetchTimes = 10;
        
        // 颜色判别优先级配置（1-5，值为颜色名称：Red/Blue/Green/Yellow/White/NULL）
        public string m_ColorPriority1 = "Red";
        public string m_ColorPriority2 = "White";
        public string m_ColorPriority3 = "Blue";
        public string m_ColorPriority4 = "Yellow";
        public string m_ColorPriority5 = "Green";

        // 串口输出配置（每种颜色对应的输出指令：Y1/Y2/Y3/Y4/Y3&Y4）
        public string m_OutputRed = "Y1";
        public string m_OutputBlue = "Y2";
        public string m_OutputGreen = "Y3";
        public string m_OutputYellow = "Y4";
        public string m_OutputWhite = "Y3&Y4";
        // 电机运动信息（预设）
        public int m_motorMinX = -100;
        public int m_motorMinZ = -100;
        public int m_motorMaxX = 100;
        public int m_motorMaxZ = 100;


        public bool Delay(int delayTime)
        {
            DateTime now = DateTime.Now; int s;
            do
            {
                TimeSpan spand = DateTime.Now - now;
                s = spand.Seconds;
                Application.DoEvents();
            }
            while (s < delayTime);
            return true;
        }
    }

}
