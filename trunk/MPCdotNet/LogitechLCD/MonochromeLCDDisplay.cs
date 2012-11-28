using System;
using System.ComponentModel;
using LgLcd;

namespace LogitechLCD
{
    public abstract class MonochromeLCDDisplay : IApplet
    {
        protected Device Device;
        private Applet _applet;
        protected IntPtr Handle;

        public Surface Surface { get; internal set; }

        protected MonochromeLCDDisplay(string friendlyName, IntPtr handle)
        {
            Handle = handle;
            Surface = new Surface(160, 43, 1);
            UpdateLcdScreen += WinFormsApplet_UpdateLcdScreen;

            _applet = new AppletProxy(this);
            Connect(friendlyName, true, AppletCapabilities.Monochrome);

            Device = new Device();
            Device.Open(_applet, DeviceType.Monochrome);
        }

        /// <summary>
        /// Forces deriving classes to implement a callback for when the screen needs to be updated
        /// </summary>
        public abstract event EventHandler UpdateLcdScreen;

        void WinFormsApplet_UpdateLcdScreen(object sender, EventArgs e)
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

        #region abstracted interface methods

        public virtual void OnDeviceArrival(DeviceType deviceType) { }
        public virtual void OnDeviceRemoval(DeviceType deviceType) { }
        public virtual void OnAppletEnabled() { }
        public virtual void OnAppletDisabled() { }
        public virtual void OnCloseConnection() { }
        public virtual void OnConfigure() { }

        #endregion

        #region Applet proxy
        internal class AppletProxy : Applet
        {
            private readonly IApplet _proxy;
            public AppletProxy(IApplet proxy)
            {
                _proxy = proxy;
            }
            public override void OnDeviceArrival(DeviceType deviceType) { _proxy.OnDeviceArrival(deviceType); }
            public override void OnDeviceRemoval(DeviceType deviceType) { _proxy.OnDeviceRemoval(deviceType); }
            public override void OnAppletEnabled() { _proxy.OnAppletEnabled(); }
            public override void OnAppletDisabled() { _proxy.OnAppletDisabled(); }
            public override void OnCloseConnection() { _proxy.OnCloseConnection(); }
            public override void OnConfigure() { _proxy.OnConfigure(); }
        }

        public void Connect(string friendlyName, bool autostartable, AppletCapabilities appletCaps)
        {
            _applet.Connect(friendlyName, autostartable, appletCaps);
        }
        public void Disconnect()
        {
            _applet.Disconnect();
        }
        #endregion
    }
}