using System;
using System.Runtime.InteropServices;

namespace VervePlace.YandexGames
{
    public static class ServerTime
    {
        public static DateTime Date => DateTimeOffset.FromUnixTimeMilliseconds(Milliseconds).UtcDateTime;
        public static long Milliseconds => long.Parse(ServerTimeGetTime());

        [DllImport("__Internal")]
        private static extern string ServerTimeGetTime();
    }
}