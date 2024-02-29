using NUnit.Framework;
using unwidner.Models.AmadeusApiServiceModels.HotelSearchModels;
using unwinder.Services.AmadeusApiService.HotelSearch;
using System;

namespace unwinder.test.Services.AmadeusApiService;

[TestFixture]
public class HotelSearchParametersBuilderTests
{
    private string _mockCheckInDate;
    private string _mockCheckOutDate;
    private Fixture _fixture;

    [SetUp]
    public void Setup()
    {
        _mockCheckInDate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
        _mockCheckOutDate = DateTime.Now.AddDays(3).ToString("yyyy-MM-dd");
        _fixture = new Fixture();
    }

    [Test]
    public void BuildHotelIds_SetsHotelIdsProperty()
    {
        var hotelIds = _fixture.CreateMany<HotelSearchDatum>(5).ToList();
        var hotelSearchListOutputModel = new HotelSearchListOutputModel
        {
            Data = hotelIds
        };

        var builder = new HotelSearchParametersBuilder();

        var result = builder.BuildHotelIds(hotelSearchListOutputModel).Build();

        var expectedHotelIds = hotelIds.Select(datum => datum.HotelId).ToList();

        Assert.IsNotNull(result.HotelIds);
        CollectionAssert.AreEquivalent(expectedHotelIds, result.HotelIds);
    }


    [Test]
    public void BuildNumberOfAdults_SetsNumberOfAdultsProperty()
    {
        var builder = new HotelSearchParametersBuilder();
        int numberOfAdults = 2;

        var result = builder.BuildNumberOfAdults(numberOfAdults).Build();

        Assert.That(result.Adults, Is.EqualTo(numberOfAdults));
    }

    [Test]
    public void BuildInOutDates_SetsCheckInAndCheckOutDates()
    {
        var builder = new HotelSearchParametersBuilder();

        var result = builder.BuildInOutDates(_mockCheckInDate, _mockCheckOutDate).Build();

        Assert.That(result.CheckInDate, Is.EqualTo(_mockCheckInDate));
        Assert.That(result.CheckOutDate, Is.EqualTo(_mockCheckOutDate));
    }

    [Test]
    public void BuildDefaultValues_SetsDefaultCurrency()
    {
        var builder = new HotelSearchParametersBuilder();

        var result = builder.BuildDefaultValues().Build();

        Assert.That(result.Currency, Is.EqualTo("EUR"));
    }

    [Test]
    public void Build_ReturnsCompleteHotelSearchParametersModel()
    {
        var builder = new HotelSearchParametersBuilder();
        var hotelIds = _fixture.CreateMany<HotelSearchDatum>(5).ToList();

        var expectedHotelIds = hotelIds.Select(datum => datum.HotelId).ToList();

        var hotelSearchListOutputModel = new HotelSearchListOutputModel
        {
            Data = hotelIds
        };
        int numberOfAdults = 2;

        var result = builder
            .BuildHotelIds(hotelSearchListOutputModel)
            .BuildNumberOfAdults(numberOfAdults)
            .BuildInOutDates(_mockCheckInDate, _mockCheckOutDate)
            .BuildDefaultValues()
            .Build();

        CollectionAssert.AreEquivalent(expectedHotelIds, result.HotelIds);
        Assert.That(result.Adults, Is.EqualTo(numberOfAdults));
        Assert.That(result.CheckInDate, Is.EqualTo(_mockCheckInDate));
        Assert.That(result.CheckOutDate, Is.EqualTo(_mockCheckOutDate));
        Assert.That(result.Currency, Is.EqualTo("EUR"));
    }
}