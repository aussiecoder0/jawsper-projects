namespace SkyTraqCs
{
    internal struct SkyTraqCommand
    {
        internal static readonly byte 
            // command codes
            SKYTRAQ_COMMAND_SYSTEM_RESTART = 0x01,
            SKYTRAQ_COMMAND_QUERY_SOFTWARE_VERSION = 0x02,
            SKYTRAQ_COMMAND_QUERY_SOFTWARE_CRC = 0x03,
            SKYTRAQ_COMMAND_SET_FACTORY_DEFAULTS = 0x04,
            SKYTRAQ_COMMAND_CONFIGURE_SERIAL_PORT = 0x05,
            SKYTRAQ_COMMAND_CONFIGURE_NMEA_MESSAGE = 0x08,
            SKYTRAQ_COMMAND_CONFIGURE_MESSAGE_TYPE = 0x09,
            SKYTRAQ_COMMAND_GET_CONFIG = 0x17,
            SKYTRAQ_COMMAND_WRITE_CONFIG = 0x18,
            SKYTRAQ_COMMAND_ERASE = 0x19,
            SKYTRAQ_COMMAND_READ_SECTOR = 0x1b,
            SKYTRAQ_COMMAND_READ_BATCH = 0x1d,
            SKYTRAQ_COMMAND_GET_EPHERMERIS = 0x30,
            SKYTRAQ_COMMAND_SET_EPHEMERIS = 0x31,
            SKYTRAQ_COMMAND_READ_AGPS_STATUS = 0x34,
            SKYTRAQ_COMMAND_SEND_AGPS_DATA = 0x35,

            // response codes
            SKYTRAQ_RESPONSE_SOFTWARE_VERSION = 0x80,
            SKYTRAQ_RESPONSE_SOFTWARE_CRC = 0x81,
            SKYTRAQ_RESPONSE_ACK = 0x83,
            SKYTRAQ_RESPONSE_NACK = 0x84,
            SKYTRAQ_RESPONSE_EPHEMERIS_DATA = 0xb1;
    }
}