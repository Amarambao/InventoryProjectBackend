namespace CommonLayer.Extensions
{
    public static class StringExtensions
    {
        public static string CustomNormalize(this string str)
            => str.Trim()
                .Normalize()
                .ToUpperInvariant();
    }
}
