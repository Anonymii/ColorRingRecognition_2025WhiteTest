namespace IdentitySys
{
    partial class ConfigForm
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
            this.skinLabel1 = new CCWin.SkinControl.SkinLabel();
            this.tbImagePath = new CCWin.SkinControl.SkinTextBox();
            this.skinLabel2 = new CCWin.SkinControl.SkinLabel();
            this.combMotorX = new CCWin.SkinControl.SkinComboBox();
            this.skinLabel3 = new CCWin.SkinControl.SkinLabel();
            this.combMotorZ = new CCWin.SkinControl.SkinComboBox();
            this.btnOK = new CommonLibrary.myButton();
            this.btnCancel = new CommonLibrary.myButton();
            this.btnChosePath = new CommonLibrary.myButton();
            this.skinLabel4 = new CCWin.SkinControl.SkinLabel();
            this.rbDebug = new CCWin.SkinControl.SkinCheckBox();
            this.tbLightIPAddress = new CCWin.SkinControl.SkinTextBox();
            this.skinLabel5 = new CCWin.SkinControl.SkinLabel();
            this.tbIntensity = new CCWin.SkinControl.SkinTextBox();
            this.skinLabel6 = new CCWin.SkinControl.SkinLabel();
            this.SuspendLayout();
            // 
            // skinLabel1
            // 
            this.skinLabel1.AutoSize = true;
            this.skinLabel1.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel1.BorderColor = System.Drawing.Color.Black;
            this.skinLabel1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel1.ForeColor = System.Drawing.Color.White;
            this.skinLabel1.Location = new System.Drawing.Point(22, 63);
            this.skinLabel1.Name = "skinLabel1";
            this.skinLabel1.Size = new System.Drawing.Size(92, 17);
            this.skinLabel1.TabIndex = 0;
            this.skinLabel1.Text = "图片存储路径：";
            // 
            // tbImagePath
            // 
            this.tbImagePath.BackColor = System.Drawing.Color.Transparent;
            this.tbImagePath.DownBack = null;
            this.tbImagePath.Icon = null;
            this.tbImagePath.IconIsButton = false;
            this.tbImagePath.IconMouseState = CCWin.SkinClass.ControlState.Normal;
            this.tbImagePath.IsPasswordChat = '\0';
            this.tbImagePath.IsSystemPasswordChar = false;
            this.tbImagePath.Lines = new string[] {
        "skinTextBox1"};
            this.tbImagePath.Location = new System.Drawing.Point(117, 58);
            this.tbImagePath.Margin = new System.Windows.Forms.Padding(0);
            this.tbImagePath.MaxLength = 32767;
            this.tbImagePath.MinimumSize = new System.Drawing.Size(28, 28);
            this.tbImagePath.MouseBack = null;
            this.tbImagePath.MouseState = CCWin.SkinClass.ControlState.Normal;
            this.tbImagePath.Multiline = false;
            this.tbImagePath.Name = "tbImagePath";
            this.tbImagePath.NormlBack = null;
            this.tbImagePath.Padding = new System.Windows.Forms.Padding(5);
            this.tbImagePath.ReadOnly = false;
            this.tbImagePath.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.tbImagePath.Size = new System.Drawing.Size(151, 28);
            // 
            // 
            // 
            this.tbImagePath.SkinTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbImagePath.SkinTxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbImagePath.SkinTxt.Font = new System.Drawing.Font("微软雅黑", 9.75F);
            this.tbImagePath.SkinTxt.Location = new System.Drawing.Point(5, 5);
            this.tbImagePath.SkinTxt.Name = "BaseText";
            this.tbImagePath.SkinTxt.Size = new System.Drawing.Size(141, 18);
            this.tbImagePath.SkinTxt.TabIndex = 0;
            this.tbImagePath.SkinTxt.Text = "skinTextBox1";
            this.tbImagePath.SkinTxt.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.tbImagePath.SkinTxt.WaterText = "";
            this.tbImagePath.TabIndex = 1;
            this.tbImagePath.Text = "skinTextBox1";
            this.tbImagePath.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.tbImagePath.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.tbImagePath.WaterText = "";
            this.tbImagePath.WordWrap = true;
            // 
            // skinLabel2
            // 
            this.skinLabel2.AutoSize = true;
            this.skinLabel2.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel2.BorderColor = System.Drawing.Color.Black;
            this.skinLabel2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel2.ForeColor = System.Drawing.Color.White;
            this.skinLabel2.Location = new System.Drawing.Point(22, 102);
            this.skinLabel2.Name = "skinLabel2";
            this.skinLabel2.Size = new System.Drawing.Size(88, 17);
            this.skinLabel2.TabIndex = 0;
            this.skinLabel2.Text = "X轴电机地址：";
            // 
            // combMotorX
            // 
            this.combMotorX.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.combMotorX.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combMotorX.FormattingEnabled = true;
            this.combMotorX.Items.AddRange(new object[] {
            "COM1",
            "COM2",
            "COM3",
            "COM4",
            "COM5",
            "COM6",
            "COM7",
            "COM8",
            "COM9"});
            this.combMotorX.Location = new System.Drawing.Point(117, 100);
            this.combMotorX.Name = "combMotorX";
            this.combMotorX.Size = new System.Drawing.Size(185, 22);
            this.combMotorX.TabIndex = 2;
            this.combMotorX.WaterText = "";
            // 
            // skinLabel3
            // 
            this.skinLabel3.AutoSize = true;
            this.skinLabel3.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel3.BorderColor = System.Drawing.Color.Black;
            this.skinLabel3.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel3.ForeColor = System.Drawing.Color.White;
            this.skinLabel3.Location = new System.Drawing.Point(22, 140);
            this.skinLabel3.Name = "skinLabel3";
            this.skinLabel3.Size = new System.Drawing.Size(87, 17);
            this.skinLabel3.TabIndex = 3;
            this.skinLabel3.Text = "Z轴电机地址：";
            // 
            // combMotorZ
            // 
            this.combMotorZ.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.combMotorZ.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combMotorZ.FormattingEnabled = true;
            this.combMotorZ.Items.AddRange(new object[] {
            "COM1",
            "COM2",
            "COM3",
            "COM4",
            "COM5",
            "COM6",
            "COM7",
            "COM8",
            "COM9"});
            this.combMotorZ.Location = new System.Drawing.Point(117, 140);
            this.combMotorZ.Name = "combMotorZ";
            this.combMotorZ.Size = new System.Drawing.Size(185, 22);
            this.combMotorZ.TabIndex = 5;
            this.combMotorZ.WaterText = "";
            // 
            // btnOK
            // 
            this.btnOK.BackColor = System.Drawing.Color.Transparent;
            this.btnOK.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(45)))), ((int)(((byte)(110)))));
            this.btnOK.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.btnOK.DownBack = null;
            this.btnOK.ForeColor = System.Drawing.Color.White;
            this.btnOK.Location = new System.Drawing.Point(117, 260);
            this.btnOK.MouseBack = null;
            this.btnOK.Name = "btnOK";
            this.btnOK.NormlBack = null;
            this.btnOK.Radius = 10;
            this.btnOK.RoundStyle = CCWin.SkinClass.RoundStyle.All;
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(45)))), ((int)(((byte)(110)))));
            this.btnCancel.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.btnCancel.DownBack = null;
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(227, 260);
            this.btnCancel.MouseBack = null;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.NormlBack = null;
            this.btnCancel.Radius = 10;
            this.btnCancel.RoundStyle = CCWin.SkinClass.RoundStyle.All;
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnChosePath
            // 
            this.btnChosePath.BackColor = System.Drawing.Color.Transparent;
            this.btnChosePath.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(45)))), ((int)(((byte)(110)))));
            this.btnChosePath.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.btnChosePath.DownBack = null;
            this.btnChosePath.ForeColor = System.Drawing.Color.White;
            this.btnChosePath.Location = new System.Drawing.Point(271, 58);
            this.btnChosePath.MouseBack = null;
            this.btnChosePath.Name = "btnChosePath";
            this.btnChosePath.NormlBack = null;
            this.btnChosePath.Radius = 10;
            this.btnChosePath.RoundStyle = CCWin.SkinClass.RoundStyle.All;
            this.btnChosePath.Size = new System.Drawing.Size(31, 28);
            this.btnChosePath.TabIndex = 6;
            this.btnChosePath.Text = "...";
            this.btnChosePath.UseVisualStyleBackColor = true;
            this.btnChosePath.Click += new System.EventHandler(this.btnChosePath_Click);
            // 
            // skinLabel4
            // 
            this.skinLabel4.AutoSize = true;
            this.skinLabel4.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel4.BorderColor = System.Drawing.Color.Black;
            this.skinLabel4.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel4.ForeColor = System.Drawing.Color.White;
            this.skinLabel4.Location = new System.Drawing.Point(22, 23);
            this.skinLabel4.Name = "skinLabel4";
            this.skinLabel4.Size = new System.Drawing.Size(92, 17);
            this.skinLabel4.TabIndex = 7;
            this.skinLabel4.Text = "调试信息开关：";
            // 
            // rbDebug
            // 
            this.rbDebug.AutoSize = true;
            this.rbDebug.BackColor = System.Drawing.Color.Transparent;
            this.rbDebug.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.rbDebug.DownBack = null;
            this.rbDebug.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rbDebug.Location = new System.Drawing.Point(122, 26);
            this.rbDebug.MouseBack = null;
            this.rbDebug.Name = "rbDebug";
            this.rbDebug.NormlBack = null;
            this.rbDebug.SelectedDownBack = null;
            this.rbDebug.SelectedMouseBack = null;
            this.rbDebug.SelectedNormlBack = null;
            this.rbDebug.Size = new System.Drawing.Size(15, 14);
            this.rbDebug.TabIndex = 8;
            this.rbDebug.UseVisualStyleBackColor = false;
            // 
            // tbLightIPAddress
            // 
            this.tbLightIPAddress.BackColor = System.Drawing.Color.Transparent;
            this.tbLightIPAddress.DownBack = null;
            this.tbLightIPAddress.Icon = null;
            this.tbLightIPAddress.IconIsButton = false;
            this.tbLightIPAddress.IconMouseState = CCWin.SkinClass.ControlState.Normal;
            this.tbLightIPAddress.IsPasswordChat = '\0';
            this.tbLightIPAddress.IsSystemPasswordChar = false;
            this.tbLightIPAddress.Lines = new string[] {
        "skinTextBox1"};
            this.tbLightIPAddress.Location = new System.Drawing.Point(117, 176);
            this.tbLightIPAddress.Margin = new System.Windows.Forms.Padding(0);
            this.tbLightIPAddress.MaxLength = 32767;
            this.tbLightIPAddress.MinimumSize = new System.Drawing.Size(28, 28);
            this.tbLightIPAddress.MouseBack = null;
            this.tbLightIPAddress.MouseState = CCWin.SkinClass.ControlState.Normal;
            this.tbLightIPAddress.Multiline = false;
            this.tbLightIPAddress.Name = "tbLightIPAddress";
            this.tbLightIPAddress.NormlBack = null;
            this.tbLightIPAddress.Padding = new System.Windows.Forms.Padding(5);
            this.tbLightIPAddress.ReadOnly = false;
            this.tbLightIPAddress.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.tbLightIPAddress.Size = new System.Drawing.Size(185, 28);
            // 
            // 
            // 
            this.tbLightIPAddress.SkinTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbLightIPAddress.SkinTxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbLightIPAddress.SkinTxt.Font = new System.Drawing.Font("微软雅黑", 9.75F);
            this.tbLightIPAddress.SkinTxt.Location = new System.Drawing.Point(5, 5);
            this.tbLightIPAddress.SkinTxt.Name = "BaseText";
            this.tbLightIPAddress.SkinTxt.Size = new System.Drawing.Size(175, 18);
            this.tbLightIPAddress.SkinTxt.TabIndex = 0;
            this.tbLightIPAddress.SkinTxt.Text = "skinTextBox1";
            this.tbLightIPAddress.SkinTxt.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.tbLightIPAddress.SkinTxt.WaterText = "";
            this.tbLightIPAddress.TabIndex = 10;
            this.tbLightIPAddress.Text = "skinTextBox1";
            this.tbLightIPAddress.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.tbLightIPAddress.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.tbLightIPAddress.WaterText = "";
            this.tbLightIPAddress.WordWrap = true;
            // 
            // skinLabel5
            // 
            this.skinLabel5.AutoSize = true;
            this.skinLabel5.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel5.BorderColor = System.Drawing.Color.Black;
            this.skinLabel5.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel5.ForeColor = System.Drawing.Color.White;
            this.skinLabel5.Location = new System.Drawing.Point(22, 181);
            this.skinLabel5.Name = "skinLabel5";
            this.skinLabel5.Size = new System.Drawing.Size(79, 17);
            this.skinLabel5.TabIndex = 9;
            this.skinLabel5.Text = "光源IP地址：";
            // 
            // tbIntensity
            // 
            this.tbIntensity.BackColor = System.Drawing.Color.Transparent;
            this.tbIntensity.DownBack = null;
            this.tbIntensity.Icon = null;
            this.tbIntensity.IconIsButton = false;
            this.tbIntensity.IconMouseState = CCWin.SkinClass.ControlState.Normal;
            this.tbIntensity.IsPasswordChat = '\0';
            this.tbIntensity.IsSystemPasswordChar = false;
            this.tbIntensity.Lines = new string[] {
        "skinTextBox1"};
            this.tbIntensity.Location = new System.Drawing.Point(117, 217);
            this.tbIntensity.Margin = new System.Windows.Forms.Padding(0);
            this.tbIntensity.MaxLength = 32767;
            this.tbIntensity.MinimumSize = new System.Drawing.Size(28, 28);
            this.tbIntensity.MouseBack = null;
            this.tbIntensity.MouseState = CCWin.SkinClass.ControlState.Normal;
            this.tbIntensity.Multiline = false;
            this.tbIntensity.Name = "tbIntensity";
            this.tbIntensity.NormlBack = null;
            this.tbIntensity.Padding = new System.Windows.Forms.Padding(5);
            this.tbIntensity.ReadOnly = false;
            this.tbIntensity.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.tbIntensity.Size = new System.Drawing.Size(185, 28);
            // 
            // 
            // 
            this.tbIntensity.SkinTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbIntensity.SkinTxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbIntensity.SkinTxt.Font = new System.Drawing.Font("微软雅黑", 9.75F);
            this.tbIntensity.SkinTxt.Location = new System.Drawing.Point(5, 5);
            this.tbIntensity.SkinTxt.Name = "BaseText";
            this.tbIntensity.SkinTxt.Size = new System.Drawing.Size(175, 18);
            this.tbIntensity.SkinTxt.TabIndex = 0;
            this.tbIntensity.SkinTxt.Text = "skinTextBox1";
            this.tbIntensity.SkinTxt.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.tbIntensity.SkinTxt.WaterText = "";
            this.tbIntensity.TabIndex = 12;
            this.tbIntensity.Text = "skinTextBox1";
            this.tbIntensity.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.tbIntensity.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.tbIntensity.WaterText = "";
            this.tbIntensity.WordWrap = true;
            // 
            // skinLabel6
            // 
            this.skinLabel6.AutoSize = true;
            this.skinLabel6.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel6.BorderColor = System.Drawing.Color.Black;
            this.skinLabel6.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel6.ForeColor = System.Drawing.Color.White;
            this.skinLabel6.Location = new System.Drawing.Point(22, 222);
            this.skinLabel6.Name = "skinLabel6";
            this.skinLabel6.Size = new System.Drawing.Size(80, 17);
            this.skinLabel6.TabIndex = 11;
            this.skinLabel6.Text = "光源曝光值：";
            // 
            // ConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(120)))), ((int)(((byte)(194)))));
            this.ClientSize = new System.Drawing.Size(335, 298);
            this.Controls.Add(this.tbIntensity);
            this.Controls.Add(this.skinLabel6);
            this.Controls.Add(this.tbLightIPAddress);
            this.Controls.Add(this.skinLabel5);
            this.Controls.Add(this.rbDebug);
            this.Controls.Add(this.skinLabel4);
            this.Controls.Add(this.btnChosePath);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.combMotorZ);
            this.Controls.Add(this.skinLabel3);
            this.Controls.Add(this.combMotorX);
            this.Controls.Add(this.tbImagePath);
            this.Controls.Add(this.skinLabel2);
            this.Controls.Add(this.skinLabel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConfigForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "设置";
            this.Load += new System.EventHandler(this.ConfigForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CCWin.SkinControl.SkinLabel skinLabel1;
        private CCWin.SkinControl.SkinTextBox tbImagePath;
        private CCWin.SkinControl.SkinLabel skinLabel2;
        private CCWin.SkinControl.SkinComboBox combMotorX;
        private CCWin.SkinControl.SkinLabel skinLabel3;
        private CCWin.SkinControl.SkinComboBox combMotorZ;
        private CommonLibrary.myButton btnOK;
        private CommonLibrary.myButton btnCancel;
        private CommonLibrary.myButton btnChosePath;
        private CCWin.SkinControl.SkinLabel skinLabel4;
        private CCWin.SkinControl.SkinCheckBox rbDebug;
        private CCWin.SkinControl.SkinTextBox tbLightIPAddress;
        private CCWin.SkinControl.SkinLabel skinLabel5;
        private CCWin.SkinControl.SkinTextBox tbIntensity;
        private CCWin.SkinControl.SkinLabel skinLabel6;
    }
}