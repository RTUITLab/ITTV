using System;
using System.Runtime.InteropServices;

namespace ITTV.WPF.Core.Helpers
{
    public static class IdleTimeDetector
    {
        /// <summary>
        /// Details https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getlastinputinfo
        /// </summary>
        /// <param name="plii"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);

        public static IdleTimeInfo GetIdleTimeInfo()
        {
            var systemUptime = Environment.TickCount;
            var idleTicks = 0;

            var lastInputInfo = new LASTINPUTINFO();
            lastInputInfo.cbSize = (uint)Marshal.SizeOf(lastInputInfo);
            lastInputInfo.dwTime = 0;

            if (GetLastInputInfo(ref lastInputInfo))
            {
                var lastInputTicks = (int)lastInputInfo.dwTime;

                idleTicks = systemUptime - lastInputTicks;
            }

            var idleTime = TimeSpan.FromMilliseconds(idleTicks);

            return new IdleTimeInfo(DateTime.Now - idleTime,
                idleTime,
                systemUptime);
        }
    }

    public class IdleTimeInfo
    {
        public IdleTimeInfo()
        { }

        public IdleTimeInfo(DateTime lastInputTime, TimeSpan idleTime, int systemUptimeMilliseconds)
        {
            LastInputTime = lastInputTime;
            IdleTime = idleTime;
            SystemUptimeMilliseconds = systemUptimeMilliseconds;
        }
        public DateTime LastInputTime { get; }

        public TimeSpan IdleTime { get; }

        public int SystemUptimeMilliseconds { get; }
    }

    public struct LASTINPUTINFO
    {
        public uint cbSize;
        public uint dwTime;
    }
}