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

#endregion

namespace eu.Vanaheimr.Nyi
{

    /// <summary>
    /// Extentions methods for TimeSpan.
    /// </summary>
    public static partial class TimeSpanExt
    {

        #region FromMilliSeconds(Milliseconds)

        /// <summary>
        /// Return a TimeSpan for the given number of milliseconds.
        /// </summary>
        /// <param name="Milliseconds">The number of milliseconds.</param>
        public static TimeSpan FromMilliSeconds(UInt32 Milliseconds)
        {
            return TimeSpan.FromTicks(Milliseconds * TimeSpan.TicksPerMillisecond);
        }

        #endregion

        #region FromSeconds(Seconds)

        /// <summary>
        /// Return a TimeSpan for the given number of seconds.
        /// </summary>
        /// <param name="Seconds">The number of seconds.</param>
        public static TimeSpan FromSeconds(UInt32 Seconds)
        {
            return TimeSpan.FromTicks(Seconds * TimeSpan.TicksPerSecond);
        }

        #endregion

        #region FromMinutes(Minutes)

        /// <summary>
        /// Return a TimeSpan for the given number of minutes.
        /// </summary>
        /// <param name="Minutes">The number of minutes.</param>
        public static TimeSpan FromMinutes(UInt32 Minutes)
        {
            return TimeSpan.FromTicks(Minutes * TimeSpan.TicksPerMinute);
        }

        #endregion

        #region FromHours(Hours)

        /// <summary>
        /// Return a TimeSpan for the given number of hours.
        /// </summary>
        /// <param name="Hours">The number of hours.</param>
        public static TimeSpan FromHours(UInt32 Hours)
        {
            return TimeSpan.FromTicks(Hours * TimeSpan.TicksPerHour);
        }

        #endregion

        #region FromDays(Days)

        /// <summary>
        /// Return a TimeSpan for the given number of days.
        /// </summary>
        /// <param name="Days">The number of days.</param>
        public static TimeSpan FromDays(UInt32 Days)
        {
            return TimeSpan.FromTicks(Days * TimeSpan.TicksPerDay);
        }

        #endregion

    }

}
