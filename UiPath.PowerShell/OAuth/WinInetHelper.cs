using System;
using System.Runtime.InteropServices;

namespace UiPath.PowerShell.OAuth
{
    internal class WinInetHelper
    {
        // https://stackoverflow.com/a/35630408/105929

        private const int INTERNET_SUPPRESS_COOKIE_PERSIST = 3;
        private const int INTERNET_SUPPRESS_COOKIE_PERSIST_RESET = 4;
        private const int INTERNET_OPTION_SUPPRESS_BEHAVIOR = 81;
        private const int INTERNET_OPTION_END_BROWSER_SESSION = 42;

        public static bool SupressCookiePersist()
        {
            return SetOption(INTERNET_OPTION_SUPPRESS_BEHAVIOR, INTERNET_SUPPRESS_COOKIE_PERSIST);
        }

        public static bool ResetCookiePersist()
        {
            return SetOption(INTERNET_SUPPRESS_COOKIE_PERSIST_RESET, null);
        }

        public static bool EndBrowserSession()
        {
            return SetOption(INTERNET_OPTION_END_BROWSER_SESSION, null);
        }

        static bool SetOption(int settingCode, int? option)
        {
            IntPtr optionPtr = IntPtr.Zero;
            int size = 0;
            if (option.HasValue)
            {
                size = sizeof(int);
                optionPtr = Marshal.AllocCoTaskMem(size);
                Marshal.WriteInt32(optionPtr, option.Value);
            }
            try
            {
                return InternetSetOption(0, settingCode, optionPtr, size);
            }
            finally
            {
                if (optionPtr != IntPtr.Zero)
                {
                    Marshal.Release(optionPtr);
                }
            }
        }

        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool InternetSetOption(
            int hInternet,
            int dwOption,
            IntPtr lpBuffer,
            int dwBufferLength
        );
    }
}
