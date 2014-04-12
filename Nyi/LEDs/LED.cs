/*
 * Copyright (c) 2011-2014, Achim 'ahzf' Friedland <achim@graphdefined.org>
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

#endregion

namespace eu.Vanaheimr.Nyi
{

    /// <summary>
    /// A LED.
    /// </summary>
    public class LED
    {
        
        #region Data

        /// <summary>
        /// The outport of the LED.
        /// </summary>
        public readonly OutputPort OutputPort;

        /// <summary>
        /// The type of the LED.
        /// </summary>
        public readonly LEDType LEDType;

        #endregion

        #region Properties

        #region InitialState

        /// <summary>
        /// The initial state of the LED.
        /// </summary>
        public Boolean InitialState
        {
            get
            {
                return OutputPort.InitialState;
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        #region (protected internal) LED(CPUPin, LEDType, InitialState = false)

        /// <summary>
        /// Creates a new LED object.
        /// </summary>
        /// <param name="CPUPin">The digital CPU pin.</param>
        /// <param name="LEDType">The type of the LED.</param>
        /// <param name="InitialState">The optional initial state of the LED.</param>
        //protected internal LED(Cpu.Pin CPUPin, LEDType LEDType, Boolean InitialState = false)
        public LED(Cpu.Pin CPUPin, LEDType LEDType, Boolean InitialState = false)
        {
            this.OutputPort = new OutputPort(CPUPin, InitialState);
            this.LEDType    = LEDType;
        }

        #endregion

        #endregion


        #region Blink(OnMS, OffMS, Iterations = 1)

        /// <summary>
        /// Blink the LED for the given time periods.
        /// </summary>
        /// <param name="OnMS">The ON time.</param>
        /// <param name="OffMS">The OFF time.</param>
        /// <param name="Iterations">The optional number of iterations.</param>
        public void Blink(UInt32 OnMS, UInt32 OffMS, UInt32 Iterations = 1)
        {
            for (var i = 0; i < Iterations; i++)
            {
                this.OutputPort.Write(true);
                Thread.Sleep((Int32) OnMS);
                this.OutputPort.Write(false);
                Thread.Sleep((Int32) OffMS);
            }
        }

        #endregion

        #region On()

        /// <summary>
        /// Turns the LED on.
        /// </summary>
        public void On()
        {
            this.OutputPort.Write(true);
        }

        #endregion

        #region Off()

        /// <summary>
        /// Turns the LED off.
        /// </summary>
        public void Off()
        {
            this.OutputPort.Write(false);
        }

        #endregion

    }

}
