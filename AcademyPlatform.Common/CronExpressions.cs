namespace AcademyPlatform.Common
{
    public static class CronExpressions
    {
        public static string EveryDay = "0 0 3 1/1 * ? *";

        public static string EveryHour = "0 0 0/1 1/1 * ? *";

        public static string EveryThirtyMinutes = "0 0/30 * 1/1 * ? *";

        public static string EveryFiveMinutes = "0 0/5 * 1/1 * ? *";
    }
}
