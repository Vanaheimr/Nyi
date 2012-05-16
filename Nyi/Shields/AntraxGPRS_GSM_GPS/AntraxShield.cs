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
using Microsoft.SPOT.Hardware;

#endregion

namespace de.ahzf.Vanaheimr.Nyi.AntraxMobileShield
{

    /// <summary>
    /// An Antrax GPRS/GSM/GPS shield wrapper.
    /// </summary>
    public class AntraxShield
    {

        #region Data

        private readonly OutputPort _PowerOnPin;

        #endregion

        #region Properties

        public GSM GSM { get; private set; }

        public GPS GPS { get; private set; }

        #endregion

        #region Constructor(s)

        #region AntraxShield(PowerOnPin)

        /// <summary>
        /// Creates a new Antrax GPRS/GSM/GPS shield wrapper.
        /// </summary>
        /// <param name="PowerOnPin">The CPU pin to enpower the shield.</param>
        public AntraxShield(Cpu.Pin PowerOnPin)
        {
            _PowerOnPin = new OutputPort(PowerOnPin, true);
            _PowerOnPin.Write(true);
        }

        #endregion

        #endregion


        #region InitGSM(PIN, SerialPort = "COM1", Baud = 9600)

        /// <summary>
        /// Initializes the GSM module.
        /// </summary>
        /// <param name="PIN"></param>
        /// <param name="SerialPort"></param>
        /// <param name="Baud"></param>
        public GSM InitGSM(String PIN, String SerialPort = "COM1", Int32 Baud = 9600)
        {
            
            if (GSM == null)
                GSM = new GSM(PIN, SerialPort, Baud);

            return GSM;

        }

        #endregion

        #region InitGPS(SPIModule, SPISelectPin)

        /// <summary>
        /// Initializes the GPS module.
        /// </summary>
        /// <param name="SPIModule">The SPI module to use.</param>
        /// <param name="SPISelectPin">The SPISelect pin.</param>
        public GPS InitGPS(SPI.SPI_module SPIModule, Cpu.Pin SPISelectPin)
        {

            if (GPS == null)
                GPS = new GPS(SPIModule, SPISelectPin);

            return GPS;

        }

        #endregion

    }

}
