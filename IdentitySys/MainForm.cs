using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;
//using ThridLibray;
using HalconDotNet;
using System.IO.Ports;
using CCWin;
using System.IO;
using MotorLibrary;
//using ImageLocateLibrary;
using ImageIdentityLibrary;
using CommonLibrary;
using UserManager;
using CCWin.SkinControl;
using MVSDK;
using CameraHandle = System.Int32;
using HalconDotNet;
//using CCWin.SkinControl;

namespace IdentitySys
{
    public partial class MainForm : CCSkinMain
    {
        //List<IGrabbedRawData> m_frameList = new List<IGrabbedRawData>();        // 图像缓存列表 | frame data list 
        //Thread renderThread = null;         // 显示线程 | image display thread 
        protected ColorPalette m_GrayPal;
        Thread autoIdentityThread = null;         // 自动识别线程 
        Mutex m_mutex = new Mutex();        // 锁，保证多线程安全 | mutex 
        private Graphics _g = null;
        private Graphics _a = null;         //显示检测后的图像
        int m_result = 0;                   // 返回图像判断结果  
        const int DEFAULT_INTERVAL = 40;
        Stopwatch m_stopWatch = new Stopwatch();
        //private IDevice m_dev;
        //private ConfigClass m_oldConfig = new ConfigClass();
        private ConfigClass m_newConfig = new ConfigClass();
        private MotorClass m_motor;// = new MotorClass();
        ImageIdentityClass m_identity;// = new ImageIdentityClass();
        //ImageLocateClass m_locate;// = new ImageLocateClass();

        //IGrabbedRawData m_identityFrame = null;
        //private List<Button> m_open = new List<Button>();
        //private List<Button> m_close = new List<Button>();
        private OPTControllerAPI m_oPTController = null;
        // 登陆成功信息字符串
        private string loginInfo;
        private CounterManage counterManage = new CounterManage();
        private bool bAutoStart = false;
        private bool bManualInto = false;

        #region variable
        private IntPtr m_Grabber = IntPtr.Zero;
        private CameraHandle m_hCamera = 0;
        private tSdkCameraDevInfo m_DevInfo;
        private pfnCameraGrabberFrameListener m_RawCallback;
        private int m_SaveIndex = 0;
        private AsyncSaveImage m_AsyncSave = new AsyncSaveImage();

        //private MMTimer m_TriggerTimer = new MMTimer();
        #endregion
        
        public MainForm(string info, CounterManage counter, MotorClass motor, ImageIdentityClass identity)//, ImageLocateClass locate
        {
            if (!Directory.Exists("D:\\Graphics"))
            {
                Directory.CreateDirectory("D:\\Graphics");
            }
            loginInfo = info;
            counterManage = counter;
            m_motor = motor;
            m_identity = identity;
            //m_locate = locate;
            motor.showMessageEvent += showMessage;
            //motor.getPoseEvent += getPose;
            motor.poseNotifyEvent += poseNotify;
            motor.triggerNotifyEvent += triggerNotify;
            identity.showMessageEvent += showMessage;
            //locate.showMessageEvent += showMessage;
            InitializeComponent();
            ReadConfig(m_newConfig);
            MemeToUI();
            m_oPTController = new OPTControllerAPI();
            AddCom();

        }
        private void AddCom()
        {
            foreach (string s in System.IO.Ports.SerialPort.GetPortNames())
            {
                string strCom = s;
                cbSerial.Items.Add(strCom);
                if (s != null)
                {
                    cbSerial.SelectedIndex = 0;
                }
            }
        }
    
        private void MemeToUI()
        {
            tbIntensity.Text = m_newConfig.m_intensity.ToString();
            RedNumber.Text = m_newConfig.m_RedNumber.ToString();
            WhiteNumber.Text = m_newConfig.m_WhiteNumber.ToString();
            OtherColorNumber.Text = m_newConfig.m_OtherColorNumber.ToString();
            CameraDelayTime.Text = m_newConfig.CameraDelayTime.ToString();
            tbFetchPath.Text = m_newConfig.m_imageFetchPath;
            coloridentity.Text = m_newConfig.m_colorImageSavePath;
            tbFetchInterval.Text = m_newConfig.m_fetchInterval.ToString();
            tbFetchTimes.Text = m_newConfig.m_fetchTimes.ToString();
            
            rbDebug.Checked = m_newConfig.m_bDebug;
            tbImagePath.Text = m_newConfig.m_imagePath;
            tbLightIPAddress.Text = m_newConfig.m_lightIPAddress;

            //add by shj 2021.3.18
            btnCloseAll.Enabled = false;
            btnOpenAll.Enabled = true;
            btnAutoIdentity.Enabled = false;
            ManualColorIdentity.Enabled = false;
        }

        private string UIToMemeLevel2(int dir,int XorZ)
        {
            string str = LimitMotorPar(dir, XorZ);
            m_newConfig.m_intensity = Convert.ToInt32(tbIntensity.Text);
            m_newConfig.m_RedNumber = Convert.ToInt32(RedNumber.Text);
            m_newConfig.m_WhiteNumber = Convert.ToInt32(WhiteNumber.Text);
            m_newConfig.m_OtherColorNumber = Convert.ToInt32(OtherColorNumber.Text);
            m_newConfig.m_imageFetchPath = tbFetchPath.Text;
            m_newConfig.m_colorImageSavePath = coloridentity.Text;
            m_newConfig.m_fetchInterval = Convert.ToInt32(tbFetchInterval.Text);
            m_newConfig.m_fetchTimes = Convert.ToInt32(tbFetchTimes.Text);

            if (!Directory.Exists(m_newConfig.m_imageFetchPath))
            {
                Directory.CreateDirectory(m_newConfig.m_imageFetchPath);
            }
            if (!Directory.Exists(m_newConfig.m_colorImageSavePath))
            {
                Directory.CreateDirectory(m_newConfig.m_colorImageSavePath);
            }
            return str;
        }
        private string LimitMotorPar(int dir,int XOrZ)
        {
            string strError = "Error";
            return "Success";
        }
        private void UIToMemeLevel1()
        {
            m_newConfig.m_bDebug = rbDebug.Checked;
            m_newConfig.m_imagePath = tbImagePath.Text;
            m_newConfig.m_lightIPAddress = tbLightIPAddress.Text;
            if (!Directory.Exists(m_newConfig.m_imagePath))
            {
                Directory.CreateDirectory(m_newConfig.m_imagePath);
            }
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {            
            //skinTabControl1.SelectedIndex = 0;
            AddLog.User = loginInfo.Split(';')[1];
            if (loginInfo.Split(';')[3] == "一级")
            {
                //level = 1;
                skinGroupBox9.Enabled = true;
                skinGroupBoxOp.Enabled = true;
                btnCounterManage.Enabled = btnDeleteRecord.Enabled = true;
                this.Text = "测试系统-【" + loginInfo.Split(';')[1] + "；一级】";
                showMessage("管理员登录");
            }
            else if (loginInfo.Split(';')[3] == "二级")
            {

                //level = 2;
                skinGroupBox7.Enabled = false;
                skinGroupBox8.Enabled = false;
                skinGroupBox9.Enabled = false;
                skinGroupBox10.Enabled = false;
                colortext.Visible = false;
                btnCounterManage.Enabled = btnDeleteRecord.Enabled = false;
                skinLabel2.Visible = true;// tubearrive.Visible = false;
                //btnStop.Visible = true;
                this.Text = "测试系统-【" + loginInfo.Split(';')[1] + "；二级】";
                showMessage("操作员登录");
            }

            //btnCloseIdentify.Enabled = btnCloseMotorX.Enabled = btnCloseMotorZ.Enabled = btnCloseLight.Enabled = false;

            //skinGroupBoxOp.Enabled = false;

            //if (null == renderThread)
            //{
            //    renderThread = new Thread(new ThreadStart(ShowThread));
            //    renderThread.Start();
            //}*/

            
             m_stopWatch.Start();
             skinSplitContainer2.SplitterWidth = skinSplitContainer3.SplitterWidth = 10;

             ///*System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
             //path.AddEllipse(this.btnAutoIdentity.ClientRectangle);
             //this.btnAutoIdentity.Region = new Region(path);
             if (!Directory.Exists(m_newConfig.m_imagePath))
             {
                 Directory.CreateDirectory(m_newConfig.m_imagePath);
             }
             if (!Directory.Exists(m_newConfig.m_imageFetchPath))
             {
                 Directory.CreateDirectory(m_newConfig.m_imageFetchPath);
             }
             if (!Directory.Exists(m_newConfig.m_colorImageSavePath))
             {
                 Directory.CreateDirectory(m_newConfig.m_colorImageSavePath);
             }

             skinTabControl1.SelectedIndex = skinTabControl2.SelectedIndex = 0;
             showMessage("系统启动。");
            //this.WindowState = FormWindowState.Maximized;
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
        }

        bool bAutoSave = false;
        private int CameraGrabberRawCallback(
            IntPtr Grabber,
            int Phase,
            IntPtr pFrameBuffer,
            ref tSdkFrameHead pFrameHead,
            IntPtr Context)
        {
            if (Phase == 0)
            {
                // 获取保存参数
                bool bSaveOnlyTrigger = false;
                string Dir = "";
                int Fmt = 2;
                //this.Invoke((EventHandler)delegate
                //{
                //    /*bAutoSave = checkBoxAutoSave.Checked;
                //    bSaveOnlyTrigger = checkBoxAutoSaveOnlyTrigger.Checked;
                //    Dir = textBoxSaveDir.Text;
                //    Fmt = comboBoxSaveFormat.SelectedIndex;*/
                //});

                if (bAutoSave && !(bSaveOnlyTrigger && pFrameHead.bIsTrigger == 0))
                {
                    string path = m_newConfig.m_imageFetchPath + "\\" + "Sig_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_fff");
                    bAutoSave = false;
                    /*string filename = DateTime.Now.ToString("yyyyMMddHHmmss");
                    ++m_SaveIndex;
                    filename += "_" + m_SaveIndex.ToString("0000");*/
                    SaveImage(CombineImageSavePath(Dir, path), Fmt, pFrameBuffer, pFrameHead);
                    showMessage("存至：" + path);
                    return 0;
                }
            }

            return 1;
        }
        //注释掉的是宝令做的，删除注释可以直接用
        //private void CameraGrabberFrameCallback(
        //  IntPtr Grabber,
        //  IntPtr pFrameBuffer,
        //  ref tSdkFrameHead pFrameHead,
        //  IntPtr Context)
        //{
        //    // 数据处理回调

        //    // 由于黑白相机在相机打开后设置了ISP输出灰度图像
        //    // 因此此处pFrameBuffer=8位灰度数据
        //    // 否则会和彩色相机一样输出BGR24数据

        //    // 彩色相机ISP默认会输出BGR24图像
        //    // pFrameBuffer=BGR24数据

        //    // 执行一次GC，释放出内存
        //    GC.Collect();

        //    // 由于SDK输出的数据默认是从底到顶的，转换为Bitmap需要做一下垂直镜像
        //    MvApi.CameraFlipFrameBuffer(pFrameBuffer, ref pFrameHead, 1);

        //    int w = pFrameHead.iWidth;
        //    int h = pFrameHead.iHeight;
        //    Boolean gray = (pFrameHead.uiMediaType == (uint)MVSDK.emImageFormat.CAMERA_MEDIA_TYPE_MONO8);
        //    IdentityImage = new Bitmap(w, h,
        //        gray ? w : w * 3,
        //        gray ? PixelFormat.Format8bppIndexed : PixelFormat.Format24bppRgb,
        //        pFrameBuffer);

        //    // 如果是灰度图要设置调色板
        //    if (gray)
        //    {
        //        IdentityImage.Palette = m_GrayPal;
        //    }

        //    /*this.Invoke((EventHandler)delegate
        //    {
        //        hwcImageIdentityResult.Image = IdentityImage;
        //        hwcImageIdentityResult.Refresh();
        //    });*/
        //}
        
        //迈德威视工程师改过的
        private void CameraGrabberFrameCallback(
            IntPtr Grabber,
            IntPtr pFrameBuffer,
            ref tSdkFrameHead pFrameHead,
            IntPtr Context)
        {
            // 数据处理回调

            // 由于黑白相机在相机打开后设置了ISP输出灰度图像
            // 因此此处pFrameBuffer=8位灰度数据
            // 否则会和彩色相机一样输出BGR24数据

            // 彩色相机ISP默认会输出BGR24图像
            // pFrameBuffer=BGR24数据

            // 执行一次GC，释放出内存
            GC.Collect();

            // 由于SDK输出的数据默认是从底到顶的，转换为Bitmap需要做一下垂直镜像
            MvApi.CameraFlipFrameBuffer(pFrameBuffer, ref pFrameHead, 1);

            int w = pFrameHead.iWidth;
            int h = pFrameHead.iHeight;
            Boolean gray = (pFrameHead.uiMediaType == (uint)MVSDK.emImageFormat.CAMERA_MEDIA_TYPE_MONO8);
            Bitmap Image = new Bitmap(w, h,
                gray ? w : w * 3,
                gray ? PixelFormat.Format8bppIndexed : PixelFormat.Format24bppRgb,
                pFrameBuffer);

            // 如果是灰度图要设置调色板
            if (gray)
            {
                Image.Palette = m_GrayPal;
            }

            IdentityImage = Image.Clone(new Rectangle(0, 0, Image.Width, Image.Height), Image.PixelFormat);

            /*this.Invoke((EventHandler)delegate
         {
          hwcImageIdentityResult.Image = IdentityImage;
          hwcImageIdentityResult.Refresh();
         });*/
        }
        Bitmap IdentityImage;
        private string CameraFileConfig = "C:\\Users\\Administered\\Desktop\\cfg3.Config";
        protected pfnCameraGrabberFrameCallback m_FrameCallback;
        private bool CameraConnectStatus = false;
        private void btnOpenIdentify_Click(object sender, EventArgs e)
        {
            m_RawCallback = new pfnCameraGrabberFrameListener(CameraGrabberRawCallback);
            m_FrameCallback = new pfnCameraGrabberFrameCallback(CameraGrabberFrameCallback);
            if (InitCamera() == 0)
            {
                MvApi.CameraReadParameterFromFile(m_hCamera, CameraFileConfig);
                //MvApi.CameraGrabber_SaveImage
                m_AsyncSave.Start();
                m_TriggerTimer.Start();
                CameraConnectStatus = true;
                showMessage("相机打开成功！");
                btnOpenIdentify.Enabled = false;
                btnCloseIdentify.Enabled = true;
            }
        }

