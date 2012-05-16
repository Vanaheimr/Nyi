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
using Microsoft.SPOT.Hardware;

using GHIElectronics.NETMF.FEZ;

#endregion

namespace com.aperis.dotNetMF.FEZPandaII.Toolbox.Shields
{

    /// <summary>
    /// An Antrax GPRS/GSM/GPS shield to FEZ Panda II mapping.
    /// </summary>
    public partial class AntraxGPRS_GSM_GPS
    {

        /// <summary>
        /// The CPU pin to enpower the Antrax GPRS/GSM/GPS shield.
        /// </summary>
        public const Cpu.Pin        PowerOn    = (Cpu.Pin) FEZ_Pin.Digital.Di7;

        /// <summary>
        /// The Antrax GPRS/GSM/GPS SPI device.
        /// </summary>
        public const SPI.SPI_module SPIModule  = SPI.SPI_module.SPI1;

        /// <summary>
        /// The serial port.
        /// </summary>
        public const String         SerialPort = "COM1";

        /// <summary>
        /// The CPU pin to enable the SPI on the Antrax GPRS/GSM/GPS shield.
        /// </summary>
        public const Cpu.Pin        SSPin      = (Cpu.Pin) FEZ_Pin.Digital.Di10;


        /// <summary>
        /// The buttons of the Antrax GPRS/GSM/GPS shield.
        /// </summary>
        public enum Buttons
        {

            /// <summary>
            /// The GSM Button of the Antrax GPRS/GSM/GPS shield.
            /// </summary>
            GSM = FEZ_Pin.AnalogIn.An0,

            /// <summary>
            /// The GPS Button of the Antrax GPRS/GSM/GPS shield.
            /// </summary>
            GPS = FEZ_Pin.AnalogIn.An1

        }

        /// <summary>
        /// The LEDs of the Antrax GPRS/GSM/GPS shield.
        /// </summary>
        public enum LED
        {

            /// <summary>
            /// The GPS LED of the Antrax GPRS/GSM/GPS shield.
            /// </summary>
            GPS = FEZ_Pin.Digital.Di9

        }

    }

}
