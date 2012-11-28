using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using LgLcd;

namespace LogitechLCD
{
    public abstract class LCDDisplay : MonochromeLCDDisplay
    {
        protected string friendlyName;
        protected BackgroundWorker worker;
        protected Dictionary<int, DrawableText> m_TextMap = new Dictionary<int, DrawableText>();

        public LCDDisplay(string friendlyName, IntPtr handle)
            : base(friendlyName, handle)
        {
            this.friendlyName = friendlyName;
            worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(worker_DoWork);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (UpdateLcdScreen != null) UpdateLcdScreen(this, null);
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
        }

        public override event EventHandler UpdateLcdScreen;

        public override void OnDeviceArrival(DeviceType deviceType)
        {
            Console.WriteLine("OnDeviceArrival({0})", deviceType);
        }
        public override void OnAppletEnabled()
        {
            Console.WriteLine("OnAppletEnabled");
        }
        public override void OnAppletDisabled()
        {
            Console.WriteLine("OnAppletDisabled");
        }
        public override void OnDeviceRemoval(DeviceType deviceType)
        {
            Console.WriteLine("OnDeviceRemoval({0})", deviceType);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        protected void Draw()
        {
            foreach (var text in m_TextMap)
            {
                text.Value.Draw(Surface);
            }
        }

        public virtual void Update()
        {
            Draw();
            if (UpdateLcdScreen != null) UpdateLcdScreen(this, null);
        }
    }
}
