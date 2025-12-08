using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace CommonLibrary
{
    public class IniFile
    {
        private string m_iniPath;
        // 声明读写INI文件的API函数
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        public IniFile(string iniPath)
        {
            m_iniPath = iniPath;
        }

        public void IniWriteValue(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, this.m_iniPath);
        }
        public void IniWriteValue(string Section, string Key, bool Value)
        {
            WritePrivateProfileString(Section, Key, Value.ToString(), this.m_iniPath);
        }
        public void IniWriteValue(string Section, string Key, int Value)
        {
            WritePrivateProfileString(Section, Key, Value.ToString(), this.m_iniPath);
        }
        public void IniWriteValue(string Section, string Key, double Value)
        {
            WritePrivateProfileString(Section, Key, Value.ToString(), this.m_iniPath);
        }
        public void IniWriteValue(string Section, string Key, int[] Value)
        {
            string temp = "";
            foreach (int mValue in Value)
            {
                temp = temp + mValue.ToString() + ";";
            }
            temp = temp.Remove(temp.Length - 1);
            WritePrivateProfileString(Section, Key, temp, this.m_iniPath);
        }
        public void IniWriteValue(string Section, string Key, double[] Value)
        {
            string temp = "";
            foreach (double mValue in Value)
            {
                temp = temp + mValue.ToString() + ";";
            }
            temp = temp.Remove(temp.Length - 1);
            WritePrivateProfileString(Section, Key, temp, this.m_iniPath);
        }
        public void IniWriteValue(string Section, string Key, List<Int32> Value)
        {
            string temp = "";
            foreach (int mValue in Value)
            {
                temp = temp + mValue.ToString() + ";";
            }
            temp = temp.Remove(temp.Length - 1);
            WritePrivateProfileString(Section, Key, temp, this.m_iniPath);
        }
        public void IniWriteValue(string Section, string Key, List<Double> Value)
        {
            string temp = "";
            foreach (double mValue in Value)
            {
                temp = temp + mValue.ToString() + ";";
            }
            temp = temp.Remove(temp.Length - 1);
            WritePrivateProfileString(Section, Key, temp, this.m_iniPath);
        }


        public string IniReadValue(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(1024);
            int i = GetPrivateProfileString(Section, Key, "", temp, 1024, this.m_iniPath);
            string tt = temp.ToString();
            if (tt.Contains("±") && (tt.Split('±')[0] == ""))
            {
                return " " + tt;
            }
            else
            {
                return tt;
            }
            //return temp.ToString();
        }
        public int[] IniReadIntArrow(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(666);
            int i = GetPrivateProfileString(Section, Key, "", temp, 666, this.m_iniPath);
            // 分离int数组
            string[] sArrow = temp.ToString().Split(';');
            int[] nReturn = new int[sArrow.Length];
            for (int j = 0; j < sArrow.Length; ++j)
            {
                nReturn[j] = Convert.ToInt32(sArrow[j]);
            }
            return nReturn;
        }
        public double[] IniReadDoubleArrow(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(666);
            int i = GetPrivateProfileString(Section, Key, "", temp, 666, this.m_iniPath);
            // 分离double数组
            string[] sArrow = temp.ToString().Split(';');
            double[] nReturn = new double[sArrow.Length];
            for (int j = 0; j < sArrow.Length; ++j)
            {
                nReturn[j] = Convert.ToDouble(sArrow[j]);
            }
            return nReturn;
        }
        public List<Int32> IniReadIntList(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(666);
            int i = GetPrivateProfileString(Section, Key, "", temp, 666, this.m_iniPath);
            // 分离int数组
            string[] sArrow = temp.ToString().Split(';');
            List<Int32> nReturn = new List<Int32>();
            for (int j = 0; j < sArrow.Length; ++j)
            {
                nReturn.Add(Convert.ToInt32(sArrow[j]));
            }
            return nReturn;
        }
        public List<Double> IniReadDoubleList(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(666);
            int i = GetPrivateProfileString(Section, Key, "", temp, 666, this.m_iniPath);
            // 分离double数组
            string[] sArrow = temp.ToString().Split(';');
            List<Double> nReturn = new List<Double>();
            for (int j = 0; j < sArrow.Length; ++j)
            {
                nReturn.Add(Convert.ToDouble(sArrow[j]));
            }
            return nReturn;
        }
    }
}
