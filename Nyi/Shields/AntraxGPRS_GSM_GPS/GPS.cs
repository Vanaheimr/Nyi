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
    /// An Antrax GPRS/GSM/GPS shield GPS listener.
    /// </summary>
    public class GPS
    {

        #region Data

        private readonly SPI.Configuration _SPIConfig;

        private readonly Byte[]            _SPI_TxData;
        private readonly Byte[]            _SPI_RxData;

        private readonly Byte[] gps_data_buffer;
        private const    String no_gps_message = "no gps";

        private readonly Byte[] gps_data;
        private readonly Byte[] latitude;
        private readonly Byte[] longitude;
        private readonly Byte[] coordinates;
        private readonly SPI _SPI;

        #endregion

        #region Constructor(s)

        #region GPS(SPIModule, SPISelectPin)

        /// <summary>
        /// Creates a new Antrax GPRS/GSM/GPS shield GPS listener.
        /// </summary>
        /// <param name="SPIModule">The SPI module to use.</param>
        /// <param name="SPISelectPin">The SPISelect pin.</param>
        public GPS(SPI.SPI_module SPIModule, Cpu.Pin SPISelectPin)
        {

            _SPI_TxData   = new Byte[80];
            _SPI_RxData   = new Byte[80];

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
                                               ChipSelect_SetupTime:   0,
                                               ChipSelect_HoldTime:    0,
                                               Clock_IdleState:        false,
                                               Clock_Edge:             false,
                                               Clock_RateKHz:          1000,
                                               SPI_mod:                SPIModule);

            _SPI = new SPI(_SPIConfig);

            gps_data_buffer = new Byte[20];

            gps_data    = new Byte[80];
            latitude    = new Byte[20];
            longitude   = new Byte[20];
            coordinates = new Byte[40];

        }

        #endregion

        #endregion


        public String GetGPS()
        {

            Byte command_result;
            int  high_Byte = 0;
            int  i, j;
            int  GPGGA;
            int  Position;
            
            //SS_PIN.Write(false);

            // SPI mode 1 / clock /2 --> 2MHz
            //spi.mode((0 << CPOL) | (1 << CPHA) | (1 << SPI2X));

            //Thread.Sleep(1000);

            var data = new Byte[1];
            for (var ii = _SPI_RxData.Length - 1; ii > 0; ii--)
                _SPI_RxData[ii] = 0xff;

            i = 0;
            do
            {

                if (high_Byte == 1)
                {
                    // send LowByte first
                    _SPI.WriteRead(new Byte[1] { 0xA7 }, data);
                    high_Byte = 0;
                }
                else
                {
                    // send HighByte second
                    _SPI.WriteRead(new Byte[1] { 0xB4 }, data);
                    high_Byte = 1;
                }

                _SPI_RxData[i] = data[0];

                if (data[0] == 0x0D)
                    break;

                i++;

            } while (i < 80);

            var str = "";

            foreach (byte b in _SPI_RxData)
            {
                str += ((char)b).ToString();
            }

            Debug.Print("response: " + str);

            return "nix!";


            

     //       _SPI.WriteRead(tx_data, rx_data);

            if ((_SPI_RxData[0] == '$') &&
                (_SPI_RxData[1] == 'G') &&
                (_SPI_RxData[2] == 'P') &&
                (_SPI_RxData[3] == 'G') &&
                (_SPI_RxData[4] == 'G') &&
                (_SPI_RxData[5] == 'A'))
            {
                GPGGA = 1;
                Debug.Print("GPGGA found!");
                // $GPGGA,191410,4735.5634,N,00739.3538,E,1,04,4.4,351.5,M,48.0,M,,*45
                // ^      ^           ^            ^ ^  ^   ^       ^     
                // |      |           |            | |  |   |       |    
                // |      |           |            | |  |   |       Höhe Geoid minus 
                // |      |           |            | |  |   |       Höhe Ellipsoid (WGS84)
                // |      |           |            | |  |   |       in Metern (48.0,M)
                // |      |           |            | |  |   |
                // |      |           |            | |  |   Höhe über Meer (über Geoid)
                // |      |           |            | |  |   in Metern (351.5,M)
                // |      |           |            | |  |
                // |      |           |            | |  HDOP (horizontal dilution
                // |      |           |            | |  of precision) Genauigkeit
                // |      |           |            | |
                // |      |           |            | Anzahl der erfassten Satelliten
                // |      |           |            | 
                // |      |           |            Qualität der Messung
                // |      |           |            (0 = ungültig)
                // |      |           |            (1 = GPS)
                // |      |           |            (2 = DGPS)
                // |      |           |            (6 = geschätzt nur NMEA-0183 2.3)
                // |      |           | 
                // |      |           Längengrad
                // |      |
                // |      Breitengrad 
                // |
                // Uhrzeit
            }


            if (_SPI_RxData[18] == 44)
                return "Mist!";

            //var s = new String(System.Text.Encoding.UTF8.GetChars(rx_data));

            //return s;

            //GPGGA = 0;
            //i = 0;

            //do
            //{

            //    // start transmitting data over SPI
            //    //digitalWrite(SS_PIN, LOW);
            //    //SS_PIN.Write(false);

            //    if (high_Byte == 1)
            //    {
            //        // send LowByte first
            //        command_result = spi.transfer(0xA7);
            //        high_Byte = 0;
            //    }
            //    else
            //    {
            //        // send HighByte second
            //        command_result = spi.transfer(0xB4);
            //        high_Byte = 1;
            //    }

            //    // FIFO-System to buffer incomming GPS-Data
            //    gps_data_buffer[0]  = gps_data_buffer[1];
            //    gps_data_buffer[1]  = gps_data_buffer[2];
            //    gps_data_buffer[2]  = gps_data_buffer[3];
            //    gps_data_buffer[3]  = gps_data_buffer[4];
            //    gps_data_buffer[4]  = gps_data_buffer[5];
            //    gps_data_buffer[5]  = gps_data_buffer[6];
            //    gps_data_buffer[6]  = gps_data_buffer[7];
            //    gps_data_buffer[7]  = gps_data_buffer[8];
            //    gps_data_buffer[8]  = gps_data_buffer[9];
            //    gps_data_buffer[9]  = gps_data_buffer[10];
            //    gps_data_buffer[10] = gps_data_buffer[11];
            //    gps_data_buffer[11] = gps_data_buffer[12];
            //    gps_data_buffer[12] = gps_data_buffer[13];
            //    gps_data_buffer[13] = gps_data_buffer[14];
            //    gps_data_buffer[14] = gps_data_buffer[15];
            //    gps_data_buffer[15] = gps_data_buffer[16];
            //    gps_data_buffer[16] = gps_data_buffer[17];
            //    gps_data_buffer[17] = gps_data_buffer[18];
            //    gps_data_buffer[18] = command_result;

            //    if ((gps_data_buffer[0] == '$') &&
            //        (gps_data_buffer[1] == 'G') &&
            //        (gps_data_buffer[2] == 'P') &&
            //        (gps_data_buffer[3] == 'G') &&
            //        (gps_data_buffer[4] == 'G') &&
            //        (gps_data_buffer[5] == 'A'))
            //    {
            //        GPGGA = 1;
            //    }

            //    if ((GPGGA == 1) && (i < 80))
            //    {

            //        // every answer of the GPS-Modul ends with an cr=0x0D
            //        if ((gps_data_buffer[0] == 0x0D))
            //        {
            //            i = 80;
            //            GPGGA = 0;
            //        }

            //        else
            //        {
            //            // write Buffer into public variable
            //            gps_data[i] = gps_data_buffer[0];
            //            i++;
            //        }

            //    }

            //    // end transmit data over SPI
            //    //digitalWrite(SS_PIN, HIGH);
            //    //SS_PIN.Write(true);

            //}
            //while (i < 80);

            // filter gps data
            
                //if (gps_data[18] == 44)
                //{
                //    j = 0;
                //    for(i = 0; i < 10; i++)
                //    {
                //        // no gps data available at present!
                //        coordinates[j] = 0x00;// no_gps_message [i];
                //        j++;      
                //    }                                    
                //}
            /*    else
                {
                    j = 0;                                                                    // format latitude   
                    for(i = 18; i < 29 ; i++)
                    {   
                        if(gps_data[i] != ',')
                        {
                            latitude[j] = gps_data[i];  
                            j++;                             
                        }        
              
                        if(j==2)
                        {
                            latitude[j] = ' ';
                            j++;
                        }
                    }   
          
                    j = 0;
                    for(i = 31; i < 42 ; i++)
                    {                                                                         // format longitude          
                        if(gps_data[i] != ',')
                        {
                            longitude[j] = gps_data[i];   
                            j++;                            
                        }   
               
                        if(j==2)
                        {
                            longitude[j] = ' ';
                            j++;
                        }   
                    }   
          
                    for(i = 0; i < 40; i++)                                                   // clear coordinates   
                        coordinates[i] = ' ';
          
                    j = 0;
                    for(i = 0; i < 11; i++)                                                   // write gps data to coordinates                                    
                    {
                        coordinates[j] = latitude[i];
                        j++;      
                    }
          
                    coordinates[j] = ',';
                    j++;
          
                    coordinates[j] = ' ';
                    j++;

                    for(i = 0; i < 11; i++)
                    {
                        coordinates[j] = longitude[i];
                        j++;      
                    }

                }
                */

                return no_gps_message;

        }

    }

}
