namespace Naami.Distributor.SDK;

public static class SuiConstants
{
    public const string ObjectExistsStatus = "Exists";

    public const string SuiPackageId = "0x2";
    
    public const string CoinModule = "coin";
    public const string CoinStruct = "Coin";
    public static string CoinIdentifier => $"{SuiPackageId}::{CoinModule}::{CoinStruct}";
    
    public const string SuiModule = "sui";
    public const string SuiStruct = "SUI";
    public const byte SuiDecimals = 9;
    public static string SuiCoinIdentifier => $"{SuiPackageId}::{SuiModule}::{SuiStruct}";
}

public struct Address
{
    private readonly string _value;

    public Address(string value)
    {
        _value = value;
    }

    public override string ToString()
    {
        return _value
            .TrimStart('0')
            .TrimStart('x')
            .TrimStart('0');
    }
    
    public static implicit operator string(Address address) => address._value;
    public static implicit operator Address(string address) => new(address);
}