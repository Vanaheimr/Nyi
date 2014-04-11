
#region Usings

using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;

using Microsoft.SPOT;

#endregion

namespace eu.Vanaheimr.Nyi.Networking
{

    /// <summary>
    /// A NTP client.
    /// </summary>
    public class NTPClient
    {

        #region Data

        private IPAddress[] NTPServerIPAddresses;

        #endregion

        #region Properties

        #region NTPServer

        private readonly String _NTPServer;

        public String NTPServer
        {
            get
            {
                return _NTPServer;
            }
        }

        #endregion

        #region TimeZoneOffset

        private readonly Int16 _TimeZoneOffset;

        public Int16 TimeZoneOffset
        {
            get
            {
                return _TimeZoneOffset;
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        #region NTPClient(NTPServer = "pool.ntp.org", TimeZoneOffset = 0)

        public NTPClient(String NTPServer = "pool.ntp.org", Int16 TimeZoneOffset = 0)
        {
            this._NTPServer       = NTPServer;
            this._TimeZoneOffset  = TimeZoneOffset;
        }

        #endregion

        #endregion


        #region GetNetworkTime()

        public DateTime GetNetworkTime()
        {

            var NTPData             = new Byte[48];

            // LeapIndicator = 0 (no warning)
            // VersionNum    = 3 (IPv4 only)
            // Mode          = 3 (Client Mode)
            NTPData[0]              = 0x1B;

            NTPServerIPAddresses    = Dns.GetHostEntry(_NTPServer).AddressList;
            var ipEndPoint          = new IPEndPoint(NTPServerIPAddresses[0], 123);
            var socket              = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            socket.Connect(ipEndPoint);
            socket.Send(NTPData);
            socket.Receive(NTPData);
            socket.Close();

            var intPart             = (UInt64) NTPData[40] << 24 | (UInt64) NTPData[41] << 16 | (UInt64) NTPData[42] << 8 | (UInt64) NTPData[43];
            var fractPart           = (UInt64) NTPData[44] << 24 | (UInt64) NTPData[45] << 16 | (UInt64) NTPData[46] << 8 | (UInt64) NTPData[47];

            var milliseconds        = (intPart * 1000) + ((fractPart * 1000) / 0x100000000L);
            var networkDateTime     = (new DateTime(1900, 1, 1)).AddMilliseconds((long)milliseconds);

            return networkDateTime;

        }

        #endregion

    }

}
