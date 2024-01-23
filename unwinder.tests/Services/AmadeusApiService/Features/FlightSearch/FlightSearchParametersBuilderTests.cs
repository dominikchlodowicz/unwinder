using FluentAssertions;
using unwinder.Services;
using unwinder.Services.AmadeusApiService;
using unwinder.Services.AmadeusApiService.FlightSearch;
using unwinder.Models.AmadeusApiServiceModels.FlightSearchModels;
using FluentAssertions.Primitives;
using Newtonsoft.Json.Bson;

namespace unwinder.tests.Services.AmadeusApiService;

public class FlightSearchParametersBuilderTests
{
    private Fixture _fixture;

    [SetUp]
    public void Setup()
    {
        _fixture = new Fixture();
    }

    [Test]
    public void BuildNumberOfTravelers_WithValidTravelers_CreatesCorrectTravelersList()
    {
        var builder = new FlightSearchParametersBuilder();
        var travelers = new List<string> { "ADULT", "CHILD"};
        
        builder.BuildNumberOfTravelers(travelers);
        var result = builder.Build();

        Assert.That(result.Travelers.Count, Is.EqualTo(travelers.Count));
        for (int i = 0; i < travelers.Count; i++)
        {
            Assert.That(result.Travelers[i].Id, Is.EqualTo(i.ToString()));
            Assert.That(result.Travelers[i].TravelerType, Is.EqualTo(travelers[i]));
        }
    }

    [Test]
    public void BuildNumberOfTravelers_WithEmptyTravelersList_ThrowsArgumentNullException()
    {
        var builder = new FlightSearchParametersBuilder();
        var travelers = new List<string> {};

        Assert.Throws<ArgumentNullException>(() => builder.BuildNumberOfTravelers(travelers));
    }

    [Test]
    public void BuildNumberOfTravelers_WithInvalidTravelerType_ThrowsArgumentException()
    {
        var builder = new FlightSearchParametersBuilder();
        var travelers = new List<string> {"Test"};

        Assert.Throws<ArgumentException>(() => builder.BuildNumberOfTravelers(travelers));
    }

    [Test]
    public void BuildDateTimeRange_WithValidFormat_CreatesCorrectDateTimeRange()
    {
        var builder = new FlightSearchParametersBuilder();
        var date = "2024-12-31";
        var time = "14:25:22";

        builder.BuildDateTimeRange(date, time);
        builder.BuildOriginDestinations("test", "test");
        var result = builder.Build();

        Assert.That(result.OriginDestinations[0].DepartureDateTimeRange.Date, Is.EqualTo(date));
        Assert.That(result.OriginDestinations[0].DepartureDateTimeRange.Time, Is.EqualTo(time));
    }

    [Test]
    public void BuildDateTimeRange_WithInvalidFormat_ThrowsInvalidOperationException()
    {
        var builder = new FlightSearchParametersBuilder();
        var date = "2024-31-12";
        var time = "26:25:22";

        Assert.Throws<ArgumentException>(() => builder.BuildDateTimeRange(date, time));
    }

    [Test]
    public void BuildOriginDestinations_WithValidArguments_CreatesCorrectOriginDestinations()
    {
        var builder = new FlightSearchParametersBuilder();
        var date = "2024-12-31";
        var time = "14:25:22";
        var originLocationCode = "WAW";
        var destinationLocationCode = "NYC";

        builder.BuildDateTimeRange(date, time);
        builder.BuildOriginDestinations(originLocationCode, destinationLocationCode);
        var result = builder.Build();

        Assert.That(result.OriginDestinations[0].OriginLocationCode, Is.EqualTo(originLocationCode));
        Assert.That(result.OriginDestinations[0].DestinationLocationCode, Is.EqualTo(destinationLocationCode));
    }

    [Test]
    public void BuildOriginDestinations_WithoutDateTimeRange_ThrowsInvalidOperationException()
    {
        var builder = new FlightSearchParametersBuilder();
        var originLocationCode = "WAW";
        var destinationLocationCode = "NYC";

        Assert.Throws<InvalidOperationException>(() => builder.BuildOriginDestinations(originLocationCode, destinationLocationCode));
    }

    [Test]
    public void BuildCurrencyCode_WithValidCurrency_CreatesCurrency()
    {
        var builder = new FlightSearchParametersBuilder();
        var currencyCode = "USD";

        builder.BuildCurrencyCode(currencyCode);
        var result = builder.Build();

        Assert.That(result.CurrencyCode, Is.EqualTo(currencyCode));
    }

    [Test]
    public void BuildCurrencyCode_WithInvalidCurrency_ThrowsArgumentException()
    {
        var builder = new FlightSearchParametersBuilder();
        var currencyCode = "BWP";

        Assert.Throws<ArgumentException>(() => builder.BuildCurrencyCode(currencyCode));
    }

    [Test]
    public void BuildDefaultValues_WithValidValues_CreatesDefaultValues()
    {
        var builder = new FlightSearchParametersBuilder();
        string expectedCabin = "ECONOMY";
        string expectedCoverage = "MOST_SEGMENTS";
        List<string> expectedOriginDestinationIds = new List<string> { "1" };
        int expectedMaxFlightOffers = 5;
        List<string> expectedSources = new List<string> { "GDS" };

        builder.BuildDefaultValues();
        var parameters = builder.Build();

        var cabinRestrictions = parameters.SearchCriteria.FlightFilters.CabinRestrictions.First();
        Assert.That(expectedCabin, Is.EqualTo(cabinRestrictions.Cabin));
        Assert.That(expectedCoverage, Is.EqualTo(cabinRestrictions.Coverage));
        Assert.That(expectedOriginDestinationIds, Is.EqualTo(cabinRestrictions.OriginDestinationIds));
        Assert.That(expectedMaxFlightOffers, Is.EqualTo(parameters.SearchCriteria.MaxFlightOffers));
        Assert.That(expectedSources, Is.EqualTo(parameters.Sources));
    }
}

