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

//using GHIElectronics.NETMF.FEZ;

//using GHIElectronics.NETMF.Net;
//using GHIElectronics.NETMF.Net.NetworkInformation;
//using GHIElectronics.NETMF.Net.Sockets;

//#endregion

//namespace com.aperis.dotNetMF.FEZPandaII.Toolbox
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
//            LED = new Blinky(new OutputPort((Cpu.Pin) FEZ_Pin.Digital.LED, false));

//            InterruptPort IntButton = new InterruptPort(portId:       (Cpu.Pin) FEZ_Pin.Interrupt.LDR,
//                                                        glitchFilter: true,
//                                                        resistor:     Port.ResistorMode.PullUp,
//                                                        interrupt:    Port.InterruptMode.InterruptEdgeLow);


//            // Add an interrupt handler to the pin
//            IntButton.OnInterrupt += new NativeEventHandler(IntButton_OnInterrupt);

//            //const Int32 c_port = 80;
//            //byte[] ip = { 192, 168, 0, 200 };
//            //byte[] subnet = { 255, 255, 255, 0 };
//            //byte[] gateway = { 192, 168, 0, 1 };
//            //byte[] mac = { 43, 185, 44, 2, 206, 127 };
//            //WIZnet_W5100.Enable(SPI.SPI_module.SPI1, (Cpu.Pin)FEZ_Pin.Digital.Di10,
//            //(Cpu.Pin)FEZ_Pin.Digital.Di9, true);
//            //NetworkInterface.EnableStaticIP(ip, subnet, gateway, mac);
//            //NetworkInterface.EnableStaticDns(new byte[] { 192, 168, 0, 1 });
//            //Socket server = new Socket(AddressFamily.InterNetwork,
//            //SocketType.Stream, ProtocolType.Tcp);
//            //IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, c_port);
//            //server.Bind(localEndPoint);
//            //server.Listen(1);

//            //while (true)
//            //{
//            //    // Wait for a client to connect.
//            //    Socket clientSocket = server.Accept();
//            //    // Process the client request. true means asynchronous.
//            //    //new ProcessClientRequest(clientSocket, true);
//            //}

//            var _EnergyMeter = new EnergyMeter(MCIEnergyShield2.SPIModule, MCIEnergyShield2.SSPin);


//            //var _Antrax = new AntraxShield(AntraxGPRS_GSM_GPS.PowerOn);
//            //_GSM = _Antrax.InitGSM("2993");

//            //Debug.Print("Baudrate => '"                   + _GSM.Baudrate);
//            //Debug.Print("ManufacturerIdentification => '" + _GSM.ManufacturerIdentification + "'");
//            //Debug.Print("ModelIdentification => '"        + _GSM.ModelIdentification + "'");
//            //Debug.Print("RevisionIdentification => '"     + _GSM.RevisionIdentification + "'");

//            //Debug.Print("ProviderInfo: '"                 + _GSM.ProviderInfo + "'");
//            //Debug.Print("SMS Service Center Address: '"   + _GSM.SMSServiceCenter + "'");

//            //_GSM.SendSMS("+491728930852", "Hello world!");
//            //_GPS = _Antrax.InitGPS(AntraxGPRS_GSM_GPS.SPIModule, AntraxGPRS_GSM_GPS.SSPin);

//            // There will be no morning ;)
//            Thread.Sleep(Timeout.Infinite);

//        }

//        #endregion

//    }

//}
