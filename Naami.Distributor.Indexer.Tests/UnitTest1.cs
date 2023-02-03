using System.Text;
using Naami.Distributor.Data;
using Naami.Distributor.Indexer.Jobs;
using Naami.Distributor.SDK.Models.Share.Events;
using Naami.SuiNet.Apis.Event;
using Naami.SuiNet.Apis.Event.Query;
using Naami.SuiNet.Types;
using Naami.SuiNet.Types.Events;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using ServiceStack;

namespace Naami.Distributor.Indexer.Tests;

public class Tests
{
    private const string ShareType = "0x6a16517208e903f86a3e52a56a336865994e3359::registry::ShareCreated";


    [Test]
    public async Task Test1()
    {
        var shareType = "0x6a16517208e903f86a3e52a56a336865994e3359::registry::ShareCreated";
        var moveEvent = new MoveEvent(
            "",
            "",
            Utils.TestingSignerAddress,
            shareType,
            Array.Empty<byte>()
        )
        {
            Fields = new ObjectDictionary(new ShareCreated(
                Encoding.ASCII.GetBytes("123::Test"),
                Encoding.ASCII.GetBytes("TST"),
                Encoding.ASCII.GetBytes("TESTSHARE"),
                1234,
                "",
                ""
            ).ToObjectDictionary())
        };

        var env = new SuiEventEnvelope(0, new EventId("abc", 0), new SuiEvent
        {
            MoveEvent = moveEvent
        });

        var eventApiSubstitute = Substitute
            .For<IEventApi>();

        eventApiSubstitute.GetEvents(Arg.Any<IEventQuery>(), Arg.Any<uint>(), Arg.Any<bool>())
            .Returns(Task.Run(() => new EventPage(new[] { env }, null)));

        var fakeDbSet = new List<ShareType>().AsFakeDbSet();

        var j = new IndexSharesJob(eventApiSubstitute, new VaultContext
        {
            ShareTypes = fakeDbSet
        });

        // var eventReadApi = new EventApi(Utils.JsonRpcClient.Value);
        //
        //
        //
        // var j = new IndexSharesJob(eventReadApi);
        await j.RunAsync();
        //
        // Assert.Pass();
    }


    private IEventApi _eventApiSubstitute;
    private VaultContext _dbContext;
    private IndexSharesJob _sut;

    [SetUp]
    public void Setup()
    {
        _eventApiSubstitute = Substitute.For<IEventApi>();
        _dbContext = Substitute.For<VaultContext>();
        _sut = new IndexSharesJob(_eventApiSubstitute, _dbContext);
    }

    [Test]
    public async Task Ensure_ShareTypes_Are_Stored_In_Database()
    {
        var createdEvent = new ShareCreated(
            Encoding.ASCII.GetBytes("123::Test"),
            Encoding.ASCII.GetBytes("TST"),
            Encoding.ASCII.GetBytes("TESTSHARE"),
            1234,
            "",
            ""
        );

        var suiEvent = CreateSuiEvent(createdEvent);

        // create EventEnvelope
        var envelope = new SuiEventEnvelope(3, new EventId("TEST", 0), suiEvent);

        // create Page
        var page = new EventPage(new[] { envelope }, null);

        _eventApiSubstitute
            .GetEvents(Arg.Any<IEventQuery>(), Arg.Any<uint>(), Arg.Any<bool>())
            .Returns(Task.Run(() => page));

        var fakeDbSet = Array.Empty<ShareType>().AsFakeDbSet();
        _dbContext.ShareTypes = fakeDbSet;

        await _sut.RunAsync();
        await _dbContext
            .ShareTypes
            .Received(1)
            .AddRangeAsync(Arg.Is<ShareType[]>(c => c.Length == 1));
    }

    [Test]
    public async Task Ensure_Already_Existing_ShareTypes_Are_Not_Stored_In_Database()
    {
        const string newType = "NEW";
        const string existingType = "EXISTING";
        var newEvent = CreateSuiEvent(new ShareCreated(
            Encoding.ASCII.GetBytes(newType),
            Encoding.ASCII.GetBytes("TST"),
            Encoding.ASCII.GetBytes("TESTSHARE"),
            1234,
            "",
            ""
        ));


        var existingEvent = CreateSuiEvent(new ShareCreated(
            Encoding.ASCII.GetBytes(existingType),
            Encoding.ASCII.GetBytes("TST"),
            Encoding.ASCII.GetBytes("TESTSHARE"),
            1234,
            "",
            ""
        ));

        // create EventEnvelope
        var envelopes = new[] { existingEvent, newEvent }
            .Select(x => new SuiEventEnvelope(3, new EventId("TEST", 0), x))
            .ToArray();

        // create Page
        var page = new EventPage(envelopes, null);

        _eventApiSubstitute
            .GetEvents(Arg.Any<IEventQuery>(), Arg.Any<uint>(), Arg.Any<bool>())
            .Returns(Task.Run(() => page));

        var fakeDbSet = new[]{ new ShareType
        {
            ObjectType = existingType
        } }.AsFakeDbSet();
        _dbContext.ShareTypes = fakeDbSet;

        await _sut.RunAsync();

        await _dbContext
            .ShareTypes
            .Received(1)
            .AddRangeAsync(Arg.Is<ShareType[]>(c => c.Length == 1 && c.First().ObjectType == newType));
    }

    [Test]
    public async Task Ensure_ShareTypes_Are_Linked_Through_EventId()
    {
        // consider multiple pages (last item from previous MUST be linked with first item from new page)
    }

    [Test]
    public async Task Ensure_Last_Page_Item_Does_Not_Have_A_Linked_Element()
    {
    }

    private SuiEvent CreateSuiEvent(ShareCreated shareCreatedEvent)
    {
        var moveEvent = new MoveEvent(
            "",
            "",
            Utils.TestingSignerAddress,
            ShareType,
            Array.Empty<byte>()
        )
        {
            Fields = new ObjectDictionary(shareCreatedEvent.ToObjectDictionary())
        };

        return new SuiEvent() { MoveEvent = moveEvent };
    }
}