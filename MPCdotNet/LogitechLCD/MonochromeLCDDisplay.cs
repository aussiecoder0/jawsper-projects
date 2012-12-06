using System;
using System.ComponentModel;
using LgLcd;

namespace LogitechLCD
{
    public abstract class MonochromeLCDDisplay : Applet
    {
        protected Device Device { get; set; }
        public Surface Surface { get; internal set; }
        public abstract event EventHandler UpdateLcdScreen;

        protected MonochromeLCDDisplay(string friendlyName, bool autoStartable)
        {
            try
            {
                Connect(friendlyName, false, AppletCapabilities.Monochrome);

                Device = new Device();
                Device.Open(this, DeviceType.Monochrome);

                Surface = new Surface(160, 43, 1);
                UpdateLcdScreen += _UpdateLcdScreen;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        protected void _UpdateLcdScreen(object sender, EventArgs e)
        {
            try
            {
                Device.UpdateByteArray(Surface.Data, Priority.Normal);
            }
            catch (Win32Exception win32Exception)
            {
                Console.WriteLine("Win32Exception: {0}", win32Exception.Message);
            }
            catch (InvalidOperationException) { }
            catch (InvalidAsynchronousStateException) { }
        }
    }
}