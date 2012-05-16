/*
 * Copyright (c) 2011-2012, Belectric ITS GmbH <opensource@belectric.com>
 * Autor: Achim 'ahzf' Friedland <achim.friedland@belectric.com>
 * This file is part of Vanaheimr Nyi <http://www.github.com/Vanaheimr/Nyi>
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#region Usings

using System;
using System.IO.Ports;
using System.Text;
using System.Threading;

#endregion

namespace de.ahzf.Vanaheimr.Nyi
{

    public static class SerialPortExtensions
    {

        #region ReadSerialPort(this SerialPort, WaitMSBeforeRead = 100, MaxReadSize = 1024)

        /// <summary>
        /// Read a string from the serial port.
        /// </summary>
        /// <param name="SerialPort">The serial port.</param>
        /// <param name="WaitMSBeforeRead">Wait the given amount of ms before reading the serial port.</param>
        /// <param name="MaxReadSize">The maximal size of the read buffer.</param>
        public static String ReadSerialPort(this SerialPort SerialPort, UInt32 WaitMSBeforeRead = 100, UInt16 MaxReadSize = 1024)
        {

            if (MaxReadSize > Int16.MaxValue)
                throw new ArgumentException("Parameter 'ReadBufSize' must not be larger than Int16.MaxValue!");

            Thread.Sleep((Int32) WaitMSBeforeRead);
            var readbuf = new Byte[(Int32) MaxReadSize];
            var readBytes = 0;
            var Str = "";

            if (SerialPort.BytesToRead > 0)
            {
                readBytes = SerialPort.Read(readbuf, 0, readbuf.Length);
                Str = new String(Encoding.UTF8.GetChars(readbuf));
            }

            return Str.TrimStart().TrimEnd();

        }

        #endregion


        public static void Write(this SerialPort SerialPort, Char Char)
        {
            var _ByteArray = new Byte[1];
            _ByteArray[0] = (Byte) Char;
            SerialPort.Write(_ByteArray, 0, 1);
        }

        public static void Write(this SerialPort SerialPort, Byte Byte)
        {
            var _ByteArray = new Byte[1];
            _ByteArray[0] = Byte;
            SerialPort.Write(_ByteArray, 0, 1);
        }

        public static void Write(this SerialPort SerialPort, String Text)
        {
            var _ByteArray = Encoding.UTF8.GetBytes(Text);
            SerialPort.Write(_ByteArray, 0, _ByteArray.Length);
        }

        public static void WriteLine(this SerialPort SerialPort, String Text)
        {
            SerialPort.Write(Text + "\r\n");
        }


        #region WriteAndRead(this SerialPort, Text, WaitMSBeforeRead = 100, MaxReadSize = 1024)

        /// <summary>
        /// Write the given string to the serial port and read the response.
        /// </summary>
        /// <param name="SerialPort">The serial port.</param>
        /// <param name="Text">The text to write to the serial port.</param>
        /// <param name="WaitMSBeforeRead">Wait the given amount of ms before reading the serial port.</param>
        /// <param name="MaxReadSize">The maximal size of the read buffer.</param>
        public static String WriteAndRead(this SerialPort SerialPort, String Text, UInt32 WaitMSBeforeRead = 100, UInt16 MaxReadSize = 1024)
        {
            var _ByteArray = Encoding.UTF8.GetBytes(Text);
            SerialPort.Write(_ByteArray, 0, _ByteArray.Length);
            return SerialPort.ReadSerialPort(WaitMSBeforeRead, MaxReadSize);
        }

        #endregion

        #region WriteLineAndRead(this SerialPort, Text, WaitMSBeforeRead = 100, MaxReadSize = 1024)

        /// <summary>
        /// Write the given string to the serial port and read the response.
        /// </summary>
        /// <param name="SerialPort">The serial port.</param>
        /// <param name="Text">The text to write to the serial port.</param>
        /// <param name="WaitMSBeforeRead">Wait the given amount of ms before reading the serial port.</param>
        /// <param name="MaxReadSize">The maximal size of the read buffer.</param>
        public static String WriteLineAndRead(this SerialPort SerialPort, String Text, UInt32 WaitMSBeforeRead = 100, UInt16 MaxReadSize = 1024)
        {
            
            var _ResponseString = SerialPort.WriteAndRead(Text + "\r\n", WaitMSBeforeRead, MaxReadSize);
            if (_ResponseString == null)
                throw new Exception("Invalid response!");

            var _Response       = _ResponseString.Split(new Char[2]{ '\r', '\n' });
            if (_Response.Length > 1 && _Response[_Response.Length - 1] != "OK")
                throw new Exception("Invalid response: '" + _ResponseString + "'!");

            return _Response[0];

        }

        #endregion

    }

}
