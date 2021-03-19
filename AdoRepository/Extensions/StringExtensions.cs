namespace AdoRepository.Extensions
{
    public static class StringExtensions
    {
        public static string ToEmptyWhenNull(this string value)
        {
            return string.IsNullOrWhiteSpace(value) ? "" : value;
        }
    }
}