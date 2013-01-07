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
using System.Threading;
using Microsoft.SPOT;
using System.Text;

#endregion

namespace de.ahzf.Vanaheimr.Nyi.AntraxMobileShield
{

    /// <summary>
    /// An Antrax GPRS/GSM/GPS shield GSM listener.
    /// </summary>
    public class GSM
    {

        #region Data

        private readonly SerialPort _SerialPort;

        #endregion

        #region Properties

        #region Baudrate

        /// <summary>
        /// The baudrate for accessing the serial port.
        /// </summary>
        public UInt32 Baudrate { get; private set; }

        #endregion


        #region ManufacturerIdentification

        /// <summary>
        /// The identification of the GSM manufacturer.
        /// </summary>
        public String ManufacturerIdentification
        {
            get
            {
                return _SerialPort.WriteLineAndRead("AT+GMI");
            }
        }

        #endregion

        #region ModelIdentification

        /// <summary>
        /// The identification of the GSM model.
        /// </summary>
        public String ModelIdentification
        {
            get
            {
                return _SerialPort.WriteLineAndRead("AT+GMM");
            }
        }

        #endregion

        #region RevisionIdentification

        /// <summary>
        /// The revision identification of the GSM model.
        /// </summary>
        public String RevisionIdentification
        {
            get
            {
                return _SerialPort.WriteLineAndRead("AT+GMR");
            }
        }

        #endregion


        #region ProviderInfo

        /// <summary>
        /// Information on the current mobile network.
        /// </summary>
        public String ProviderInfo
        {
            get
            {
                return _SerialPort.WriteLineAndRead("AT+COPS?");
            }
        }

        #endregion

        #region SMSServiceCenter

        /// <summary>
        /// Information on the current SMS service center.
        /// </summary>
        public String SMSServiceCenter
        {
            get
            {
                return _SerialPort.WriteLineAndRead("AT+CSCA?");
            }
        }

        #endregion

        #region NetworkReport

        /// <summary>
        /// Return a GSM network report.
        /// </summary>
        public String NetworkReport
        {
            get
            {
                _SerialPort.WriteLine("AT+CREG?");
                return _SerialPort.ReadSerialPort(WaitMSBeforeRead: 1000);
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        #region GSM(Baud)

        /// <summary>
        /// Creates a new Antrax GPRS/GSM/GPS shield GPS listener.
        /// </summary>
        /// <param name="PIN">The SIM card authentiation pin.</param>
        /// <param name="SerialPort">The serial port for talking to the GPRS/GSM shield.</param>
        /// <param name="Baud">The serial port baud rate.</param>
        /// <param name="ReadTimeout">The serial port read timeout.</param>
        public GSM(String PIN, String SerialPort = "COM1", Int32 Baud = 9600, UInt32 ReadTimeout = 1000)
        {

            _SerialPort             = new SerialPort(SerialPort, Baud);
            _SerialPort.ReadTimeout = (Int32) ReadTimeout;
            _SerialPort.Open();

            var _OpenserialPortRetries = 0;
            while (!_SerialPort.IsOpen && _OpenserialPortRetries <= 100)
            {
                Debug.Print("Waiting for the " + SerialPort);
                Thread.Sleep(100);
                _OpenserialPortRetries++;
            }

            if (_OpenserialPortRetries > 100)
                throw new Exception("There seems to be a problem with the serial port!");

            _SerialPort.WriteLineAndRead("AT");
            
            // Disable echo
            _SerialPort.WriteLineAndRead("ATE0");
            
            // Set Serial port baudrate
            _SerialPort.WriteLineAndRead("AT+IPR=" + Baud.ToString());

            // Set SIM Detection mode (SIM)
            Debug.Print("set SIM Detection => " + _SerialPort.WriteLineAndRead("AT#SIMDET=1"));
            Thread.Sleep(5000);

            Debug.Print("pin need  => " + _SerialPort.WriteLineAndRead("AT+CPIN?"));
            Debug.Print("enter pin => " + _SerialPort.WriteLineAndRead("AT+CPIN=" + PIN));

            var _NetworkReport = "";
            do
            {
                _NetworkReport = this.NetworkReport;
                Debug.Print(_NetworkReport);
            }
            while (_NetworkReport.IndexOf("0,1") < 2);

            //Serial.println(“AT*PSCPOF”); // switch the module off

        }

        #endregion

        #endregion


        #region SendSMS(Number, SMSText)

        /// <summary>
        /// Send a SMS.
        /// </summary>
        /// <param name="Number">The number of the receiver.</param>
        /// <param name="SMSText">The SMS text.</param>
        public void SendSMS(String Number, String SMSText)
        {

            // Use text-format for sms
            
            Debug.Print("SMS format: " + _SerialPort.WriteLineAndRead("AT+CMGF=1"));

            _SerialPort.Write("AT+CMGS=");
            _SerialPort.Write(0x22);
            _SerialPort.Write(Number);
            _SerialPort.Write(0x22);

            if (Number[0] == '+')
                // 145 - number in international format (contains the "+")
                _SerialPort.WriteLine(",145");
            else
                // 129 - number in national format
                _SerialPort.WriteLine(",129");

            Debug.Print("Set SMS receiver: " + _SerialPort.ReadSerialPort());

            _SerialPort.Write(SMSText);
            // 0x1A - End of SMS
            _SerialPort.Write(0x1A);

            Debug.Print("SMS send: " + _SerialPort.ReadSerialPort());

        }

        #endregion

    }

}
