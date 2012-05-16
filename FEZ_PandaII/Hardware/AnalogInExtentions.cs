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

using GHIElectronics.NETMF.Hardware;

#endregion

namespace de.ahzf.Vanaheimr.Nyi.FEZ
{

    /// <summary>
    /// Extention methods for AnalogIn.
    /// </summary>
    public static class AnalogInExtentions
    {

        #region ReadUnsigned(this AnalogIn)

        /// <summary>
        /// Reads an unsigned integer from the given analog input pin.
        /// </summary>
        /// <param name="AnalogIn">An analog input pin.</param>
        public static UInt32 ReadUnsigned(this AnalogIn AnalogIn)
        {

            var Integer = AnalogIn.Read();

            if (Integer < 0)
                return 0;
                
            return (UInt32) Integer;

        }

        #endregion

    }

}
