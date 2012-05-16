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
    /// A MCI Energy Meter.
    /// </summary>
    public class EnergyMeter
    {

        #region Data

        private readonly SPI.Configuration _SPIConfig;

        private readonly Byte[]            _SPI_TxData;
        private readonly Byte[]            _SPI_RxData;

        private readonly SPI _SPI;

        #endregion

        #region Constructor(s)

        #region GPS(SPIModule, SPISelectPin)

        /// <summary>
        /// Creates a new MCI Energy Meter.
        /// </summary>
        /// <param name="SPIModule">The SPI module to use.</param>
        /// <param name="SPISelectPin">The SPISelect pin.</param>
        public EnergyMeter(SPI.SPI_module SPIModule, Cpu.Pin SPISelectPin)
        {

            _SPI_TxData   = new Byte[1];
            _SPI_RxData   = new Byte[1];

            // The SPI control register (SPCR) has 8 bits, each of which control a particular SPI setting.
            // SPCR
            // | 7    | 6    | 5    | 4    | 3    | 2    | 1    | 0    |
            // | SPIE | SPE  | DORD | MSTR | CPOL | CPHA | SPR1 | SPR0 |
            // 
            // SPIE - Enables the SPI interrupt when 1
            // SPE - Enables the SPI when 1
            // DORD - Sends data least Significant Bit First when 1, most Significant Bit first when 0
            // MSTR - Sets the Arduino in master mode when 1, slave mode when 0
            // CPOL - Sets the data clock to be idle when high if set to 1, idle when low if set to 0
            // CPHA - Samples data on the falling edge of the data clock when 1, rising edge when 0
            // SPR1 and SPR0 - Sets the SPI speed, 00 is fastest (4MHz) 11 is slowest (250KHz)
            //
            //// SPI mode 1 / clock /2 --> 2MHz
            // spi.mode((0 << CPOL) | (1 << CPHA) | (1 << SPI2X));
            //
            // Cpu.Pin        ChipSelect_Port
            // bool           ChipSelect_ActiveState
            // uint           ChipSelect_SetupTime
            // uint           ChipSelect_HoldTime
            // bool           Clock_IdleState           (CPOL 0/false) The idle state of the clock. If true, the SPI clock signal will be set to high while the device is idle; if false, the SPI clock signal will be set to low while the device is idle. The idle state occurs whenever the chip is not selected.
            // bool           Clock_Edge                (CPHA 1/false) The sampling clock edge. If true, data is sampled on the SPI clock rising edge; if false, the data is sampled on the SPI clock falling edge.
            // uint           Clock_RateKHz
            // SPI.SPI_module SPI_mod
            _SPIConfig = new SPI.Configuration(ChipSelect_Port:        SPISelectPin,
                                               ChipSelect_ActiveState: false,
                                               ChipSelect_SetupTime:   5,
                                               ChipSelect_HoldTime:    5,
                                               Clock_IdleState:        false,
                                               Clock_Edge:             false,
                                               Clock_RateKHz:          1000,
                                               SPI_mod:                SPIModule);

            _SPI = new SPI(_SPIConfig);

        }

        #endregion

        #endregion




        /**
         * Read 8 bits from the device at specified register
         * @param char containing register direction
         * @return char with contents of register
         *
         */
        public Byte Read8(Byte reg)
        {
            // enableChip();
            //Thread.Sleep(5);
            //SPI.transfer(reg);
            _SPI_TxData[0] = reg;
            _SPI.Write(_SPI_TxData);
            //Thread.Sleep(5);
            //return (unsigned long) SPI.transfer(0x00);
            _SPI_TxData[0] = 0x00;
            _SPI.WriteRead(_SPI_TxData, _SPI_RxData);            
            //disableChip();
            return _SPI_RxData[0];
        }


    }

}