        private int InitCamera()
        {
            CameraSdkStatus status = 0;

            tSdkCameraDevInfo[] DevList;
            MvApi.CameraEnumerateDevice(out DevList);
            int NumDev = (DevList != null ? DevList.Length : 0);
            if (NumDev < 1)
            {                
                showMessage("未扫描到相机！", InfoType.Error);
                return -1;
            }
            else if (NumDev == 1)
            {
                status = MvApi.CameraGrabber_Create(out m_Grabber, ref DevList[0]);
            }
            else
            {
                status = MvApi.CameraGrabber_CreateFromDevicePage(out m_Grabber);
            }

            if (status == 0)
            {
                MvApi.CameraGrabber_GetCameraDevInfo(m_Grabber, out m_DevInfo);
                MvApi.CameraGrabber_GetCameraHandle(m_Grabber, out m_hCamera);
                MvApi.CameraCreateSettingPage(m_hCamera, this.Handle, m_DevInfo.acFriendlyName, null, (IntPtr)0, 0);

                MvApi.CameraGrabber_SetFrameListener(m_Grabber, m_RawCallback, IntPtr.Zero);
                MvApi.CameraGrabber_SetRGBCallback(m_Grabber, m_FrameCallback, IntPtr.Zero);

                // 黑白相机设置ISP输出灰度图像
                // 彩色相机ISP默认会输出BGR24图像
                tSdkCameraCapbility cap;
                MvApi.CameraGetCapability(m_hCamera, out cap);
                if (cap.sIspCapacity.bMonoSensor != 0)
                {
                    MvApi.CameraSetIspOutFormat(m_hCamera, (uint)MVSDK.emImageFormat.CAMERA_MEDIA_TYPE_MONO8);
                }

                // 设置触发模式
                MvApi.CameraSetTriggerMode(m_hCamera, 1);
                MvApi.CameraSetTriggerCount(m_hCamera, 1);

                MvApi.CameraGrabber_SetHWnd(m_Grabber, this.pbImageIdentity.Handle);
                MvApi.CameraGrabber_StartLive(m_Grabber);
            }
            else
            {                
                showMessage(String.Format("打开相机失败，原因：{0}", status), InfoType.Error);
                return -1;
            }
            return 0;
        }

        private void btnOpenMotorX_Click(object sender, EventArgs e)
        {
            int ret = m_motor.OpenMotorX(m_newConfig.m_motorAddressX);
            if (ret == 0)
            {
                //btnOpenMotorX.Enabled = false;
                //btnCloseMotorX.Enabled = true;
            }
        }

        private void btnOpenMotorZ_Click(object sender, EventArgs e)
        {
            int ret = m_motor.OpenMotorZ(m_newConfig.m_motorAddressZ);
            if (ret == 0)
            {
               // btnOpenMotorZ.Enabled = false;
                //btnCloseMotorZ.Enabled = true;
            }
        }
        private string _light_SerialPortName = null;
        private bool blConnectLightCon = false;
        private void btnOpenLight_Click(object sender, EventArgs e)
        {
            long lRet = -1;
            _light_SerialPortName = "COM4";
            if ("" == _light_SerialPortName)
            {
                showMessage("Serial name can not be empty");
                return;
            }

            lRet = m_oPTController.InitSerialPort(_light_SerialPortName);
            if (0 != lRet)
            {
                showMessage("Failed to initialize serial port") ;
                return;
            }
            else
            {
                blConnectLightCon = true;
                showMessage("光源连接成功");
                m_oPTController.TurnOffChannel(1);
            }
            ////String IPAddr = m_newConfig.m_lightIPAddress;
            //String SNNumber = "";
            //long lRet = -1;
            ////if ("" == IPAddr)
            ////{
            ////    showMessage("IP地址不能为空！", InfoType.Warn);
            ////    return;
            ////}
            //if ("" == SNNumber)
            //{
            //    showMessage("设备序列号不能为空！", InfoType.Warn);
            //    return;
            //}
            ////lRet = m_oPTController.CreateEthernetConnectionByIP(IPAddr);
            //lRet = m_oPTController.CreateEthernetConnectionBySN(SNNumber); 
            //if (0 != lRet)
            //{
            //    showMessage("无法通过序列号创建连接！", InfoType.Warn);
            //    return;
            //}
            //else
            //{
            //    blConnectLightCon = true;
            //    m_oPTController.TurnOffChannel(1);//启动光源的时候，默认关闭
            //    showMessage("光源初始化成功！");
            //}
        }
        private void connectlightforauto()
        {
            String IPAddr = m_newConfig.m_lightIPAddress;
            long lRet = -1;
            if ("" == IPAddr)
            {
                showMessage("IP地址不能为空！", InfoType.Warn);
                return;
            }

            lRet = m_oPTController.CreateEthernetConnectionByIP(IPAddr);
            if (0 != lRet)
            {
                showMessage("无法通过IP创建连接！", InfoType.Warn);
                return;
            }
            else
            {
                blConnectLightCon = true;
                showMessage("光源初始化成功！");
            }
        }

        private void btnOpenAll_Click(object sender, EventArgs e)
        {
            int tem = skinTabControl1.SelectedIndex;
            skinTabControl1.SelectedIndex = 1;
            bool tem1 = skinGroupBox7.Enabled;
            skinGroupBox7.Enabled = true;
            bool tem2 = skinGroupBox8.Enabled;
            skinGroupBox8.Enabled = true;


            btnOpenIdentify.PerformClick();
            btnOpenLight.PerformClick();
            btnOpenCOM.PerformClick();


            //add by shj 2021.3.18
            //btnOpenIdentify.Enabled = false;
            skinTabControl1.SelectedIndex = tem;
            skinGroupBox7.Enabled = tem1;
            skinGroupBox8.Enabled = tem2;

            //if (CameraConnectStatus == true && blConnectLightCon == true&& m_motor.blIsIoComOpen==true)
            if (CameraConnectStatus == true && blConnectLightCon == true && m_motor.blIsIoComOpen == true)
            {
                skinGroupBoxOp.Enabled = true;
                btnOpenAll.Enabled = false;
                btnCloseAll.Enabled = true;
                btnAutoIdentity.Enabled = ManualColorIdentity.Enabled = true;
                btnPauseTest.Enabled = btnContinueTest.Enabled = btnStopTest.Enabled = ManualColorIdentityEND.Enabled = false;
            }
            else
            {
                showMessage("设备连接失败");
                return;
            }

            string folderFullName = "D:\\FetchPath";
            List<string> pathGroup = new List<string>();
            DirectoryInfo TheFolder = new DirectoryInfo(folderFullName);
            //遍历文件夹
            foreach (DirectoryInfo NextFolder in TheFolder.GetDirectories())
            {
                pathGroup.Add(NextFolder.Name);
            }
            //    this.listBox1.Items.Add(NextFolder.Name);
            ////遍历文件
            if (pathGroup.Count >= 3)
            {
                foreach (string item in pathGroup)
                    Directory.Delete("D:\\FetchPath\\" + item, true);
            }
        }
        // 停止码流 stop grabbing         
        private void btnCloseIdentify_Click(object sender, EventArgs e)
        {
            m_TriggerTimer.Stop();
            if (m_Grabber != IntPtr.Zero)
                MvApi.CameraGrabber_StopLive(m_Grabber);
            m_AsyncSave.Stop();
            m_AsyncSave.Clear();
            MvApi.CameraGrabber_Destroy(m_Grabber);
            CameraConnectStatus = false;
            btnOpenIdentify.Enabled = true;
            btnCloseIdentify.Enabled = false;
            /*try
            {
                m_bSoftTrig = false;
                if (m_dev == null)
                {
                    throw new InvalidOperationException("Device is invalid");
                }

                m_dev.StreamGrabber.ImageGrabbed -= OnImageGrabbed;         // 反注册回调 | unregister grab event callback 
                m_dev.ShutdownGrab();                                       // 停止码流 | stop grabbing 
                m_dev.Close();                                              // 关闭相机 | close camera 
            }
            catch (Exception exception)
            {
                // Catcher.Show(exception);
                showMessage(exception.Message, InfoType.Error);
            }*/
        }

        private void btnCloseMotorX_Click(object sender, EventArgs e)
        {
            int ret = m_motor.CloseMotorX();
            if (ret == 0)
            {
               // btnOpenMotorX.Enabled = true;
               // btnCloseMotorX.Enabled = false;
            }
        }

        private void btnCloseMotorZ_Click(object sender, EventArgs e)
        {
            int ret = m_motor.CloseMotorZ();
            if (ret == 0)
            {
              //  btnOpenMotorZ.Enabled = true;
                //btnCloseMotorZ.Enabled = false;
            }
        }
       
        private void btnCloseLight_Click(object sender, EventArgs e)
        {
            long lRet = -1;
            lRet = m_oPTController.DestroyEthernetConnect();
            if (0 != lRet)
            {
                showMessage("通过IP断开连接失败！", InfoType.Warn);
                return;
            }
            blConnectLightCon = false;
            showMessage("光源关闭成功！");
        }

