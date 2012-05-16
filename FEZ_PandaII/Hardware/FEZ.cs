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
using System.Threading;

using Microsoft.SPOT.Hardware;

using GHIElectronics.NETMF.FEZ;

#endregion

namespace de.ahzf.Vanaheimr.Nyi
{

    /// <summary>
    /// Extention methods for LEDs on FEZ hardware.
    /// </summary>
    public static partial class LEDFactory
    {

        /// <summary>
        /// Creates a new LED attached to a digital pin on FEZ hardware.
        /// </summary>
        /// <param name="Pin">The digital CPU pin.</param>
        /// <param name="InitialState">The optional initial state of the LED.</param>
        /// <returns>A new LED object.</returns>
        public static LED New(FEZ_Pin.Digital Pin, Boolean InitialState = false)
        {
            return new LED((Cpu.Pin)Pin, LEDType.DigitalPin, InitialState);
        }

        ///// <summary>
        ///// Creates a new LED attached to a PWM pin on FEZ hardware.
        ///// </summary>
        ///// <param name="Pin">The digital CPU pin.</param>
        ///// <param name="InitialState">The optional initial state of the LED.</param>
        ///// <returns>A new LED object.</returns>
        //public static LED New(FEZ_Pin.PWM Pin, Boolean InitialState = false)
        //{
            
        //    return new LED((Cpu.Pin) Pin, LEDType.PWNPin, InitialState);
            
        //}

    }

}
