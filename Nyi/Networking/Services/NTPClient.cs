
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

        private static readonly DateTime BaseTime = new DateTime(1900, 1, 1, 0, 0, 0);

        private IPAddress[] NTPServerIPAddresses;

        private Timer _Timer;

        #endregion

        #region Properties

        #region Interval

        private readonly TimeSpan _Interval;

        public TimeSpan Interval
        {
            get
            {
                return _Interval;
            }
        }

        #endregion

        #region Host

        private readonly String _Host;

        public String Host
        {
            get
            {
                return _Host;
            }
        }

        #endregion

        #region Port

        private readonly UInt16 _Port;

        public UInt16 Port
        {
            get
            {
                return _Port;
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

        #region Events

        #region OnNewDateTime

        public delegate void OnNewDateTimeDelegate(NTPClient NTPClient, DateTime DateTime);

        public event OnNewDateTimeDelegate OnNewDateTime;

        #endregion

        #endregion

        #region Constructor(s)

        #region NTPClient(Host = "pool.ntp.org", Port = 123, TimeZoneOffset = 0)

        public NTPClient(String   Host            = "pool.ntp.org",
                         UInt16   Port            = 123,
                         Int16    TimeZoneOffset  = 0)
        {

            this._Host            = Host;
            this._Port            = Port;
            this._TimeZoneOffset  = TimeZoneOffset;

        }

        #endregion

        #region NTPClient(Interval, Host = "pool.ntp.org", Port = 123, TimeZoneOffset = 0)

        public NTPClient(TimeSpan Interval,
                         String   Host            = "pool.ntp.org",
                         UInt16   Port            = 123,
                         Int16    TimeZoneOffset  = 0)
        {

            this._Interval        = Interval;
            this._Host            = Host;
            this._Port            = Port;
            this._TimeZoneOffset  = TimeZoneOffset;
            this._Timer           = new Timer(ProcessTimer, null, TimeSpan.Zero, Interval);

        }

        #endregion

        #endregion


        #region (private) ProcessTimer(State)

        private void ProcessTimer(Object State)
        {
            DateTime DateTime;
            TryGetDateTime(out DateTime);
        }

        #endregion


        #region GetDateTime()

        public DateTime GetDateTime()
        {

            DateTime DateTime;

            if (TryGetDateTime(out DateTime))
                return DateTime;

            return BaseTime;

        }

        #endregion

        #region TryGetDateTime(out DateTime)

        public Boolean TryGetDateTime(out DateTime DateTime)
        {

            var NTPData             = new Byte[48];

            // LeapIndicator = 0 (no warning)
            // VersionNum    = 3 (IPv4 only)
            // Mode          = 3 (Client Mode)
            NTPData[0]              = 0x1B;

            try
            {

                NTPServerIPAddresses = Dns.GetHostEntry(_Host).AddressList;
                var ipEndPoint          = new IPEndPoint(NTPServerIPAddresses[0], _Port);
                var socket              = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                socket.Connect(ipEndPoint);
                socket.Send(NTPData);
                socket.Receive(NTPData);
                socket.Close();

                var intPart             = (UInt64) NTPData[40] << 24 | (UInt64) NTPData[41] << 16 | (UInt64) NTPData[42] << 8 | (UInt64) NTPData[43];
                var fractPart           = (UInt64) NTPData[44] << 24 | (UInt64) NTPData[45] << 16 | (UInt64) NTPData[46] << 8 | (UInt64) NTPData[47];

                var milliseconds        = (intPart * 1000) + ((fractPart * 1000) / 0x100000000L);
                var networkDateTime     = BaseTime.AddMilliseconds((long) milliseconds);

                var NewDateTimeLocal = OnNewDateTime;
                if (NewDateTimeLocal != null)
                    NewDateTimeLocal(this, networkDateTime);

                DateTime = networkDateTime;
                return true;

            }
            catch (Exception e)
            {

                DateTime = BaseTime;
                return false;

            }

        }

        #endregion

    }

}
