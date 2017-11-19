﻿using System;

namespace Rhisis.Core.IO
{
    public static class Time
    {
        private static readonly long Start = Environment.TickCount;
        private static readonly DateTime Utc;

        /// <summary>
        /// Initialize the Utc time.
        /// </summary>
        static Time()
        {
            Utc = new DateTime(1970, 1, 1);
        }

        /// <summary>
        /// Gets the time in seconds from a specified date.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static double TimeInSeconds(DateTime date)
        {
            if (date < Utc)
                date = Utc;

            return (date - Utc).TotalSeconds;
        }

        /// <summary>
        /// Gets the time in seconds from now.
        /// </summary>
        /// <returns></returns>
        public static double TimeInSeconds()
        {
            return TimeInSeconds(DateTime.UtcNow);
        }

        /// <summary>
        /// Gets the number of milliseconds since the program has started.
        /// </summary>
        /// <returns></returns>
        public static long GetElapsedTime()
        {
            return Environment.TickCount - Start;
        }
    }
}
