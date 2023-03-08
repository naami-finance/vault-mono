namespace Naami.Distributor.SDK;

public static class StringExtensions
{
    public static string AsTrimmedAddress(this string address) => address
        .TrimStart('0')
        .TrimStart('x')
        .TrimStart('0');

    public static string AsFormattedAddress(this string address)
    {
        var trimmed = address
            .TrimStart('0')
            .TrimStart('x')
            .TrimStart('0');

        return $"0x{trimmed}";
    } 
}