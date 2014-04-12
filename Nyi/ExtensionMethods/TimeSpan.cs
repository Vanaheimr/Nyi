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
using System.Text;
using System.Threading;

#endregion

namespace eu.Vanaheimr.Nyi
{

    public static partial class TimeSpanExt
    {

        #region FromMilliSeconds(Milliseconds)

        public static TimeSpan FromMilliSeconds(UInt32 Milliseconds)
        {
            return TimeSpan.FromTicks(Milliseconds * TimeSpan.TicksPerMillisecond);
        }

        #endregion

        #region FromSeconds(Seconds)

        public static TimeSpan FromSeconds(UInt32 Seconds)
        {
            return TimeSpan.FromTicks(Seconds * TimeSpan.TicksPerSecond);
        }

        #endregion

        #region FromMinutes(Minutes)

        public static TimeSpan FromMinutes(UInt32 Minutes)
        {
            return TimeSpan.FromTicks(Minutes * TimeSpan.TicksPerMinute);
        }

        #endregion

        #region FromHours(Hours)

        public static TimeSpan FromHours(UInt32 Hours)
        {
            return TimeSpan.FromTicks(Hours * TimeSpan.TicksPerHour);
        }

        #endregion

    }

}
