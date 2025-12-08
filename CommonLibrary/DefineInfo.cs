using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CommonLibrary
{
    public enum InfoType { Info = 0, Warn, Error, Debug };
    public delegate void smEventHandler(string message, InfoType Type = InfoType.Info);
    public partial class myButton : CCWin.SkinControl.SkinButton
    {
        System.Drawing.Color EnableC_Base = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(45)))), ((int)(((byte)(110)))));
        System.Drawing.Color EnableC_Back = System.Drawing.Color.Transparent;//.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(120)))), ((int)(((byte)(194)))));
        System.Drawing.Color unEnableC = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(112)))), ((int)(((byte)(145)))));
        public myButton()
        {
            base.BaseColor = EnableC_Base;
            base.BackColor = EnableC_Back;

            base.Radius = 10;
            base.RoundStyle = CCWin.SkinClass.RoundStyle.All;
            base.ForeColor = System.Drawing.Color.White;
        }
    }
}
