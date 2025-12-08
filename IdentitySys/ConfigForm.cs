using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using CCWin;
using CommonLibrary;
using System.Xml;

namespace IdentitySys
{
    public partial class ConfigForm : CCSkinMain
    {
        private ConfigClass m_newConfig; 
        public ConfigForm(ConfigClass newConfig)
        {
            InitializeComponent();
            m_newConfig = newConfig;
        }

        private void ConfigForm_Load(object sender, EventArgs e)
        {
            rbDebug.Checked = m_newConfig.m_bDebug;
            tbImagePath.Text = m_newConfig.m_imagePath;
            combMotorX.Text = m_newConfig.m_motorAddressX;
            combMotorZ.Text = m_newConfig.m_motorAddressZ;
            tbLightIPAddress.Text = m_newConfig.m_lightIPAddress;
            tbIntensity.Text = m_newConfig.m_intensity;
        }

        private void btnChosePath_Click(object sender, EventArgs e)
        {            
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件路径";

            if (dialog.ShowDialog() == DialogResult.OK)
            {                
                tbImagePath.Text = dialog.SelectedPath;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            m_newConfig.m_bDebug = rbDebug.Checked;
            m_newConfig.m_imagePath = tbImagePath.Text;
            m_newConfig.m_motorAddressX = combMotorX.Text;
            m_newConfig.m_motorAddressZ = combMotorZ.Text;
            m_newConfig.m_lightIPAddress = tbLightIPAddress.Text;
            m_newConfig.m_intensity = tbIntensity.Text;            
            if (!Directory.Exists(m_newConfig.m_imagePath))
            {
                Directory.CreateDirectory(m_newConfig.m_imagePath);
            }
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void Test()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("config.xml");
            XmlNode root = doc.DocumentElement;
            List<string> m_BinList = new List<string>(); // 传递给测试文件变量
            foreach (XmlNode chileNode in root.ChildNodes)
            {
                if (chileNode.Name == "Bins")
                {
                    foreach (XmlNode chileNodeNext in chileNode.ChildNodes)
                    {
                        m_BinList.Add(chileNodeNext.Attributes["number"].Value + ";" + chileNodeNext.Attributes["name"].Value + ";" + chileNodeNext.Attributes["Bin"].Value);
                    }
                }
            }
        }
    }
}
