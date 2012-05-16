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

namespace de.ahzf.Vanaheimr.Nyi.MCIEnergyShield
{

    /// <summary>
    /// A MCI Energy Shield wrapper.
    /// </summary>
    public class EnergyShield
    {

        #region Data

        private readonly OutputPort _PowerOnPin;

        #endregion

        #region Properties

        /// <summary>
        /// A MCI Energy Meter.
        /// </summary>
        public EnergyMeter EnergyMeter { get; private set; }

        #endregion

        #region Constructor(s)

        #region EnergyShield(PowerOnPin)

        /// <summary>
        /// Creates a new MCI Energy Shield wrapper.
        /// </summary>
        /// <param name="PowerOnPin">The CPU pin to enpower the shield.</param>
        public EnergyShield(Cpu.Pin PowerOnPin)
        {
            _PowerOnPin = new OutputPort(PowerOnPin, true);
            _PowerOnPin.Write(true);
        }

        #endregion

        #endregion


        #region InitEnergyMeter(SPIModule, SPISelectPin)

        /// <summary>
        /// Initializes the energy metering module.
        /// </summary>
        /// <param name="SPIModule">The SPI module to use.</param>
        /// <param name="SPISelectPin">The SPISelect pin.</param>
        public EnergyMeter InitEnergyMeter(SPI.SPI_module SPIModule, Cpu.Pin SPISelectPin)
        {

            if (EnergyMeter == null)
                EnergyMeter = new EnergyMeter(SPIModule, SPISelectPin);

            return EnergyMeter;

        }

        #endregion

    }

}
