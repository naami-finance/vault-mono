namespace Naami.Distributor.SDK.Tests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void AsTrimmedAddress_Trims_Leading_0x()
    {
        var address = "0xabc";
        Assert.IsTrue(address.AsTrimmedAddress().Equals("abc"));
    }
    
    [Test]
    public void AsTrimmedAddress_Trims_Leading_0()
    {
        var address = "00abc";
        Assert.IsTrue(address.AsTrimmedAddress().Equals("abc"));
    }
        
    [Test]
    public void AsTrimmedAddress_Trims_Leading_0x0()
    {
        var address = "0x0abc";
        Assert.IsTrue(address.AsTrimmedAddress().Equals("abc"));
    }
}