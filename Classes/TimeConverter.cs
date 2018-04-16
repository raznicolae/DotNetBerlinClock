using System;
using System.Linq;
using System.Text;

namespace BerlinClock
{
    public class TimeConverter : ITimeConverter
    {
        private const int LampsTopHour = 4;
        private const int LampsBottomHour = 4;
        private const string HourLampWhenOn = "R";
        private const string HourLampWhenOff = "O";

        private const int LampsTopMinutes = 11;
        private const int LampsBottomMinutes = 4;
        private const string MinutesLampWhenOn = "Y";
        private const string MinutesLampWhenOff = "O";
        private const string MinutesLampOverrideOn = "R";

        private const string SecondsLampWhenOn = "Y";
        private const string SecondsLampWhenOff = "O";

        public string ConvertTime(string aTime)
        {
            StringBuilder berlinTime = new StringBuilder();

            var hhMmSs = SplitTimeIntoHHMMSS(aTime);

            berlinTime.AppendLine(GetBerlinSeconds(hhMmSs[2]));
            berlinTime.AppendLine(GetBerlinHours(hhMmSs[0]));
            berlinTime.Append(GetBerlinMinutes(hhMmSs[1]));

            return berlinTime.ToString();
        }

        private string GetBerlinHours(int hours)
        {
            StringBuilder sb = new StringBuilder();

            var topHours = hours / 5;
            var bottomHours = hours % 5;

            sb.AppendLine(ConvertValueToRowLamps(topHours, LampsTopHour, HourLampWhenOn, HourLampWhenOff));
            sb.Append(ConvertValueToRowLamps(bottomHours, LampsBottomHour, HourLampWhenOn, HourLampWhenOff));

            return sb.ToString();
        }

        private string GetBerlinMinutes(int minutes)
        {
            StringBuilder sb = new StringBuilder();

            var topMinutes = minutes / 5;
            var bottomMinutes = minutes % 5;
            bool Condition(int i) => i % 3 == 0;

            sb.AppendLine(ConvertValueToRowLamps(topMinutes, LampsTopMinutes, MinutesLampWhenOn, MinutesLampWhenOff, MinutesLampOverrideOn, Condition));
            sb.Append(ConvertValueToRowLamps(bottomMinutes, LampsBottomMinutes, MinutesLampWhenOn, MinutesLampWhenOff));

            return sb.ToString();
        }

        private string GetBerlinSeconds(int seconds) => seconds % 2 == 0 ? SecondsLampWhenOn : SecondsLampWhenOff;

        private int[] SplitTimeIntoHHMMSS(string aTime) => aTime.Split(':').Select(s => int.Parse(s)).ToArray();

        private string ConvertValueToRowLamps(int value, int maxLampsPerRow, string valueWhenOn,
            string valueWhenOff, string overrideOnValue = null,
            Func<int, bool> overrideCondition = null)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < value; i++)
            {
                sb.Append(overrideCondition != null && overrideCondition(i + 1) ? overrideOnValue : valueWhenOn);
            }

            for (int i = 0; i < maxLampsPerRow - value; i++)
            {
                sb.Append(valueWhenOff);
            }

            return sb.ToString();
        }
    }

}
