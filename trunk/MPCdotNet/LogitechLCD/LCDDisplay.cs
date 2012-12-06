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
        protected Dictionary<int, Drawable> m_Drawables = new Dictionary<int, Drawable>();

        public LCDDisplay(string friendlyName, bool autoStartable)
            : base(friendlyName, autoStartable)
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
            foreach (var item in m_Drawables.OrderBy(i => i.Key))
            {
                item.Value.Draw(Surface);
            }
        }

        public virtual void Update()
        {
            Draw();
            if (UpdateLcdScreen != null) UpdateLcdScreen(this, null);
        }

        public void SetText(int id, string value)
        {
            if (m_Drawables.ContainsKey(id) && m_Drawables[id] is DrawableText)
            {
                (m_Drawables[id] as DrawableText).Text = value;
            }
        }
    }
}
