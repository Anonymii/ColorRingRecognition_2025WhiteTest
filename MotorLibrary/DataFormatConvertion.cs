using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MotorLibrary
{
    //***************************************************************
    //
    //             用于数据格式的转换（如字符和16进制数）
    //                                                  zhang yonghao
    //                                              2020.12.29
    //
    //****************************************************************
    public class DataFormatConvertion
    {

        /// <summary>
        /// 将十进制数转化为2个十六进制数 并拼接为2个字节
        /// 例如 十进制2400 = 0x0960 转化为 0x60,0x09
        /// </summary>
        /// <param name="vel">vel为界面输入的速度值</param>
        /// <returns></returns>
        public string[] VelConvert(string vel)
        {
            string[] strVelRet = new string[2] { "", "" };
            Int32 Vel = Convert.ToInt32(vel);
            string strVel = Vel.ToString("x4");// x表示转换的格式是16进制，4表示填充位为4位，不够4位补0。
            strVelRet[1] = strVel.Substring(0, 2);//高八位 09
            strVelRet[0] = strVel.Substring(2, 2);//低八位 60
            return strVelRet;
        }

        /// <summary>
        /// 将转化为16进制数并拼接为4个字节
        /// </summary>
        /// <param name="IncPar">增量运动的增量值</param>
        /// <returns></returns>
        public string[] IncParConvert(string IncPar)
        {
            string[] strIncParRet = new string[4];
            Int32 Inc = Convert.ToInt32(IncPar);
            if (Inc >= 0)
            {
                string strInc = Inc.ToString("x8");// x表示转换的格式是16进制，8表示填充位为8位，不够8位补0。    
                strIncParRet[3] = strInc.Substring(0, 2);
                strIncParRet[2] = strInc.Substring(2, 2);
                strIncParRet[1] = strInc.Substring(4, 2);
                strIncParRet[0] = strInc.Substring(6, 2);
                return strIncParRet;
            }
            else if (Inc < 0)        //若增量是负数
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
    }
}
