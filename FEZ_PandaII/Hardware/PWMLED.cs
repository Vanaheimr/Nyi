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

using System;
using System.Text;
using System.Threading;
using System.IO.Ports;

using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

using GHIElectronics.NETMF.FEZ;
using GHIElectronics.NETMF.Hardware;
using GHIElectronics.NETMF.Net;
using GHIElectronics.NETMF.Net.NetworkInformation;
using GHIElectronics.NETMF.Net.Sockets;


namespace de.ahzf.Vanaheimr.Nyi
{

    public class PWMLED
    {

        public readonly PWM Fader;

        public PWMLED(FEZ_Pin.PWM Pin, Boolean InitialState = false)
        {
            this.Fader = new PWM((PWM.Pin) Pin);
        }

        public void Fade(Byte value)
        {
            Fader.Set(10000, value);
        }

    }

}
