namespace ConfigGenerator
{
    public static class Extensions
    {
        public static bool IsPositive(this int i) => i >= 0;

        public static bool IsNegative(this int i) => i < 0;
    }
}