        private void btnCloseAll_Click(object sender, EventArgs e)
        {
            int tem = skinTabControl1.SelectedIndex;
            skinTabControl1.SelectedIndex = 1;
            bool tem1 = skinGroupBox7.Enabled;
            skinGroupBox7.Enabled = true;
            bool tem2 = skinGroupBox8.Enabled;
            skinGroupBox8.Enabled = true;

            //btnCloseIdentify.PerformClick();
            btnCloseLight.PerformClick();
            //btnOpenCOM.PerformClick();

            //add by shj 2021.3.18

            skinTabControl1.SelectedIndex = tem;
            skinGroupBox7.Enabled = tem1;
            skinGroupBox8.Enabled = tem2;
            //if (CameraConnectStatus==false && blConnectLightCon== false && m_motor.blIsIoComOpen == false)
            if (blConnectLightCon == false)
            {
                skinGroupBoxOp.Enabled = false;
                btnCloseAll.Enabled = false;
                btnOpenAll.Enabled = true;
            }
            else
            {
                showMessage("设备断开失败");
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (m_newConfig.m_RedNumber != 0 || m_newConfig.m_WhiteNumber != 0 || m_newConfig.m_OtherColorNumber!= 0)
            {
                if (DialogResult.OK == MessageBox.Show("计数未清零！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning))
                {
                    //e.Cancel = true;
                    return;
                }
            }
            int tem = skinTabControl1.SelectedIndex;
            skinTabControl1.SelectedIndex = 1;
            bool tem1 = skinGroupBox7.Enabled;
            skinGroupBox7.Enabled = true;
            bool tem2 = skinGroupBox8.Enabled;
            skinGroupBox8.Enabled = true;

            btnCloseIdentify.PerformClick();
            btnCloseLight.PerformClick();
            btnOpenCOM.PerformClick();

            //add by shj 2021.3.18
            btnCloseAll.Enabled = false;
            btnOpenAll.Enabled = true;
            btnAutoIdentity.Enabled = false;
            ManualColorIdentity.Enabled = false;

            skinTabControl1.SelectedIndex = tem;
            skinGroupBox7.Enabled = tem1;
            skinGroupBox8.Enabled = tem2;
            this.Close();
        }

        private void btnLevel1Store_Click(object sender, EventArgs e)
        {
            UIToMemeLevel1();
            WriteConfig(m_newConfig);
            showMessage("系统参数设置 保存至配置文件 成功!");
        }

        private void btnLevel2Store_Click(object sender, EventArgs e)
        {
            UIToMemeLevel2(0,1);
            WriteConfig(m_newConfig);
            showMessage("高级系统设置 生效 并 保存至配置文件 成功!");
        }

        private void btnChangePassword_Click(object sender, EventArgs e)
        {
            PassWordForm pf = new PassWordForm(loginInfo, counterManage);
            pf.showMessageEvent += showMessage;
            pf.ShowDialog();
        }

        private void btnCounterManage_Click(object sender, EventArgs e)
        {
            UsersForm uf = new UsersForm(counterManage);
            uf.showMessageEvent += showMessage;
            uf.ShowDialog();
        }

        private void btnDeleteRecord_Click(object sender, EventArgs e)
        {
            ConfDeleForm cdf = new ConfDeleForm(loginInfo);
            cdf.showMessageEvent += showMessage;
            cdf.ShowDialog();
        }

        private void btnFetchRecord_Click(object sender, EventArgs e)
        {
            OpLogForm olf = new OpLogForm();
            olf.showMessageEvent += showMessage;
            olf.ShowDialog();
        }

        private void HObject2Bpp24(HObject image, out Bitmap res)
        {
            HTuple hred, hgreen, hblue, type, width, height;

            HOperatorSet.GetImagePointer3(image, out hred, out hgreen, out hblue, out type, out width, out height);

            res = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);

            Rectangle rect = new Rectangle(0, 0, width, height);
            BitmapData bitmapData = res.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppRgb);
            unsafe
            {
                byte* bptr = (byte*)bitmapData.Scan0;
                byte* r = ((byte*)hred.I);
                byte* g = ((byte*)hgreen.I);
                byte* b = ((byte*)hblue.I);

                int lengh = width * height;
                for (int i = 0; i < lengh; i++)
                {
                    bptr[i * 4] = (b)[i];
                    bptr[i * 4 + 1] = (g)[i];
                    bptr[i * 4 + 2] = (r)[i];
                    bptr[i * 4 + 3] = 255;
                }
            }
            res.UnlockBits(bitmapData);
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            bAutoStart = false;
            m_motor.TransforConfig(m_newConfig);
            m_motor.Stop();
            showMessage("急停！", InfoType.Warn);
        }

        private void btnGotoX_Click(object sender, EventArgs e)
        {
            if (UIToMemeLevel2(5,0) == "Success")
                m_motor.MoveParFlag(5, 0, m_newConfig);
        }

        private void btnGotoZ_Click(object sender, EventArgs e)
        {
            if (UIToMemeLevel2(5,1) == "Success")
                m_motor.MoveParFlag(5, 1, m_newConfig);
        }

        private void btnRelX_Click(object sender, EventArgs e)
        {
            if (UIToMemeLevel2(4,0) == "Success")
                m_motor.MoveParFlag(4, 0, m_newConfig);
        }

        private void btnRelZ_Click(object sender, EventArgs e)
        {
            if (UIToMemeLevel2(4,1) == "Success")
                m_motor.MoveParFlag(4, 1, m_newConfig);
        }

        private void btnXPosMoveToLimit_Click(object sender, EventArgs e)
        {
            if (UIToMemeLevel2(1,0) == "Success")
                m_motor.MoveParFlag(1, 0, m_newConfig);
        }

        private void btnZPosMoveToLimit_Click(object sender, EventArgs e)
        {
            if (UIToMemeLevel2(1,1) == "Success")
                m_motor.MoveParFlag(1, 1, m_newConfig);
        }

        private void btnXNegMoveToLimit_Click(object sender, EventArgs e)
        {
            if (UIToMemeLevel2(0,0) == "Success")
                m_motor.MoveParFlag(0, 0, m_newConfig);
        }

        private void btnZNegMoveToLimit_Click(object sender, EventArgs e)
        {
            if (UIToMemeLevel2(0,1) == "Success")
                m_motor.MoveParFlag(0, 1, m_newConfig);
        }

        private void btnXMoveToZero_Click(object sender, EventArgs e)
        {
            if (UIToMemeLevel2(2,0) == "Success")
                m_motor.MoveParFlag(2, 0, m_newConfig);
        }

        private void btnZMoveToZero_Click(object sender, EventArgs e)
        {
            if (UIToMemeLevel2(2,1) == "Success")
                m_motor.MoveParFlag(2, 1, m_newConfig);
        }

        private void btn_EnabledChanged(object sender, EventArgs e)
        {
            //Button ClickedButton = sender as Button;
            if (btnOpenIdentify.Enabled || btnOpenLight.Enabled)
            {
                btnOpenAll.Enabled = true;
            }
            else
            {
                btnOpenAll.Enabled = false;
                skinGroupBoxOp.Enabled = true;
            }
            if (btnCloseIdentify.Enabled || btnCloseLight.Enabled)
            {
                btnCloseAll.Enabled = true;
            }
            else
            {
                btnCloseAll.Enabled = false;
            }
            if (btnCloseIdentify.Enabled)
            {
                //btnManualIdentity.Enabled = true;
            }
            else
            {
                //btnManualIdentity.Enabled = false;
            }

           /* if (btnCloseMotorX.Enabled)
            {
                skinGroupBoxX.Enabled = true;
            }
            else
            {
                skinGroupBoxX.Enabled = false;
            }

            if (btnCloseMotorZ.Enabled)
            {
                skinGroupBoxZ.Enabled = true;
            }
            else
            {
                skinGroupBoxZ.Enabled = false;
            }*/
        }
        // 位置检查
        public int getPose(out int XPos, out int ZPos)
        {
            XPos = ZPos = -1;
            try
            {
                HObject ho_image = null;
                //m_locate.locateAction(hwcImageLocate.HalconWindow, ho_image, out XPos, out ZPos);
            }
            catch (Exception exception)
            {
                // Catcher.Show(exception);
                showMessage(exception.Message, InfoType.Error);
                return -1;
            }
            return 0;
        }
        private void ChangeSkinTrackBarX()
        {
            /*if (skinTrackBarX.InvokeRequired)
            {
                skinTrackBarX.Invoke(new Action(() =>
                {
                    skinTrackBarX.Value = m_motor.CurrentX;
                }));
            }*/
        }
        private void ChangeSkinTrackBarZ()
        {
            /*if (skinTrackBarZ.InvokeRequired)
            {
                skinTrackBarZ.Invoke(new Action(() =>
                {
                    skinTrackBarZ.Value = m_motor.CurrentX;
                }));
            }*/
        }
        //public int poseNotify(int ID)
        //{
        //    try
        //    {
        //        switch (ID)
        //        {
        //            case 0:
        //                if (tbCurrentX.InvokeRequired)
        //                {
        //                    tbCurrentX.Invoke(new Action(() =>
        //                    {
        //                        tbCurrentX.Text = m_motor.CurrentX.ToString();
        //                    }));
        //                }
        //                if (skinTrackBarX.Value <= m_newConfig.m_motorMinX)
        //                {
        //                    ChangeSkinTrackBarX();
        //                    btnLimitNegX.BaseColor = Color.Red;
        //                    btnLimitPosX.BaseColor = Color.Green;
        //                }
        //                else if (skinTrackBarX.Value >= m_newConfig.m_motorMaxX)
        //                {
        //                    ChangeSkinTrackBarX();
        //                    btnLimitNegX.BaseColor = Color.Green;
        //                    btnLimitPosX.BaseColor = Color.Red;
        //                }
        //                else
        //                {
        //                    ChangeSkinTrackBarX();
        //                    btnLimitNegX.BaseColor = Color.Green;
        //                    btnLimitPosX.BaseColor = Color.Green;
        //                }
        //                break;
        //            case 1:
        //                if (tbCurrentZ.InvokeRequired)
        //                {
        //                    tbCurrentZ.Invoke(new Action(() =>
        //                    {
        //                        tbCurrentZ.Text = m_motor.CurrentZ.ToString();
        //                    }));
        //                }
        //                if (skinTrackBarZ.Value <= m_newConfig.m_motorMinZ)
        //                {
        //                    ChangeSkinTrackBarZ();
        //                    btnLimitNegZ.BaseColor = Color.Red;
        //                    btnLimitPosZ.BaseColor = Color.Green;
        //                }
        //                else if (skinTrackBarZ.Value >= m_newConfig.m_motorMaxZ)
        //                {
        //                    ChangeSkinTrackBarZ();
        //                    btnLimitNegZ.BaseColor = Color.Green;
        //                    btnLimitPosZ.BaseColor = Color.Red;
        //                }
        //                else
        //                {
        //                    ChangeSkinTrackBarZ();
        //                    btnLimitNegZ.BaseColor = Color.Green;
        //                    btnLimitPosZ.BaseColor = Color.Green;
        //                }
        //                break;
        //        }
        //    }
        //    catch (Exception exception)
        //    {
        //        showMessage(exception.Message, InfoType.Error);
        //        return -1;
        //    }
        //    return 0;
        //}
        //位置更新触发通知
        public int poseNotify(int ID)
        {
            try
            {
                switch (ID)
                {
                    case 0:
                        /*if (tbCurrentX.InvokeRequired)
                        {
                            tbCurrentX.Invoke(new Action(() =>
                            {
                                tbCurrentX.Text = m_motor.CurrentX.ToString();
                            }));
                        }*/
                        /*if (skinTrackBarX.InvokeRequired)
                        {
                            if (m_motor.CurrentX <= m_newConfig.m_motorMaxX && m_motor.CurrentX >= m_newConfig.m_motorMinX)
                            {
                                skinTrackBarX.Invoke(new Action(() =>
                                {
                                    skinTrackBarX.Value = m_motor.CurrentX;
                                }));
                            }
                        }*/
                        //if(m_motor.blNegLimitX) //将负限位接到0点以后  负限位不触发  现在改为 编码值为0代表 零位（也即负限位触发）
                        /*if (m_motor.blXFPosIsZero)
                        {
                            btnLimitNegX.BaseColor = Color.Red;
                        }
                        else
                        {
                            btnLimitNegX.BaseColor = Color.Green;
                        }
                        if (m_motor.blPosLimitX)
                        {
                            btnLimitPosX.BaseColor = Color.Red;
                        }
                        else
                        {
                            btnLimitPosX.BaseColor = Color.Green;
                        }
                        */
                        break;
                    case 1:
                        /*if (tbCurrentZ.InvokeRequired)
                        {
                            tbCurrentZ.Invoke(new Action(() =>
                            {
                                tbCurrentZ.Text = m_motor.CurrentZ.ToString();
                            }));
                        }*/
                        /*if (skinTrackBarZ.InvokeRequired)
                        {
                            if (m_motor.CurrentZ <= m_newConfig.m_motorMaxZ && m_motor.CurrentZ >= m_newConfig.m_motorMinZ)
                            {
                                skinTrackBarZ.Invoke(new Action(() =>
                                {
                                    skinTrackBarZ.Value = m_motor.CurrentZ;
                                }));
                            }
                        }
                        if (m_motor.blZFPosIsZero)
                        {
                            btnLimitNegZ.BaseColor = Color.Red;
                        }
                        else
                        {
                            btnLimitNegZ.BaseColor = Color.Green;
                        }
                        if (m_motor.blPosLimitZ)
                        {
                            btnLimitPosZ.BaseColor = Color.Red;
                        }
                        else
                        {
                            btnLimitPosZ.BaseColor = Color.Green;
                        }*/
                        break;
                }
            }
            catch (Exception exception)
            {
                // Catcher.Show(exception);
                showMessage(exception.Message, InfoType.Error);
                return -1;
            }
            return 0;
        }

        public void showMessage(string message, InfoType Type = InfoType.Info)
        {
            string headInfo = "【" + DateTime.Now.ToString("HH:mm:ss") + "】";
            string DateInfo = "【" + DateTime.Now.ToString("yyyy.MM.dd") + "】";
            switch (Type)
            {
                case InfoType.Info:
                    headInfo = headInfo + "提示" + "：";
                    break;
                case InfoType.Warn:
                    headInfo = headInfo + "警告" + "：";
                    break;
                case InfoType.Error:
                    headInfo = headInfo + "错误" + "：";
                    break;
                case InfoType.Debug:
                    if (m_newConfig.m_bDebug)
                    {
                        headInfo = headInfo + "调试" + "：";
                    }
                    break;
            }

            if (FrameSave.InvokeRequired)
            {
                FrameSave.Invoke(new Action(() =>
                {
                    FrameSave.Items.Add(headInfo + message);
                    AddLog.Record(DateInfo + headInfo + message);
                    FrameSave.TopIndex = FrameSave.Items.Count - 1;
                }));
            }
            else
            {
                FrameSave.Items.Add(headInfo + message);
                AddLog.Record(DateInfo + headInfo + message);
                FrameSave.TopIndex = FrameSave.Items.Count - 1;
            }
        }

        private void lbInfo_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();          //先调用基类实现
            if (e.Index < 0)            //form load 的时候return
                return;
            //因为此函数每一个 listItem drawing 都要调用， 所以不能简单的只写e.Graphics.DrawString(listBox1.Items[e.Index].ToString(),e.Font, Brushes.Red, e.Bounds);
            //那样会造成所有item一个颜色
            //这里是用item字符串是否包含某些词决定的 ， 不好
            if (FrameSave.Items[e.Index].ToString().Contains("】提示："))
            {
                e.Graphics.DrawString(FrameSave.Items[e.Index].ToString(),
                e.Font, Brushes.Black, e.Bounds);
            }
            else if (FrameSave.Items[e.Index].ToString().Contains("】警告："))
            {
                e.Graphics.DrawString(FrameSave.Items[e.Index].ToString(),
                    e.Font, Brushes.Purple, e.Bounds);

            }
            else if (FrameSave.Items[e.Index].ToString().Contains("】错误："))
            {
                e.Graphics.DrawString(((ListBox)sender).Items[e.Index].ToString(),
                        e.Font, Brushes.Red, e.Bounds);
            }
            else if (FrameSave.Items[e.Index].ToString().Contains("】调试："))
            {
                e.Graphics.DrawString(((ListBox)sender).Items[e.Index].ToString(),
                        e.Font, Brushes.Blue, e.Bounds);
            }
        }
        // 转码显示线程 
        // display thread routine 
        /*private void ShowThread()
        {
            while (m_bShowLoop)
            {
                // 软件触发
                if (m_dev != null && m_bSoftTrig)
                {                    
                    m_dev.TriggerSet.ExecuteSoftwareTrigger();
                }

                if (m_frameList.Count == 0)
                {
                    Thread.Sleep(10);
                    continue;
                }

                // 图像队列取最新帧 
                // always get the latest frame in list 
                m_mutex.WaitOne();
                m_identityFrame = m_frameList.ElementAt(m_frameList.Count - 1);
                m_frameList.Clear();
                m_mutex.ReleaseMutex();
                       
                if (rbFetchPicture.Checked == true)// 连续存储
                {
                    //int nRGB = RGBFactory.EncodeLen(m_identityFrame.Width, m_identityFrame.Height, true);
                    //IntPtr pData = Marshal.AllocHGlobal(nRGB);
                    //RGBFactory.ToRGB(m_identityFrame.Image,
                    //m_identityFrame.Width,
                    //m_identityFrame.Height,
                    //true,
                    //m_identityFrame.PixelFmt,
                    //pData,
                    //nRGB);
                    //HObject ho_image;
                    //HOperatorSet.GenImageInterleaved(out ho_image,
                    //(HTuple)pData,
                    //"bgr",
                    //m_identityFrame.Width,
                    //m_identityFrame.Height,
                    //0,
                    //"byte",
                    //m_identityFrame.Width,
                    //m_identityFrame.Height,
                    //0, 0, 8, 0);       
                    Bitmap res;
                    res = m_identityFrame.ToBitmap(false);
                    string path = m_newConfig.m_imageFetchPath + "\\Fet_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_fff") + ".bmp";
                    res.Save(path, System.Drawing.Imaging.ImageFormat.Bmp);
                }


                // 主动调用回收垃圾 
                // call garbage collection 
                GC.Collect();

                // 控制显示最高帧率为25FPS 
                // control frame display rate to be 25 FPS 
                if (false == isTimeToDisplay())
                {
                    continue;
                }

                try
                {
                    // 图像转码成bitmap图像 
                    // raw frame data converted to bitmap 
                    var bitmap = m_identityFrame.ToBitmap(false);
                    m_bShowByGDI = true;

                    // 由IGrabbedRawData转换为HObject
                    int nRGB = RGBFactory.EncodeLen(m_identityFrame.Width, m_identityFrame.Height, false);
                    IntPtr pData = Marshal.AllocHGlobal(nRGB);
                    Marshal.Copy(m_identityFrame.Image, 0, pData, m_identityFrame.ImageSize);
                    HObject ho_Image = null;
                    HOperatorSet.GenEmptyObj(out ho_Image);
                    HOperatorSet.GenImage1Extern(out ho_Image,
                    "byte",
                    m_identityFrame.Width,//nWidth,
                    m_identityFrame.Height,//nHeight,
                    (HTuple)pData,
                    0);
                    Marshal.FreeHGlobal(pData);    // 2021 02 16              

                    //进行模板匹配
                    //HObject ho_ImageScrew, ho_ROIScrew = null;
                    //HObject ho_DefectRegion = null;
                    //HTuple hv_WindowHandle = new HTuple(), hv_RowCheck = new HTuple();
                    //HTuple hv_ColumnCheck = new HTuple(), hv_AngleCheck = new HTuple();
                    //HTuple hv_Score = new HTuple(), hv_Length = new HTuple();
                    //HTuple hv_i = new HTuple();       // hv_PathScrew = new HTuple();
                    //HTuple hv_Height = new HTuple();  //HTuple hv_score = new HTuple(),
                    //HTuple hv_PathDefect = new HTuple();
                    //HOperatorSet.GenEmptyObj(out ho_ImageScrew);
                    //HOperatorSet.GenEmptyObj(out ho_ROIScrew);
                    //HOperatorSet.GenEmptyObj(out ho_DefectRegion);
                    ////清空变量
                    //hv_Length.Dispose(); hv_Height.Dispose(); hv_RowCheck.Dispose(); hv_ColumnCheck.Dispose(); hv_AngleCheck.Dispose(); hv_Score.Dispose();
                    //TempMatch(ho_Image, "C:\\Users\\Admin\\Desktop\\czw\\ImageSave\\ScrewModle.shm", 0.9,
                    //            out hv_Length, out hv_Height, out hv_RowCheck, out hv_ColumnCheck, out hv_AngleCheck, out hv_Score);
                    //if ((int)(new HTuple((new HTuple(hv_Score.TupleLength())).TupleGreater(0))) != 0)
                    //{
                    //    for (hv_i = 0; (int)hv_i <= (int)((new HTuple(hv_Score.TupleLength())) - 1); hv_i = (int)hv_i + 1)
                    //    {
                    //        //画矩形,标注出螺纹区域位置
                    //        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    //        {
                    //            ho_ROIScrew.Dispose();
                    //            HOperatorSet.GenRectangle2(out ho_ROIScrew, hv_RowCheck.TupleSelect(hv_i),
                    //                hv_ColumnCheck.TupleSelect(hv_i), hv_AngleCheck.TupleSelect(hv_i), hv_Length / 2,
                    //                hv_Height / 2);
                    //        }
                    //    }
                    //}
                    //ho_ImageScrew.Dispose();
                    //HOperatorSet.ReduceDomain(ho_Image, ho_ROIScrew, out ho_ImageScrew);
                    //hv_Length.Dispose(); hv_Height.Dispose(); hv_RowCheck.Dispose(); hv_ColumnCheck.Dispose(); hv_AngleCheck.Dispose(); hv_Score.Dispose();
                    //TempMatch(ho_ImageScrew, "C:\\Users\\Admin\\Desktop/czw\\ImageSave\\DefectModle.shm", 0.93, out hv_Length, out hv_Height,
                    //    out hv_RowCheck, out hv_ColumnCheck, out hv_AngleCheck, out hv_Score);
                    //if ((int)(new HTuple((new HTuple(hv_Score.TupleLength())).TupleGreater(0))) != 0)
                    //{
                    //    for (hv_i = 0; (int)hv_i <= (int)((new HTuple(hv_Score.TupleLength())) - 1); hv_i = (int)hv_i + 1)
                    //    {
                    //        //画矩形,标注出螺纹区域位置
                    //        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    //        {
                    //            ho_DefectRegion.Dispose();
                    //            HOperatorSet.GenRectangle2(out ho_DefectRegion, hv_RowCheck.TupleSelect(hv_i),
                    //                hv_ColumnCheck.TupleSelect(hv_i), hv_AngleCheck.TupleSelect(hv_i), hv_Length / 2,
                    //                hv_Height / 2);
                    //        }
                    //        HOperatorSet.OverpaintRegion(ho_Image, ho_DefectRegion, 255, "margin");
                    //    }
                    //}

                    //Bitmap res;
                    //HObject2Bpp24(ho_Image, out res);

                    // 传入显示控件及图像，返回检测结果
                    //m_identity.identifyAction(hwcImageIdentity.HalconWindow, ho_image, out m_result);
                    //showMessage("临时识别为【" + m_result + "】。", InfoType.Debug);
                    if (m_bShowByGDI)
                    {
                        // 使用GDI绘图 
                        // create graphic object 
                        if (_g == null)
                        {
                            _g = pbImageIdentity.CreateGraphics();
                        }
                        //if (_a == null)
                        //{
                        //    _a = hwcImageIdentity.CreateGraphics();
                        //}
                        _g.DrawImage(bitmap, new Rectangle(0, 0, pbImageIdentity.Width, pbImageIdentity.Height),
                        new Rectangle(0, 0, bitmap.Width, bitmap.Height), GraphicsUnit.Pixel);
                        bitmap.Dispose();
                        //_a.DrawImage(res, new Rectangle(0, 0, hwcImageIdentity.Width, hwcImageIdentity.Height),
                        //                new Rectangle(0, 0, res.Width, res.Height), GraphicsUnit.Pixel);
                        //res.Dispose();

                    }
                    else
                    {
                        // 使用控件绘图,会造成主界面卡顿 
                        // assign bitmap to image control will cause main window reflect slowly  
                        if (InvokeRequired)
                        {
                            BeginInvoke(new MethodInvoker(() =>
                            {
                                try
                                {
                                    pbImageIdentity.Image = bitmap;
                                }
                                catch (Exception exception)
                                {
                                    // Catcher.Show(exception);
                                    showMessage(exception.Message, InfoType.Error);
                                }
                            }));
                        }
                    }
                }
                catch (Exception exception)
                {
                    // Catcher.Show(exception);
                    showMessage(exception.Message, InfoType.Error);
                }
            }
        }*/
        //模板匹配算法程序
        //输入参数: ho_Image        待检测图像
        //          hv_PathScrew    保存模板路径
        //          hv_score        检测相似度
        //输出结果：hv_Length       模板区域的宽度
        //          hv_Height       模板区域的高度
        //          hv_RowCheck     数组，待检测图像匹配区域中心点行坐标
        //          hv_ColumnCheck  数组，待检测图像匹配区域中心点列坐标
        //          hv_AngleCheck   数组，待检测图像匹配区域旋转角度
        //          hv_Score        数组，待检测图像匹配区域相似度分数
        private void TempMatch(HObject ho_Image, HTuple hv_PathScrew, HTuple hv_score,
                                out HTuple hv_Length, out HTuple hv_Height, out HTuple hv_RowCheck, out HTuple hv_ColumnCheck,
                                out HTuple hv_AngleCheck, out HTuple hv_Score)
        {
            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_ScrewShape, ho_ClosedScrew, ho_ScrewRegion;

            // Local control variables 

            HTuple hv_ScrewModel = new HTuple(), hv_Rows = new HTuple();
            HTuple hv_Columns = new HTuple(), hv_MaxRow = new HTuple();
            HTuple hv_MinRow = new HTuple(), hv_MaxColumn = new HTuple();
            HTuple hv_MinColumn = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_ScrewShape);
            HOperatorSet.GenEmptyObj(out ho_ClosedScrew);
            HOperatorSet.GenEmptyObj(out ho_ScrewRegion);
            hv_Length = new HTuple();
            hv_Height = new HTuple();
            hv_RowCheck = new HTuple();
            hv_ColumnCheck = new HTuple();
            hv_AngleCheck = new HTuple();
            hv_Score = new HTuple();
            hv_ScrewModel.Dispose();
            HOperatorSet.ReadShapeModel(hv_PathScrew, out hv_ScrewModel);
            ho_ScrewShape.Dispose();
            HOperatorSet.GetShapeModelContours(out ho_ScrewShape, hv_ScrewModel, 1);
            ho_ClosedScrew.Dispose();
            HOperatorSet.CloseContoursXld(ho_ScrewShape, out ho_ClosedScrew);
            //轮廓转换成区域
            ho_ScrewRegion.Dispose();
            HOperatorSet.GenRegionContourXld(ho_ClosedScrew, out ho_ScrewRegion, "filled");
            //将所有区域连通
            {
                HObject ExpTmpOutVar_0;
                HOperatorSet.Union1(ho_ScrewRegion, out ExpTmpOutVar_0);
                ho_ScrewRegion.Dispose();
                ho_ScrewRegion = ExpTmpOutVar_0;
            }
            {
                HObject ExpTmpOutVar_0;
                HOperatorSet.ShapeTrans(ho_ScrewRegion, out ExpTmpOutVar_0, "rectangle1");
                ho_ScrewRegion.Dispose();
                ho_ScrewRegion = ExpTmpOutVar_0;
            }
            hv_Rows.Dispose(); hv_Columns.Dispose();
            HOperatorSet.GetRegionPoints(ho_ScrewRegion, out hv_Rows, out hv_Columns);
            hv_MaxRow.Dispose();
            HOperatorSet.TupleMax(hv_Rows, out hv_MaxRow);
            hv_MinRow.Dispose();
            HOperatorSet.TupleMin(hv_Rows, out hv_MinRow);
            hv_MaxColumn.Dispose();
            HOperatorSet.TupleMax(hv_Columns, out hv_MaxColumn);
            hv_MinColumn.Dispose();
            HOperatorSet.TupleMin(hv_Columns, out hv_MinColumn);
            hv_Length.Dispose();
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                hv_Length = (hv_MaxColumn - hv_MinColumn) + 1;
            }
            hv_Height.Dispose();
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                hv_Height = (hv_MaxRow - hv_MinRow) + 1;
            }
            hv_RowCheck.Dispose(); hv_ColumnCheck.Dispose(); hv_AngleCheck.Dispose(); hv_Score.Dispose();
            HOperatorSet.FindShapeModel(ho_Image, hv_ScrewModel, 0, 0, hv_score, 0, 0.2,
                "least_squares", 0, 1, out hv_RowCheck, out hv_ColumnCheck, out hv_AngleCheck,
                out hv_Score);
            ho_ScrewShape.Dispose();
            ho_ClosedScrew.Dispose();
            ho_ScrewRegion.Dispose();

            hv_ScrewModel.Dispose();
            hv_Rows.Dispose();
            hv_Columns.Dispose();
            hv_MaxRow.Dispose();
            hv_MinRow.Dispose();
            hv_MaxColumn.Dispose();
            hv_MinColumn.Dispose();
     
            return;
        }


        // 判断是否应该做显示操作 
        // calculate interval to determine if it's show time now 
        private bool isTimeToDisplay()
        {
            m_stopWatch.Stop();
            long m_lDisplayInterval = m_stopWatch.ElapsedMilliseconds;
            if (m_lDisplayInterval <= DEFAULT_INTERVAL)
            {
                m_stopWatch.Start();
                return false;
            }
            else
            {
                m_stopWatch.Reset();
                m_stopWatch.Start();
                return true;
            }
        }
        // 设备对象 
        // device object 


        // 相机打开回调 
        // camera open event callback 
        //private void OnCameraOpen(object sender, EventArgs e)
        //{
        //    this.Invoke(new Action(() =>
        //    {
        //        btnOpenIdentify.Enabled = false;
        //        btnCloseIdentify.Enabled = true;
        //    }));
        //}
        // 相机关闭回调 
        // camera close event callback 
        //private void OnCameraClose(object sender, EventArgs e)
        //{
        //    this.Invoke(new Action(() =>
        //    {
        //        btnOpenIdentify.Enabled = true;
        //        btnCloseIdentify.Enabled = false;
        //    }));
        //}
        // 相机丢失回调 
        // camera disconnect event callback 
        //private void OnConnectLoss(object sender, EventArgs e)
        //{
        //    m_dev.ShutdownGrab();
        //    m_dev.Dispose();
        //    m_dev = null;

        //    this.Invoke(new Action(() =>
        //    {
        //        btnOpenIdentify.Enabled = true;
        //        btnCloseIdentify.Enabled = false;
        //    }));
        //}
        // 码流数据回调 
        // grab callback function 
        //private void OnImageGrabbed(Object sender, GrabbedEventArgs e)
        //{
        //    //m_motor.TurnOffLight();
        //    m_mutex.WaitOne();
        //    m_frameList.Add(e.GrabResult.Clone());
        //    m_mutex.ReleaseMutex();
        //}
        // 窗口关闭 
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (bAutoStart)
            {
                if (DialogResult.OK == MessageBox.Show("测试正在进行！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning))
                {
                    e.Cancel = true;
                    return;
                }
            }
            if (m_newConfig.m_RedNumber != 0 || m_newConfig.m_WhiteNumber != 0 || m_newConfig.m_OtherColorNumber != 0)
            {
                if (DialogResult.OK == MessageBox.Show("计数未清零！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning))
                {
                    e.Cancel = true;
                    return;
                }
            }
            //if (btnCloseIdentify.Enabled || btnCloseMotorX.Enabled || btnCloseMotorZ.Enabled)
            if (btnCloseIdentify.Enabled==true||btnCloseAll.Enabled == true || btnAutoIdentity.Enabled == true || btnStopTest.Enabled == true || ManualColorIdentity.Enabled == true || ManualColorIdentityEND.Enabled == true)
            {
                if (DialogResult.OK != MessageBox.Show("尚有设备未手动关闭，确定退出？\r\n确定退出前将尝试自动关闭。", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Question))
                {
                    e.Cancel = true;
                    return;
                }
                else
                {
                    btnCloseAll.PerformClick();
                    e.Cancel = false;
                    return;
                }
            }
        }
        // Window Closed 
        protected override void OnClosed(EventArgs e)
        {
            //if (m_dev != null)
            //{
            //    m_dev.Dispose();
            //    m_dev = null;
            //}

            // renderThread.Join();
            if (autoIdentityThread != null)
                autoIdentityThread.Join();
            if (_g != null)
            {
                _g.Dispose();
                _g = null;
            }
            UIToMemeLevel1();
            UIToMemeLevel2(0,0);
            WriteConfig(m_newConfig);
            showMessage("退出系统!");
            base.OnClosed(e);
        }

        private void ReadConfig(ConfigClass newConfig)
        {            
            IniFile iniFile = new IniFile("D:\\Graphics" + "\\LightPro.dll");
            newConfig.m_bDebug = Convert.ToBoolean(iniFile.IniReadValue("配置信息", "m_bDebug") == "" ? "false" : iniFile.IniReadValue("配置信息", "m_bDebug"));
            newConfig.m_imagePath = iniFile.IniReadValue("配置信息", "m_imagePath") == "" ? "C:\\IdentityPath" : iniFile.IniReadValue("配置信息", "m_imagePath");
            newConfig.m_motorAddressX = iniFile.IniReadValue("配置信息", "m_motorAddressX") == "" ? "COM1" : iniFile.IniReadValue("配置信息", "m_motorAddressX");
            newConfig.m_motorAddressZ = iniFile.IniReadValue("配置信息", "m_motorAddressZ") == "" ? "COM2" : iniFile.IniReadValue("配置信息", "m_motorAddressZ");
            newConfig.m_IoComAddress = iniFile.IniReadValue("配置信息", "m_motorAddressZ") == "" ? "COM2" : iniFile.IniReadValue("配置信息", "m_motorAddressZ");
            //newConfig.m_lightIPAddress = iniFile.IniReadValue("配置信息", "m_lightIPAddress") == "" ? "169.254.76.15" : iniFile.IniReadValue("配置信息", "m_lightIPAddress");
            newConfig.m_lightIPAddress = iniFile.IniReadValue("配置信息", "m_lightIPAddress") == "" ? "169.254.95.130" : iniFile.IniReadValue("配置信息", "m_lightIPAddress");

            newConfig.m_motorStepX = Convert.ToInt32(iniFile.IniReadValue("界面信息", "m_motorStepX") == "" ? "1" : iniFile.IniReadValue("界面信息", "m_motorStepX"));
            newConfig.m_motorStepZ = Convert.ToInt32(iniFile.IniReadValue("界面信息", "m_motorStepZ") == "" ? "1" : iniFile.IniReadValue("界面信息", "m_motorStepZ"));
            newConfig.m_motorGotoX = Convert.ToInt32(iniFile.IniReadValue("界面信息", "m_motorGotoX") == "" ? "10" : iniFile.IniReadValue("界面信息", "m_motorGotoX"));
            newConfig.m_motorGotoZ = Convert.ToInt32(iniFile.IniReadValue("界面信息", "m_motorGotoZ") == "" ? "10" : iniFile.IniReadValue("界面信息", "m_motorGotoZ"));
            newConfig.m_motorSpeedX = Convert.ToInt32(iniFile.IniReadValue("界面信息", "m_motorSpeedX") == "" ? "30" : iniFile.IniReadValue("界面信息", "m_motorSpeedX"));
            newConfig.m_motorSpeedZ = Convert.ToInt32(iniFile.IniReadValue("界面信息", "m_motorSpeedZ") == "" ? "30" : iniFile.IniReadValue("界面信息", "m_motorSpeedZ"));
            newConfig.m_motorSafeX = Convert.ToInt32(iniFile.IniReadValue("界面信息", "m_motorSafeX") == "" ? "10" : iniFile.IniReadValue("界面信息", "m_motorSafeX"));
            newConfig.m_motorSafeZ = Convert.ToInt32(iniFile.IniReadValue("界面信息", "m_motorSafeZ") == "" ? "10" : iniFile.IniReadValue("界面信息", "m_motorSafeZ"));
            newConfig.m_intensity = Convert.ToInt32(iniFile.IniReadValue("界面信息", "m_intensity") == "" ? "122" : iniFile.IniReadValue("界面信息", "m_intensity"));
            newConfig.m_imageFetchPath = iniFile.IniReadValue("界面信息", "m_imageFetchPath") == "" ? "C:\\FetchPath" : iniFile.IniReadValue("界面信息", "m_imageFetchPath");
            newConfig.m_colorImageSavePath = iniFile.IniReadValue("界面信息", "m_colorImageSavePath") == "" ? "C:\\FetchPath" : iniFile.IniReadValue("界面信息", "m_colorImageSavePath");
            newConfig.m_fetchInterval = Convert.ToInt32(iniFile.IniReadValue("界面信息", "m_fetchInterval") == "" ? "10" : iniFile.IniReadValue("界面信息", "m_fetchInterval"));
            newConfig.m_fetchTimes = Convert.ToInt32(iniFile.IniReadValue("界面信息", "m_fetchTimes") == "" ? "10" : iniFile.IniReadValue("界面信息", "m_fetchTimes"));

            newConfig.m_motorMinX = Convert.ToInt32(iniFile.IniReadValue("电机运动信息", "m_motorMinX") == "" ? "-100" : iniFile.IniReadValue("电机运动信息", "m_motorMinX"));
            newConfig.m_motorMinZ = Convert.ToInt32(iniFile.IniReadValue("电机运动信息", "m_motorMinZ") == "" ? "-100" : iniFile.IniReadValue("电机运动信息", "m_motorMinZ"));
            newConfig.m_motorMaxX = Convert.ToInt32(iniFile.IniReadValue("电机运动信息", "m_motorMaxX") == "" ? "100" : iniFile.IniReadValue("电机运动信息", "m_motorMaxX"));
            newConfig.m_motorMaxZ = Convert.ToInt32(iniFile.IniReadValue("电机运动信息", "m_motorMaxZ") == "" ? "100" : iniFile.IniReadValue("电机运动信息", "m_motorMaxZ"));
        }

        private void WriteConfig(ConfigClass newConfig)
        {
            IniFile iniFile = new IniFile("D:\\Graphics" + "\\LightPro.dll");
            iniFile.IniWriteValue("配置信息", "m_bDebug", newConfig.m_bDebug);
            iniFile.IniWriteValue("配置信息", "m_imagePath", newConfig.m_imagePath);
            iniFile.IniWriteValue("配置信息", "m_motorAddressX", newConfig.m_motorAddressX);
            iniFile.IniWriteValue("配置信息", "m_motorAddressZ", newConfig.m_motorAddressZ);
            iniFile.IniWriteValue("配置信息", "m_lightIPAddress", newConfig.m_lightIPAddress);
            iniFile.IniWriteValue("配置信息", "m_IoComAddress", newConfig.m_IoComAddress);

            
            iniFile.IniWriteValue("界面信息", "m_motorStepX", newConfig.m_motorStepX);
            iniFile.IniWriteValue("界面信息", "m_motorStepZ", newConfig.m_motorStepZ);
            iniFile.IniWriteValue("界面信息", "m_motorGotoX", newConfig.m_motorGotoX);
            iniFile.IniWriteValue("界面信息", "m_motorGotoZ", newConfig.m_motorGotoZ);
            iniFile.IniWriteValue("界面信息", "m_motorSpeedX", newConfig.m_motorSpeedX);
            iniFile.IniWriteValue("界面信息", "m_motorSpeedZ", newConfig.m_motorSpeedZ);
            iniFile.IniWriteValue("界面信息", "m_motorSafeX", newConfig.m_motorSafeX);
            iniFile.IniWriteValue("界面信息", "m_motorSafeZ", newConfig.m_motorSafeZ);
            iniFile.IniWriteValue("界面信息", "m_intensity", newConfig.m_intensity);
            iniFile.IniWriteValue("界面信息", "m_imageFetchPath", newConfig.m_imageFetchPath);
            iniFile.IniWriteValue("界面信息", "m_colorImageSavePath", newConfig.m_colorImageSavePath);
            iniFile.IniWriteValue("界面信息", "m_fetchInterval", newConfig.m_fetchInterval);
            iniFile.IniWriteValue("界面信息", "m_fetchTimes", newConfig.m_fetchTimes);

            iniFile.IniWriteValue("电机运动信息", "m_motorMinX", newConfig.m_motorMinX);
            iniFile.IniWriteValue("电机运动信息", "m_motorMinZ", newConfig.m_motorMinZ);
            iniFile.IniWriteValue("电机运动信息", "m_motorMaxX", newConfig.m_motorMaxX);
            iniFile.IniWriteValue("电机运动信息", "m_motorMaxZ", newConfig.m_motorMaxZ);
        }
        // 传入文件路径，返回文件路径所在硬盘空间是否大于200M
        bool JustFreeSpaceFun(string path)
        {
            char myChar = path[0];
            int Index = Convert.ToInt32(myChar) - 67;
            long FreeSpace = DriveInfo.GetDrives()[0].AvailableFreeSpace / 1024 / 1024;//剩余M
            if (FreeSpace < 200)
            {
                // 硬件空间不足200M
                return false;
            }
            else
            {
                // 硬件空间充足
                return true;
            }
        }
        private void btnLightTest_Click(object sender, EventArgs e)
        {
            int channel = 1;
            UIToMemeLevel2(0,0);
            if (m_oPTController.SetIntensity(channel, m_newConfig.m_intensity) == 0)
            {
                showMessage("设置曝光值成功！");
                m_motor.OPTController = m_oPTController;
            }
            else
            {
                showMessage("设置曝光值失败！", InfoType.Warn);
            }
            m_motor.TurnOnLight();
            //Thread.Sleep(1000);
            //m_motor.TurnOffLight();
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

        private void btnChoseFetchPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件路径";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                tbFetchPath.Text = dialog.SelectedPath;
            }
        }

        private void btnManualIdentity_Click(object sender, EventArgs e)
        {
            bManualInto = true;
        }
        string strCurrentTime;
        string strCurTime;
        private void btnAutoIdentity_Click(object sender, EventArgs e)
        {
            bAutoStart = true;//自动识别标志位
            bManualInto = false;//手动识别标志位
            btnAutoIdentity.Enabled = false;
            btnStopTest.Enabled = true;
            m_IOCNT.Interval = 100;//设置定时器周期
            m_IOCNT.Enabled = true;//打开界面定时器 zyh
            showMessage("开始自动识别！");
            skinGroupBoxConnect.Enabled = btnExit.Enabled = ManualColorIdentity.Enabled = ManualColorIdentityEND.Enabled = false;
            btnStopTest.Enabled = btnPauseTest.Enabled = true;//界面除暂停识别和停止识别外，其他都屏蔽掉
            
        }
        private string FinalTubeColor = "";
        private int  iIrTrigFlag_Bit0 = 0;//油管触发信号，当黑箱中有油管时一直触发
        private int NumberOfTimer = 0;//为了在有油管触发信号时，间隔一定时间拍照，设置定时器周期后，拍照间隔为T*a整除的数
        private int IdentityStatus = 0;//自动识别过程中状态机的状态
        private string ColorOfTube = null;//调用halcon处理图像后的返回值，返回油管颜色
        private string LastTubeColorSave = null;//存处理过后的结果，显示在界面上
        List<string> strListColoeMessage = new List<string>();//存放每根管子的所有有颜色的处理结果
        private int IOCount = 0;//误识别计数，因为有短暂的误触发问题，目前通过计数方式解决，小于300ms的到位信号不算
        private int TuneLeftCount = 0;//计数，当油管离开限位后还需要再进行处理
        private int TimerForOut = 0;
        private int NumberOfCameraSave = 0;
        private bool testsig = true;
        bool iotsrt=true;
        int bcount = 0;
        private string _colortest=null;
        int testcount = 0;
        //自动识别定时器
        private void m_IOCNT_Tick(object sender, EventArgs e)
        {
            
            if (bAutoStart==true)//自动识别标志位，暂停或停止时为false
            {             
                //******************************1.等待状态******************************
                if (IdentityStatus == 0)
                {
                    iIrTrigFlag_Bit0 = m_motor.tunelIsInpos;//读取到位信号          

                    //当油管到位信号为0时，计数为0
                    if (iIrTrigFlag_Bit0 == 0)
                    {
                        IOCount = 0;
                    }
                    
                    //油管到位信号持续300ms以上才进入下一状态识别
                    if (iIrTrigFlag_Bit0 == 1)
                    {
                        IOCount++;

                        if((IOCount%2)==0)
                        {
                            strListColoeMessage.Clear();//清空色环泛型数组
                            m_motor.OPTController = m_oPTController;
                            //当油管到位后，才打开光源
                            if (blConnectLightCon)
                            {
                                m_motor.TurnOnLight();
                            }
                            //计数器清零
                            NumberOfTimer = 0;
                            TuneLeftCount = 0;
                            TimerForOut = 0;
                            NumberOfCameraSave = 0;
                            IOCount = 0;
                            IdentityStatus =1;
                            
                        }
                    }             
                }
                //=======================================================================//
                //******************************2.识别状态******************************          
                if (IdentityStatus == 1)
                {
                    //计算定时器触发的次数，当偶数的时候，才采集图像。也就意味200ms触发一次
                    //if ((NumberOfTimer % m_newConfig.CameraDelayTime) == 0)
                    //{
                        //相机存图
                        ImageSave();
                        string MidColor = null;
                        //halcon处理，将检测到的颜色存到MidColor
                        MidColor = halconIdentity(IdentitySys.tubecolor.pathoftubepic, IdentitySys.tubecolor.savecolorpic);
                        //将检测到的所有颜色存入数组中，在输出时进行优先级判断
                        if (MidColor != null)
                        {
                            ColorOfTube = MidColor;
                            strListColoeMessage.Add(ColorOfTube); //检测到颜色，把颜色存到数组里                           
                        }
                        else
                        {
                            MidColor = null;
                        }
                    NumberOfCameraSave++;
                   // }
                   //计算定时器触发的次数，当奇数的时候，进行图像处理
                   // NumberOfTimer++;
                   //如果未勾选，则不存储图像
                   //if (rbFetchPicture.Checked == false)
                   //{
                   //    File.Delete(tubecolor.pathoftubepic);
                   //}
                   //油管离开视野，进入输出命令状态
                    if (m_motor.tunelIsInpos == 0)
                    {
                        TuneLeftCount++;
                        //计数：因为油管离开后还需要拍几张照片
                        if (TuneLeftCount >= 5)//计数超过五次后，输出结果
                        {
                            TuneLeftCount = 0;//计数清零
                            IdentityStatus = 2;//进入输出状态
                        }
                    }
                }
                //=======================================================================//
                //******************************3.输出状态****************************** 
                if (IdentityStatus == 2)
                {
                    //输出时设置一个计数器，先显示油管最终颜色，颜色显示1.6秒后再进行输出
                    if (NumberOfCameraSave > 40)
                    {
                        //if (TimerForOut == 0)
                        //{
                            FinalTubeColor = ColorOfTheWholeTube();//根据优先级判断色环数组的最后输出结果 红 绿 蓝 黄 白
                            showMessage("油管颜色为" + FinalTubeColor + "" + LevelofTube(FinalTubeColor));
                            LastTubeColorSave = FinalTubeColor;
                            strListColoeMessage.Clear();//清空色环泛型数组
                            ShowLastTubeColor(LastTubeColorSave);//界面显示色环判断结果
                            ShowNumber();//界面显示色环识别计数结果
                                         //检测完毕后，关闭光源
                            m_motor.OPTController = m_oPTController;
                            if (blConnectLightCon)
                            {
                                m_motor.TurnOffLight();
                            }
            
                            //}

                            //TimerForOut++;

                            //IO板卡输出最终结果

                            //if (TimerForOut == 15)
                            //{
                            if (FinalTubeColor == "White")
                        {
                            m_motor.OutputColorResult("Red");
                            Thread.Sleep(1500);//输出信号持续一秒
                            m_motor.OutputColorResult("CloseAll");//关闭IO卡
                        }
                           
                            if (m_motor.tunelIsInpos == 1)//油管到位
                            {
                                btnTunelIsInpos.Text = "油管到位";
                                btnTunelIsInpos.Enabled = true;
                                btnTunelIsInpos.BaseColor = Color.Red;
                            }
                            else
                            {
                                btnTunelIsInpos.Text = "油管未到位";
                                btnTunelIsInpos.Enabled = false;
                                btnTunelIsInpos.BaseColor = Color.White;
                            }
                           
                            ColorOfTube = null;
                            strListColoeMessage.Clear();//清空色环泛型数组
                            TimerForOut = 0;
                            NumberOfCameraSave = 0;
                            IdentityStatus = 0;
                        //}
                    }
                    else
                    {
                        ColorOfTube = null;
                        strListColoeMessage.Clear();//清空色环泛型数组
                        TimerForOut = 0;
                        NumberOfCameraSave = 0;
                        IdentityStatus = 0;
                    }
                }
            }         
        }

        //界面显示色环结果
        private int ShowLastTubeColor(string str)
        {
            switch (str)
            {
                case "Red":
                    LastTubeColor.Text = "红色";
                    LastTubeColor.BaseColor = Color.Red;
                    break;
                case "Green":
                    LastTubeColor.Text = "绿色";
                    LastTubeColor.BaseColor = Color.Green;
                    break;
                case "Yellow":
                    LastTubeColor.Text = "黄色";
                    LastTubeColor.BaseColor = Color.Yellow;
                    break;
                case "Blue":
                    LastTubeColor.Text = "蓝色";
                    LastTubeColor.BaseColor = Color.Blue;
                    break;
                case "White":
                    LastTubeColor.Text = "白色";
                    LastTubeColor.BaseColor = Color.White;
                    break;
                case "无色环":
                    LastTubeColor.Text = "无色环";
                    LastTubeColor.BaseColor = Color.Gray;
                    break;
                default:
                    LastTubeColor.Text = "无色环";
                    LastTubeColor.BaseColor = Color.Gray;
                    break;
            }
            return 0;
        }
        
        //根据色环结果显示油管等级
        private string LevelofTube(string str)
        {
            string level = "";
            if (str == "Red")
            {
                level = "报废管";
            }
            else if (str == "Green")
            {
                level = "二次维修管";
            }
            else if (str == "Yellow")
            {
                level = "一级管";
            }
            else if (str == "Blue")
            {
                level = "二级管";
            }
            else if (str == "White")
            {
                level = "三级管";
            }
            else if (str == "无色环")
            {
                level = "二次维修管";
            }
            return level;
        }

        //存储图片
        private void ImageSave()
        {
            //if (JustFreeSpaceFun(m_newConfig.m_imageFetchPath) == false)//判断存储空间
            //{
            //    showMessage("存储空间不足,停止识别");
            //    m_IOCNT.Enabled = false;//关闭定时器
            //    bAutoStart = false;
            //    btnAutoIdentity.Enabled = skinGroupBoxConnect.Enabled = btnExit.Enabled = true;
            //    // 空间不够了
            //    return;
            //}
            // 
            //IdentityImage 这是个BitMap图像，用这个来做处理
            SetImagePath();//设置存储路径
            IdentityImage.Save(tubecolor.pathoftubepic,ImageFormat.Jpeg);//存储照片
        }

        /// <summary>
        /// 为拍摄图片和识别后的图片 以日期为名称  建立目录路径
        /// </summary>
        private void SetImagePath()
        {           
            strCurrentTime = System.DateTime.Now.ToString("yyyy-MM-dd");
            strCurTime = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_fff");
            if (!System.IO.Directory.Exists(m_newConfig.m_imageFetchPath + "\\" + strCurrentTime))
            {
                System.IO.Directory.CreateDirectory(m_newConfig.m_imageFetchPath + "\\" + strCurrentTime);
            }
            if (!System.IO.Directory.Exists(m_newConfig.m_colorImageSavePath + "\\" + strCurrentTime))
            {
                System.IO.Directory.CreateDirectory(m_newConfig.m_colorImageSavePath + "\\" + strCurrentTime);
            }
            tubecolor.pathoftubepic = m_newConfig.m_imageFetchPath + "\\" + strCurrentTime + "\\" + "Sig_" + strCurTime+".jpg";

            //IdentityImage.Save(tubecolor.pathoftubepic, ImageFormat.Jpeg);

            tubecolor.savecolorpic = m_newConfig.m_colorImageSavePath + "\\" + strCurrentTime + "\\" + "colorSig_" + strCurTime;//这是处理后图片的路径
            //tubecolor.newsavecolorpic = m_newConfig.m_colorImageSavePath + "\\" + strCurrentTime + "\\" + "WeijiancecolorSig_" + strCurTime;//这是处理后图片的路径
            //path_pathoftubepic = m_newConfig.m_imageFetchPath + "\\" + strCurrentTime;

        }

        //halcon处理，输出检测颜色
        private string halconIdentity(string CamSave1, string ProSave1)
        {
            string TubeColor = null;
            HDevelopExport w = new HDevelopExport();
            TubeColor = w.RunHalcon(this.hWindowControl1.HalconWindow, CamSave1, ProSave1);//存储处理之后的图片
            //tubecolor.color = colorofpic;
            colortext.Text = TubeColor;
            return TubeColor;
        }
        
        //优先级判断函数，输出色环数组最后结果
        private string ColorOfTheWholeTube()
        {
            string str = null;
            if (strListColoeMessage.Count != 0)
            {
                if (strListColoeMessage.Contains("Red"))//判断数组中是否有Red红色
                {
                    str = "Red";
                    m_newConfig.m_RedNumber++;
                    return str;
                }
                else if (strListColoeMessage.Contains("White"))
                {
                    str = "White";
                    m_newConfig.m_WhiteNumber++;
                    return str;
                }
                else if (strListColoeMessage.Contains("Blue"))
                {
                    str = "Blue";
                    m_newConfig.m_OtherColorNumber++;
                    return str;
                }
                else if (strListColoeMessage.Contains("Yellow"))
                {
                    str = "Yellow";
                    m_newConfig.m_OtherColorNumber++;
                    return str;
                }
                else if (strListColoeMessage.Contains("Green"))
                {
                    str = "Green";
                    m_newConfig.m_OtherColorNumber++;
                    return str;
                }
            }
            else if(strListColoeMessage.Count == 0)
            {
                str = "无色环";
                m_newConfig.m_OtherColorNumber++;
                return str;
            }
            return "White";
        }

        //手动识别判断油管最后的颜色
        private string ManualColorOfTheWholeTube()
        {
            string strColor = "";
            if (redColor.BaseColor == Color.IndianRed)//红色按钮为浅红色
            {
                strColor = "Red";
                m_newConfig.m_RedNumber++;
                return strColor;
            }
            else if (whiteColor.BaseColor == Color.Gray)
            {
                strColor = "White";
                m_newConfig.m_WhiteNumber++;
                return strColor;
            }
            else if (yellowColor.BaseColor == Color.PaleGoldenrod)
            {
                strColor = "Yellow";
                m_newConfig.m_OtherColorNumber++;
                return strColor;
            }
            else if (blueColor.BaseColor == Color.CornflowerBlue)
            {
                strColor = "Blue";
                m_newConfig.m_OtherColorNumber++;
                return strColor;
            }
            else if (greenColor.BaseColor == Color.PaleGreen)
            {
                strColor = "Green";
                m_newConfig.m_OtherColorNumber++;
                return strColor;
            }
            else
            {
                strColor = "无色环";
                m_newConfig.m_OtherColorNumber++;
                return strColor;
            }
        }

        private void btnPauseTest_Click(object sender, EventArgs e)
        {
            //bAutoStart = false;
            //showMessage("暂停识别！");
            //btnContinueTest.Enabled = true;
            //btnPauseTest.Enabled = false;
            m_motor.OutputColorResult("Red");
            Thread.Sleep(1500);
            m_motor.OutputColorResult("CloseAll");
        }

        private void btnContinueTest_Click(object sender, EventArgs e)
        {
            //bAutoStart = true;
            //showMessage("继续识别！");
            //btnContinueTest.Enabled = false;
            //btnPauseTest.Enabled = true;
            m_motor.OutputColorResult("CloseAll");
        }

        private void btnStopTest_Click(object sender, EventArgs e)
        {
                btnAutoIdentity.Enabled = true;
            m_IOCNT.Enabled = false;
            bAutoStart = false;
                showMessage("停止识别！");
                strListColoeMessage.Clear();
                ManualColorIdentity.Enabled = true;
            btnStopTest.Enabled = false;
                skinGroupBox1.Invoke(new Action(() =>
                {
                    skinGroupBoxConnect.Enabled = btnExit.Enabled = true;
                }));
            //m_motor.OutputColorResult("CloseAll");
        }

        public int triggerNotify(int para)
        {
            if (bAutoStart)
            {
                autoIdentityThread = new Thread(new ThreadStart(AutoIdentityThread));
                autoIdentityThread.Start();
            }
            return 0;
        }

        private void AutoIdentityThread()
        {
            int result = 0;
            // 相机移动到拍照位置
            result = m_motor.autoLocate();
            m_motor.TurnOnLight();            
            if (result != 0)
            {
                // 不成功 和 异常
                m_motor.markResult(result);
                return;
            }
            for (int i = 0; i < m_newConfig.m_fetchTimes; ++i)
            {
                Thread.Sleep(m_newConfig.m_fetchInterval);
                // 0：正常 -1：损坏 -2：异常
                result = SingleIdentity();
                if (result == -1 || bManualInto)
                {
                    // 存储损坏图片
                    Bitmap res;
                    string path = m_newConfig.m_imagePath + "\\" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_fff") + ".bmp";
                    m_motor.markResult(result);
                    bManualInto = false;
                    return;
                }
                else if (result == -2)
                {
                    // 异常
                    m_motor.markResult(result);
                    return;
                }
            }
            m_motor.TurnOffLight();
            // 俩电机回安全位置
            m_motor.MoveParFlag(6, 2, m_newConfig);
            m_motor.markResult(0);
            return;
        }
        // 0：正常 -1：损坏 -2：异常
        private int SingleIdentity()
        {
            try
            {
                if (m_Grabber != IntPtr.Zero)
                {
                    HObject ho_image;
                    IntPtr Image;
                    if (MvApi.CameraGrabber_SaveImage(m_Grabber, out Image, 2000) == CameraSdkStatus.CAMERA_STATUS_SUCCESS)
                    {

                        HOperatorSet.GenImageInterleaved(out ho_image,
                        (HTuple)Image,
                        "bgr",
                        pbImageIdentity.Width,
                        pbImageIdentity.Height,
                        0,
                        "byte",
                        pbImageIdentity.Width,
                        pbImageIdentity.Height,
                        0, 0, 8, 0);
                        // 传入显示控件及图像，返回检测结果                
                        //m_result = m_identity.identifyAction(hwcImageIdentity.HalconWindow, ho_image, out ho_resultImage);
                        // 开始识别
                        showMessage("图像识别结果为【" + m_result + "】。");
                        return m_result;
                    }
                    else
                    {
                        //MessageBox.Show("Snap failed");
                        showMessage("图像获取失败", InfoType.Warn);
                        return -1;
                    }
                }
                else
                {
                    return -1;
                }
            }
            catch (Exception exception)
            {
                // Catcher.Show(exception);
                showMessage(exception.Message, InfoType.Error);
                return -2;
            }
        }

        private void myButton1_Click(object sender, EventArgs e)
        {
            TestForm myTestForm = new TestForm();
            myTestForm.Show();
        }

        private void btnInit_Click(object sender, EventArgs e)
        {

            if (UIToMemeLevel2(0,2) == "Success")
                m_motor.InitMotor(m_newConfig);
        }

        private void btnStopX_Click(object sender, EventArgs e)
        {
            m_motor.MoveParFlag(3, 0, m_newConfig);
        }

        private void btnStopZ_Click(object sender, EventArgs e)
        {
            m_motor.MoveParFlag(3, 1, m_newConfig);
        }

        private void myButton2_Click(object sender, EventArgs e)
        {
            if (UIToMemeLevel2(1,0) == "Success")
                m_motor.TriggerMotor(m_newConfig);
        }

        private void btnXToSafe_Click(object sender, EventArgs e)
        {
            if (UIToMemeLevel2(8,0) == "Success")
                m_motor.MoveParFlag(8, 0, m_newConfig);
        }

        private void btnZToSafe_Click(object sender, EventArgs e)
        {
            if (UIToMemeLevel2(8,1) == "Success")
                m_motor.MoveParFlag(8, 1, m_newConfig);
        }

        private void myButton3_Click(object sender, EventArgs e)
        {
            Form1 myTestForm = new Form1();
            myTestForm.Show();
        }

        private void btnLightControllerConfig_Click(object sender, EventArgs e)
        {
            StringBuilder ip = new StringBuilder(tbLightIPAddress.Text.Trim());
            StringBuilder subnetMask = new StringBuilder("255.255.0.0");
            StringBuilder defaultGateway = new StringBuilder("192.168.1.1");
            try
            {
                long lret = m_oPTController.SetIPConfiguration(ip, subnetMask, defaultGateway);
                if (lret == 0)
                {
                    showMessage("光源控制器IP地址配置成功！");
                }
                else
                {
                    showMessage("光源控制器IP地址配置失败！", InfoType.Warn);
                }
            }
            catch (Exception ex)
            {
                showMessage(ex + "IP地址格式不正确！", InfoType.Error);
            }
        }

        private void btnTurnOffLight_Click(object sender, EventArgs e)
        {
              m_motor.OPTController = m_oPTController;
            if (blConnectLightCon)
                m_motor.TurnOffLight();
            else
                showMessage("请先连接光源控制器！", InfoType.Warn);
        }

        private void btnTurnOnLight_Click(object sender, EventArgs e)
        {
            m_motor.OPTController = m_oPTController;
            if (blConnectLightCon)
            {
                m_motor.TurnOnLight();
            }
            else
            {
                showMessage("请先连接光源控制器！", InfoType.Warn);
            }
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            m_motor.OPTController = m_oPTController;
            if (UIToMemeLevel2(8,2) == "Success")
            {
                m_motor.TransforConfig(m_newConfig);
                m_motor.TestXZAxisBackAndForth();
            }
             
        }

        private bool SaveImage(string path, int FmtIndex, IntPtr pFrameBuffer, tSdkFrameHead FrameHead)
        {
            int Fmt = 2;
            switch (FmtIndex)
            {
                case 0:
                    Fmt = (FrameHead.uiMediaType == (uint)MVSDK.emImageFormat.CAMERA_MEDIA_TYPE_MONO8 ? 16 : 2);
                    break;
                case 1:
                    Fmt = 8;
                    break;
                case 2:
                    Fmt = 1;
                    break;
            }
            return m_AsyncSave.SaveImage(m_hCamera, path, pFrameBuffer, ref FrameHead, (emSdkFileType)Fmt, 80);
        }

        private string CombineImageSavePath(string dir, string filename)
        {
            if (dir.Length > 0)
            {
                try
                {
                    dir = Path.GetFullPath(dir);
                }
                catch (Exception e)
                {
                    dir = "";
                }
            }
            if (dir.Length == 0)
            {
                dir = AppDomain.CurrentDomain.BaseDirectory.ToString();
            }
            return System.IO.Path.Combine(dir, filename);
        }
        private void SingleFrameSave_Click(object sender, EventArgs e)
        {
            //SingleIdentity();
            //Bitmap res;
            if (m_Grabber != IntPtr.Zero)
            {
                IntPtr Image;
                if (MvApi.CameraGrabber_SaveImage(m_Grabber, out Image, 2000) == CameraSdkStatus.CAMERA_STATUS_SUCCESS)
                {
                    /*string path = CombineImageSavePath("",
                        string.Format("Snap{0}", System.Environment.TickCount));*/
                    tubecolor.pathoftubepic = m_newConfig.m_imageFetchPath +"//"+ "Sig_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_fff");
                    //string path = m_newConfig.m_imageFetchPath + "Sig_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_fff");
                    IntPtr pFrameBuffer, pFrameHead;
                    MvApi.CameraImage_GetData(Image, out pFrameBuffer, out pFrameHead);
                    tSdkFrameHead FrameHead = (tSdkFrameHead)Marshal.PtrToStructure(pFrameHead, typeof(tSdkFrameHead));

                    //SaveImage(path, 2, pFrameBuffer, FrameHead);
                    SaveImage(tubecolor.pathoftubepic, 2, pFrameBuffer, FrameHead);
                    MvApi.CameraImage_Destroy(Image);

                    //showMessage("存至：" + path);
                    showMessage("存至：" + tubecolor.pathoftubepic);
                }
                else
                {
                    //MessageBox.Show("Snap failed");
                    showMessage("图像获取失败",InfoType.Warn);
                }
            }
            string path = m_newConfig.m_imageFetchPath + "\\Sig_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_fff") + ".bmp";
        }

        private void SoftTrigger()
        {
            if (m_Grabber != IntPtr.Zero)
            {
                int mode = 0;
                MvApi.CameraGetTriggerMode(m_hCamera, ref mode);
                if (mode != 0)
                {
                    // 只触发模式下调用软触发指令
                    MvApi.CameraSoftTrigger(m_hCamera);
                }
            }
        }

        private void btnStopTestXZ_Click(object sender, EventArgs e)
        {
            m_motor.StopTestXZAxisBackAndForth();
        }

        private void hwcImageIdentity_HMouseMove(object sender, HMouseEventArgs e)
        {

        }

        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            if (btnOpenIdentify.Enabled == false)
           if (MvApi.CameraGrabber_SetHWnd != null)
             MvApi.CameraGrabber_SetHWnd(m_Grabber, this.pbImageIdentity.Handle);
        }

        //界面定时器
        private void m_TriggerTimer_Tick(object sender, EventArgs e)
        {
            SoftTrigger();
            if (m_motor.tunelIsInpos == 1)//油管到位
            {
                
                btnTunelIsInpos.Text = "油管到位";
                btnTunelIsInpos.Enabled = true;
                btnTunelIsInpos.BaseColor = Color.Red;
            }
            else
            {
                btnTunelIsInpos.Text = "油管未到位";
                btnTunelIsInpos.Enabled = false;
                btnTunelIsInpos.BaseColor = Color.White;
            }
            if (bManualInto == false)
            {
                 redColor.Enabled = whiteColor.Enabled = greenColor.Enabled = yellowColor.Enabled = blueColor.Enabled = false;
            }
            else
            {
                ShowLastTubeColor(ManualColor);
                 redColor.Enabled = whiteColor.Enabled = greenColor.Enabled = yellowColor.Enabled = blueColor.Enabled = true;
            }
            if (btnOpenAll.Enabled == true && btnCloseAll.Enabled == false)
            {
                skinGroupBoxOp.Enabled = false;
            }
            if (btnAutoIdentity.Enabled == true)
            {
                btnPauseTest.Enabled = btnContinueTest.Enabled = false;
            }
        }

        private void btnCamSet_Click(object sender, EventArgs e)
        {
            if (m_Grabber != IntPtr.Zero)
                MvApi.CameraShowSettingPage(m_hCamera, 1);
        }
        List<string> strListManualColor = new List<string>();//定义数组，存手动识别的识别结果
        
        
        private void ManualColorIdentity_Click(object sender, EventArgs e)
        {
            bManualInto = true;//手动识别标志位
            bAutoStart = false;
            manual_IOCNT.Interval = 100;
            manual_IOCNT.Enabled = true;//打开手动识别定时器
            m_IOCNT.Enabled = false;//手动识别打开时把定时器关闭
            showMessage("开始手动识别");
            //界面按钮屏蔽
            ManualColorIdentity.Enabled=btnAutoIdentity.Enabled = btnPauseTest.Enabled = btnContinueTest.Enabled = btnOpenAll.Enabled =btnStopTest.Enabled= btnCloseAll.Enabled=false;
            skinGroupBoxConnect.Enabled = btnExit.Enabled = true;
            ManualColorIdentityEND.Enabled = true;
            m_oPTController.TurnOnChannel(1);
        }
        private int Manual_tubeArriveSignal = 0;//油管到位标志位
        private int ManualStatus = 0;//手动识别标志位
        private string ManualColor = null;
        private int CalculateTimeOfSignal = 0;
        private int WaitTimeForOutput = 0;
        
        //手动识别定时器
        private void manual_IOCNT_Tick(object sender, EventArgs e)
        {
            Manual_tubeArriveSignal = m_motor.tunelIsInpos;//油管到位信号
            //****************状态1：等待状态****************
            if (ManualStatus == 0)
            {
                if (Manual_tubeArriveSignal == 0)
                {
                    WaitTimeForOutput = 0;
                    CalculateTimeOfSignal = 0;
                }

                if (Manual_tubeArriveSignal == 1)
                {
                    CalculateTimeOfSignal++;
                }
                if (CalculateTimeOfSignal > 9)
                {
                    ManualStatus = 1;
                    CalculateTimeOfSignal = 0;
                }
            }
            //****************状态2：输出状态****************
            if (ManualStatus == 1)
            {
                if (Manual_tubeArriveSignal == 0 && bManualInto == true)//当油管到位信号消失后，控制IO板卡输出
                {
                    if (WaitTimeForOutput == 0)
                    {
                        ManualColor = ManualColorOfTheWholeTube();//IO卡输出信号
                        ShowNumber();//计数显示
                        showMessage("手动识别油管颜色为" + ManualColor + "" + LevelofTube(ManualColor));
                    }
                    WaitTimeForOutput++;
                    if (WaitTimeForOutput == 3)
                    {
                        if (ManualColor == "White")
                        {
                            m_motor.OutputColorResult("Red");
                            //输出
                            Thread.Sleep(1500);//输出信号持续1.5秒
                            m_motor.OutputColorResult("CloseAll");
                        }
                      
                        //清空状态
                        ColorOfTube = null;
                        IdentityStatus = 0;
                        strListColoeMessage.Clear();
                        WaitTimeForOutput = 0;
                        CalculateTimeOfSignal = 0;
                        ManualStatus = 0;
                    }
                }
            }
        }

        //显示计数结果
        private void ShowNumber()
        {
            RedNumber.Text = m_newConfig.m_RedNumber.ToString();
            WhiteNumber.Text = m_newConfig.m_WhiteNumber.ToString();
            OtherColorNumber.Text = m_newConfig.m_OtherColorNumber.ToString();
        }
        private void myButton2_Click_1(object sender, EventArgs e)
        {

            if (m_oPTController.SoftwareTrigger(1, 100) == 0)
            {
                showMessage("相机软触发设置成功！");
            }
            else
            {
                showMessage("相机软触发设置失败！");
            }
            //m_oPTController.SoftwareTrigger(1, 100);
        }

        private void TubeOnPos_Click(object sender, EventArgs e)
        {
            iIrTrigFlag_Bit0 = 1;
            showMessage("油管到位");            
        }

        private void btnCNT_Click(object sender, EventArgs e)
        {
            //AC6652_CNT_Run(hDevice, 0, 0);
            m_IOCNT.Enabled = true;
        }

        private void btnCNTS_Click(object sender, EventArgs e)
        {
            m_IOCNT.Enabled = false;
        }

        private void myButton5_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件路径";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                coloridentity.Text = dialog.SelectedPath;
            }
        }

        private void btnOpenCOM_Click(object sender, EventArgs e)
        {
            string serialName = cbSerial.SelectedItem.ToString();
            if(btnOpenCOM.Text=="打开串口")
            {
                if (m_motor.OpenCOM(serialName) == 0)
                    btnOpenCOM.Text = "关闭串口";
            }
            else if(btnOpenCOM.Text == "关闭串口")
            {
                m_motor.CloseCOM(serialName);
                btnOpenCOM.Text = "打开串口";
            }
            
        }

        private void btnOpenY1_Click(object sender, EventArgs e)
        {
           m_motor.OutputColorResult("Red");
        }

        private void btnOpenY2_Click(object sender, EventArgs e)
        {
            m_motor.OutputColorResult("Yellow");
        }

        private void btnOpenY3_Click(object sender, EventArgs e)
        {
            m_motor.OutputColorResult("Blue");
        }

        private void btnOpenY4_Click(object sender, EventArgs e)
        {
            m_motor.OutputColorResult("Green");
        }

        private void btnCloseAllY1_4_Click(object sender, EventArgs e)
        {
            m_motor.OutputColorResult("CloseAll");
        }

        private void btnOpenY34_Click(object sender, EventArgs e)
        {
            m_motor.OutputColorResult("White");
        }


        private void tubearrive_Paint(object sender, PaintEventArgs e)
        {

        }

        private void ManualColorIdentityEND_Click(object sender, EventArgs e)
        {
            manual_IOCNT.Enabled = false;
            showMessage("手动识别停止");
            redColor.BaseColor = Color.Red;
            greenColor.BaseColor = Color.Green;
            yellowColor.BaseColor = Color.Yellow;
            whiteColor.BaseColor = Color.White;
            blueColor.BaseColor = Color.Blue;
            bManualInto = false;
            ManualColorIdentity.Enabled = btnAutoIdentity.Enabled = btnPauseTest.Enabled = btnContinueTest.Enabled  = btnCloseAll.Enabled = true;
            ManualColorIdentityEND.Enabled = false;
            m_oPTController.TurnOffChannel(1);
        }

        private void radioNoneColor_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

        private void redColor_Click(object sender, EventArgs e)
        {
            if (bManualInto == true && redColor.BaseColor != Color.IndianRed && greenColor.BaseColor != Color.PaleGreen)
            {
                showMessage("色环为红色");
                redColor.BaseColor = Color.IndianRed;
                return;
            }
            if (bManualInto == true && redColor.BaseColor == Color.IndianRed && greenColor.BaseColor != Color.PaleGreen)
            {
                showMessage("删除红色色环");
                redColor.BaseColor = Color.Red;
                return;
            }
        }

        private void greenColor_Click(object sender, EventArgs e)
        {
            if (bManualInto == true && greenColor.BaseColor != Color.PaleGreen && redColor.BaseColor != Color.IndianRed)
            {
                showMessage("色环为绿色");
                greenColor.BaseColor = Color.PaleGreen;
                return;
            }
            if (bManualInto == true && greenColor.BaseColor == Color.PaleGreen && redColor.BaseColor != Color.IndianRed)
            {
                showMessage("删除绿色色环");
                greenColor.BaseColor = Color.Green;
                return;
            }
        }

        private void blueColor_Click(object sender, EventArgs e)
        {
            if (bManualInto == true && blueColor.BaseColor != Color.CornflowerBlue && whiteColor.BaseColor != Color.Gray && yellowColor.BaseColor != Color.PaleGoldenrod)
            {
                showMessage("色环为蓝色");
                blueColor.BaseColor = Color.CornflowerBlue;
                return;
            }
            if (bManualInto == true && blueColor.BaseColor == Color.CornflowerBlue && whiteColor.BaseColor != Color.Gray && yellowColor.BaseColor != Color.PaleGoldenrod)
            {
                showMessage("删除蓝色色环");
                blueColor.BaseColor = Color.Blue;
                return;
            }
        }

        private void whiteColor_Click(object sender, EventArgs e)
        {
            if (bManualInto == true && whiteColor.BaseColor != Color.Gray && yellowColor.BaseColor != Color.PaleGoldenrod && blueColor.BaseColor != Color.CornflowerBlue)
            {
                showMessage("色环为白色");
                whiteColor.BaseColor = Color.Gray;
                return;
            }
            if (bManualInto == true && whiteColor.BaseColor == Color.Gray && yellowColor.BaseColor != Color.PaleGoldenrod && blueColor.BaseColor != Color.CornflowerBlue)
            {
                showMessage("删除白色色环");
                whiteColor.BaseColor = Color.White;
                return;
            }
        }

        private void yellowColor_Click(object sender, EventArgs e)
        {
            if (bManualInto == true && yellowColor.BaseColor != Color.PaleGoldenrod && whiteColor.BaseColor != Color.Gray && blueColor.BaseColor != Color.CornflowerBlue)
            {
                showMessage("色环为黄色");
                yellowColor.BaseColor = Color.PaleGoldenrod;
                return;
            }
            if (bManualInto == true && yellowColor.BaseColor == Color.PaleGoldenrod && whiteColor.BaseColor != Color.Gray && blueColor.BaseColor != Color.CornflowerBlue)
            {
                showMessage("删除黄色色环");
                yellowColor.BaseColor = Color.Yellow;
                return;
            }
        }

        private void noneColor_Click(object sender, EventArgs e)
        {

        }

        //清空计数
        private void ClearNum_Click(object sender, EventArgs e)
        {
            m_newConfig.m_RedNumber = m_newConfig.m_WhiteNumber = m_newConfig.m_OtherColorNumber = 0;
            RedNumber.Text = m_newConfig.m_RedNumber.ToString();
            WhiteNumber.Text = m_newConfig.m_WhiteNumber.ToString();

            OtherColorNumber.Text = m_newConfig.m_OtherColorNumber.ToString();

        }

        private void ImageTest_Click(object sender, EventArgs e)
        {
            string MidColor = null;

            // 选择图片
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "图像文件|*.jpg;*.jpeg;*.png;*.bmp";
            dialog.Title = "选择油管图像";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string selectedImage = dialog.FileName;

                // 验证图片尺寸（可选）
                using (Image img = Image.FromFile(selectedImage))
                {
                    if (img.Width < 100 || img.Height < 100)
                    {
                        MessageBox.Show("图片尺寸过小，请选择更大的图片", "警告",
                                       MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                // 执行识别
                string outputPath = Path.Combine(
                    Path.GetDirectoryName(selectedImage),
                    "processed",
                    Path.GetFileName(selectedImage)
                );

                // 确保输出目录存在
                Directory.CreateDirectory(Path.GetDirectoryName(outputPath));

                // 调用识别函数
                MidColor = halconIdentity(selectedImage, outputPath);
            }

            //将检测到的所有颜色存入数组中，在输出时进行优先级判断
            if (MidColor != null)
            {
                ColorOfTube = MidColor;
                strListColoeMessage.Add(ColorOfTube); //检测到颜色，把颜色存到数组里                           
            }
            else
            {
                MidColor = null;
            }

            FinalTubeColor = ColorOfTheWholeTube();//根据优先级判断色环数组的最后输出结果 红 绿 蓝 黄 白
            showMessage("油管颜色为" + FinalTubeColor + "" + LevelofTube(FinalTubeColor));
            LastTubeColorSave = FinalTubeColor;
            strListColoeMessage.Clear();//清空色环泛型数组
            ShowLastTubeColor(LastTubeColorSave);//界面显示色环判断结果
            ShowNumber();//界面显示色环识别计数结果

            if (FinalTubeColor == "White")
            {
                m_motor.OutputColorResult("Red");
                Thread.Sleep(1500);//输出信号持续一秒
                m_motor.OutputColorResult("CloseAll");//关闭IO卡
            }

        }
    }
    public class tubecolor
    {
        public static string color;
        public static bool startcoloridentity = true;
        public static string pathoftubepic;
        public static bool youguandaowei=false;
        public static string savecolorpic;
        //public static string newsavecolorpic;
    }
    public class AsyncSaveImage
    {
        private class Item
        {
            public CameraHandle hCamera;
            public IntPtr Image;
            public string FileName;
            public emSdkFileType FileType;
            public byte Quality;
        }

        private Queue<Item> mImageQ = new Queue<Item>();
        private int mMaxQSize = 1024;
        private EventWaitHandle mQEvent = new EventWaitHandle(false, EventResetMode.ManualReset);
        private EventWaitHandle mQuitEvent = new EventWaitHandle(false, EventResetMode.ManualReset);
        private List<Thread> mSaveThreads;
        private int mThreadsNum = System.Environment.ProcessorCount;

        public AsyncSaveImage(int MaxQSize = 0, int nThreadNum = 0)
        {
            if (MaxQSize > 0)
            {
                mMaxQSize = MaxQSize;
            }
            if (nThreadNum > 0)
            {
                mThreadsNum = nThreadNum;
            }
        }

        public void Start()
        {
            if (mSaveThreads != null)
                return;

            mSaveThreads = new List<Thread>();
            mQuitEvent.Reset();
            for (int i = 0; i < mThreadsNum; ++i)
            {
                var thread = new Thread(SaveProc);
                thread.Start();
                mSaveThreads.Add(thread);
            }
        }

        public void Stop()
        {
            if (mSaveThreads != null)
            {
                mQuitEvent.Set();
                foreach (var thread in mSaveThreads)
                {
                    thread.Join();
                }
                mSaveThreads = null;
            }
        }

        public void Clear()
        {
            lock (mImageQ)
            {
                foreach (var item in mImageQ)
                {
                    MvApi.CameraImage_Destroy(item.Image);
                }
                mImageQ.Clear();
            }
        }

        public bool SaveImage(CameraHandle hCamera,
            string lpszFileName,
            IntPtr pbyImageBuffer,
            ref tSdkFrameHead pFrInfo,
            emSdkFileType byFileType,
            Byte byQuality)
        {
            IntPtr Image;
            if (MvApi.CameraImage_Create(out Image, pbyImageBuffer, ref pFrInfo, 1) != CameraSdkStatus.CAMERA_STATUS_SUCCESS)
                return false;

            Item new_item = new Item();
            new_item.hCamera = hCamera;
            new_item.Image = Image;
            new_item.FileName = lpszFileName;
            new_item.FileType = byFileType;
            new_item.Quality = byQuality;

            lock (mImageQ)
            {
                if (mImageQ.Count < mMaxQSize)
                {
                    mImageQ.Enqueue(new_item);
                    mQEvent.Set();
                    return true;
                }
                else
                {
                    MvApi.CameraImage_Destroy(Image);
                    return false;
                }
            }
        }

        private void SaveProc()
        {
            while (true)
            {
                if (WaitHandle.WaitAny(new WaitHandle[] { mQuitEvent, mQEvent }) == 0)
                    break;

                Item item;
                lock (mImageQ)
                {
                    if (mImageQ.Count < 1)
                    {
                        mQEvent.Reset();
                        continue;
                    }
                    item = mImageQ.Dequeue();
                }

                SaveItem(item);
                MvApi.CameraImage_Destroy(item.Image);
            }
        }

        private void SaveItem(Item item)
        {
            IntPtr Image = item.Image;

            IntPtr pRaw, pHeadPtr;
            MvApi.CameraImage_GetData(Image, out pRaw, out pHeadPtr);
            tSdkFrameHead FrameHead = (tSdkFrameHead)Marshal.PtrToStructure(pHeadPtr, typeof(tSdkFrameHead));

            uint uIspOutFmt = 0;
            MvApi.CameraGetIspOutFormat(item.hCamera, ref uIspOutFmt);

            int w = FrameHead.iWidth, h = FrameHead.iHeight;
            if (FrameHead.iWidthZoomSw > 0 && FrameHead.iHeightZoomSw > 0)
            {
                w = FrameHead.iWidthZoomSw;
                h = FrameHead.iHeightZoomSw;
            }

            bool IspOutGray = (uIspOutFmt == (uint)MVSDK.emImageFormat.CAMERA_MEDIA_TYPE_MONO8);
            int RequireBufferSize = w * h * (IspOutGray ? 1 : 3);
            IntPtr pFrameBuffer = MvApi.CameraAlignMalloc(RequireBufferSize, 16);
            if (pFrameBuffer != IntPtr.Zero)
            {
                MvApi.CameraImageProcess(item.hCamera, pRaw, pFrameBuffer, ref FrameHead);
                MvApi.CameraDisplayRGB24(item.hCamera, pFrameBuffer, ref FrameHead);

                MvApi.CameraSaveImage(item.hCamera, item.FileName, pFrameBuffer, ref FrameHead, item.FileType, item.Quality);
                MvApi.CameraAlignFree(pFrameBuffer);
            }
        }
    }
    public class MMTimer : IDisposable
    {
        //Lib API declarations
        [DllImport("Winmm.dll", CharSet = CharSet.Auto)]
        static extern uint timeSetEvent(uint uDelay, uint uResolution, TimerCallback lpTimeProc, UIntPtr dwUser,
                                        uint fuEvent);

        [DllImport("Winmm.dll", CharSet = CharSet.Auto)]
        static extern uint timeKillEvent(uint uTimerID);

        [DllImport("Winmm.dll", CharSet = CharSet.Auto)]
        static extern uint timeGetTime();

        [DllImport("Winmm.dll", CharSet = CharSet.Auto)]
        static extern uint timeBeginPeriod(uint uPeriod);

        [DllImport("Winmm.dll", CharSet = CharSet.Auto)]
        static extern uint timeEndPeriod(uint uPeriod);

        //Timer type definitions
        [Flags]
        public enum fuEvent : uint
        {
            TIME_ONESHOT = 0, //Event occurs once, after uDelay milliseconds. 
            TIME_PERIODIC = 1,
            TIME_CALLBACK_FUNCTION = 0x0000, /* callback is function */
            //TIME_CALLBACK_EVENT_SET = 0x0010, /* callback is event - use SetEvent */
            //TIME_CALLBACK_EVENT_PULSE = 0x0020  /* callback is event - use PulseEvent */
            TIME_KILL_SYNCHRONOUS = 0x0100,
        }

        //Delegate definition for the API callback
        delegate void TimerCallback(uint uTimerID, uint uMsg, UIntPtr dwUser, UIntPtr dw1, UIntPtr dw2);

        //IDisposable code
        private bool disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    Stop();
                }
            }
            disposed = true;
        }

        ~MMTimer()
        {
            Dispose(false);
        }

        /// <summary>
        /// The current timer instance ID
        /// </summary>
        uint id = 0;

        /// <summary>
        /// The callback used by the the API
        /// </summary>
        TimerCallback thisCB;

        /// <summary>
        /// The timer elapsed event 
        /// </summary>
        public event EventHandler Timer;

        protected virtual void OnTimer(EventArgs e)
        {
            if (Timer != null)
            {
                Timer(this, e);
            }
        }

        public MMTimer()
        {
            //Initialize the API callback
            thisCB = CBFunc;
        }

        /// <summary>
        /// Stop the current timer instance (if any)
        /// </summary>
        public void Stop()
        {
            lock (this)
            {
                if (id != 0)
                {
                    timeKillEvent(id);
                    Debug.WriteLine("MMTimer " + id.ToString() + " stopped");
                    id = 0;
                }
            }
        }

        /// <summary>
        /// Start a timer instance
        /// </summary>
        /// <param name="ms">Timer interval in milliseconds</param>
        /// <param name="repeat">If true sets a repetitive event, otherwise sets a one-shot</param>
        public void Start(uint ms, bool repeat)
        {
            //Kill any existing timer
            Stop();

            //Set the timer type flags
            fuEvent f = fuEvent.TIME_CALLBACK_FUNCTION | fuEvent.TIME_KILL_SYNCHRONOUS
                | (repeat ? fuEvent.TIME_PERIODIC : fuEvent.TIME_ONESHOT);

            lock (this)
            {
                id = timeSetEvent(ms, 1, thisCB, UIntPtr.Zero, (uint)f);
                if (id == 0)
                {
                    throw new Exception("timeSetEvent error");
                }
                Debug.WriteLine("MMTimer " + id.ToString() + " started");
            }
        }

        void CBFunc(uint uTimerID, uint uMsg, UIntPtr dwUser, UIntPtr dw1, UIntPtr dw2)
        {
            //Callback from the MMTimer API that fires the Timer event. Note we are in a different thread here
            OnTimer(new EventArgs());
        }
    }
}