using UnityEngine;
using System.Collections;

namespace Boardgame
{
    namespace Board
    {
        public static class State
        {
            public enum TimeOfDay
            {
                day,
                night
            }

            private static TimeOfDay m_timeOfDay;

            public static void SetTime(TimeOfDay input)
            {
                m_timeOfDay = input;
            }

            public static bool IsDayTime()
            {
                return m_timeOfDay == TimeOfDay.day;
            }

        }
    }
}
