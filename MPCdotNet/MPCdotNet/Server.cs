using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.ComponentModel;

namespace MPCdotNet
{
    [TypeConverter(typeof(ServerConverter))]
    [SettingsSerializeAs(SettingsSerializeAs.String)]
    public class Server
    {
        public Server()
        {
            Host = "localhost";
            Port = 6600;
        }
        public string Host { get; set; }
        public int Port { get; set; }
    }
    public class ServerConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string);
        }
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is string)
            {
                string[] parts = ((string)value).Split(new char[] { ',' });
                Server server = new Server();
                server.Host = parts[0];
                if (parts.Length > 1) server.Port = int.Parse(parts[1]);
                return server;
            }
            return base.ConvertFrom(context, culture, value);
        }
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                Server room = value as Server;
                return string.Format("{0},{1}", room.Host, room.Port);
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
