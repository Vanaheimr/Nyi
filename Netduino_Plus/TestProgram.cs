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

//#region Usings

//using System;
//using System.Threading;
//using Microsoft.SPOT;
//using Microsoft.SPOT.Hardware;

//using SecretLabs.NETMF.Hardware.NetduinoPlus;

//using com.aperis.dotNetMF.Toolbox;
//using com.aperis.dotNetMF.AntraxMobileShield;
//using com.aperis.dotNetMF.NetduinoPlus.Toolbox.Shields;

//#endregion

//namespace com.aperis.dotNetMF.NetduinoPlus.Toolbox
//{

//    /// <summary>
//    /// A small blinky test program.
//    /// </summary>
//    public class Program
//    {

//        #region Data

//        /// <summary>
//        /// The blinky LED.
//        /// </summary>
//        private static Blinky LED;

//        private static GSM _GSM;
//        private static GPS _GPS;

//        #endregion

//        #region (private) IntButton_OnInterrupt(Port, State, Timestamp)

//        /// <summary>
//        /// Interrupt handler
//        /// </summary>
//        /// <param name="Port">The port.</param>
//        /// <param name="State">The state of the port.</param>
//        /// <param name="Timestamp">The timestamp of the interrupt.</param>
//        private static void IntButton_OnInterrupt(UInt32 Port, UInt32 State, DateTime Timestamp)
//        {

//            Debug.Print("Button pressed!");

//            _GPS.GetGPS();

//            LED.Blink(Cycles: 5,
//                      OnMS:   100,
//                      OffMS:  200);

//        }

//        #endregion


//        #region Main()

//        /// <summary>
//        /// Main()
//        /// </summary>
//        public static void Main()
//        {

//            // The LED to blink...
//            LED = new Blinky(new OutputPort((Cpu.Pin) Pins.ONBOARD_LED, false));

//            InterruptPort IntButton = new InterruptPort(portId:       (Cpu.Pin) Pins.ONBOARD_SW1,
//                                                        glitchFilter: true,
//                                                        resistor:     Port.ResistorMode.PullUp,
//                                                        interrupt:    Port.InterruptMode.InterruptEdgeLow);


//            // Add an interrupt handler to the pin
//            IntButton.OnInterrupt += new NativeEventHandler(IntButton_OnInterrupt);

//            var _Antrax = new AntraxShield(AntraxGPRS_GSM_GPS.PowerOn);

//            var xx = Microsoft.SPOT.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces();

//            _GSM = _Antrax.InitGSM("2993");
//            _GPS = _Antrax.InitGPS(AntraxGPRS_GSM_GPS.SPIModule, AntraxGPRS_GSM_GPS.SSPin);

//            // There will be no morning ;)
//            Thread.Sleep(Timeout.Infinite);

//        }

//        #endregion

//    }

//}
