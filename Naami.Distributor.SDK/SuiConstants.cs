namespace Naami.Sdk;

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