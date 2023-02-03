namespace Naami.Distributor.SDK;

public record ModuleConstants(
    string ShareBucketModule,
    string ShareBucketStruct,
    string ShareRegistryModule,
    string ShareRegistryStruct
);

public static class Constants
{
    public static ModuleConstants ModuleConstants => new(
        "share_bucket",
        "ShareBucket",
        "share_registry",
        "ShareRegistry"
    );

    public static NetworkConstants NetworkConstants
        => new NetworkConstants("", "", "");
}